using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace RealTimeGraph
{
    public partial class RTGControl : UserControl
    {
        // 初始状态下的 X, Y 起始和终止坐标
        private float xStartInitial;
        private float xEndInitial;
        private float yStartInitial;
        private float yEndInitial;

        // 当前显示波形的 X, Y 起始和终止坐标
        private float xStartCurrent;
        private float xEndCurrent;
        private float yStartCurrent;
        private float yEndCurrent;

        /// <summary>波形是否随时间移动（即是否处于实时显示状态）。
        /// 设为 false 时用于“框选放大模式”，
        /// 设为 true 时用于“全局”和“滚动”实时显示模式。
        /// </summary>
        private bool isAutoMove;

        /// <summary>设置波形是否实时调整坐标尺度范围。
        /// 设为 false 时用于固定坐标尺度的“滚动实时显示”模式，
        /// 设为 true 时用于显示所有数据点的“全局实时显示”模式。
        /// 当 isAutoMove=false 时，isAutoScale 无效。
        /// </summary>
        private bool isAutoScale;

        /// <summary>待绘制的数据点集
        /// </summary>
        private List<PointF> pointsList;

        Pen penBorder;    // 边界刻度所用钢笔
        Font fontBorder;    // 边界刻度值字体
        private int borderLength;  // 坐标边界刻度线的长度

        private Font fontTitle; // 曲线标题字体
        private Font fontAxis;  // 坐标轴标题字体

        private Point startPoint; // 用于记录拖动操作时鼠标按下的位置
    }
}
