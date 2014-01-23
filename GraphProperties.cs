using System.Drawing;
using System.Drawing.Drawing2D;

namespace RealTimeGraph
{
    public class GraphProperties
    {
        public const int BORDER_LENGTH = 15;  // 坐标边界刻度线的长度
        public const int FIRST_SCALE_LENGTH = 10; // 一级坐标刻度线的长度
        public const int SECOND_SCALE_LENGTH = 8;
        public const int FIRST_SCALE_MIN_LENGTH = 100; // 一级坐标刻度线的最小间隔
        public const int SECOND_SCALE_MIN_LENGTH = 15;
        public const int CURVE_HEIGHT_PADDING = 10;

        public Pen BorderPen;    // 边界刻度所用钢笔
        public Font BorderFont;    // 边界刻度值字体
        public Pen FirstScalePen;    // 一级刻度所用钢笔
        public Font FirstScaleFont;    // 一级刻度值字体
        public Pen SecondScalePen;
        public Pen FirstGridPen;       // 一级网格所用钢笔
        public Pen SecondGridPen;
        public Font TitleFont; // 曲线标题字体
        public Font AxisTitleFont;  // 坐标轴标题字体
        public Pen CurvePen;

        public GraphProperties()
        {
            BorderPen = new Pen(Color.Black, 2);
            BorderFont = new Font("Verdana", 8, FontStyle.Bold);
            FirstScalePen = new Pen(Color.Black, 2);
            SecondScalePen = new Pen(Color.Black, 1);
            FirstScaleFont = new Font("Verdana", 8);
            FirstGridPen = new Pen(Color.FromArgb(160, Color.White), 1);
            SecondGridPen = new Pen(Color.FromArgb(60, Color.White), 1);
            TitleFont = new Font("SimHei", 14);
            AxisTitleFont = new Font("FangSong", 10);
            CurvePen = new Pen(Color.Yellow, 1);
            CurvePen.LineJoin = LineJoin.Bevel;
        }
    }
}