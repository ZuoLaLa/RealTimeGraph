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
            UpdateAxisCurrent();
            UpdateAxisScale();
            UpdateCurveSize();

            Graphics g = e.Graphics;
            if (ShowGrid)
            {
                Gridding(g);
            }

            pbAxisX.Refresh();
            pbAxisY.Refresh();

            DrawCurve(g);
        }

        private void UpdateCurveSize()
        {
            curveHeight = pbCurve.Height - 2 * CURVE_HEIGHT_MARGIN;
            curveWidth = pbCurve.Width;
        }
        /// <summary>绘制曲线
        /// </summary>
        /// <param name="g"></param>
        private void DrawCurve(Graphics g)
        {
            pointsList.Clear();
            // 绘图原点坐标变换到控件的左下角，转换为通常的笛卡尔坐标系，以方便画曲线。
            g.TranslateTransform(0, pbCurve.Height - 1 - CURVE_HEIGHT_MARGIN);
            g.ScaleTransform(1, -1);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (DataToPoints(curveWidth, curveHeight))
            {
                if (pointsList.Count > 1)
                {
                    Pen p = new Pen(Color.Yellow, 1);
                    p.LineJoin = LineJoin.Bevel;
                    g.DrawLines(p, pointsList.ToArray());
                    p.Dispose();
                }
            }
        }
        /// <summary>绘制网格
        /// </summary>
        /// <param name="g"></param>
        private void Gridding(Graphics g)
        {
            float xGrid1Start = (xScale1Min - dispalyRect.XMin) * scaleX;

            for (int i = 0; i < xScale1Sum; i++)
            {
                float xGrid1Pos = xGrid1Start + xScale1Length * i;
                if (IsInCurveX(xGrid1Pos))
                {
                    g.DrawLine(penGrid1, xGrid1Pos, 0,
                        xGrid1Pos, pbCurve.Height);
                }


                for (int j = 1; j < xScale2Num; j++)
                {
                    float xGrid2Pos = xGrid1Pos + xScale2Length * j;
                    if (IsInCurveX(xGrid2Pos))
                    {
                        g.DrawLine(penGrid2, xGrid2Pos, 0,
                            xGrid2Pos, pbCurve.Height);
                    }
                }
            }

            float yGrid1Start = pbCurve.Height - CURVE_HEIGHT_MARGIN
                - (yScale1Min - dispalyRect.YMin) * scaleY;
            for (int i = 0; i <= yScale1Sum; i++)
            {
                float yGrid1Pos = yGrid1Start - yScale1Length * i;
                if (IsInCurveY(yGrid1Pos))
                {
                    g.DrawLine(penGrid1, 0, yGrid1Pos,
                        pbCurve.Width, yGrid1Pos);
                }

                for (int j = 1; j < yScale2Num; j++)
                {
                    float yGrid2Pos = yGrid1Pos - yScale2Length * j;
                    if (IsInCurveY(yGrid2Pos))
                    {
                        g.DrawLine(penGrid2, 0, yGrid2Pos,
                            pbCurve.Width, yGrid2Pos);
                    }
                }
            }
        }

        // 鼠标进入绘图区域时，根据绘图模式改变鼠标形态
        private void pbCurve_MouseEnter(object sender, EventArgs e)
        {
            switch (GraphType)
            {

                case GraphTypes.RectZoomInMode:
                    this.Cursor = Cursors.Cross;
                    break;
                case GraphTypes.DragMode:
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
            if (graphType == GraphTypes.DragMode &&
                e.Button == MouseButtons.Left)
            {
                startPoint = e.Location;
            }
            else if (graphType == GraphTypes.RectZoomInMode &&
                e.Button == MouseButtons.Left)
            {
                pbZoom.Parent = pbCurve;
                pbZoom.Visible = false;
                pbZoomStart = e.Location;
            }
        }

        private void pbCurve_MouseMove(object sender, MouseEventArgs e)
        {
            if (graphType == GraphTypes.DragMode &&
                e.Button == MouseButtons.Left)
            {
                float xD = e.X - startPoint.X;
                float yD = e.Y - startPoint.Y;
                DragMove(xD, yD);
                startPoint = e.Location;
                pbCurve.Refresh();
            }
            else if (graphType == GraphTypes.RectZoomInMode &&
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
                pbZoomLoc.X = (pbZoomStart.X < pbZoomEnd.X) ?
                    pbZoomStart.X : pbZoomEnd.X;
                pbZoomLoc.Y = (pbZoomStart.Y < pbZoomEnd.Y) ?
                    pbZoomStart.Y : pbZoomEnd.Y;
                // 无法直接对 pbZoom.Location.X 进行赋值，故通过 pbZoomLoc 过渡。
                pbZoom.Location = pbZoomLoc;
                pbZoom.Width = Math.Abs(pbZoomEnd.X - pbZoomStart.X);
                pbZoom.Height = Math.Abs(pbZoomEnd.Y - pbZoomStart.Y);
                pbZoom.Visible = true;
            }
        }

        private void pbCurve_MouseUp(object sender, MouseEventArgs e)
        {
            if (graphType == GraphTypes.RectZoomInMode &&
                e.Button == MouseButtons.Left)
            {
                pbZoom.Visible = false;
                RectZoomIn();
                pbCurve.Refresh();
            }
        }

        private void pbCurve_MouseWheel(object sender, MouseEventArgs e)
        {
            if (GraphType == GraphTypes.FixedMoveMode)
            {
                float xDiff = dispalyRect.XRange;
                float xDelta = e.Delta / 1200F;
                dispalyRect.XMin -= xDiff * xDelta;
                dispalyRect.XMax += xDiff * xDelta;
                pbCurve.Refresh();
            }
        }

        private void pbCurve_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (GraphType == GraphTypes.FixedMoveMode
                && e.Button == MouseButtons.Middle)
            {
                dispalyRect.XMax = dataRect.XMax;
                dispalyRect.XMin = ((dispalyRect.XMax - (XEndInitial - XStartInitial)) > XStartInitial)
                    ? (dispalyRect.XMax - (XEndInitial - XStartInitial)) : XStartInitial;
            }
        }

        private void pbAxisX_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // 绘制边界坐标线
            g.DrawLine(penBorder, pbAxisY.Width, 0,
                pbAxisY.Width, borderLength);
            g.DrawLine(penBorder, pbAxisY.Width + pbCurve.Width, 0,
                pbAxisY.Width + pbCurve.Width, borderLength);

            // 绘制 X 轴标题
            StringFormat titleFormat = new StringFormat();
            titleFormat.Alignment = StringAlignment.Center;
            g.DrawString(GraphXTitle, fontAxis, Brushes.Black,
                pbAxisY.Width + pbCurve.Width / 2F,
                pbAxisX.Height / 2F + fontTitle.Height / 5F,
                titleFormat);

            StringFormat centerFormat = new StringFormat();
            centerFormat.Alignment = StringAlignment.Center;
            // 绘制其他各级刻度线， 以及1级刻度值
            float xScale1Start = pbAxisY.Width + (xScale1Min - dispalyRect.XMin) * scaleX;
            for (int i = 0; i < xScale1Sum; i++)
            {
                float xScale1Pos = xScale1Start + xScale1Length * i;   // 1级刻度坐标位置
                if (IsInAxisX(xScale1Pos))
                {
                    g.DrawLine(penScale1, xScale1Pos, 0,
                    xScale1Pos, SCALE1_LENGTH);
                    float xScale1Value = xScale1Min + xScale1 * i / xScale1Num;  // 1级刻度处坐标值
                    g.DrawString(xScale1Value.ToString(), fontScale1, Brushes.Black,
                        xScale1Pos, borderLength, centerFormat);
                }


                for (int j = 1; j < xScale2Num; j++)
                {
                    float xScale2Pos = xScale1Pos + xScale2Length * j;
                    if (IsInAxisX(xScale2Pos))
                    {
                        g.DrawLine(penScale2, xScale2Pos, 0,
                        xScale2Pos, SCALE2_LENGTH);
                    }
                }
            }

            // 标识边界坐标值
            SolidBrush b = new SolidBrush(pbAxisX.BackColor);

            String str = dispalyRect.XMin.ToString("#0.##");
            SizeF sf = g.MeasureString(str, fontBorder);
            g.FillRectangle(b, pbAxisY.Width - sf.Width / 2, borderLength,
                sf.Width, sf.Height);   // 防止坐标的重叠
            g.DrawString(str, fontBorder, Brushes.Black,
                pbAxisY.Width, borderLength, centerFormat);

            str = dispalyRect.XMax.ToString("#0.##");
            sf = g.MeasureString(str, fontBorder);
            g.FillRectangle(b, pbAxisY.Width + pbCurve.Width - sf.Width / 2, borderLength,
                sf.Width, sf.Height);
            g.DrawString(str, fontBorder, Brushes.Black,
                pbAxisY.Width + pbCurve.Width, borderLength, centerFormat);
        }

        private void pbAxisY_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // 绘制边界坐标线
            g.DrawLine(penBorder, pbAxisY.Width - borderLength,
                pbAxisY.Height - 1 - CURVE_HEIGHT_MARGIN,
                pbAxisY.Width, pbAxisY.Height - 1 - CURVE_HEIGHT_MARGIN);
            g.DrawLine(penBorder, pbAxisY.Width - borderLength,
                pbTitle.Height + CURVE_HEIGHT_MARGIN,
                pbAxisY.Width, pbTitle.Height + CURVE_HEIGHT_MARGIN);

            // 绘制其他各级刻度线， 以及1级刻度值
            StringFormat scaleYFormat = new StringFormat();
            scaleYFormat.Alignment = StringAlignment.Far;
            scaleYFormat.LineAlignment = StringAlignment.Center;
            float yScale1Start = pbAxisY.Height - CURVE_HEIGHT_MARGIN
                - (yScale1Min - dispalyRect.YMin) * scaleY;
            for (int i = 0; i < yScale1Sum; i++)
            {
                float yScale1Pos = yScale1Start - yScale1Length * i;   // 1级刻度坐标位置
                if (IsInAxisY(yScale1Pos))
                {
                    g.DrawLine(penScale1, pbAxisY.Width - 1 - SCALE1_LENGTH, yScale1Pos,
                    pbAxisY.Width - 1, yScale1Pos);
                    double yScale1Value = yScale1Min + yScale1 * i / yScale1Num;  // 1级刻度处坐标值
                    g.DrawString(yScale1Value.ToString("#0.##"), fontScale1, Brushes.Black,
                        pbAxisY.Width - 1 - borderLength, yScale1Pos, scaleYFormat);
                }

                for (int j = 1; j < yScale2Num; j++)
                {
                    float yScale2Pos = yScale1Pos - yScale2Length * j;
                    if (IsInAxisY(yScale2Pos))
                    {
                        g.DrawLine(penScale2, pbAxisY.Width - 1, yScale2Pos,
                            pbAxisY.Width - 1 - SCALE2_LENGTH, yScale2Pos);
                    }
                }
            }

            // 标识边界坐标值
            StringFormat borderYFormat = new StringFormat();
            borderYFormat.Alignment = StringAlignment.Far;
            borderYFormat.LineAlignment = StringAlignment.Center;
            SolidBrush b = new SolidBrush(pbAxisY.BackColor);

            String str = dispalyRect.YMin.ToString("#0.###");
            SizeF sf = g.MeasureString(str, fontBorder);
            g.FillRectangle(b, pbAxisY.Width - borderLength - sf.Width,
                pbTitle.Height + pbCurve.Height - CURVE_HEIGHT_MARGIN - sf.Height / 2F,
                sf.Width, sf.Height);
            g.DrawString(str, fontBorder, Brushes.Black,
                pbAxisY.Width - borderLength, pbAxisY.Height - CURVE_HEIGHT_MARGIN,
                borderYFormat);

            str = dispalyRect.YMax.ToString("#0.###");
            sf = g.MeasureString(str, fontBorder);
            g.FillRectangle(b, pbAxisY.Width - borderLength - sf.Width,
                pbTitle.Height + CURVE_HEIGHT_MARGIN - sf.Height / 2F,
                sf.Width, sf.Height);
            g.DrawString(str, fontBorder, Brushes.Black,
                pbAxisY.Width - borderLength, pbTitle.Height + CURVE_HEIGHT_MARGIN,
                borderYFormat);
        }

        private void pbTitle_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // 绘制标题           
            StringFormat titleFormat = new StringFormat();
            titleFormat.Alignment = StringAlignment.Center;
            g.DrawString(GraphTitle, fontTitle, Brushes.Black,
                pbTitle.Width / 2F, pbTitle.Height / 2F - fontTitle.Height / 5F,
                titleFormat);
            // 绘制Y轴标签
            g.DrawString(GraphYTitle, fontAxis, Brushes.Black,
                fontAxis.Height / 5F, pbTitle.Height - fontAxis.Height * 1.2F);
        }
    }
}
