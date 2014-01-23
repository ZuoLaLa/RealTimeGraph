using System.Drawing;

namespace RealTimeGraph
{
    public partial class GraphControl
    {
        private Point startPoint; // 用于记录拖动操作时鼠标按下的位置

        private Point pbZoomStart; // 框选放大框的左上角位置
        private Point pbZoomEnd; // 框选放大框的右下角位置

        private Size drawAreaSize;
        private GraphProperties graphProperties;
    }
}
