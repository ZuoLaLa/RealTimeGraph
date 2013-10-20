using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RealTimeGraph
{
    public partial class RTGControl : UserControl
    {
        private string graphTitle = "";
        /// <summary>
        /// 曲线标题
        /// </summary>
        public string GraphTitle
        {
            get { return graphTitle; }
            set { graphTitle = value; }
        }

        private string graphXTitle = "Time";
        /// <summary>
        /// X轴标题
        /// </summary>
        public string GraphXTitle
        {
            get { return graphXTitle; }
            set { graphXTitle = value; }
        }

        private string graphYTitle = "";
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
            // 全局实时显示模式
            GlobalMode,
            // 固定坐标尺度的滚动实时显示模式
            FixedMoveMode,
            // 框选放大模式
            ZoomInMode,
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
                    case GraphTypes.ZoomInMode:
                        isAutoMove = false;
                        isAutoScale = false;
                        break;
                    default:
                        break;
                }
            }
        }

        public List<float> XDataList;
        public List<float> YDataList;
    }
}
