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
        public List<PointF> PointsList;

        public DataGraph()
        {
            XDataAccuracy = DEFAULT_DATA_X_ACCURACY;
            YDataAccuracy = DEFAULT_DATA_Y_ACCURACY;
            XDataList = new List<float>();
            YDataList = new List<float>();
            PointsList = new List<PointF>();
        }
    }
}
