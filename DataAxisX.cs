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

        internal IEnumerable<float> GetFirstGridPositions()
        {
            float xGrid1Start = (FirstScaleRange.Min - Min) * UnitLenght;
            for (int i = 0; i < SumOfFirstScale; i++)
            {
                yield return xGrid1Start + FirstScaleInterval * i;
            }
        }

        internal IEnumerable<float> GetSecondGridPositions()
        {
            float xGrid1Start = (FirstScaleRange.Min - Min) * UnitLenght;
            for (int i = 0; i < SumOfFirstScale; i++)
            {
                for (int j = 1; j < NumOfSecondScalePerFirstScale; j++)
                {
                    float xGrid2Pos = xGrid1Start + FirstScaleInterval * i
                        + SecondScaleInterval * j;
                    yield return xGrid2Pos;
                }
            }
        }
    }
}
