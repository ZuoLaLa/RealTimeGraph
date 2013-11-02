using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RealTimeGraph
{
    public partial class RTGControl : UserControl
    {
        private void pbCurve_Paint(object sender, PaintEventArgs e)
        {
            int width = pbCurve.Width;
            int height = pbCurve.Height;
            // 绘图原点坐标变换到控件的左下角
            Graphics g = e.Graphics;
            g.TranslateTransform(0, height - 1);
            g.ScaleTransform(1, -1);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            #region **根据画图模式和数据调整坐标显示**
            if (XDataList != null)
            {
                if (isAutoMove)
                {
                    if (isAutoScale)    // 此即为 GlobalMode 模式
                    {
                        xStartCurrent = (xDataMin < xStartInitial)
                            ? xDataMin : xStartInitial;
                        xEndCurrent = (xDataMax > xEndInitial)
                            ? xDataMax : xEndInitial;
                        yStartCurrent = (yDataMin < yStartInitial)
                            ? yDataMin : yStartInitial;
                        yEndCurrent = (yDataMax > yEndInitial)
                            ? yDataMax : yEndInitial;
                    }
                    else    // 此即为 FixedMoveMode 模式
                    {
                        if (xDataMax > xEndCurrent)
                        {
                            xStartCurrent += xDataMax - xEndCurrent;
                            xEndCurrent = xDataMax;
                        }

                        yStartCurrent = (yDataMin < yStartInitial)
                            ? yDataMin : yStartInitial;
                        yEndCurrent = (yDataMax > yEndInitial)
                            ? yDataMax : yEndInitial;
                    }
                }
            }
            #endregion

            pbAxisX.Refresh();
            pbAxisY.Refresh();

            #region **绘制曲线**
            pointsList.Clear();
            if (dataToPoints(width, height))
            {
                if (pointsList.Count > 1)
                {
                    Pen p = new Pen(Color.Yellow, 1);
                    p.LineJoin = LineJoin.Bevel;
                    g.DrawLines(p, pointsList.ToArray());
                    p.Dispose();
                }
            }
            #endregion
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
                case GraphTypes.GlobalMode:
                case GraphTypes.FixedMoveMode:
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
                dragMove(xD, yD);
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
                rectZoomIn();
                pbCurve.Refresh();
            }
        }

        private void pbCurve_MouseWheel(object sender, MouseEventArgs e)
        {
            if (GraphType == GraphTypes.FixedMoveMode)
            {
                float xDiff = xEndCurrent - xStartCurrent;
                float xDelta = e.Delta / 1200F;
                xStartCurrent -= xDiff * xDelta;
                xEndCurrent += xDiff * xDelta;
                pbCurve.Refresh();
            }
        }

        private void pbCurve_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (GraphType == GraphTypes.FixedMoveMode
                && e.Button == MouseButtons.Middle)
            {
                xEndCurrent = xDataMax;
                xStartCurrent = ((xEndCurrent - (xEndInitial - xStartInitial)) > xStartInitial)
                    ? (xEndCurrent - (xEndInitial - xStartInitial)) : xStartInitial;
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

            // 标识边界坐标值
            StringFormat centerFormat = new StringFormat();

            centerFormat.Alignment = StringAlignment.Center;
            g.DrawString(xStartCurrent.ToString(), fontBorder, Brushes.Black,
                pbAxisY.Width, borderLength, centerFormat);
            g.DrawString(xEndCurrent.ToString(), fontBorder, Brushes.Black,
                pbAxisY.Width + pbCurve.Width, borderLength, centerFormat);

            float xScale1Min;
            float xScale1Max;
            float xScale1;
            getScale1Limits(xStartCurrent, xEndCurrent, out xScale1Min, out xScale1Max, out xScale1);
            // 绘制其他各级刻度线， 以及1级刻度值
            int scale1Num = getScaleNum((int)(pbCurve.Width * xScale1 / (xEndCurrent - xStartCurrent)), scale1Interval);
            int scale1Sum = (int)((xScale1Max - xScale1Min) / xScale1 * scale1Num);
            float xScale1Length = (float)(pbCurve.Width * xScale1 / ((xEndCurrent - xStartCurrent) * scale1Num));
            int scale2Num = getScaleNum((int)xScale1Length, scale2Interval);
            float xScale2Length = xScale1Length / scale2Num;

            float scaleX = pbCurve.Width / (xEndCurrent - xStartCurrent);
            float xScale1Pos = (float)(pbAxisY.Width + (xScale1Min - xStartCurrent) * scaleX);   // 1级刻度坐标位置
            float xScale2Pos;
            float xScale1Value;  // 1级刻度处坐标值

            for (int i = 0; i < scale1Sum; i++)
            {
                xScale1Pos = (float)(pbAxisY.Width + (xScale1Min - xStartCurrent) * scaleX)
                    + xScale1Length * i;
                if (isInGraphX(xScale1Pos))
                {
                    g.DrawLine(penScale1, xScale1Pos, 0,
                    xScale1Pos, scale1Length);
                    xScale1Value = xScale1Min + xScale1 * i / scale1Num;
                    g.DrawString(xScale1Value.ToString(), fontScale1, Brushes.Black,
                        xScale1Pos, borderLength, centerFormat);
                }


                for (int j = 1; j < scale2Num; j++)
                {
                    xScale2Pos = xScale1Pos + xScale2Length * j;
                    if (isInGraphX(xScale2Pos))
                    {
                        g.DrawLine(penScale2, xScale2Pos, 0,
                        xScale2Pos, scale2Length);
                    }
                }
            }
        }

        private void pbAxisY_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // 绘制边界坐标线
            g.DrawLine(penBorder, pbAxisY.Width - borderLength, pbAxisY.Height - 1,
                pbAxisY.Width, pbAxisY.Height - 1);
            g.DrawLine(penBorder, pbAxisY.Width - borderLength, pbTitle.Height,
                pbAxisY.Width, pbTitle.Height);

            // 标识边界坐标值
            StringFormat borderYFormat = new StringFormat();
            borderYFormat.Alignment = StringAlignment.Far;
            borderYFormat.LineAlignment = StringAlignment.Far;
            g.DrawString(yStartCurrent.ToString("#0.###"), fontBorder, Brushes.Black,
                pbAxisY.Width - borderLength, pbAxisY.Height,
                borderYFormat);
            g.DrawString(yEndCurrent.ToString("#0.###"), fontBorder, Brushes.Black,
                pbAxisY.Width - borderLength, pbTitle.Height,
                borderYFormat);

            // 绘制其他各级刻度线， 以及1级刻度值
            float yScale1Min;
            float yScale1Max;
            float yScale1;
            getScale1Limits(yStartCurrent, yEndCurrent, out yScale1Min,
                out yScale1Max, out yScale1);

            int scale1Num = getScaleNum(pbCurve.Height * yScale1 / (yEndCurrent - yStartCurrent), scale1Interval);
            int scale1Sum = (int)(scale1Num * (yScale1Max - yScale1Min) / yScale1);
            float yScale1Length = (float)(pbCurve.Height * yScale1 / ((yEndCurrent - yStartCurrent) * scale1Num));
            int scale2Num = getScaleNum((int)yScale1Length, scale2Interval);
            float yScale2Length = yScale1Length / scale2Num;
            float yScale1Pos;   // 1级刻度坐标位置
            float yScale2Pos;
            double yScale1Value;  // 1级刻度处坐标值
            StringFormat scaleYFormat = new StringFormat();
            scaleYFormat.Alignment = StringAlignment.Far;
            scaleYFormat.LineAlignment = StringAlignment.Center;
            float scaleY = pbCurve.Height / (yEndCurrent - yStartCurrent);

            for (int i = 0; i < scale1Sum; i++)
            {
                yScale1Pos = (float)(pbAxisY.Height - (yScale1Min - yStartCurrent) * scaleY
                    - yScale1Length * i);
                if (isInGraphY(yScale1Pos))
                {
                    g.DrawLine(penScale1, pbAxisY.Width - 1 - scale1Length, yScale1Pos,
                    pbAxisY.Width - 1, yScale1Pos);
                    yScale1Value = yScale1Min + yScale1 * i / scale1Num;
                    g.DrawString(yScale1Value.ToString("#0.##"), fontScale1, Brushes.Black,
                        pbAxisY.Width - 1 - scale1Length, yScale1Pos, scaleYFormat);
                }

                for (int j = 1; j < scale2Num; j++)
                {
                    yScale2Pos = yScale1Pos - yScale2Length * j;
                    if (isInGraphY(yScale2Pos))
                    {
                        g.DrawLine(penScale2, pbAxisY.Width - 1, yScale2Pos,
                            pbAxisY.Width - 1 - scale2Length, yScale2Pos);
                    }
                }
            }
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
