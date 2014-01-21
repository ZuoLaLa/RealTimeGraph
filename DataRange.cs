using System;
using System.IO;

namespace RealTimeGraph
{
    class DataRange
    {
        public float Min { get; set; }
        public float Max { get; set; }

        public float Range
        {
            get { return Max - Min; }
        }

        public DataRange()
        {
        }

        public DataRange(float min, float max)
        {
            if (min > max)
            {
                throw new ArgumentException("Min can not be larger than max.");
            }
            else
            {
                Min = min;
                Max = max;
            }
        }
    }
}
