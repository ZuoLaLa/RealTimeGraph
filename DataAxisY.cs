using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealTimeGraph
{
    class DataAxisY : DataAxis
    {
        internal IEnumerable<float> GetFirstGridPositions(float yStart)
        {
            float yGrid1Start = yStart - (FirstScaleRange.Min - Min) * UnitLenght;
            for (int i = 0; i < SumOfFirstScale; i++)
            {
                yield return yGrid1Start - FirstScaleInterval * i;
            }
        }

        internal IEnumerable<float> GetSecondGridPositions(float yStart)
        {
            float yGrid1Start = yStart - (FirstScaleRange.Min - Min) * UnitLenght;
            for (int i = 0; i < SumOfFirstScale; i++)
            {
                for (int j = 1; j < NumOfSecondScalePerFirstScale; j++)
                {
                    float yGrid2Pos = yGrid1Start - FirstScaleInterval * i
                        - SecondScaleInterval * j;
                    yield return yGrid2Pos;
                }
            }
        }
    }
}
