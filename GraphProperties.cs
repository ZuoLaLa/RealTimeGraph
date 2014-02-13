using System.Drawing;
using System.Drawing.Drawing2D;

namespace RealTimeGraph
{
    public class GraphProperties
    {
        public const int BORDER_LENGTH = 15;  // ����߽�̶��ߵĳ���
        public const int FIRST_SCALE_LENGTH = 10; // һ������̶��ߵĳ���
        public const int SECOND_SCALE_LENGTH = 8;
        public const int FIRST_SCALE_MIN_INTERVAL = 100; // һ������̶��ߵ���С���
        public const int SECOND_SCALE_MIN_INTERVAL = 15;
        public const int CURVE_HEIGHT_PADDING = 10;

        public Pen BorderPen;    // �߽�̶����øֱ�
        public Font BorderFont;    // �߽�̶�ֵ����
        public Pen FirstScalePen;    // һ���̶����øֱ�
        public Font FirstScaleFont;    // һ���̶�ֵ����
        public Pen SecondScalePen;
        public Pen FirstGridPen;       // һ���������øֱ�
        public Pen SecondGridPen;
        public Font TitleFont; // ���߱�������
        public Font AxisTitleFont;  // �������������
        public Pen[] CurvePens;

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
            CurvePens = new Pen[]
            {
                new Pen(Color.Yellow, 1),
                new Pen(Color.RoyalBlue,1), 
                new Pen(Color.Green,1), 
            };
            foreach (Pen curvePen in CurvePens)
            {
                curvePen.LineJoin = LineJoin.Round;
            }
        }
    }
}