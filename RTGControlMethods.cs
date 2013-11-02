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
            penScale1 = new Pen(Color.Black, 2);
            penScale2 = new Pen(Color.Black, 1);
            fontBorder = new Font("Verdana", 10, FontStyle.Bold);
            fontScale1 = new Font("Verdana", 8);
            penGrid1 = new Pen(Color.FromArgb(160, Color.White), 1);
            penGrid2 = new Pen(Color.FromArgb(60, Color.White), 1);

            fontTitle = new Font("SimHei", 14);
            fontAxis = new Font("FangSong", 10);
            GraphTitle = "位移实时显示曲线";
            GraphXTitle = "时间(s)";
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

        /// <summary>判断坐标位置是否位于X轴可绘制区域内
        /// </summary>
        /// <param name="scalePos">x刻度坐标位置</param>
        /// <returns>若坐标位置位于X轴可绘制区域内，则返回true.</returns>
        private bool isInAxisX(float scalePos)
        {
            return scalePos > pbAxisY.Width + 1 &&
                                scalePos < pbAxisY.Width + pbCurve.Width - 1;
        }

        /// <summary>判断坐标位置是否位于Y轴可绘制区域内
        /// </summary>
        /// <param name="scalePos">Y刻度坐标位置</param>
        /// <returns>若坐标位置位于X轴可绘制区域内，则返回true.</returns>
        private bool isInAxisY(float scalePos)
        {
            return scalePos > pbAxisY.Height - pbCurve.Height + 1 &&
                                scalePos < pbAxisY.Height - 1;
        }

        /// <summary>判断纵向网格位置是否位于可绘制区域内
        /// </summary>
        /// <param name="scalePos">纵向网格位置</param>
        /// <returns>若坐标位置位于可绘制区域内，则返回true.</returns>
        private bool isInCurveX(float scalePos)
        {
            return scalePos > 0 && scalePos < pbCurve.Width - 1;
        }

        /// <summary>判断横向网格位置是否位于可绘制区域内
        /// </summary>
        /// <param name="scalePos">横向网格位置</param>
        /// <returns>若坐标位置位于可绘制区域内，则返回true.</returns>
        private bool isInCurveY(float scalePos)
        {
            return scalePos > 0 && scalePos < pbCurve.Height - 1;
        }

        /// <summary>根据画图模式和数据调整坐标显示
        /// </summary>
        private void updateAxisCurrent()
        {
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
        }
        /// <summary>根据坐标范围调整坐标刻度参数。
        /// </summary>
        private void updateAxisScale()
        {
            scaleX = pbCurve.Width / (xEndCurrent - xStartCurrent);
            getScale1Limits(xStartCurrent, xEndCurrent,
                out xScale1Min, out xScale1Max, out xScale1);
            xScale1Start = pbAxisY.Width + (xScale1Min - xStartCurrent) * scaleX;
            xScale1Num = getScaleNum(pbCurve.Width * xScale1 / (xEndCurrent - xStartCurrent),
                scale1Interval);
            xScale1Sum = (int)((xScale1Max - xScale1Min) / xScale1 * xScale1Num);
            xScale1Length = pbCurve.Width * xScale1
                / ((xEndCurrent - xStartCurrent) * xScale1Num);
            xScale2Num = getScaleNum((int)xScale1Length, scale2Interval);
            xScale2Length = xScale1Length / xScale2Num;

            scaleY = pbCurve.Height / (yEndCurrent - yStartCurrent);
            getScale1Limits(yStartCurrent, yEndCurrent, out yScale1Min,
                out yScale1Max, out yScale1);
            yScale1Start = pbAxisY.Height - (yScale1Min - yStartCurrent) * scaleY;
            yScale1Num = getScaleNum(pbCurve.Height * yScale1 / (yEndCurrent - yStartCurrent),
                scale1Interval);
            yScale1Sum = (int)(yScale1Num * (yScale1Max - yScale1Min) / yScale1);
            yScale1Length = pbCurve.Height * yScale1
                / ((yEndCurrent - yStartCurrent) * yScale1Num);
            yScale2Num = getScaleNum((int)yScale1Length, scale2Interval);
            yScale2Length = yScale1Length / yScale2Num;
        }
    }
}
