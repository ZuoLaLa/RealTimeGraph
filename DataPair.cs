using System;

namespace RealTimeGraph
{
    public struct DataPair<T>
        where T : struct
    {
        public T X;
        public T Y;

        public DataPair(T x, T y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return String.Format("{0},{1}", X, Y);
        }
    }
}