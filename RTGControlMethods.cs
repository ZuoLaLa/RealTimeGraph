using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace RealTimeGraph
{
    public partial class RTGControl : UserControl
    {
        /// <summary>
        /// 初始化曲线控件
        /// </summary>
        private void initialGraph()
        {
            xStartInitial = 0;
            xEndInitial = 100;
            yStartInitial = 0;
            yEndInitial = 1;

            xStartCurrent = xStartInitial;
            xEndCurrent = xEndInitial;
            yStartCurrent = yStartInitial;
            yEndCurrent = yEndInitial;

            // 默认初始处于滚动模式
            graphType = GraphTypes.FixedMoveMode;
            isAutoMove = true;
            isAutoScale = false;

            XDataList = new List<float>();
            YDataList = new List<float>();
            xDataAccuracy = 0.5f;
            yDataAccuracy = 0.1f;

            pointsList = new List<PointF>();

            penBorder = new Pen(Color.Black, 2);
            fontBorder = new Font("Verdana", 10, FontStyle.Bold);
            borderLength = 15;
        }

        /// <summary>数据转换为待绘制区域上的点集
        /// </summary>
        /// <param name="width">待绘制区域的宽度</param>
        /// <param name="height">待绘制区域的高度</param>
        /// <returns>转换成功，返回 true</returns>
        private bool dataToPoints(int width, int height)
        {
            // 坐标起始和结束值之差小于精度范围则返回false
            if ((xEndCurrent - xStartCurrent) > xDataAccuracy &&
                (yEndCurrent - yStartCurrent) > yDataAccuracy)
            {
                if (XDataList != null)
                {
                    PointF currentPointF = new PointF();
                    for (int i = 0; i < XDataList.Count; i++)
                    {

                        // 转换为像素坐标
                        currentPointF.X = (XDataList[i] - xStartCurrent) *
                            (width - 1) / (xEndCurrent - xStartCurrent);
                        currentPointF.Y = (YDataList[i] - yStartCurrent) *
                            (height - 1) / (yEndCurrent - yStartCurrent);
                        // 装载坐标
                        pointsList.Add(currentPointF);
                    }

                    if (pointsList.Count > 0)
                    {
                        return true;
                    }
                }
            }


            return false;
        }
    }
}
