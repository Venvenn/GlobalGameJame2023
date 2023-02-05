using System;
using UnityEngine.Assertions;

namespace Siren
{
    /// <summary>
    /// Works similarly to a regular queue except each element added to the queue can be given a priority allowing the
    /// queue to be sorted as its constructed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class PriorityQueue<T> where T : IComparable<T>
    {
        private int[] m_heap;
        private int[] m_heapInverse;
        private T[] m_objects;

        public PriorityQueue(int maxSize)
        {
            Resize(maxSize);
        }

        public int Count { get; private set; }

        public T this[int index]
        {
            get
            {
                Assert.IsTrue(index < m_objects.Length && index >= 0,
                    $"IndexedPriorityQueue.[]: Index '{index}' out of range");
                return m_objects[index];
            }
            set
            {
                Assert.IsTrue(index < m_objects.Length && index >= 0,
                    $"IndexedPriorityQueue.[]: Index '{index}' out of range");
                Set(index, value);
            }
        }

        /// <summary>
        /// Inserts a new value with the given index
        /// </summary>
        /// <param name="index">index to insert at</param>
        /// <param name="value">value to insert</param>
        public void Insert(int index, T value)
        {
            Assert.IsTrue(index < m_objects.Length && index >= 0,
                $"IndexedPriorityQueue.Insert: Index '{index}' out of range");

            ++Count;

            // add object
            m_objects[index] = value;

            // add to heap
            m_heapInverse[index] = Count;
            m_heap[Count] = index;

            // update heap
            SortHeapUpward(Count);
        }

        /// <summary>
        /// Gets the top element of the queue
        /// </summary>
        /// <returns>The top element</returns>
        public T Top()
        {
            // top of heap [first element is 1, not 0]
            return m_objects[m_heap[1]];
        }

        /// <summary>
        /// Removes the top element from the queue
        /// </summary>
        /// <returns>The removed element</returns>
        public T Pop()
        {
            Assert.IsTrue(Count > 0, "IndexedPriorityQueue.Pop: Queue is empty");

            if (Count == 0) return default;

            // swap front to back for removal
            Swap(1, Count--);

            // re-sort heap
            SortHeapDownward(1);

            // return popped object
            return m_objects[m_heap[Count + 1]];
        }

        /// <summary>
        /// Updates the value at the given index. Note that this function is not
        /// as efficient as the DecreaseIndex/IncreaseIndex methods, but is
        /// best when the value at the index is not known
        /// </summary>
        /// <param name="index">index of the value to set</param>
        /// <param name="obj">new value</param>
        public void Set(int index, T obj)
        {
            if (obj.CompareTo(m_objects[index]) <= 0)
                DecreaseIndex(index, obj);
            else
                IncreaseIndex(index, obj);
        }

        /// <summary>
        /// Decrease the value at the current index
        /// </summary>
        /// <param name="index">index to decrease value of</param>
        /// <param name="obj">new value</param>
        public void DecreaseIndex(int index, T obj)
        {
            Assert.IsTrue(index < m_objects.Length && index >= 0,
                $"IndexedPriorityQueue.DecreaseIndex: Index '{index}' out of range");
            Assert.IsTrue(obj.CompareTo(m_objects[index]) <= 0,
                $"IndexedPriorityQueue.DecreaseIndex: object '{obj}' isn't less than current value '{m_objects[index]}'");

            m_objects[index] = obj;
            SortUpward(index);
        }

        /// <summary>
        /// Increase the value at the current index
        /// </summary>
        /// <param name="index">index to increase value of</param>
        /// <param name="obj">new value</param>
        public void IncreaseIndex(int index, T obj)
        {
            Assert.IsTrue(index < m_objects.Length && index >= 0,
                $"IndexedPriorityQueue.DecreaseIndex: Index '{index}' out of range");
            Assert.IsTrue(obj.CompareTo(m_objects[index]) >= 0,
                $"IndexedPriorityQueue.DecreaseIndex: object '{obj}' isn't greater than current value '{m_objects[index]}'");

            m_objects[index] = obj;
            SortDownward(index);
        }

        public void Clear()
        {
            Count = 0;
        }

        /// <summary>
        /// Set the maximum capacity of the queue
        /// </summary>
        /// <param name="maxSize">new maximum capacity</param>
        public void Resize(int maxSize)
        {
            Assert.IsTrue(maxSize >= 0, $"IndexedPriorityQueue.Resize: Invalid size '{maxSize}'");

            m_objects = new T[maxSize];
            m_heap = new int[maxSize + 1];
            m_heapInverse = new int[maxSize];
            Count = 0;
        }

        private void SortUpward(int index)
        {
            SortHeapUpward(m_heapInverse[index]);
        }

        private void SortDownward(int index)
        {
            SortHeapDownward(m_heapInverse[index]);
        }

        private void SortHeapUpward(int heapIndex)
        {
            // move toward top if better than parent
            while (heapIndex > 1 &&
                   m_objects[m_heap[heapIndex]].CompareTo(m_objects[m_heap[Parent(heapIndex)]]) < 0)
            {
                // swap this node with its parent
                Swap(heapIndex, Parent(heapIndex));

                // reset iterator to be at parents old position
                // (child's new position)
                heapIndex = Parent(heapIndex);
            }
        }

        private void SortHeapDownward(int heapIndex)
        {
            // move node downward if less than children
            while (FirstChild(heapIndex) <= Count)
            {
                var child = FirstChild(heapIndex);

                // find smallest of two children (if 2 exist)
                if (child < Count && m_objects[m_heap[child + 1]].CompareTo(m_objects[m_heap[child]]) < 0) ++child;

                // swap with child if less
                if (m_objects[m_heap[child]].CompareTo(m_objects[m_heap[heapIndex]]) < 0)
                {
                    Swap(child, heapIndex);
                    heapIndex = child;
                }

                // no swap necessary
                else
                {
                    break;
                }
            }
        }

        private void Swap(int i, int j)
        {
            // swap elements in heap
            var temp = m_heap[i];
            m_heap[i] = m_heap[j];
            m_heap[j] = temp;

            // reset inverses
            m_heapInverse[m_heap[i]] = i;
            m_heapInverse[m_heap[j]] = j;
        }

        private int Parent(int heapIndex)
        {
            return heapIndex / 2;
        }

        private int FirstChild(int heapIndex)
        {
            return heapIndex * 2;
        }

        private int SecondChild(int heapIndex)
        {
            return heapIndex * 2 + 1;
        }
    }
}