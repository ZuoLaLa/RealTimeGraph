using System.Drawing;

namespace RealTimeGraph
{
    public partial class GraphControl
    {
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

        private Size drawAreaSize;
        private GraphProperties graphProperties;
    }
}
