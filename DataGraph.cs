using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace RealTimeGraph
{
    class DataGraph
    {
        public const float DEFAULT_DATA_X_ACCURACY = 1F;
        public const float DEFAULT_DATA_Y_ACCURACY = 0.1F;
        public List<float> XDataList { get; set; }
        public List<float> YDataList { get; set; }

        private float xDataAccuracy;
        public float XDataAccuracy
        {
            get { return xDataAccuracy; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(
                        "The data accuracy must be greater than zero!");
                }
                else
                {
                    xDataAccuracy = value;
                }
            }
        }

        private float yDataAccuracy;
        public float YDataAccuracy
        {
            get { return yDataAccuracy; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(
                        "The data accuracy must be greater than zero!");
                }
                else
                {
                    yDataAccuracy = value;
                }
            }
        }

        /// <summary>待绘制的数据点集
        /// </summary>
        private List<PointF> pointsList;

        // 当前显示波形的数据范围
        public DataRect DisplayRect;

        public DataGraph()
        {
            XDataAccuracy = DEFAULT_DATA_X_ACCURACY;
            YDataAccuracy = DEFAULT_DATA_Y_ACCURACY;
            XDataList = new List<float>();
            YDataList = new List<float>();
            pointsList = new List<PointF>();
            DisplayRect = new DataRect();
        }

        /// <summary>数据转换为待绘制区域上的点集
        /// </summary>
        /// <param name="width">待绘制区域的宽度</param>
        /// <param name="height">待绘制区域的高度</param>
        /// <returns>转换成功，返回 true</returns>
        private bool GetPointsToDrawIn(int width, int height)
        {
            // 坐标起始和结束值之差小于精度范围则返回false
            if (DisplayRect.XRange > 0.9F * XDataAccuracy ||
                DisplayRect.YRange > 0.9F * YDataAccuracy)
            {
                if (XDataList != null)
                {
                    PointF currentPointF = new PointF();
                    for (int i = 0; i < XDataList.Count; i++)
                    {

                        // 转换为像素坐标
                        currentPointF.X = (XDataList[i] - DisplayRect.XMin) *
                            (width - 1) / DisplayRect.XRange;
                        currentPointF.Y = (YDataList[i] - DisplayRect.YMin) *
                            (height - 1) / DisplayRect.YRange;
                        // 装载坐标
                        pointsList.Add(currentPointF);
                    }

                    if (pointsList.Count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void DrawCurve(Graphics g, int curveWidth, int curveHeight)
        {
            pointsList.Clear();
            if (GetPointsToDrawIn(curveWidth, curveHeight))
            {
                if (pointsList.Count > 1)
                {
                    Pen p = new Pen(Color.Yellow, 1);
                    p.LineJoin = LineJoin.Bevel;
                    g.DrawLines(p, pointsList.ToArray());
                    p.Dispose();
                }
            }
        }
    }
}
