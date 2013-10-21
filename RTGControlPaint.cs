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
            switch (GraphType)
            {
                case GraphTypes.GlobalMode:
                    if (XDataList != null)
                    {
                        for (int i = 0; i < XDataList.Count; i++)
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
                    break;
                case GraphTypes.FixedMoveMode:
                    if (XDataList != null)
                    {
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
                    break;
                case GraphTypes.ZoomInMode:
                    {
                        // TODO: 放大模式下的 Paint 事件处理
                    }
                    break;
                default:
                    break;
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
                    Pen p = new Pen(Color.White, 1);
                    p.LineJoin = LineJoin.Bevel;
                    g.DrawLines(p, pointsList.ToArray());
                    p.Dispose();
                }
            }
            #endregion
        }
    }
}
