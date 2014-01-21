using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealTimeGraph
{
    class DataRect
    {
        private DataRange axisX;
        private DataRange axisY;

        public float XMin
        {
            get { return axisX.Min; }
            set { axisX.Min = value; }
        }

        public float XMax
        {
            get { return axisX.Max; }
            set { axisX.Max = value; }
        }

        public float YMin
        {
            get { return axisY.Min; }
            set { axisY.Min = value; }
        }

        public float YMax
        {
            get { return axisY.Max; }
            set { axisY.Max = value; }
        }

        public float XRange
        {
            get { return axisX.Range; }
        }

        public float YRange
        {
            get { return axisY.Range; }
        }

        public DataRect(float xMin, float xMax, float yMin, float yMax)
        {
            axisX = new DataRange(xMin, xMax);
            axisY = new DataRange(yMin, yMax);
        }
    }
}
