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
                        ResetAxis();

                        for (int i = 1; i < XDataList.Count; i++)
                        {
                            if (XDataList[i] < xStartCurrent)
                            {
                                xStartCurrent = XDataList[i];
                            }
                            else if (XDataList[i] > xEndCurrent)
                            {
                                xEndCurrent = XDataList[i];
                            }

                            if (YDataList[i] < yStartCurrent)
                            {
                                yStartCurrent = YDataList[i];
                            }
                            else if (YDataList[i] > yEndCurrent)
                            {
                                yEndCurrent = YDataList[i];
                            }
                        }
                    }
                    else    // 此即为 FixedMoveMode 模式
                    {
                        yStartCurrent = yStartInitial;
                        yEndCurrent = yEndInitial;

                        for (int i = 0; i < XDataList.Count; i++)
                        {
                            if (XDataList[i] > xEndCurrent)
                            {
                                xStartCurrent += XDataList[i] - xEndCurrent;
                                xEndCurrent = XDataList[i];
                            }

                            if (YDataList[i] < yStartCurrent)
                            {
                                yStartCurrent = YDataList[i];
                            }
                            else if (YDataList[i] > yEndCurrent)
                            {
                                yEndCurrent = YDataList[i];
                            }
                        }
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

        private void pbCurve_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPoint = e.Location;
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
        }

        private void pbAxisX_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // 绘制边界坐标线
            g.DrawLine(penBorder, pbAxisY.Width, 0,
                pbAxisY.Width, borderLength);
            g.DrawLine(penBorder, pbAxisY.Width + pbCurve.Width, 0,
                pbAxisY.Width + pbCurve.Width, borderLength);

            // 标识边界坐标值
            StringFormat centerFormat = new StringFormat();

            centerFormat.Alignment = StringAlignment.Center;
            g.DrawString(xStartCurrent.ToString(), fontBorder, Brushes.Black,
                pbAxisY.Width, borderLength, centerFormat);
            g.DrawString(xEndCurrent.ToString(), fontBorder, Brushes.Black,
                pbAxisY.Width + pbCurve.Width, borderLength, centerFormat);
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
