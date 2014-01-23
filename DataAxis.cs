using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealTimeGraph
{
    class DataAxis
    {
        public float UnitLenght { get; set; }
        public DataRange FirstScaleRange { get; set; }
        public int NumOfFirstScalePerWeight { get; set; }
        public int SumOfFirstScale { get; set; }
        public int NumOfSecondScalePerFirstScale { get; set; }
        public float FirstScaleInterval { get; set; }
        public float SecondScaleInterval { get; set; }

        public DataAxis()
        {
            FirstScaleRange = new DataRange();
        }

        public void Update(DataRange axisRange, int axisLength)
        {
            UnitLenght = axisLength / axisRange.Range;
            FirstScaleRange.Min = Convert.ToSingle(
                Math.Floor(Convert.ToDecimal(axisRange.Min)
                / axisRange.Weight) * axisRange.Weight);
            FirstScaleRange.Max = Convert.ToSingle(
                Math.Ceiling(Convert.ToDecimal(axisRange.Max)
                / axisRange.Weight) * axisRange.Weight);
            NumOfFirstScalePerWeight = GetScaleNum(axisLength * (float)axisRange.Weight
                / axisRange.Range,
                GraphProperties.FIRST_SCALE_MIN_INTERVAL);
            SumOfFirstScale = (int)(FirstScaleRange.Range / (float)axisRange.Weight * NumOfFirstScalePerWeight);
            FirstScaleInterval = axisLength * (float)axisRange.Weight
                / (axisRange.Range * NumOfFirstScalePerWeight);
            NumOfSecondScalePerFirstScale = GetScaleNum(FirstScaleInterval, GraphProperties.SECOND_SCALE_MIN_INTERVAL);
            SecondScaleInterval = FirstScaleInterval / NumOfSecondScalePerFirstScale;
        }

        private int GetScaleNum(double scaleLength, int scaleInterval)
        {
            int scaleNum = 1;
            if (scaleLength / 10 >= scaleInterval)
            {
                scaleNum = 10;
            }
            else if (scaleLength / 5 >= scaleInterval)
            {
                scaleNum = 5;
            }
            else if (scaleLength / 2 >= scaleInterval)
            {
                scaleNum = 2;
            }

            return scaleNum;
        }
    }
}
