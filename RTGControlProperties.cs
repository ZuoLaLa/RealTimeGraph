using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RealTimeGraph
{
    public partial class RTGControl : UserControl
    {
        private string graphTitle;
        /// <summary>
        /// 曲线标题
        /// </summary>
        public string GraphTitle
        {
            get { return graphTitle; }
            set { graphTitle = value; }
        }

        private string graphXTitle;
        /// <summary>
        /// X轴标题
        /// </summary>
        public string GraphXTitle
        {
            get { return graphXTitle; }
            set { graphXTitle = value; }
        }

        private string graphYTitle;
        /// <summary>
        /// Y轴标题
        /// </summary>
        public string GraphYTitle
        {
            get { return graphYTitle; }
            set { graphYTitle = value; }
        }

        /// <summary>
        /// 可设定的曲线显示模式类型
        /// </summary>
        public enum GraphTypes
        {
            /// <summary>
            /// 全局实时显示模式
            /// </summary>
            GlobalMode,
            /// <summary>
            /// 固定坐标尺度的滚动实时显示模式
            /// </summary>
            FixedMoveMode,
            /// <summary>
            /// 框选放大模式
            /// </summary>
            RectZoomInMode,
            /// <summary>
            /// 拖动模式
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
                    default:
                        break;
                }
            }
        }

        private float xDataAccuracy;
        /// <summary>
        /// X 数据精度
        /// </summary>
        public float XDataAccuracy
        {
            get { return xDataAccuracy; }
            set { xDataAccuracy = value; }
        }

        private float yDataAccuracy;
        /// <summary>
        /// Y 数据精度
        /// </summary>
        public float YDataAccuracy
        {
            get { return yDataAccuracy; }
            set { yDataAccuracy = value; }
        }

        public List<float> XDataList;
        public List<float> YDataList;
    }
}
