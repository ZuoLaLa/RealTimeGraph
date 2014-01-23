using System.Drawing;

namespace RealTimeGraph
{
    public partial class GraphControl
    {
        private Point startPoint; // 用于记录拖动操作时鼠标按下的位置

        private Point pbZoomStart; // 框选放大框的左上角位置
        private Point pbZoomEnd; // 框选放大框的右下角位置



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
