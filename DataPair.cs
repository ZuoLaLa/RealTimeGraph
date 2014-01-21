using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealTimeGraph
{
    class DataPair
    {
        public float X { get; set; }
        public float Y { get; set; }

        public DataPair(float x, float y)
        {
            X = x;
            Y = y;
        }

        public DataPair()
        {
        }
    }
}
