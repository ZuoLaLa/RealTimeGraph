using System;
using System.Collections.Generic;
using System.Drawing;

namespace RealTimeGraph
{
    class DataGraph
    {
        public DataPairLists<float> DataLists { get; set; }

        /// <summary>
        /// 当前显示波形的数据范围
        /// </summary>
        public DataRect DisplayRect;

        public decimal DisplayWeightX
        {
            get { return DisplayRect.WeightX; }
        }

        public decimal DisplayWeightY
        {
            get { return DisplayRect.WeightY; }
        }

        /// <summary>根据画图模式和数据调整坐标显示
        /// </summary>
        public void UpdateDisplayRect(DataRect initialRect, GraphMode graphStyle)
        {
            if (DataLists.HasData())
            {
                if (graphStyle == GraphMode.GlobalMode)
                {
                    DisplayRect.XMin = (DataLists.MinX < initialRect.XMin)
                        ? DataLists.MinX : initialRect.XMin;
                    DisplayRect.XMax = (DataLists.MaxX > initialRect.XMax)
                        ? DataLists.MaxX : initialRect.XMax;
                    DisplayRect.YMin = (DataLists.MinY < initialRect.YMin)
                        ? DataLists.MinY : initialRect.YMin;
                    DisplayRect.YMax = (DataLists.MaxY > initialRect.YMax)
                        ? DataLists.MaxY : initialRect.YMax;
                }
                else if (graphStyle == GraphMode.FixMoveMode)
                {
                    if (DataLists.MaxX > DisplayRect.XMax)
                    {
                        DisplayRect.XMin += DataLists.MaxX - DisplayRect.XMax;
                        DisplayRect.XMax = DataLists.MaxX;
                    }

                    DisplayRect.YMin = (DataLists.MinY < DisplayRect.YMin)
                        ? DataLists.MinY : DisplayRect.YMin;
                    DisplayRect.YMax = (DataLists.MaxY > DisplayRect.YMax)
                        ? DataLists.MaxY : DisplayRect.YMax;
                }
            }
            else
            {
                DisplayRect.UpdateRect(initialRect);
            }
        }

        public void ResetDisplayRectWidth(DataRect initialRect)
        {
            DisplayRect.XMax = DataLists.MaxX;
            DisplayRect.XMin = ((DisplayRect.XMax - initialRect.XRange) > initialRect.XMin)
                ? (DisplayRect.XMax - initialRect.XRange)
                : initialRect.XMin;
        }

        /// <summary>数据转换为待绘制区域上的点集
        /// </summary>
        /// <param name="width">待绘制区域的宽度</param>
        /// <param name="height">待绘制区域的高度</param>
        public List<PointF[]> GetPointsToDraw(int width, int height)
        {
            List<PointF[]> pointsLists = new List<PointF[]>();
            if ((DisplayRect.XRange > 0.9F * XDataAccuracy ||
                 DisplayRect.YRange > 0.9F * YDataAccuracy)
                && DataLists.HasData())
            {
                foreach (DataPairList<float> dataList in DataLists)
                {
                    List<PointF> points = new List<PointF>();
                    foreach (DataPair<float> dataPair in dataList)
                    {
                        points.Add(new PointF
                        {
                            X = (dataPair.X - DisplayRect.XMin) * (width - 1)
                                / DisplayRect.XRange,
                            Y = (dataPair.Y - DisplayRect.YMin) * (height - 1)
                                / DisplayRect.YRange
                        });
                    }
                    pointsLists.Add(points.ToArray());
                }
            }
            return pointsLists;
        }

        public DataGraph()
        {
            XDataAccuracy = DEFAULT_DATA_X_ACCURACY;
            YDataAccuracy = DEFAULT_DATA_Y_ACCURACY;
            DataLists = new DataPairLists<float>();
            DisplayRect = new DataRect();
        }

        private const float DEFAULT_DATA_X_ACCURACY = 1F;
        private const float DEFAULT_DATA_Y_ACCURACY = 0.1F;
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
                xDataAccuracy = value;
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
                yDataAccuracy = value;
            }
        }
    }
}
