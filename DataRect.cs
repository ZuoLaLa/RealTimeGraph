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
            set { XAxisRange.Min = value; }
        }

        public float XMax
        {
            get { return XAxisRange.Max; }
            set { XAxisRange.Max = value; }
        }

        public float YMin
        {
            get { return YAxisRange.Min; }
            set { YAxisRange.Min = value; }
        }

        public float YMax
        {
            get { return YAxisRange.Max; }
            set { YAxisRange.Max = value; }
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
            XAxisRange = new DataRange();
            YAxisRange = new DataRange();
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
