﻿using System;
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

        /// <summary>
        /// 数据权值，用于获得友好坐标系的取值范围。
        /// </summary>
        public decimal Weight
        {
            get
            {
                decimal dataDiff = Convert.ToDecimal(Range);
                decimal weight = 1;

                if (dataDiff >= 1)
                {
                    while ((dataDiff /= 10) >= 1)
                    {
                        weight *= 10;
                    }
                }
                else if (dataDiff > 0 && dataDiff < 1)
                {
                    do
                    {
                        weight /= 10;
                        dataDiff *= 10;
                    } while (dataDiff < 1);
                }

                return weight;
            }
        }
    }
}
