﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealTimeGraph
{
    class DataGraph
    {
        public DataPairList<float> DataList { get; set; }

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
            if (!DataList.IsNullOrEmpty())
            {
                if (graphStyle == GraphMode.GlobalMode)
                {
                    DisplayRect.XMin = (DataList.MinX < initialRect.XMin)
                        ? DataList.MinX : initialRect.XMin;
                    DisplayRect.XMax = (DataList.MaxX > initialRect.XMax)
                        ? DataList.MaxX : initialRect.XMax;
                    DisplayRect.YMin = (DataList.MinY < initialRect.YMin)
                        ? DataList.MinY : initialRect.YMin;
                    DisplayRect.YMax = (DataList.MaxY > initialRect.YMax)
                        ? DataList.MaxY : initialRect.YMax;
                }
                else if (graphStyle == GraphMode.FixMoveMode)
                {
                    if (DataList.MaxX > DisplayRect.XMax)
                    {
                        DisplayRect.XMin += DataList.MaxX - DisplayRect.XMax;
                        DisplayRect.XMax = DataList.MaxX;
                    }

                    DisplayRect.YMin = (DataList.MinY < DisplayRect.YMin)
                        ? DataList.MinY : DisplayRect.YMin;
                    DisplayRect.YMax = (DataList.MaxY > DisplayRect.YMax)
                        ? DataList.MaxY : DisplayRect.YMax;
                }
            }
            else
            {
                DisplayRect.UpdateRect(initialRect);
            }
        }

        public void ResetDisplayRectWidth(DataRect initialRect)
        {
            DisplayRect.XMax = DataList.MaxX;
            DisplayRect.XMin = ((DisplayRect.XMax - initialRect.XRange) > initialRect.XMin)
                ? (DisplayRect.XMax - initialRect.XRange)
                : initialRect.XMin;
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

        public DataGraph()
        {
            XDataAccuracy = DEFAULT_DATA_X_ACCURACY;
            YDataAccuracy = DEFAULT_DATA_Y_ACCURACY;
            DataList = new DataPairList<float>();
            pointsList = new List<PointF>();
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
