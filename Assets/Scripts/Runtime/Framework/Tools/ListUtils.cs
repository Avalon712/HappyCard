using System.Collections.Generic;

namespace HappyCard
{
    public static class ListUtils
    {
        public static void TailDelete<T>(List<T> list, int removeIdx)
        {
            int count = list.Count;
            if (removeIdx == count - 1)
                list.RemoveAt(removeIdx);
            else
            {
                T removed = list[removeIdx];
                T tail = list[count - 1];
                list[removeIdx] = tail;
                list[count - 1] = removed;
                list.RemoveAt(count - 1);
            }
        }
    }
}
