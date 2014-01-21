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
        }

        public float XMax
        {
            get { return axisX.Max; }
        }

        public float YMin
        {
            get { return axisY.Min; }
        }

        public float YMax
        {
            get { return axisY.Max; }
        }

        public DataRect(float xMin, float xMax, float yMin, float yMax)
        {
            axisX = new DataRange(xMin, xMax);
            axisY = new DataRange(yMin, yMax);
        }
    }
}
