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
            yEndInitial = 200;

            ResetAxis();

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

            fontTitle = new Font("宋体", 14, FontStyle.Bold);
            fontAxis = new Font("宋体", 10);
            GraphTitle = "位移实时显示曲线";
            GraphYTitle = "距离(mm)";
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
        /// <summary>将显示画布上的一个坐标点转化成相应的数据
        /// </summary>
        /// <param name="p">待转换的坐标点</param>
        /// <param name="x">转换后的 X 值</param>
        /// <param name="y">转换后的 Y 值</param>
        /// <returns></returns>
        private bool pointToData(Point p, ref float x, ref float y)
        {
            try
            {
                x = xStartCurrent +
                    p.X * (xEndCurrent - xStartCurrent) / (pbCurve.Width - 1);
                // Y 轴坐标点是从上到下的
                y = yEndCurrent -
                    p.Y * (yEndCurrent - yStartCurrent) / (pbCurve.Height - 1);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>将显示画布上的一个坐标点转化成相应的数据
        /// </summary>
        /// <param name="pX">待转换的坐标点 X 像素值</param>
        /// <param name="pY">待转换的坐标点 Y 像素值</param>
        /// <param name="x">转换后的 X 值</param>
        /// <param name="y">转换后的 Y 值</param>
        /// <returns></returns>
        private bool pointToData(float pX, float pY, ref float x, ref float y)
        {
            try
            {
                x = xStartCurrent +
                    pX * (xEndCurrent - xStartCurrent) / (pbCurve.Width - 1);
                // Y 轴坐标点是从上到下的
                y = yEndCurrent -
                    pY * (yEndCurrent - yStartCurrent) / (pbCurve.Height - 1);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>将矩形区域的坐标点转换成对应的数据对。
        /// 用于框选放大模式，也可变形用于拖动模式。
        /// </summary>
        /// <param name="pS">矩形区域的左上角点</param>
        /// <param name="pE">矩形区域的右下角点</param>
        /// <param name="xS">左上角点转换后的 X 值</param>
        /// <param name="yS">左上角点转换后的 Y 值</param>
        /// <param name="xE">右下角点转换后的 X 值</param>
        /// <param name="yE">右下角点转换后的 Y 值</param>
        /// <returns></returns>
        private bool rectPointsToData(Point pS, Point pE,
            ref float xS, ref float yS, ref float xE, ref float yE)
        {
            try
            {
                // 建立几个临时变量，防止转换过程中对 XStartCurrent 等修改
                float xST, yST, xET, yET;

                xST = xStartCurrent +
                    pS.X * (xEndCurrent - xStartCurrent) / (pbCurve.Width - 1);
                yST = yEndCurrent -
                    pS.Y * (yEndCurrent - yStartCurrent) / (pbCurve.Height - 1);
                xET = xStartCurrent +
                    pE.X * (xEndCurrent - xStartCurrent) / (pbCurve.Width - 1);
                yET = yEndCurrent -
                    pE.Y * (yEndCurrent - yStartCurrent) / (pbCurve.Height - 1);

                xS = xST;
                yS = yST;
                xE = xET;
                yE = yET;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>将矩形区域的坐标点转换成对应的数据对。
        /// 用于框选放大模式，也可变形用于拖动模式。
        /// </summary>
        /// <param name="pSX">矩形区域的左上角点 X 像素值</param>
        /// <param name="pSY">矩形区域的左上角点 Y 像素值</param>
        /// <param name="pEX">矩形区域的右下角点 X 像素值</param>
        /// <param name="pEY">矩形区域的右下角点 Y 像素值</param>
        /// <param name="xS">左上角点转换后的 X 值</param>
        /// <param name="yS">左上角点转换后的 Y 值</param>
        /// <param name="xE">右下角点转换后的 X 值</param>
        /// <param name="yE">右下角点转换后的 Y 值</param>
        /// <returns></returns>
        private bool rectPointsToData(float pSX, float pSY, float pEX, float pEY,
            ref float xS, ref float yS, ref float xE, ref float yE)
        {
            try
            {
                // 建立几个临时变量，防止转换过程中对 XStartCurrent 等修改
                float xST, yST, xET, yET;

                xST = xStartCurrent +
                    pSX * (xEndCurrent - xStartCurrent) / (pbCurve.Width - 1);
                yST = yEndCurrent -
                    pSY * (yEndCurrent - yStartCurrent) / (pbCurve.Height - 1);
                xET = xStartCurrent +
                    pEX * (xEndCurrent - xStartCurrent) / (pbCurve.Width - 1);
                yET = yEndCurrent -
                    pEY * (yEndCurrent - yStartCurrent) / (pbCurve.Height - 1);

                xS = xST;
                yS = yST;
                xE = xET;
                yE = yET;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>清空显示的曲线
        /// </summary>
        public void GraphClear()
        {
            XDataList.Clear();
            YDataList.Clear();
            pbCurve.Refresh();
        }
        /// <summary>重置坐标轴，回到初始设置。
        /// </summary>
        public void ResetAxis()
        {
            xStartCurrent = xStartInitial;
            xEndCurrent = xEndInitial;
            yStartCurrent = yStartInitial;
            yEndCurrent = yEndInitial;
        }
        /// <summary>重置 X 轴为初始数据宽度（即显示多少个数据点）
        /// </summary>
        public void ResetAxisXWidth()
        {
            xStartCurrent = xEndCurrent - (xEndInitial - xStartInitial);
        }
    }
}
