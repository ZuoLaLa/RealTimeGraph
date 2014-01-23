using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RealTimeGraph
{
    public partial class GraphControl
    {
        private void pbCurve_Paint(object sender, PaintEventArgs e)
        {
            UpdateDrawAreaSize();
            graphData.UpdateDataRect();
            graphData.UpdateDisplayRect(initialRect, GraphStyle);
            UpdateAxisScale();

            Graphics g = e.Graphics;
            if (IsShowGrid)
            {
                DrawGrid(g);
            }
            pbAxisX.Refresh();
            pbAxisY.Refresh();
            DrawCurve(g);
        }

        private void UpdateDrawAreaSize()
        {
            drawAreaSize.Height = pbCurve.Height -
                2 * GraphProperties.CURVE_HEIGHT_PADDING;
            drawAreaSize.Width = pbCurve.Width;
        }

        private DataAxis axisX = new DataAxis();
        private DataAxis axisY = new DataAxis();

        /// <summary>根据坐标范围调整坐标刻度参数。
        /// </summary>
        private void UpdateAxisScale()
        {
            axisX.Update(graphData.DisplayRect.XAxisRange, drawAreaSize.Width);
            axisY.Update(graphData.DisplayRect.YAxisRange, drawAreaSize.Height);
        }

        /// <summary>绘制网格
        /// </summary>
        /// <param name="g"></param>
        private void DrawGrid(Graphics g)
        {
            float xGrid1Start = (axisX.FirstScaleRange.Min - graphData.DisplayRect.XMin) * axisX.UnitLenght;

            for (int i = 0; i < axisX.SumOfFirstScale; i++)
            {
                float xGrid1Pos = xGrid1Start + axisX.FirstScaleInterval * i;
                if (IsInCurveX(xGrid1Pos))
                {
                    g.DrawLine(graphProperties.FirstGridPen, xGrid1Pos, 0,
                        xGrid1Pos, pbCurve.Height);
                }


                for (int j = 1; j < axisX.NumOfSecondScalePerFirstScale; j++)
                {
                    float xGrid2Pos = xGrid1Pos + axisX.SecondScaleInterval * j;
                    if (IsInCurveX(xGrid2Pos))
                    {
                        g.DrawLine(graphProperties.SecondGridPen, xGrid2Pos, 0,
                            xGrid2Pos, pbCurve.Height);
                    }
                }
            }

            float yGrid1Start = pbCurve.Height - GraphProperties.CURVE_HEIGHT_PADDING
                - (axisY.FirstScaleRange.Min - graphData.DisplayRect.YMin) * axisY.UnitLenght;
            for (int i = 0; i <= axisY.SumOfFirstScale; i++)
            {
                float yGrid1Pos = yGrid1Start - axisY.FirstScaleInterval * i;
                if (IsInCurveY(yGrid1Pos))
                {
                    g.DrawLine(graphProperties.FirstGridPen, 0, yGrid1Pos,
                        pbCurve.Width, yGrid1Pos);
                }

                for (int j = 1; j < axisY.NumOfSecondScalePerFirstScale; j++)
                {
                    float yGrid2Pos = yGrid1Pos - axisY.SecondScaleInterval * j;
                    if (IsInCurveY(yGrid2Pos))
                    {
                        g.DrawLine(graphProperties.SecondGridPen, 0, yGrid2Pos,
                            pbCurve.Width, yGrid2Pos);
                    }
                }
            }
        }

        /// <summary>判断纵向网格位置是否位于可绘制区域内
        /// </summary>
        /// <param name="scalePos">纵向网格位置</param>
        /// <returns>若坐标位置位于可绘制区域内，则返回true.</returns>
        private bool IsInCurveX(float scalePos)
        {
            return scalePos > 0 && scalePos < pbCurve.Width - 1;
        }

        /// <summary>判断横向网格位置是否位于可绘制区域内
        /// </summary>
        /// <param name="scalePos">横向网格位置</param>
        /// <returns>若坐标位置位于可绘制区域内，则返回true.</returns>
        private bool IsInCurveY(float scalePos)
        {
            return scalePos >= 1 &&
                scalePos <= pbCurve.Height - 1;
        }

        private void DrawCurve(Graphics g)
        {
            TranslateToCartesian(g);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            PointF[] points = graphData.GetPointsToDraw(drawAreaSize.Width, drawAreaSize.Height);
            if (points != null && points.Length > 1)
            {
                g.DrawLines(graphProperties.CurvePen, points);
            }
        }

        private void TranslateToCartesian(Graphics g)
        {
            g.TranslateTransform(0,
                pbCurve.Height - 1 - GraphProperties.CURVE_HEIGHT_PADDING);
            g.ScaleTransform(1, -1);
        }

        private void pbAxisX_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // 绘制边界坐标线
            g.DrawLine(graphProperties.BorderPen, pbAxisY.Width, 0,
                pbAxisY.Width, GraphProperties.BORDER_LENGTH);
            g.DrawLine(graphProperties.BorderPen, pbAxisY.Width + pbCurve.Width, 0,
                pbAxisY.Width + pbCurve.Width, GraphProperties.BORDER_LENGTH);

            // 绘制 X 轴标题
            StringFormat titleFormat = new StringFormat();
            titleFormat.Alignment = StringAlignment.Center;
            g.DrawString(AxisXTitle, graphProperties.AxisTitleFont, Brushes.Black,
                pbAxisY.Width + pbCurve.Width / 2F,
                pbAxisX.Height / 2F + graphProperties.TitleFont.Height / 5F,
                titleFormat);

            StringFormat centerFormat = new StringFormat();
            centerFormat.Alignment = StringAlignment.Center;
            // 绘制其他各级刻度线， 以及1级刻度值
            float xScale1Start = pbAxisY.Width + (axisX.FirstScaleRange.Min - graphData.DisplayRect.XMin) * axisX.UnitLenght;
            for (int i = 0; i < axisX.SumOfFirstScale; i++)
            {
                float xScale1Pos = xScale1Start + axisX.FirstScaleInterval * i;   // 1级刻度坐标位置
                if (IsInAxisX(xScale1Pos))
                {
                    g.DrawLine(graphProperties.FirstScalePen, xScale1Pos, 0,
                    xScale1Pos, GraphProperties.FIRST_SCALE_LENGTH);
                    float xScale1Value = axisX.FirstScaleRange.Min + (float)graphData.DisplayWeightX * i / axisX.NumOfFirstScalePerWeight;  // 1级刻度处坐标值
                    g.DrawString(xScale1Value.ToString(), graphProperties.FirstScaleFont, Brushes.Black,
                        xScale1Pos, GraphProperties.BORDER_LENGTH, centerFormat);
                }


                for (int j = 1; j < axisX.NumOfSecondScalePerFirstScale; j++)
                {
                    float xScale2Pos = xScale1Pos + axisX.SecondScaleInterval * j;
                    if (IsInAxisX(xScale2Pos))
                    {
                        g.DrawLine(graphProperties.SecondScalePen, xScale2Pos, 0,
                        xScale2Pos, GraphProperties.SECOND_SCALE_LENGTH);
                    }
                }
            }

            // 标识边界坐标值
            SolidBrush b = new SolidBrush(pbAxisX.BackColor);

            String str = graphData.DisplayRect.XMin.ToString("#0.##");
            SizeF sf = g.MeasureString(str, graphProperties.BorderFont);
            g.FillRectangle(b, pbAxisY.Width - sf.Width / 2, GraphProperties.BORDER_LENGTH,
                sf.Width, sf.Height);   // 防止坐标的重叠
            g.DrawString(str, graphProperties.BorderFont, Brushes.Black,
                pbAxisY.Width, GraphProperties.BORDER_LENGTH, centerFormat);

            str = graphData.DisplayRect.XMax.ToString("#0.##");
            sf = g.MeasureString(str, graphProperties.BorderFont);
            g.FillRectangle(b, pbAxisY.Width + pbCurve.Width - sf.Width / 2, GraphProperties.BORDER_LENGTH,
                sf.Width, sf.Height);
            g.DrawString(str, graphProperties.BorderFont, Brushes.Black,
                pbAxisY.Width + pbCurve.Width, GraphProperties.BORDER_LENGTH, centerFormat);
        }

        /// <summary>判断坐标位置是否位于X轴可绘制区域内
        /// </summary>
        /// <param name="scalePos">x刻度坐标位置</param>
        /// <returns>若坐标位置位于X轴可绘制区域内，则返回true.</returns>
        private bool IsInAxisX(float scalePos)
        {
            return scalePos > pbAxisY.Width + 1 &&
                                scalePos < pbAxisY.Width + pbCurve.Width - 1;
        }

        private void pbAxisY_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // 绘制边界坐标线
            g.DrawLine(graphProperties.BorderPen, pbAxisY.Width - GraphProperties.BORDER_LENGTH,
                pbAxisY.Height - 1 - GraphProperties.CURVE_HEIGHT_PADDING,
                pbAxisY.Width, pbAxisY.Height - 1 - GraphProperties.CURVE_HEIGHT_PADDING);
            g.DrawLine(graphProperties.BorderPen, pbAxisY.Width - GraphProperties.BORDER_LENGTH,
                pbTitle.Height + GraphProperties.CURVE_HEIGHT_PADDING,
                pbAxisY.Width, pbTitle.Height + GraphProperties.CURVE_HEIGHT_PADDING);

            // 绘制其他各级刻度线， 以及1级刻度值
            StringFormat scaleYFormat = new StringFormat();
            scaleYFormat.Alignment = StringAlignment.Far;
            scaleYFormat.LineAlignment = StringAlignment.Center;
            float yScale1Start = pbAxisY.Height - GraphProperties.CURVE_HEIGHT_PADDING
                - (axisY.FirstScaleRange.Min - graphData.DisplayRect.YMin) * axisY.UnitLenght;
            for (int i = 0; i < axisY.SumOfFirstScale; i++)
            {
                float yScale1Pos = yScale1Start - axisY.FirstScaleInterval * i;   // 1级刻度坐标位置
                if (IsInAxisY(yScale1Pos))
                {
                    g.DrawLine(graphProperties.FirstScalePen, pbAxisY.Width - 1 - GraphProperties.FIRST_SCALE_LENGTH, yScale1Pos,
                    pbAxisY.Width - 1, yScale1Pos);
                    double yScale1Value = axisY.FirstScaleRange.Min + (float)graphData.DisplayWeightY * i / axisY.NumOfFirstScalePerWeight;  // 1级刻度处坐标值
                    g.DrawString(yScale1Value.ToString("#0.##"), graphProperties.FirstScaleFont, Brushes.Black,
                        pbAxisY.Width - 1 - GraphProperties.BORDER_LENGTH, yScale1Pos, scaleYFormat);
                }

                for (int j = 1; j < axisY.NumOfSecondScalePerFirstScale; j++)
                {
                    float yScale2Pos = yScale1Pos - axisY.SecondScaleInterval * j;
                    if (IsInAxisY(yScale2Pos))
                    {
                        g.DrawLine(graphProperties.SecondScalePen, pbAxisY.Width - 1, yScale2Pos,
                            pbAxisY.Width - 1 - GraphProperties.SECOND_SCALE_LENGTH, yScale2Pos);
                    }
                }
            }

            // 标识边界坐标值
            StringFormat borderYFormat = new StringFormat();
            borderYFormat.Alignment = StringAlignment.Far;
            borderYFormat.LineAlignment = StringAlignment.Center;
            SolidBrush b = new SolidBrush(pbAxisY.BackColor);

            String str = graphData.DisplayRect.YMin.ToString("#0.###");
            SizeF sf = g.MeasureString(str, graphProperties.BorderFont);
            g.FillRectangle(b, pbAxisY.Width - GraphProperties.BORDER_LENGTH - sf.Width,
                pbTitle.Height + pbCurve.Height - GraphProperties.CURVE_HEIGHT_PADDING - sf.Height / 2F,
                sf.Width, sf.Height);
            g.DrawString(str, graphProperties.BorderFont, Brushes.Black,
                pbAxisY.Width - GraphProperties.BORDER_LENGTH, pbAxisY.Height - GraphProperties.CURVE_HEIGHT_PADDING,
                borderYFormat);

            str = graphData.DisplayRect.YMax.ToString("#0.###");
            sf = g.MeasureString(str, graphProperties.BorderFont);
            g.FillRectangle(b, pbAxisY.Width - GraphProperties.BORDER_LENGTH - sf.Width,
                pbTitle.Height + GraphProperties.CURVE_HEIGHT_PADDING - sf.Height / 2F,
                sf.Width, sf.Height);
            g.DrawString(str, graphProperties.BorderFont, Brushes.Black,
                pbAxisY.Width - GraphProperties.BORDER_LENGTH, pbTitle.Height + GraphProperties.CURVE_HEIGHT_PADDING,
                borderYFormat);
        }

        /// <summary>判断坐标位置是否位于Y轴可绘制区域内
        /// </summary>
        /// <param name="scalePos">Y刻度坐标位置</param>
        /// <returns>若坐标位置位于X轴可绘制区域内，则返回true.</returns>
        private bool IsInAxisY(float scalePos)
        {
            return scalePos > pbAxisY.Height - pbCurve.Height + GraphProperties.CURVE_HEIGHT_PADDING + 1 &&
                                scalePos < pbAxisY.Height - GraphProperties.CURVE_HEIGHT_PADDING - 1;
        }

        private void pbTitle_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawGraphTitle(g);
            DrawAxisYTitle(g);
        }

        private void DrawGraphTitle(Graphics g)
        {
            StringFormat titleFormat = new StringFormat();
            titleFormat.Alignment = StringAlignment.Center;
            g.DrawString(GraphTitle, graphProperties.TitleFont, Brushes.Black,
                pbTitle.Width / 2F, pbTitle.Height / 2F - graphProperties.TitleFont.Height / 5F,
                titleFormat);
        }

        private void DrawAxisYTitle(Graphics g)
        {
            g.DrawString(AxisYTitle, graphProperties.AxisTitleFont, Brushes.Black,
                graphProperties.AxisTitleFont.Height / 5F,
                pbTitle.Height - graphProperties.AxisTitleFont.Height * 1.2F);
        }

        #region Mouse Event

        // 鼠标进入绘图区域时，根据绘图模式改变鼠标形态
        private void pbCurve_MouseEnter(object sender, EventArgs e)
        {
            switch (GraphStyle)
            {

                case GraphMode.RectZoomInMode:
                    this.Cursor = Cursors.Cross;
                    break;
                case GraphMode.DragMode:
                    this.Cursor = Cursors.Hand;
                    break;
                default:
                    this.Cursor = Cursors.Default;
                    break;
            }
            pbCurve.Focus();
        }

        private void pbCurve_MouseDown(object sender, MouseEventArgs e)
        {
            if (GraphStyle == GraphMode.DragMode &&
                e.Button == MouseButtons.Left)
            {
                startPoint = e.Location;
            }
            else if (GraphStyle == GraphMode.RectZoomInMode &&
                     e.Button == MouseButtons.Left)
            {
                pbZoom.Parent = pbCurve;
                pbZoom.Visible = false;
                pbZoomStart = e.Location;
            }
        }

        private void pbCurve_MouseMove(object sender, MouseEventArgs e)
        {
            if (GraphStyle == GraphMode.DragMode &&
                e.Button == MouseButtons.Left)
            {
                float xD = e.X - startPoint.X;
                float yD = e.Y - startPoint.Y;
                DragMove(xD, yD);
                startPoint = e.Location;
                pbCurve.Refresh();
            }
            else if (GraphStyle == GraphMode.RectZoomInMode &&
                     e.Button == MouseButtons.Left)
            {
                if (e.Location.X < 0)
                {
                    pbZoomEnd.X = 0;
                }
                else if (e.Location.X > pbCurve.Width - 1)
                {
                    pbZoomEnd.X = pbCurve.Width - 1;
                }
                else
                {
                    pbZoomEnd.X = e.Location.X;
                }

                if (e.Location.Y < 0)
                {
                    pbZoomEnd.Y = 0;
                }
                else if (e.Location.Y > pbCurve.Height - 1)
                {
                    pbZoomEnd.Y = pbCurve.Height - 1;
                }
                else
                {
                    pbZoomEnd.Y = e.Location.Y;
                }

                Point pbZoomLoc = new Point();
                pbZoomLoc.X = (pbZoomStart.X < pbZoomEnd.X)
                    ? pbZoomStart.X
                    : pbZoomEnd.X;
                pbZoomLoc.Y = (pbZoomStart.Y < pbZoomEnd.Y)
                    ? pbZoomStart.Y
                    : pbZoomEnd.Y;
                // 无法直接对 pbZoom.Location.X 进行赋值，故通过 pbZoomLoc 过渡。
                pbZoom.Location = pbZoomLoc;
                pbZoom.Width = Math.Abs(pbZoomEnd.X - pbZoomStart.X);
                pbZoom.Height = Math.Abs(pbZoomEnd.Y - pbZoomStart.Y);
                pbZoom.Visible = true;
            }
        }

        /// <summary>曲线拖动
        /// </summary>
        /// <param name="xD">X 轴方向上的拖动量</param>
        /// <param name="yD">Y 轴方向上的拖动量</param>
        private void DragMove(float xD, float yD)
        {
            float xM = xD * graphData.DisplayRect.XRange / (pbCurve.Width - 1);
            float yM = yD * graphData.DisplayRect.YRange / (pbCurve.Height - 1);
            graphData.DisplayRect = new DataRect(
                graphData.DisplayRect.XMin - xM, graphData.DisplayRect.XMax - xM,
                graphData.DisplayRect.YMin + yM, graphData.DisplayRect.YMax + yM);
        }

        private void pbCurve_MouseUp(object sender, MouseEventArgs e)
        {
            if (GraphStyle == GraphMode.RectZoomInMode &&
                e.Button == MouseButtons.Left)
            {
                pbZoom.Visible = false;
                RectZoomIn();
                pbCurve.Refresh();
            }
        }

        /// <summary>矩形框选放大
        /// </summary>
        private void RectZoomIn()
        {
            DataRect selectedRect = new DataRect(
                pbZoom.Location.X, pbZoom.Location.X + pbZoom.Width,
                pbZoom.Location.Y, pbZoom.Location.Y + pbZoom.Height);
            DataRect zoomInRect = ZoomInSelectedRect(selectedRect);

            if (zoomInRect.XRange >= XDataAccuracy && zoomInRect.YRange >= YDataAccuracy)
            {
                SetDisplayRect(zoomInRect);
                MsgOutput = "Zoom in normally";
            }
            else if (zoomInRect.XRange >= XDataAccuracy)
            {
                SetDisplayRect(zoomInRect.XMin, zoomInRect.XMax,
                    (zoomInRect.YMin + zoomInRect.YMax - YDataAccuracy) / 2F,
                    (zoomInRect.YMin + zoomInRect.YMax + YDataAccuracy) / 2F);
                MsgOutput = "Zoom in to the Y data accuracy";
            }
            else if (zoomInRect.YRange >= YDataAccuracy)
            {
                SetDisplayRect(
                    (zoomInRect.XMin + zoomInRect.XMax - XDataAccuracy) / 2F,
                    (zoomInRect.XMin + zoomInRect.XMax + XDataAccuracy) / 2F,
                    zoomInRect.YMin, zoomInRect.YMax);
                MsgOutput = "Zoom in to the X data accuracy";
            }
            else
            {
                SetDisplayRect(
                    (zoomInRect.XMin + zoomInRect.XMax - XDataAccuracy) / 2F,
                    (zoomInRect.XMin + zoomInRect.XMax + XDataAccuracy) / 2F,
                    (zoomInRect.YMin + zoomInRect.YMax - YDataAccuracy) / 2F,
                    (zoomInRect.YMin + zoomInRect.YMax + YDataAccuracy) / 2F);
                MsgOutput = "Zoom in to all data accuracy";
            }
        }

        /// <summary>设置坐标轴范围
        /// </summary>
        /// <param name="xS">更新后的 X 轴左端点</param>
        /// <param name="xE">更新后的 X 轴右端点</param>
        /// <param name="yS">更新后的 Y 轴下端点</param>
        /// <param name="yE">更新后的 Y 轴上端点</param>
        private void SetDisplayRect(float xS, float xE, float yS, float yE)
        {
            graphData.DisplayRect.UpdateRect(xS, xE, yS, yE);
        }

        private void SetDisplayRect(DataRect newRect)
        {
            graphData.DisplayRect.UpdateRect(newRect);
        }

        private DataRect ZoomInSelectedRect(DataRect selectedRect)
        {
            return new DataRect
            {
                XMin = graphData.DisplayRect.XMin +
                       selectedRect.XMin * graphData.DisplayRect.XRange / drawAreaSize.Width,
                XMax = graphData.DisplayRect.XMin +
                       selectedRect.XMax * graphData.DisplayRect.XRange / drawAreaSize.Width,
                YMin = graphData.DisplayRect.YMax -
                       selectedRect.YMax * graphData.DisplayRect.YRange / drawAreaSize.Height,
                YMax = graphData.DisplayRect.YMax -
                       selectedRect.YMin * graphData.DisplayRect.YRange / drawAreaSize.Height
            };
        }

        private void pbCurve_MouseWheel(object sender, MouseEventArgs e)
        {
            if (GraphStyle == GraphMode.FixMoveMode)
            {
                float xDiff = graphData.DisplayRect.XRange;
                float xDelta = e.Delta / 1200F;
                graphData.DisplayRect.XMin -= xDiff * xDelta;
                graphData.DisplayRect.XMax += xDiff * xDelta;
                pbCurve.Refresh();
            }
        }

        private void pbCurve_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (GraphStyle == GraphMode.FixMoveMode
                && e.Button == MouseButtons.Middle)
            {
                graphData.ResetDisplayRectWidthToInitial(initialRect);
            }
        }

        #endregion
    }
}
