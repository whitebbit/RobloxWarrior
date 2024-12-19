using System;
using System.Collections.Generic;

namespace JustAssets.AtlasMapPacker.AtlasMapping
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> _heap = new List<T>();

        public int Count => _heap.Count;
        
        public PriorityQueue() { }

        public PriorityQueue(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Enqueue(item);
            }
        }

        public void Enqueue(T item)
        {
            _heap.Add(item);
            int currentIndex = _heap.Count - 1;

            while (currentIndex > 0)
            {
                int parentIndex = (currentIndex - 1) / 2;
                if (_heap[currentIndex].CompareTo(_heap[parentIndex]) >= 0)
                    break;

                Swap(currentIndex, parentIndex);
                currentIndex = parentIndex;
            }
        }

        public T Dequeue()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("The queue is empty.");

            T dequeuedItem = _heap[0];
            int lastIndex = _heap.Count - 1;

            if (lastIndex == 0)
            {
                _heap.RemoveAt(0);
            }
            else
            {
                _heap[0] = _heap[lastIndex];
                _heap.RemoveAt(lastIndex);

                int currentIndex = 0;
                while (true)
                {
                    int leftChildIndex = 2 * currentIndex + 1;
                    int rightChildIndex = 2 * currentIndex + 2;
                    int smallestChildIndex = currentIndex;

                    if (leftChildIndex < _heap.Count && _heap[leftChildIndex].CompareTo(_heap[smallestChildIndex]) < 0)
                        smallestChildIndex = leftChildIndex;

                    if (rightChildIndex < _heap.Count && _heap[rightChildIndex].CompareTo(_heap[smallestChildIndex]) < 0)
                        smallestChildIndex = rightChildIndex;

                    if (smallestChildIndex == currentIndex)
                        break;

                    Swap(currentIndex, smallestChildIndex);
                    currentIndex = smallestChildIndex;
                }
            }

            return dequeuedItem;
        }

        private void Swap(int index1, int index2)
        {
            (_heap[index1], _heap[index2]) = (_heap[index2], _heap[index1]);
        }
    }
}
