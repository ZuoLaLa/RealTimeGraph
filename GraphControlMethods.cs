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
        public void ResetDisplayRect()
        {
            graphData.DisplayRect.UpdateRect(initialRect);
        }
    }
}
