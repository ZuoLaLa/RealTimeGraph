using System.Drawing;

namespace RealTimeGraph
{
    public class GraphProperties
    {
        public int FirstScaleLength; // 一级坐标刻度线的长度
        public int SecondScaleLength;
        public int FirstScaleInterval; // 一级坐标刻度线的最小间隔
        public int SecondScaleInterval;
        public int CurveHeightPadding;
        public int BorderLength;  // 坐标边界刻度线的长度

        public Pen BorderPen;    // 边界刻度所用钢笔
        public Font BorderFont;    // 边界刻度值字体
        public Pen FirstScalePen;    // 一级刻度所用钢笔
        public Font FirstScaleFont;    // 一级刻度值字体
        public Pen SecondScalePen;
        public Pen FirstGridPen;       // 一级网格所用钢笔
        public Pen SecondGridPen;
        public Font TitleFont; // 曲线标题字体
        public Font AxisTitleFont;  // 坐标轴标题字体

        public GraphProperties()
        {
            FirstScaleLength = 10;
            SecondScaleLength = 8;
            FirstScaleInterval = 100;
            SecondScaleInterval = 15;
            CurveHeightPadding = 10;
            BorderLength = 15;

            BorderPen = new Pen(Color.Black, 2);
            BorderFont = new Font("Verdana", 8, FontStyle.Bold);
            FirstScalePen = new Pen(Color.Black, 2);
            SecondScalePen = new Pen(Color.Black, 1);
            FirstScaleFont = new Font("Verdana", 8);
            FirstGridPen = new Pen(Color.FromArgb(160, Color.White), 1);
            SecondGridPen = new Pen(Color.FromArgb(60, Color.White), 1);
            TitleFont = new Font("SimHei", 14);
            AxisTitleFont = new Font("FangSong", 10);
        }

        public const float DEFAULT_DATA_X_ACCURACY = 1F;
        public const float DEFAULT_DATA_Y_ACCURACY = 0.1F;
    }
}