using System;

namespace RealTimeGraph
{
    class DataGraph
    {
        public DataPairLists<float> DataLists { get; set; }

        public DataGraph()
        {
            XDataAccuracy = DEFAULT_DATA_X_ACCURACY;
            YDataAccuracy = DEFAULT_DATA_Y_ACCURACY;
            DataLists = new DataPairLists<float>();
        }

        private const float DEFAULT_DATA_X_ACCURACY = 1F;
        private const float DEFAULT_DATA_Y_ACCURACY = 0.1F;
        private float xDataAccuracy;
        public float XDataAccuracy
        {
            get { return xDataAccuracy; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(
                        "The data accuracy must be greater than zero!");
                }
                xDataAccuracy = value;
            }
        }

        private float yDataAccuracy;
        public float YDataAccuracy
        {
            get { return yDataAccuracy; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(
                        "The data accuracy must be greater than zero!");
                }
                yDataAccuracy = value;
            }
        }
    }
}
