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
            XDataAccuracy = 1f;
            YDataAccuracy = 0.1f;

            pointsList = new List<PointF>();

            penBorder = new Pen(Color.Black, 2);
            fontBorder = new Font("Verdana", 10, FontStyle.Bold);
            borderLength = 15;

            fontTitle = new Font("宋体", 14, FontStyle.Bold);
            fontAxis = new Font("宋体", 10);
            GraphTitle = "位移实时显示曲线";
            GraphYTitle = "距离(mm)";

            pbZoom.BackColor = Color.FromArgb(50, 0, 64, 128);
            pbZoom.Visible = false;

            MsgOutput = "Ready";
        }

        /// <summary>数据转换为待绘制区域上的点集
        /// </summary>
        /// <param name="width">待绘制区域的宽度</param>
        /// <param name="height">待绘制区域的高度</param>
        /// <returns>转换成功，返回 true</returns>
        private bool dataToPoints(int width, int height)
        {
            // 坐标起始和结束值之差小于精度范围则返回false
            if ((xEndCurrent - xStartCurrent) > 0.9F * XDataAccuracy ||
                (yEndCurrent - yStartCurrent) > 0.9F * YDataAccuracy)
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
        private bool pointToData(Point p, out float x, out float y)
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
                x = 0;
                y = 0;
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
        private bool pointToData(float pX, float pY, out float x, out float y)
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
                x = 0;
                y = 0;
                return false;
            }
        }

        /// <summary>将矩形区域的坐标点转换成对应的数据对。
        /// 用于框选放大模式，也可变形用于拖动模式。
        /// </summary>
        /// <param name="pLX">矩形区域的左上角点 X 像素值</param>
        /// <param name="pUY">矩形区域的左上角点 Y 像素值</param>
        /// <param name="pRX">矩形区域的右下角点 X 像素值</param>
        /// <param name="pDY">矩形区域的右下角点 Y 像素值</param>
        /// <param name="xL">左上角点转换后的 X 值</param>
        /// <param name="yU">左上角点转换后的 Y 值</param>
        /// <param name="xR">右下角点转换后的 X 值</param>
        /// <param name="yD">右下角点转换后的 Y 值</param>
        private void rectPointsToData(float pLX, float pUY, float pRX, float pDY,
            out float xL, out float yU, out float xR, out float yD)
        {
            xL = xStartCurrent +
                pLX * (xEndCurrent - xStartCurrent) / (pbCurve.Width - 1);
            yU = yEndCurrent -
                pUY * (yEndCurrent - yStartCurrent) / (pbCurve.Height - 1);
            xR = xStartCurrent +
                pRX * (xEndCurrent - xStartCurrent) / (pbCurve.Width - 1);
            yD = yEndCurrent -
                pDY * (yEndCurrent - yStartCurrent) / (pbCurve.Height - 1);
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
        /// <summary>设置坐标轴范围
        /// </summary>
        /// <param name="xS">更新后的 X 轴左端点</param>
        /// <param name="xE">更新后的 X 轴右端点</param>
        /// <param name="yS">更新后的 Y 轴下端点</param>
        /// <param name="yE">更新后的 Y 轴上端点</param>
        public void ResetAxis(float xS, float xE, float yS, float yE)
        {
            xStartCurrent = xS;
            xEndCurrent = xE;
            yStartCurrent = yS;
            yEndCurrent = yE;
        }
        /// <summary>重置 X 轴为初始数据宽度（即显示多少个数据点）
        /// </summary>
        public void ResetAxisXWidth()
        {
            xStartCurrent = xEndCurrent - (xEndInitial - xStartInitial);
        }

        private void dragMove(float xD, float yD)
        {
            float xM = xD * (xEndCurrent - xStartCurrent) / (pbCurve.Width - 1);
            float yM = yD * (yEndCurrent - yStartCurrent) / (pbCurve.Height - 1);

            xStartCurrent -= xM;
            xEndCurrent -= xM;
            yStartCurrent += yM;
            yEndCurrent += yM;
        }

        private void rectZoomIn()
        {
            float xL, yU, xR, yD;
            rectPointsToData(pbZoom.Location.X, pbZoom.Location.Y,
                pbZoom.Location.X + pbZoom.Width, pbZoom.Location.Y + pbZoom.Height,
                out xL, out yU, out xR, out yD);

            if ((xR - xL) >= XDataAccuracy && (yU - yD) >= YDataAccuracy)
            {
                ResetAxis(xL, xR, yD, yU);
            }
            else if ((xR - xL) >= XDataAccuracy)
            {
                ResetAxis(xL, xR, (yD + yU - YDataAccuracy) / 2F,
                    (yD + yU + YDataAccuracy) / 2F);
                MsgOutput = "Zoom in to the Y data accuracy";
            }
            else if ((yU - yD) >= YDataAccuracy)
            {
                ResetAxis((xL + xR - XDataAccuracy) / 2F,
                    (xL + xR + XDataAccuracy) / 2F, yD, yU);
                MsgOutput = "Zoom in to the X data accuracy";
            }
            else
            {
                ResetAxis((xL + xR - XDataAccuracy) / 2F,
                    (xL + xR + XDataAccuracy) / 2F,
                    (yD + yU - YDataAccuracy) / 2F,
                    (yD + yU + YDataAccuracy) / 2F);
                MsgOutput = "Zoom in to all data accuracy";
            }
        }

        public void UpdateDataLimits(float xMin, float xMax, float yMin, float yMax)
        {
            xDataMin = xMin;
            xDataMax = xMax;
            yDataMin = yMin;
            yDataMax = yMax;
        }
    }
}
