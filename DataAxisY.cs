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

        internal IEnumerable<DataPair<float>> GetFirstScalePositionsAndValues(
            float yStart)
        {
            float yScale1Start = yStart - (FirstScaleRange.Min - Min) * UnitLenght;
            for (int i = 0; i < SumOfFirstScale; i++)
            {
                float yScale1Pos = yScale1Start - FirstScaleInterval * i; // 1级刻度坐标位置
                float yScale1Value = FirstScaleRange.Min +
                        (float)Weight * i / NumOfFirstScalePerWeight; // 1级刻度处坐标值
                yield return new DataPair<float>(yScale1Pos, yScale1Value);
            }
        }

        internal IEnumerable<float> GetSecondScalePositions(float yStart)
        {
            float yScale1Start = yStart - (FirstScaleRange.Min - Min) * UnitLenght;
            for (int i = 0; i < SumOfFirstScale; i++)
            {
                for (int j = 1; j < NumOfSecondScalePerFirstScale; j++)
                {
                    float yScale2Pos = yScale1Start -
                        FirstScaleInterval * i - SecondScaleInterval * j;
                    yield return yScale2Pos;
                }
            }
        }
    }
}
