using System;

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
        private DataRange axisRange;

        public float Min
        {
            get { return axisRange.Min; }
            set { axisRange.Min = value; }
        }

        public float Max
        {
            get { return axisRange.Max; }
            set { axisRange.Max = value; }
        }

        public float Range
        {
            get { return axisRange.Range; }
        }

        public decimal Weight
        {
            get { return axisRange.Weight; }
        }

        public void Update(int axisLength)
        {
            UnitLenght = axisLength / axisRange.Range;
            FirstScaleRange = new DataRange
            {
                Min = Convert.ToSingle(
                    Math.Floor(Convert.ToDecimal(axisRange.Min)
                    / axisRange.Weight) * axisRange.Weight),
                Max = Convert.ToSingle(
                    Math.Ceiling(Convert.ToDecimal(axisRange.Max)
                    / axisRange.Weight) * axisRange.Weight)

            };
            NumOfFirstScalePerWeight = GetScaleNum(
                axisLength * (float)axisRange.Weight / axisRange.Range,
                GraphProperties.FIRST_SCALE_MIN_INTERVAL);
            SumOfFirstScale = (int)(FirstScaleRange.Range * NumOfFirstScalePerWeight
                / (float)axisRange.Weight);
            FirstScaleInterval = axisLength * (float)axisRange.Weight
                / (axisRange.Range * NumOfFirstScalePerWeight);
            NumOfSecondScalePerFirstScale = GetScaleNum(
                FirstScaleInterval, GraphProperties.SECOND_SCALE_MIN_INTERVAL);
            SecondScaleInterval = FirstScaleInterval / NumOfSecondScalePerFirstScale;
        }

        public void UpdateGlobalRange(
            DataRange initialRange, DataRange newRange)
        {
            this.Min = Math.Min(initialRange.Min, newRange.Min);
            this.Max = Math.Max(initialRange.Max, newRange.Max);
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
