using System;
using System.Collections.Generic;
using System.Drawing;

namespace RealTimeGraph
{
    public partial class GraphControl
    {
        /// <summary>初始化控件参数
        /// </summary>
        private void InitialGraph()
        {
            graphProperties = new GraphProperties();
            initialRect = new DataRect();
            dispalyRect = new DataRect();
            dataRect = new DataRect();

            XDataAccuracy = GraphProperties.DEFAULT_DATA_X_ACCURACY;
            YDataAccuracy = GraphProperties.DEFAULT_DATA_Y_ACCURACY;


            // 默认初始处于滚动模式
            GraphStyle = GraphMode.FixMoveMode;
            isAutoMove = true;
            isAutoScale = false;

            XDataList = new List<float>();
            YDataList = new List<float>();

            pointsList = new List<PointF>();



            IsShowGrid = false;


            GraphTitle = "位移实时显示曲线";
            AxisXTitle = "时间(s)";
            AxisYTitle = "位移(mm)";

            pbZoom.BackColor = Color.FromArgb(50, 0, 64, 128);
            pbZoom.Visible = false;

            MsgOutput = "Ready";
        }
        /// <summary>数据转换为待绘制区域上的点集
        /// </summary>
        /// <param name="width">待绘制区域的宽度</param>
        /// <param name="height">待绘制区域的高度</param>
        /// <returns>转换成功，返回 true</returns>
        private bool DataToPoints(int width, int height)
        {
            // 坐标起始和结束值之差小于精度范围则返回false
            if (dispalyRect.XRange > 0.9F * XDataAccuracy ||
                dispalyRect.YRange > 0.9F * YDataAccuracy)
            {
                if (XDataList != null)
                {
                    PointF currentPointF = new PointF();
                    for (int i = 0; i < XDataList.Count; i++)
                    {

                        // 转换为像素坐标
                        currentPointF.X = (XDataList[i] - dispalyRect.XMin) *
                            (width - 1) / dispalyRect.XRange;
                        currentPointF.Y = (YDataList[i] - dispalyRect.YMin) *
                            (height - 1) / dispalyRect.YRange;
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

        /// <summary>将矩形区域的坐标点转换成对应的数据对。
        /// 用于框选放大模式，也可变形用于拖动模式。
        /// </summary>
        /// <param name="pLx">矩形区域的左上角点 X 像素值</param>
        /// <param name="pUy">矩形区域的左上角点 Y 像素值</param>
        /// <param name="pRx">矩形区域的右下角点 X 像素值</param>
        /// <param name="pDy">矩形区域的右下角点 Y 像素值</param>
        /// <param name="xL">左上角点转换后的 X 值</param>
        /// <param name="yU">左上角点转换后的 Y 值</param>
        /// <param name="xR">右下角点转换后的 X 值</param>
        /// <param name="yD">右下角点转换后的 Y 值</param>
        private void RectPointsToData(float pLx, float pUy, float pRx, float pDy,
            out float xL, out float yU, out float xR, out float yD)
        {
            xL = dispalyRect.XMin +
                pLx * dispalyRect.XRange / (pbCurve.Width - 1);
            yU = dispalyRect.YMax -
                pUy * dispalyRect.YRange / (pbCurve.Height - 1);
            xR = dispalyRect.XMin +
                pRx * dispalyRect.XRange / (pbCurve.Width - 1);
            yD = dispalyRect.YMax -
                pDy * dispalyRect.YRange / (pbCurve.Height - 1);
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
        public void ResetDisplayRect()
        {
            dispalyRect.UpdateRect(initialRect);
        }
        /// <summary>设置坐标轴范围
        /// </summary>
        /// <param name="xS">更新后的 X 轴左端点</param>
        /// <param name="xE">更新后的 X 轴右端点</param>
        /// <param name="yS">更新后的 Y 轴下端点</param>
        /// <param name="yE">更新后的 Y 轴上端点</param>
        private void ResetDisplayRect(float xS, float xE, float yS, float yE)
        {
            dispalyRect.UpdateRect(xS, xE, yS, yE);
        }

        /// <summary>曲线拖动
        /// </summary>
        /// <param name="xD">X 轴方向上的拖动量</param>
        /// <param name="yD">Y 轴方向上的拖动量</param>
        private void DragMove(float xD, float yD)
        {
            float xM = xD * dispalyRect.XRange / (pbCurve.Width - 1);
            float yM = yD * dispalyRect.YRange / (pbCurve.Height - 1);
            dispalyRect = new DataRect(
                dispalyRect.XMin - xM, dispalyRect.XMax - xM,
                dispalyRect.YMin + yM, dispalyRect.YMax + yM);
        }
        /// <summary>矩形框选放大
        /// </summary>
        private void RectZoomIn()
        {
            float xL, yU, xR, yD;
            RectPointsToData(pbZoom.Location.X, pbZoom.Location.Y,
                pbZoom.Location.X + pbZoom.Width, pbZoom.Location.Y + pbZoom.Height,
                out xL, out yU, out xR, out yD);

            if ((xR - xL) >= XDataAccuracy && (yU - yD) >= YDataAccuracy)
            {
                ResetDisplayRect(xL, xR, yD, yU);
            }
            else if ((xR - xL) >= XDataAccuracy)
            {
                ResetDisplayRect(xL, xR, (yD + yU - YDataAccuracy) / 2F,
                    (yD + yU + YDataAccuracy) / 2F);
                MsgOutput = "Zoom in to the Y data accuracy";
            }
            else if ((yU - yD) >= YDataAccuracy)
            {
                ResetDisplayRect((xL + xR - XDataAccuracy) / 2F,
                    (xL + xR + XDataAccuracy) / 2F, yD, yU);
                MsgOutput = "Zoom in to the X data accuracy";
            }
            else
            {
                ResetDisplayRect((xL + xR - XDataAccuracy) / 2F,
                    (xL + xR + XDataAccuracy) / 2F,
                    (yD + yU - YDataAccuracy) / 2F,
                    (yD + yU + YDataAccuracy) / 2F);
                MsgOutput = "Zoom in to all data accuracy";
            }
        }
        /// <summary>更新数据最值。
        /// </summary>
        /// <param name="xMin">外部数据 X 的最小值</param>
        /// <param name="xMax">外部数据 X 的最大值</param>
        /// <param name="yMin">外部数据 Y 的最小值</param>
        /// <param name="yMax">外部数据 Y 的最大值</param>
        public void UpdateDataLimits(float xMin, float xMax, float yMin, float yMax)
        {
            dataRect.UpdateRect(xMin, xMax, yMin, yMax);
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
        private bool IsInAxisX(float scalePos)
        {
            return scalePos > pbAxisY.Width + 1 &&
                                scalePos < pbAxisY.Width + pbCurve.Width - 1;
        }
        /// <summary>判断坐标位置是否位于Y轴可绘制区域内
        /// </summary>
        /// <param name="scalePos">Y刻度坐标位置</param>
        /// <returns>若坐标位置位于X轴可绘制区域内，则返回true.</returns>
        private bool IsInAxisY(float scalePos)
        {
            return scalePos > pbAxisY.Height - pbCurve.Height + graphProperties.CurveHeightPadding + 1 &&
                                scalePos < pbAxisY.Height - graphProperties.CurveHeightPadding - 1;
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
        /// <summary>根据画图模式和数据调整坐标显示
        /// </summary>
        private void UpdateAxisCurrent()
        {
            if (XDataList != null)
            {
                if (isAutoMove)
                {
                    if (isAutoScale)    // 此即为 GlobalMode 模式
                    {
                        dispalyRect.XMin = (dataRect.XMin < InitialMinX)
                            ? dataRect.XMin : InitialMinX;
                        dispalyRect.XMax = (dataRect.XMax > InitialMaxX)
                            ? dataRect.XMax : InitialMaxX;
                        dispalyRect.YMin = (dataRect.YMin < InitialMinY)
                            ? dataRect.YMin : InitialMinY;
                        dispalyRect.YMax = (dataRect.YMax > InitialMaxY)
                            ? dataRect.YMax : InitialMaxY;
                    }
                    else    // 此即为 FixedMoveMode 模式
                    {
                        if (dataRect.XMax > dispalyRect.XMax)
                        {
                            dispalyRect.XMin += dataRect.XMax - dispalyRect.XMax;
                            dispalyRect.XMax = dataRect.XMax;
                        }

                        dispalyRect.YMin = (dataRect.YMin < dispalyRect.YMin)
                            ? dataRect.YMin : dispalyRect.YMin;
                        dispalyRect.YMax = (dataRect.YMax > dispalyRect.YMax)
                            ? dataRect.YMax : dispalyRect.YMax;
                    }
                }
            }
        }
        /// <summary>根据坐标范围调整坐标刻度参数。
        /// </summary>
        private void UpdateAxisScale()
        {
            scaleX = curveWidth / dispalyRect.XRange;
            getScale1Limits(dispalyRect.XMin, dispalyRect.XMax,
                out xScale1Min, out xScale1Max, out xScale1);
            xScale1Num = getScaleNum(curveWidth * xScale1 / dispalyRect.XRange,
                graphProperties.FirstScaleInterval);
            xScale1Sum = (int)((xScale1Max - xScale1Min) / xScale1 * xScale1Num);
            xScale1Length = curveWidth * xScale1
                / (dispalyRect.XRange * xScale1Num);
            xScale2Num = getScaleNum(xScale1Length, graphProperties.SecondScaleInterval);
            xScale2Length = xScale1Length / xScale2Num;

            scaleY = curveHeight / dispalyRect.YRange;
            getScale1Limits(dispalyRect.YMin, dispalyRect.YMax, out yScale1Min,
                out yScale1Max, out yScale1);
            yScale1Num = getScaleNum(curveHeight * yScale1 / dispalyRect.YRange,
                graphProperties.FirstScaleInterval);
            yScale1Sum = (int)(yScale1Num * (yScale1Max - yScale1Min) / yScale1);
            yScale1Length = curveHeight * yScale1
                / (dispalyRect.YRange * yScale1Num);
            yScale2Num = getScaleNum(yScale1Length, graphProperties.SecondScaleInterval);
            yScale2Length = yScale1Length / yScale2Num;
        }
    }
}
