using System;
using System.Collections.Generic;

namespace JustAssets.AtlasMapPacker.AtlasMapping
{
    public class NumberDistributor
    {
        public static List<List<int>> DistributeNumbers(List<int> items, int listCount)
        {
            return DistributeNumbers(items, listCount, item => item);
        }

        public static List<List<T>> DistributeNumbers<T>(List<T> items, int listCount, Func<T, long> computeSize)
        {
            if (listCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(listCount), "The value needs to be 1 or larger.");
            
            List<List<T>> lists = new List<List<T>>();
            List<(long total, int index)> totals = new List<(long total, int index)>();

            for (int i = 0; i < listCount; i++)
            {
                lists.Add(new List<T>());
                totals.Add((0, i));
            }

            PriorityQueue<(long total, int index)> minHeap = new PriorityQueue<(long total, int index)>(totals);

            foreach (T value in items)
            {
                (long total, int index) = minHeap.Dequeue();
                lists[index].Add(value);
                minHeap.Enqueue((total + computeSize(value), index));
            }

            lists.RemoveAll(x => x.Count == 0);

            return lists;
        }
    }
}