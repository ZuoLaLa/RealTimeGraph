using System;
using System.Collections.Generic;

namespace RealTimeGraph
{
    public partial class GraphControl
    {
        public string GraphTitle { get; set; }
        public string AxisXTitle { get; set; }
        public string AxisYTitle { get; set; }

        private GraphMode graphStyle;

        public GraphMode GraphStyle
        {
            get { return graphStyle; }
            set
            {
                graphStyle = value;

                switch (graphStyle)
                {
                    case GraphMode.GlobalMode:
                        isAutoMove = true;
                        isAutoScale = true;
                        break;
                    case GraphMode.FixMoveMode:
                        isAutoMove = true;
                        isAutoScale = false;
                        break;
                    case GraphMode.RectZoomInMode:
                    case GraphMode.DragMode:
                        isAutoMove = false;
                        isAutoScale = false;
                        break;
                }
            }
        }

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

        public List<float> XDataList
        {
            get { return graphData.XDataList; }
            set { graphData.XDataList = value; }
        }

        public List<float> YDataList
        {
            get { return graphData.YDataList; }
            set { graphData.YDataList = value; }
        }

        public string MsgOutput;

        public bool IsShowGrid;

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
