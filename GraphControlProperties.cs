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

        private float xDataAccuracy;
        public float XDataAccuracy
        {
            get { return xDataAccuracy; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(
                        "The data accuracy must be greater than zero!");
                }
                else
                {
                    xDataAccuracy = value;
                }
            }
        }

        private float yDataAccuracy;
        public float YDataAccuracy
        {
            get { return yDataAccuracy; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(
                        "The data accuracy must be greater than zero!");
                }
                else
                {
                    yDataAccuracy = value;
                }
            }
        }

        public List<float> XDataList;
        public List<float> YDataList;

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
