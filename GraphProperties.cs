using System.Drawing;

namespace RealTimeGraph
{
    public class GraphProperties
    {
        public int FirstScaleLength; // һ������̶��ߵĳ���
        public int SecondScaleLength;
        public int FirstScaleInterval; // һ������̶��ߵ���С���
        public int SecondScaleInterval;
        public int CurveHeightPadding;
        public int BorderLength;  // ����߽�̶��ߵĳ���

        public Pen BorderPen;    // �߽�̶����øֱ�
        public Font BorderFont;    // �߽�̶�ֵ����
        public Pen FirstScalePen;    // һ���̶����øֱ�
        public Font FirstScaleFont;    // һ���̶�ֵ����
        public Pen SecondScalePen;
        public Pen FirstGridPen;       // һ���������øֱ�
        public Pen SecondGridPen;
        public Font TitleFont; // ���߱�������
        public Font AxisTitleFont;  // �������������

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