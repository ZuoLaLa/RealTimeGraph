using System;
using System.Collections.Generic;

namespace RealTimeGraph
{
    public partial class GraphControl
    {
        public string GraphTitle { get; set; }
        public string AxisXTitle { get; set; }
        public string AxisYTitle { get; set; }
        public GraphMode GraphStyle { get; set; }
        public bool IsShowGrid;
        public string MsgOutput;

        public float XDataAccuracy
        {
            get { return graphData.XDataAccuracy; }
            set { graphData.XDataAccuracy = value; }
        }

        public float YDataAccuracy
        {
            get { return graphData.YDataAccuracy; }
            set { graphData.YDataAccuracy = value; }
        }

        private DataGraph graphData;
        public DataPairLists<float> DataLists
        {
            get { return graphData.DataLists; }
            set { graphData.DataLists = value; }
        }

        // 初始状态下的 X, Y 起始和终止坐标
        private DataRect initialRect;

        public float InitialMinX
        {
            get { return initialRect.XMin; }
            set { initialRect.XMin = value; }
        }

        public float InitialMaxX
        {
            get { return initialRect.XMax; }
            set { initialRect.XMax = value; }
        }

        public float InitialMinY
        {
            get { return initialRect.YMin; }
            set { initialRect.YMin = value; }
        }

        public float InitialMaxY
        {
            get { return initialRect.YMax; }
            set { initialRect.YMax = value; }
        }
    }
}
