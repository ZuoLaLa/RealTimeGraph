using System.Collections.Generic;
using System.Drawing;

namespace RealTimeGraph
{
    public partial class GraphControl
    {
        // 当前显示波形的数据范围
        private DataRect dispalyRect;

        // 所有数据的范围
        private DataRect dataRect;

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


        private Point startPoint; // 用于记录拖动操作时鼠标按下的位置

        private Point pbZoomStart; // 框选放大框的左上角位置
        private Point pbZoomEnd; // 框选放大框的右下角位置

        float scaleX;       // X 轴比例尺（单位长度的像素数）
        float scaleY;

        float xScale1Min;   // X 轴上一级刻度的最小值
        float xScale1Max;
        float xScale1;      // X 轴权值
        int xScale1Num;      // 单位权值内的一级刻度划分数
        int xScale1Sum;      // 一级刻度划分总数
        float xScale1Length;// 一级刻度间隔
        int xScale2Num;      // 一级刻度内的二级刻度划分数
        float xScale2Length;

        float yScale1Min;
        float yScale1Max;
        float yScale1;
        int yScale1Num;
        int yScale1Sum;
        float yScale1Length;
        int yScale2Num;
        float yScale2Length;

        private int curveHeight;
        private int curveWidth;

        private GraphProperties graphProperties;
    }
}
