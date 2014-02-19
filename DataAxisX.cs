using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealTimeGraph
{
    class DataAxisX : DataAxis
    {
        public void UpdateFixMoveRange(float newMax)
        {
            Min += newMax - Max;
            Max = newMax;
        }
    }
}
