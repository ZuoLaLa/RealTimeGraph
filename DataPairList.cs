using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealTimeGraph
{
    public class DataPairList<T> : IEnumerable<DataPair<T>>
        where T : struct
    {
        public List<T> XDataList { get; set; }
        public List<T> YDataList { get; set; }

        public T MinX
        {
            get { return XDataList.Min(); }
        }

        public T MaxX
        {
            get { return XDataList.Max(); }
        }

        public T MinY
        {
            get { return YDataList.Min(); }
        }

        public T MaxY
        {
            get { return YDataList.Max(); }
        }

        public DataPairList()
        {
            XDataList = new List<T>();
            YDataList = new List<T>();
        }

        public DataPairList(List<T> listX, List<T> listY)
        {
            if (listX == null || listY == null)
            {
                throw new ArgumentNullException();
            }
            else if (listX.Count != listY.Count)
            {
                throw new ArgumentException(
                    "The two list must have the same count.");
            }
            XDataList = listX;
            YDataList = listY;
        }

        public int Count
        {
            get { return XDataList.Count; }
        }

        public bool IsNullOrEmpty()
        {
            if (XDataList == null || XDataList.Count == 0)
            {
                return true;
            }
            return false;
        }

        public IEnumerator<DataPair<T>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return new DataPair<T>(XDataList[i], YDataList[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
