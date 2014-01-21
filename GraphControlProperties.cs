using System.Collections.Generic;

namespace RealTimeGraph
{
    public partial class GraphControl
    {
        /// <summary>曲线标题
        /// </summary>
        public string GraphTitle { private get; set; }

        /// <summary>X轴标题
        /// </summary>
        public string GraphXTitle { private get; set; }

        /// <summary>Y轴标题
        /// </summary>
        public string GraphYTitle { private get; set; }

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

        private float xDataAccuracy;
        /// <summary>X 数据精度
        /// </summary>
        public float XDataAccuracy
        {
            private get { return xDataAccuracy; }
            set {
                xDataAccuracy = (value > 0) ? value : X_DATA_ACCURACY_DEFAULT;
            }
        }

        private float yDataAccuracy;
        /// <summary>Y 数据精度
        /// </summary>
        public float YDataAccuracy
        {
            private get { return yDataAccuracy; }
            set {
                yDataAccuracy = (value > 0) ? value : Y_DATA_ACCURACY_DEFAULT;
            }
        }

        public List<float> XDataList;
        public List<float> YDataList;

        public string MsgOutput;

        public bool ShowGrid;

        // 初始状态下的 X, Y 起始和终止坐标
        public float XStartInitial { private get; set; }
        public float XEndInitial { private get; set; }
        public float YStartInitial { private get; set; }
        public float YEndInitial { private get; set; }
    }
}
