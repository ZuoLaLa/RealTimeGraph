using System;
using System.Collections.Generic;
using System.Linq;
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
            UpdateAxis();

            Graphics g = e.Graphics;
            if (IsShowGrid)
            {
                DrawGrid(g);
            }
            pbAxisX.Refresh();
            pbAxisY.Refresh();
            DrawCurve(g);
        }

        private Size drawAreaSize;

        private void UpdateDrawAreaSize()
        {
            drawAreaSize.Height = pbCurve.Height -
                2 * GraphProperties.CURVE_HEIGHT_PADDING;
            drawAreaSize.Width = pbCurve.Width;
        }

        private DataAxisX axisX = new DataAxisX();
        private DataAxisX axisY = new DataAxisX();

        /// <summary>根据坐标范围调整坐标刻度参数。
        /// </summary>
        private void UpdateAxis()
        {
            UpdateAxisRange();
            axisX.Update(drawAreaSize.Width);
            axisY.Update(drawAreaSize.Height);
        }

        private void UpdateAxisRange()
        {
            if (DataLists.HasData())
            {
                if (GraphStyle == GraphMode.GlobalMode)
                {
                    axisX.UpdateGlobalRange(initialRect.XAxisRange,
                        new DataRange(DataLists.MinX, DataLists.MaxX));
                    axisY.UpdateGlobalRange(initialRect.YAxisRange,
                        new DataRange(DataLists.MinY, DataLists.MaxY));
                }
                else if (GraphStyle == GraphMode.FixMoveMode
                    && DataLists.MaxX > axisX.Max)
                {
                    axisX.UpdateFixMoveRange(DataLists.MaxX);
                }
                axisY.UpdateGlobalRange(initialRect.YAxisRange,
                      new DataRange(DataLists.MinY, DataLists.MaxY));
            }
            else
            {
                axisX.Min = initialRect.XMin;
                axisY.Min = initialRect.YMin;
                axisX.Max = initialRect.XMax;
                axisY.Max = initialRect.YMax;
            }
        }

        /// <summary>绘制网格
        /// </summary>
        /// <param name="g"></param>
        private void DrawGrid(Graphics g)
        {
            DrawVerticalGrid(g);
            DrawHorizontalGrid(g);
        }

        private void DrawVerticalGrid(Graphics g)
        {
            foreach (var xGrid1Pos in GetFirstGridPositions())
            {
                g.DrawLine(graphProperties.FirstGridPen, xGrid1Pos, 0,
                    xGrid1Pos, pbCurve.Height);
            }
            foreach (var xGrid2Pos in GetSecondGridPositions())
            {
                g.DrawLine(graphProperties.SecondGridPen, xGrid2Pos, 0,
                        xGrid2Pos, pbCurve.Height);
            }
        }

        private IEnumerable<float> GetFirstGridPositions()
        {
            float xGrid1Start =
                (axisX.FirstScaleRange.Min - axisX.Min)
                * axisX.UnitLenght;
            for (int i = 0; i < axisX.SumOfFirstScale; i++)
            {
                yield return xGrid1Start + axisX.FirstScaleInterval * i;
            }
        }

        private IEnumerable<float> GetSecondGridPositions()
        {
            float xGrid1Start =
                (axisX.FirstScaleRange.Min - axisX.Min)
                * axisX.UnitLenght;
            for (int i = 0; i < axisX.SumOfFirstScale; i++)
            {
                float xGrid1Pos = xGrid1Start + axisX.FirstScaleInterval * i;
                for (int j = 1; j < axisX.NumOfSecondScalePerFirstScale; j++)
                {
                    float xGrid2Pos = xGrid1Pos + axisX.SecondScaleInterval * j;
                    yield return xGrid2Pos;
                }
            }
        }

        private void DrawHorizontalGrid(Graphics g)
        {
            float yGrid1Start = pbCurve.Height - GraphProperties.CURVE_HEIGHT_PADDING
                                - (axisY.FirstScaleRange.Min - axisY.Min)
                                * axisY.UnitLenght;
            for (int i = 0; i <= axisY.SumOfFirstScale; i++)
            {
                float yGrid1Pos = yGrid1Start - axisY.FirstScaleInterval * i;
                g.DrawLine(graphProperties.FirstGridPen, 0, yGrid1Pos,
                    pbCurve.Width, yGrid1Pos);
                for (int j = 1; j < axisY.NumOfSecondScalePerFirstScale; j++)
                {
                    float yGrid2Pos = yGrid1Pos - axisY.SecondScaleInterval * j;
                    g.DrawLine(graphProperties.SecondGridPen, 0, yGrid2Pos,
                        pbCurve.Width, yGrid2Pos);
                }
            }
        }

        private void DrawCurve(Graphics g)
        {
            TranslateToCartesian(g);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            List<PointF[]> pointsLists = GetPointsToDraw(
                drawAreaSize.Width, drawAreaSize.Height);
            for (int i = 0; i < pointsLists.Count; i++)
            {
                if (pointsLists[i] != null && pointsLists[i].Length > 1)
                {
                    g.DrawLines(
                        graphProperties.CurvePens[i % graphProperties.CurvePens.Count()],
                        pointsLists[i]);
                }
            }
        }

        /// <summary>数据转换为待绘制区域上的点集
        /// </summary>
        /// <param name="width">待绘制区域的宽度</param>
        /// <param name="height">待绘制区域的高度</param>
        public List<PointF[]> GetPointsToDraw(int width, int height)
        {
            List<PointF[]> pointsLists = new List<PointF[]>();
            if ((axisX.Range > 0.9F * XDataAccuracy ||
                 axisY.Range > 0.9F * YDataAccuracy)
                && DataLists.HasData())
            {
                foreach (DataPairList<float> dataList in DataLists)
                {
                    List<PointF> points = new List<PointF>();
                    foreach (DataPair<float> dataPair in dataList)
                    {
                        points.Add(new PointF
                        {
                            X = (dataPair.X - axisX.Min) * (width - 1)
                                / axisX.Range,
                            Y = (dataPair.Y - axisY.Min) * (height - 1)
                                / axisY.Range
                        });
                    }
                    pointsLists.Add(points.ToArray());
                }
            }
            return pointsLists;
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
            DrawAxisXTitle(g);
            DrawAxisXBorder(g);
            DrawAxisXScales(g);
            // 注意先绘制其他各级刻度值，最后再绘制边界刻度值，
            // 以保证当数字重叠时，边界刻度值覆盖其他各级刻度值。
            DrawAxisXBorderValue(g);
        }

        private void DrawAxisXTitle(Graphics g)
        {
            StringFormat titleFormat = new StringFormat
            {
                Alignment = StringAlignment.Center
            };
            g.DrawString(AxisXTitle, graphProperties.AxisTitleFont, Brushes.Black,
                pbAxisY.Width + pbCurve.Width / 2F,
                pbAxisX.Height / 2F + graphProperties.TitleFont.Height / 5F,
                titleFormat);
        }

        private void DrawAxisXBorder(Graphics g)
        {
            g.DrawLine(graphProperties.BorderPen,
                pbAxisY.Width, 0,
                pbAxisY.Width, GraphProperties.BORDER_LENGTH);
            g.DrawLine(graphProperties.BorderPen,
                pbAxisY.Width + pbCurve.Width, 0,
                pbAxisY.Width + pbCurve.Width, GraphProperties.BORDER_LENGTH);
        }

        private void DrawAxisXScales(Graphics g)
        {
            StringFormat centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center
            };
            // 绘制其他各级刻度线， 以及1级刻度值
            float xScale1Start = pbAxisY.Width +
                (axisX.FirstScaleRange.Min - axisX.Min)
                    * axisX.UnitLenght;
            for (int i = 0; i < axisX.SumOfFirstScale; i++)
            {
                float xScale1Pos = xScale1Start + axisX.FirstScaleInterval * i; // 1级刻度坐标位置
                if (IsInAxisX(xScale1Pos))
                {
                    g.DrawLine(graphProperties.FirstScalePen, xScale1Pos, 0,
                        xScale1Pos, GraphProperties.FIRST_SCALE_LENGTH);
                    float xScale1Value = axisX.FirstScaleRange.Min +
                        (float)axisX.Weight * i / axisX.NumOfFirstScalePerWeight; // 1级刻度处坐标值
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

        private void DrawAxisXBorderValue(Graphics g)
        {
            SolidBrush brush = new SolidBrush(pbAxisX.BackColor);
            StringFormat centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center
            };
            String strMin = axisX.Min.ToString("#0.##");
            SizeF strMinSize = g.MeasureString(strMin, graphProperties.BorderFont);
            g.FillRectangle(brush, pbAxisY.Width - strMinSize.Width / 2, GraphProperties.BORDER_LENGTH,
                strMinSize.Width, strMinSize.Height); // 防止坐标的重叠
            g.DrawString(strMin, graphProperties.BorderFont, Brushes.Black,
                pbAxisY.Width, GraphProperties.BORDER_LENGTH, centerFormat);

            string strMax = axisX.Max.ToString("#0.##");
            SizeF strMaxSize = g.MeasureString(strMax, graphProperties.BorderFont);
            g.FillRectangle(brush, pbAxisY.Width + pbCurve.Width - strMaxSize.Width / 2, GraphProperties.BORDER_LENGTH,
                strMaxSize.Width, strMaxSize.Height);
            g.DrawString(strMax, graphProperties.BorderFont, Brushes.Black,
                pbAxisY.Width + pbCurve.Width, GraphProperties.BORDER_LENGTH, centerFormat);
        }

        private void pbAxisY_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawAxisYBorder(g);
            DrawAxisYScales(g);
            DrawAxisYBorderVaule(g);
        }

        private void DrawAxisYBorder(Graphics g)
        {
            g.DrawLine(graphProperties.BorderPen, pbAxisY.Width - GraphProperties.BORDER_LENGTH,
                pbAxisY.Height - 1 - GraphProperties.CURVE_HEIGHT_PADDING,
                pbAxisY.Width, pbAxisY.Height - 1 - GraphProperties.CURVE_HEIGHT_PADDING);
            g.DrawLine(graphProperties.BorderPen, pbAxisY.Width - GraphProperties.BORDER_LENGTH,
                pbTitle.Height + GraphProperties.CURVE_HEIGHT_PADDING,
                pbAxisY.Width, pbTitle.Height + GraphProperties.CURVE_HEIGHT_PADDING);
        }

        private void DrawAxisYScales(Graphics g)
        {
            StringFormat scaleYFormat = new StringFormat();
            scaleYFormat.Alignment = StringAlignment.Far;
            scaleYFormat.LineAlignment = StringAlignment.Center;
            float yScale1Start = pbAxisY.Height - GraphProperties.CURVE_HEIGHT_PADDING
                                 - (axisY.FirstScaleRange.Min - axisY.Min) * axisY.UnitLenght;
            for (int i = 0; i < axisY.SumOfFirstScale; i++)
            {
                float yScale1Pos = yScale1Start - axisY.FirstScaleInterval * i; // 1级刻度坐标位置
                if (IsInAxisY(yScale1Pos))
                {
                    g.DrawLine(graphProperties.FirstScalePen, pbAxisY.Width - 1 - GraphProperties.FIRST_SCALE_LENGTH, yScale1Pos,
                        pbAxisY.Width - 1, yScale1Pos);
                    double yScale1Value = axisY.FirstScaleRange.Min +
                                          (float)axisY.Weight * i / axisY.NumOfFirstScalePerWeight; // 1级刻度处坐标值
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
        }

        /// <summary>判断坐标位置是否位于Y轴可绘制区域内
        /// </summary>
        /// <param name="scalePos">Y刻度坐标位置</param>
        /// <returns>若坐标位置位于X轴可绘制区域内，则返回true.</returns>
        private bool IsInAxisY(float scalePos)
        {
            return scalePos > pbAxisY.Height - pbCurve.Height +
                                GraphProperties.CURVE_HEIGHT_PADDING + 1
                && scalePos < pbAxisY.Height - GraphProperties.CURVE_HEIGHT_PADDING - 1;
        }

        private void DrawAxisYBorderVaule(Graphics g)
        {
            StringFormat borderYFormat = new StringFormat();
            borderYFormat.Alignment = StringAlignment.Far;
            borderYFormat.LineAlignment = StringAlignment.Center;
            SolidBrush b = new SolidBrush(pbAxisY.BackColor);

            String str = axisY.Min.ToString("#0.###");
            SizeF sf = g.MeasureString(str, graphProperties.BorderFont);
            g.FillRectangle(b, pbAxisY.Width - GraphProperties.BORDER_LENGTH - sf.Width,
                pbTitle.Height + pbCurve.Height - GraphProperties.CURVE_HEIGHT_PADDING - sf.Height / 2F,
                sf.Width, sf.Height);
            g.DrawString(str, graphProperties.BorderFont, Brushes.Black,
                pbAxisY.Width - GraphProperties.BORDER_LENGTH, pbAxisY.Height - GraphProperties.CURVE_HEIGHT_PADDING,
                borderYFormat);

            str = axisY.Max.ToString("#0.###");
            sf = g.MeasureString(str, graphProperties.BorderFont);
            g.FillRectangle(b, pbAxisY.Width - GraphProperties.BORDER_LENGTH - sf.Width,
                pbTitle.Height + GraphProperties.CURVE_HEIGHT_PADDING - sf.Height / 2F,
                sf.Width, sf.Height);
            g.DrawString(str, graphProperties.BorderFont, Brushes.Black,
                pbAxisY.Width - GraphProperties.BORDER_LENGTH, pbTitle.Height + GraphProperties.CURVE_HEIGHT_PADDING,
                borderYFormat);
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

        private void pbCurve_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private Point startPoint; // 用于记录拖动操作时鼠标按下的位置
        private Point pbZoomStart; // 框选放大框的左上角位置

        private void pbCurve_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (GraphStyle == GraphMode.DragMode)
                {
                    startPoint = e.Location;
                }
                else if (GraphStyle == GraphMode.RectZoomInMode)
                {
                    pbZoom.Parent = pbCurve;
                    pbZoom.Visible = false;
                    pbZoomStart = e.Location;
                }
            }
        }

        private Point pbZoomEnd; // 框选放大框的右下角位置

        private void pbCurve_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (GraphStyle == GraphMode.DragMode)
                {
                    DragMove(e.X - startPoint.X, e.Y - startPoint.Y);
                    startPoint = e.Location;
                    pbCurve.Refresh();
                }
                else if (GraphStyle == GraphMode.RectZoomInMode)
                {
                    UpdatePictureBoxZoom(e.Location);
                }
            }
        }

        /// <summary>曲线拖动
        /// </summary>
        /// <param name="xDiff">X 轴方向上的拖动量</param>
        /// <param name="yDiff">Y 轴方向上的拖动量</param>
        private void DragMove(float xDiff, float yDiff)
        {
            float xMove = xDiff * axisX.Range / (pbCurve.Width - 1);
            float yMove = yDiff * axisY.Range / (pbCurve.Height - 1);
            axisX.Min -= xMove;
            axisX.Max -= xMove;
            axisY.Min += yMove;
            axisY.Max += yMove;
        }

        private void UpdatePictureBoxZoom(Point end)
        {
            if (end.X < 0)
            {
                pbZoomEnd.X = 0;
            }
            else if (end.X > pbCurve.Width - 1)
            {
                pbZoomEnd.X = pbCurve.Width - 1;
            }
            else
            {
                pbZoomEnd.X = end.X;
            }

            if (end.Y < 0)
            {
                pbZoomEnd.Y = 0;
            }
            else if (end.Y > pbCurve.Height - 1)
            {
                pbZoomEnd.Y = pbCurve.Height - 1;
            }
            else
            {
                pbZoomEnd.Y = end.Y;
            }

            pbZoom.Location = new Point
            {
                X = (pbZoomStart.X < pbZoomEnd.X) ? pbZoomStart.X : pbZoomEnd.X,
                Y = (pbZoomStart.Y < pbZoomEnd.Y) ? pbZoomStart.Y : pbZoomEnd.Y
            };
            pbZoom.Width = Math.Abs(pbZoomEnd.X - pbZoomStart.X);
            pbZoom.Height = Math.Abs(pbZoomEnd.Y - pbZoomStart.Y);
            pbZoom.Visible = true;
        }

        private void pbCurve_MouseUp(object sender, MouseEventArgs e)
        {
            if (GraphStyle == GraphMode.RectZoomInMode &&
                e.Button == MouseButtons.Left)
            {
                pbZoom.Visible = false;
                ZoomInSelectedRect();
                pbCurve.Refresh();
            }
        }

        /// <summary>矩形框选放大
        /// </summary>
        private void ZoomInSelectedRect()
        {
            DataRect selectedRect = new DataRect(
                pbZoom.Location.X, pbZoom.Location.X + pbZoom.Width,
                pbZoom.Location.Y, pbZoom.Location.Y + pbZoom.Height);
            DataRect zoomInRect = GetZoomedRect(selectedRect);

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

        private DataRect GetZoomedRect(DataRect selectedRect)
        {
            return new DataRect
            {
                XMin = axisX.Min +
                       selectedRect.XMin * axisX.Range / drawAreaSize.Width,
                XMax = axisX.Min +
                       selectedRect.XMax * axisX.Range / drawAreaSize.Width,
                YMin = axisY.Max -
                       selectedRect.YMax * axisY.Range / drawAreaSize.Height,
                YMax = axisY.Max -
                       selectedRect.YMin * axisY.Range / drawAreaSize.Height
            };
        }

        /// <summary>设置坐标轴范围
        /// </summary>
        /// <param name="xS">更新后的 X 轴左端点</param>
        /// <param name="xE">更新后的 X 轴右端点</param>
        /// <param name="yS">更新后的 Y 轴下端点</param>
        /// <param name="yE">更新后的 Y 轴上端点</param>
        private void SetDisplayRect(float xS, float xE, float yS, float yE)
        {
            axisX.Min = xS;
            axisX.Max = xE;
            axisY.Min = yS;
            axisY.Max = yE;
        }

        private void SetDisplayRect(DataRect newRect)
        {
            axisX.Min = newRect.XMin;
            axisX.Max = newRect.XMax;
            axisY.Min = newRect.YMin;
            axisY.Max = newRect.YMax;
        }

        private void pbCurve_MouseWheel(object sender, MouseEventArgs e)
        {
            if (GraphStyle == GraphMode.FixMoveMode)
            {
                float xDiff = axisX.Range;
                float xDelta = e.Delta / 1200F;
                axisX.Min -= xDiff * xDelta;
                axisX.Max += xDiff * xDelta;
                pbCurve.Refresh();
            }
        }

        private void pbCurve_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (GraphStyle == GraphMode.FixMoveMode
                && e.Button == MouseButtons.Middle)
            {
                axisX.Max = DataLists.MaxX;
                axisX.Min = ((axisX.Max - initialRect.XRange) > initialRect.XMin)
                    ? (axisX.Max - initialRect.XRange)
                    : initialRect.XMin;
            }
        }

        #endregion

        public void ScreenShot()
        {
            Bitmap pic = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(pic);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.CopyFromScreen(this.PointToScreen(Point.Empty),
                Point.Empty, this.Size);
            string saveName = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".png";
            pic.Save(saveName);
            MessageBox.Show("截图已保存至程序所在目录。");
            this.Refresh();
        }
    }
}
