using System.Collections;

namespace RealTimeGraph
{
    public static class ListUtil
    {
        public static bool IsNullOrEmpty(this IList list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}
