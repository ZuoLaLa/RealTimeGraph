using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealTimeGraph
{
    public class DataPairLists<T> : List<DataPairList<T>>
        where T : struct
    {
        public T MinX
        {
            get
            {
                List<T> mins = new List<T>();
                foreach (DataPairList<T> dataList in this)
                {
                    if (!dataList.IsNullOrEmpty())
                    {
                        mins.Add(dataList.MinX);
                    }
                }
                return mins.Min();
            }
        }

        public T MaxX
        {
            get
            {
                List<T> maxes = new List<T>();
                foreach (DataPairList<T> dataList in this)
                {
                    if (!dataList.IsNullOrEmpty())
                    {
                        maxes.Add(dataList.MaxX);
                    }
                }
                return maxes.Max();
            }
        }

        public T MinY
        {
            get
            {
                List<T> mins = new List<T>();
                foreach (DataPairList<T> dataList in this)
                {
                    if (!dataList.IsNullOrEmpty())
                    {
                        mins.Add(dataList.MinY);
                    }
                }
                return mins.Min();
            }
        }

        public T MaxY
        {
            get
            {
                List<T> maxes = new List<T>();
                foreach (DataPairList<T> dataList in this)
                {
                    if (!dataList.IsNullOrEmpty())
                    {
                        maxes.Add(dataList.MaxY);
                    }
                }
                return maxes.Max();
            }
        }

        public bool HasData()
        {
            foreach (DataPairList<T> dataList in this)
            {
                if (!dataList.IsNullOrEmpty())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
