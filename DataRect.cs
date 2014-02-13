using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealTimeGraph
{
    class DataRect
    {
        public DataRange XAxisRange { set; get; }
        public DataRange YAxisRange { set; get; }

        public float XMin
        {
            get { return XAxisRange.Min; }
            set
            {
                DataRange xDataRange = XAxisRange;
                xDataRange.Min = value;
                XAxisRange = xDataRange;
            }
        }

        public float XMax
        {
            get { return XAxisRange.Max; }
            set
            {
                DataRange xDataRange = XAxisRange;
                xDataRange.Max = value;
                XAxisRange = xDataRange;
            }
        }

        public float YMin
        {
            get { return YAxisRange.Min; }
            set
            {
                DataRange yDataRange = YAxisRange;
                yDataRange.Min = value;
                YAxisRange = yDataRange;
            }
        }

        public float YMax
        {
            get { return YAxisRange.Max; }
            set
            {
                DataRange yDataRange = YAxisRange;
                yDataRange.Max = value;
                YAxisRange = yDataRange;
            }
        }

        public float XRange
        {
            get { return XAxisRange.Range; }
        }

        public float YRange
        {
            get { return YAxisRange.Range; }
        }

        public decimal WeightX
        {
            get { return XAxisRange.Weight; }
        }

        public decimal WeightY
        {
            get { return YAxisRange.Weight; }
        }

        public DataRect()
        {
        }

        public DataRect(float xMin, float xMax, float yMin, float yMax)
        {
            XAxisRange = new DataRange(xMin, xMax);
            YAxisRange = new DataRange(yMin, yMax);
        }

        public void UpdateRect(float xMin, float xMax, float yMin, float yMax)
        {
            XMin = xMin;
            XMax = xMax;
            YMin = yMin;
            YMax = yMax;
        }

        public void UpdateRect(DataRect newRect)
        {
            XMin = newRect.XMin;
            XMax = newRect.XMax;
            YMin = newRect.YMin;
            YMax = newRect.YMax;
        }
    }
}
