using System;
using System.Collections.Generic;

namespace RealTimeGraph
{
    public partial class GraphControl
    {
        public string GraphTitle { get; set; }
        public string AxisXTitle { get; set; }
        public string AxisYTitle { get; set; }

        /// <summary>可设定的曲线显示模式枚举类型
        /// </summary>
        public enum GraphTypes
        {
            /// <summary>全局实时显示模式
            /// </summary>
            GlobalMode,
            /// <summary>固定坐标尺度的滚动实时显示模式
            /// </summary>
            FixedMoveMode,
            /// <summary>框选放大模式
            /// </summary>
            RectZoomInMode,
            /// <summary>拖动模式
            /// </summary>
            DragMode,
        }

        private GraphTypes graphType;

        public GraphTypes GraphType
        {
            get { return graphType; }
            set
            {
                graphType = value;

                switch (graphType)
                {
                    case GraphTypes.GlobalMode:
                        isAutoMove = true;
                        isAutoScale = true;
                        break;
                    case GraphTypes.FixedMoveMode:
                        isAutoMove = true;
                        isAutoScale = false;
                        break;
                    case GraphTypes.RectZoomInMode:
                    case GraphTypes.DragMode:
                        isAutoMove = false;
                        isAutoScale = false;
                        break;
                }
            }
        }

        private DataPair dataAccuracy;
        public float XDataAccuracy
        {
            get { return dataAccuracy.X; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(
                        "The data accuracy must be greater than zero!");
                }
                else
                {
                    dataAccuracy.X = value;
                }
            }
        }

        public float YDataAccuracy
        {
            get { return dataAccuracy.Y; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(
                        "The data accuracy must be greater than zero!");
                }
                else
                {
                    dataAccuracy.Y = value;
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
