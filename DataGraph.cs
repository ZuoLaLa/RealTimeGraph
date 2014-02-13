using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealTimeGraph
{
    class DataGraph
    {
        public const float DEFAULT_DATA_X_ACCURACY = 1F;
        public const float DEFAULT_DATA_Y_ACCURACY = 0.1F;
        public DataPairList<float> DataList { get; set; }

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

        // 当前显示波形的数据范围

        public DataRect DisplayRect;

        public decimal DisplayWeightX
        {
            get { return DisplayRect.WeightX; }
        }

        public decimal DisplayWeightY
        {
            get { return DisplayRect.WeightY; }
        }

        /// <summary>待绘制的数据点集
        /// </summary>
        private List<PointF> pointsList;

        /// <summary>数据转换为待绘制区域上的点集
        /// </summary>
        /// <param name="width">待绘制区域的宽度</param>
        /// <param name="height">待绘制区域的高度</param>
        public PointF[] GetPointsToDraw(int width, int height)
        {
            pointsList.Clear();
            if ((DisplayRect.XRange > 0.9F * XDataAccuracy ||
                 DisplayRect.YRange > 0.9F * YDataAccuracy)
                 && !DataList.IsNullOrEmpty())
            {
                foreach (DataPair<float> dataPair in DataList)
                {
                    pointsList.Add(new PointF
                    {
                        X = (dataPair.X - DisplayRect.XMin) * (width - 1)
                                / DisplayRect.XRange,
                        Y = (dataPair.Y - DisplayRect.YMin) * (height - 1)
                                / DisplayRect.YRange
                    });
                }
            }
            return pointsList.ToArray();
        }

        private DataRect dataRect;

        public void UpdateDataRect()
        {
            dataRect.XMin = DataList.MinX ?? 0;
            dataRect.XMax = DataList.MaxX ?? 0;
            dataRect.YMin = DataList.MinY ?? 0;
            dataRect.YMax = DataList.MaxY ?? 0;
        }

        /// <summary>根据画图模式和数据调整坐标显示
        /// </summary>
        public void UpdateDisplayRect(DataRect initialRect, GraphMode graphStyle)
        {
            if (!DataList.IsNullOrEmpty())
            {
                if (graphStyle == GraphMode.GlobalMode)
                {
                    DisplayRect.XMin = (dataRect.XMin < initialRect.XMin)
                        ? dataRect.XMin : initialRect.XMin;
                    DisplayRect.XMax = (dataRect.XMax > initialRect.XMax)
                        ? dataRect.XMax : initialRect.XMax;
                    DisplayRect.YMin = (dataRect.YMin < initialRect.YMin)
                        ? dataRect.YMin : initialRect.YMin;
                    DisplayRect.YMax = (dataRect.YMax > initialRect.YMax)
                        ? dataRect.YMax : initialRect.YMax;
                }
                else if (graphStyle == GraphMode.FixMoveMode)
                {
                    if (dataRect.XMax > DisplayRect.XMax)
                    {
                        DisplayRect.XMin += dataRect.XMax - DisplayRect.XMax;
                        DisplayRect.XMax = dataRect.XMax;
                    }

                    DisplayRect.YMin = (dataRect.YMin < DisplayRect.YMin)
                        ? dataRect.YMin : DisplayRect.YMin;
                    DisplayRect.YMax = (dataRect.YMax > DisplayRect.YMax)
                        ? dataRect.YMax : DisplayRect.YMax;
                }
            }
            else
            {
                DisplayRect.UpdateRect(initialRect);
            }
        }

        public void ResetDisplayRectWidth(DataRect initialRect)
        {
            DisplayRect.XMax = dataRect.XMax;
            DisplayRect.XMin = ((DisplayRect.XMax - initialRect.XRange) > initialRect.XMin)
                ? (DisplayRect.XMax - initialRect.XRange)
                : initialRect.XMin;
        }

        public DataGraph()
        {
            XDataAccuracy = DEFAULT_DATA_X_ACCURACY;
            YDataAccuracy = DEFAULT_DATA_Y_ACCURACY;
            DataList = new DataPairList<float>();
            pointsList = new List<PointF>();
            DisplayRect = new DataRect();
            dataRect = new DataRect();
        }
    }
}
