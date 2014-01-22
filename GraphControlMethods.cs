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
            graphData = new DataGraph();
            initialRect = new DataRect();
            dataRect = new DataRect();

            // 默认初始处于滚动模式
            GraphStyle = GraphMode.FixMoveMode;

            IsShowGrid = false;

            GraphTitle = "位移实时显示曲线";
            AxisXTitle = "时间(s)";
            AxisYTitle = "位移(mm)";

            pbZoom.BackColor = Color.FromArgb(50, 0, 64, 128);
            pbZoom.Visible = false;

            MsgOutput = "Ready";
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
            graphData.DisplayRect.UpdateRect(initialRect);
        }
        /// <summary>设置坐标轴范围
        /// </summary>
        /// <param name="xS">更新后的 X 轴左端点</param>
        /// <param name="xE">更新后的 X 轴右端点</param>
        /// <param name="yS">更新后的 Y 轴下端点</param>
        /// <param name="yE">更新后的 Y 轴上端点</param>
        private void ResetDisplayRect(float xS, float xE, float yS, float yE)
        {
            graphData.DisplayRect.UpdateRect(xS, xE, yS, yE);
        }

        private void ResetDisplayRect(DataRect newRect)
        {
            graphData.DisplayRect.UpdateRect(newRect);
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
                if (GraphStyle== GraphMode.GlobalMode)
                {
                    graphData.DisplayRect.XMin = (dataRect.XMin < InitialMinX)
                            ? dataRect.XMin : InitialMinX;
                    graphData.DisplayRect.XMax = (dataRect.XMax > InitialMaxX)
                        ? dataRect.XMax : InitialMaxX;
                    graphData.DisplayRect.YMin = (dataRect.YMin < InitialMinY)
                        ? dataRect.YMin : InitialMinY;
                    graphData.DisplayRect.YMax = (dataRect.YMax > InitialMaxY)
                        ? dataRect.YMax : InitialMaxY;
                }
                else if (GraphStyle== GraphMode.FixMoveMode)
                {
                    if (dataRect.XMax > graphData.DisplayRect.XMax)
                    {
                        graphData.DisplayRect.XMin += dataRect.XMax - graphData.DisplayRect.XMax;
                        graphData.DisplayRect.XMax = dataRect.XMax;
                    }

                    graphData.DisplayRect.YMin = (dataRect.YMin < graphData.DisplayRect.YMin)
                        ? dataRect.YMin : graphData.DisplayRect.YMin;
                    graphData.DisplayRect.YMax = (dataRect.YMax > graphData.DisplayRect.YMax)
                        ? dataRect.YMax : graphData.DisplayRect.YMax;
                }
            }
        }
        /// <summary>根据坐标范围调整坐标刻度参数。
        /// </summary>
        private void UpdateAxisScale()
        {
            scaleX = curveWidth / graphData.DisplayRect.XRange;
            getScale1Limits(graphData.DisplayRect.XMin, graphData.DisplayRect.XMax,
                out xScale1Min, out xScale1Max, out xScale1);
            xScale1Num = getScaleNum(curveWidth * xScale1 / graphData.DisplayRect.XRange,
                graphProperties.FirstScaleInterval);
            xScale1Sum = (int)((xScale1Max - xScale1Min) / xScale1 * xScale1Num);
            xScale1Length = curveWidth * xScale1
                / (graphData.DisplayRect.XRange * xScale1Num);
            xScale2Num = getScaleNum(xScale1Length, graphProperties.SecondScaleInterval);
            xScale2Length = xScale1Length / xScale2Num;

            scaleY = curveHeight / graphData.DisplayRect.YRange;
            getScale1Limits(graphData.DisplayRect.YMin, graphData.DisplayRect.YMax, out yScale1Min,
                out yScale1Max, out yScale1);
            yScale1Num = getScaleNum(curveHeight * yScale1 / graphData.DisplayRect.YRange,
                graphProperties.FirstScaleInterval);
            yScale1Sum = (int)(yScale1Num * (yScale1Max - yScale1Min) / yScale1);
            yScale1Length = curveHeight * yScale1
                / (graphData.DisplayRect.YRange * yScale1Num);
            yScale2Num = getScaleNum(yScale1Length, graphProperties.SecondScaleInterval);
            yScale2Length = yScale1Length / yScale2Num;
        }
    }
}
