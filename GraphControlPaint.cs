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

        float scaleX;       // X 轴比例尺（单位长度的像素数）
        float scaleY;
        float xScale1Min;   // X 轴上一级刻度的最小值
        float xScale1Max;
        float xScale1;      // X 轴权值
        int xScale1Num;      // 单位权值内的一级刻度划分数
        int xScale1Sum;      // 一级刻度划分总数
        float xScale1Length;// 一级刻度间隔
        int xScale2Num;      // 一级刻度内的二级刻度划分数
        float xScale2Length;
        /// <summary>根据坐标范围调整坐标刻度参数。
        /// </summary>
        private void UpdateAxisScale()
        {
            scaleX = drawAreaSize.Width / graphData.DisplayRect.XRange;
            getScale1Limits(graphData.DisplayRect.XMin, graphData.DisplayRect.XMax,
                out xScale1Min, out xScale1Max, out xScale1);
            xScale1Num = getScaleNum(drawAreaSize.Width * xScale1 / graphData.DisplayRect.XRange,
                GraphProperties.FIRST_SCALE_MIN_LENGTH);
            xScale1Sum = (int)((xScale1Max - xScale1Min) / xScale1 * xScale1Num);
            xScale1Length = drawAreaSize.Width * xScale1
                / (graphData.DisplayRect.XRange * xScale1Num);
            xScale2Num = getScaleNum(xScale1Length, GraphProperties.SECOND_SCALE_MIN_LENGTH);
            xScale2Length = xScale1Length / xScale2Num;

            scaleY = drawAreaSize.Height / graphData.DisplayRect.YRange;
            getScale1Limits(graphData.DisplayRect.YMin, graphData.DisplayRect.YMax, out yScale1Min,
                out yScale1Max, out yScale1);
            yScale1Num = getScaleNum(drawAreaSize.Height * yScale1 / graphData.DisplayRect.YRange,
                GraphProperties.FIRST_SCALE_MIN_LENGTH);
            yScale1Sum = (int)(yScale1Num * (yScale1Max - yScale1Min) / yScale1);
            yScale1Length = drawAreaSize.Height * yScale1
                / (graphData.DisplayRect.YRange * yScale1Num);
            yScale2Num = getScaleNum(yScale1Length, GraphProperties.SECOND_SCALE_MIN_LENGTH);
            yScale2Length = yScale1Length / yScale2Num;
        }

        /// <summary>获取友好坐标系统下的一级坐标显示范围。
        /// 显示坐标的最小值不大于显示数据点的最小值，
        /// 最大值不小于显示数据点的最大值，且均取整（广义上的）。
        /// </summary>
        /// <param name="dataMin">待绘制的数据最小值</param>
        /// <param name="dataMax">待绘制的数据最大值</param>
        /// <param name="scale1Min">计算得到的一级坐标最小值</param>
        /// <param name="scale1Max">计算得到的一级坐标最大值</param>
        /// <param name="scale1">计算得到的一级坐标的分度值</param>
        private void getScale1Limits(double dataMin, double dataMax,
            out float scale1Min, out float scale1Max, out float scale1)
        {
            decimal dataDiff = Convert.ToDecimal(dataMax - dataMin);
            decimal scale = 1;

            if (dataDiff >= 1)
            {
                while ((dataDiff /= 10) >= 1)
                {
                    scale *= 10;
                }
            }
            else if (dataDiff > 0 && dataDiff < 1)
            {
                do
                {
                    scale /= 10;
                    dataDiff *= 10;
                } while (dataDiff < 1);
            }

            scale1Max = Convert.ToSingle(
                Math.Ceiling(Convert.ToDecimal(dataMax) / scale) * scale);
            scale1Min = Convert.ToSingle(
                Math.Floor(Convert.ToDecimal(dataMin) / scale) * scale);
            scale1 = Convert.ToSingle(scale);
        }

        /// <summary>获取刻度划分数
        /// </summary>
        /// <param name="totalWidth">待划分的上级刻度长度</param>
        /// <param name="scaleInterval">本级刻度最小间隔</param>
        /// <returns>刻度划分数（10，5，2，1）</returns>
        private int getScaleNum(double totalWidth, int scaleInterval)
        {
            int scaleNum = 1;
            if (totalWidth / 10 >= scaleInterval)
            {
                scaleNum = 10;
            }
            else if (totalWidth / 5 >= scaleInterval)
            {
                scaleNum = 5;
            }
            else if (totalWidth / 2 >= scaleInterval)
            {
                scaleNum = 2;
            }

            return scaleNum;
        }

        /// <summary>绘制网格
        /// </summary>
        /// <param name="g"></param>
        private void DrawGrid(Graphics g)
        {
            float xGrid1Start = (xScale1Min - graphData.DisplayRect.XMin) * scaleX;

            for (int i = 0; i < xScale1Sum; i++)
            {
                float xGrid1Pos = xGrid1Start + xScale1Length * i;
                if (IsInCurveX(xGrid1Pos))
                {
                    g.DrawLine(graphProperties.FirstGridPen, xGrid1Pos, 0,
                        xGrid1Pos, pbCurve.Height);
                }


                for (int j = 1; j < xScale2Num; j++)
                {
                    float xGrid2Pos = xGrid1Pos + xScale2Length * j;
                    if (IsInCurveX(xGrid2Pos))
                    {
                        g.DrawLine(graphProperties.SecondGridPen, xGrid2Pos, 0,
                            xGrid2Pos, pbCurve.Height);
                    }
                }
            }

            float yGrid1Start = pbCurve.Height - GraphProperties.CURVE_HEIGHT_PADDING
                - (yScale1Min - graphData.DisplayRect.YMin) * scaleY;
            for (int i = 0; i <= yScale1Sum; i++)
            {
                float yGrid1Pos = yGrid1Start - yScale1Length * i;
                if (IsInCurveY(yGrid1Pos))
                {
                    g.DrawLine(graphProperties.FirstGridPen, 0, yGrid1Pos,
                        pbCurve.Width, yGrid1Pos);
                }

                for (int j = 1; j < yScale2Num; j++)
                {
                    float yGrid2Pos = yGrid1Pos - yScale2Length * j;
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
            float xScale1Start = pbAxisY.Width + (xScale1Min - graphData.DisplayRect.XMin) * scaleX;
            for (int i = 0; i < xScale1Sum; i++)
            {
                float xScale1Pos = xScale1Start + xScale1Length * i;   // 1级刻度坐标位置
                if (IsInAxisX(xScale1Pos))
                {
                    g.DrawLine(graphProperties.FirstScalePen, xScale1Pos, 0,
                    xScale1Pos, GraphProperties.FIRST_SCALE_LENGTH);
                    float xScale1Value = xScale1Min + xScale1 * i / xScale1Num;  // 1级刻度处坐标值
                    g.DrawString(xScale1Value.ToString(), graphProperties.FirstScaleFont, Brushes.Black,
                        xScale1Pos, GraphProperties.BORDER_LENGTH, centerFormat);
                }


                for (int j = 1; j < xScale2Num; j++)
                {
                    float xScale2Pos = xScale1Pos + xScale2Length * j;
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
                - (yScale1Min - graphData.DisplayRect.YMin) * scaleY;
            for (int i = 0; i < yScale1Sum; i++)
            {
                float yScale1Pos = yScale1Start - yScale1Length * i;   // 1级刻度坐标位置
                if (IsInAxisY(yScale1Pos))
                {
                    g.DrawLine(graphProperties.FirstScalePen, pbAxisY.Width - 1 - GraphProperties.FIRST_SCALE_LENGTH, yScale1Pos,
                    pbAxisY.Width - 1, yScale1Pos);
                    double yScale1Value = yScale1Min + yScale1 * i / yScale1Num;  // 1级刻度处坐标值
                    g.DrawString(yScale1Value.ToString("#0.##"), graphProperties.FirstScaleFont, Brushes.Black,
                        pbAxisY.Width - 1 - GraphProperties.BORDER_LENGTH, yScale1Pos, scaleYFormat);
                }

                for (int j = 1; j < yScale2Num; j++)
                {
                    float yScale2Pos = yScale1Pos - yScale2Length * j;
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
