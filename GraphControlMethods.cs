using System;
using System.Collections.Generic;
using System.Drawing;

namespace RealTimeGraph
{
    public partial class GraphControl
    {
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
        public void SetDisplayRect()
        {
            graphData.DisplayRect.UpdateRect(initialRect);
        }

        /// <summary>更新数据最值。
        /// </summary>
        /// <param name="xMin">外部数据 X 的最小值</param>
        /// <param name="xMax">外部数据 X 的最大值</param>
        /// <param name="yMin">外部数据 Y 的最小值</param>
        /// <param name="yMax">外部数据 Y 的最大值</param>
        public void UpdateDataRect(float xMin, float xMax, float yMin, float yMax)
        {
            dataRect.UpdateRect(xMin, xMax, yMin, yMax);
        }
    }
}
