using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TasiYokan.Algorithm.DataStructure
{
    /// <summary>
    /// Minimum heap
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T>
    {
        private IComparer<T> m_comparer;
        private T[] m_heap;
        private int m_count;
        private const int DefaultCapacity = 6;

        public int Count
        {
            get
            {
                return m_count;
            }
        }

        public T Top
        {
            get
            {
                try
                {
                    if (m_count > 0)
                        return m_heap[0];
                    else
                        throw new System.IndexOutOfRangeException("PriorityQueue is empty!");
                }
                catch (System.Exception exp)
                {
                    if (exp is System.IndexOutOfRangeException)
                        return default(T);

                    return default(T);
                }
            }
        }

        public PriorityQueue()
            : this(0, null)
        {
        }

        public PriorityQueue(int _capacity)
            : this(_capacity, null)
        {
        }

        public PriorityQueue(int _capacity, IComparer<T> _comparer)
        {
            this.m_heap = new T[(_capacity > 0) ? _capacity : DefaultCapacity];
            this.m_count = 0;
            this.m_comparer = (_comparer == null) ? Comparer<T>.Default : _comparer;
        }

        private static int LeftChild(int i)
        {
            return ((i * 2) + 1);
        }

        private static int RightOfLeftChild(int i)
        {
            return (i + 1);
        }

        private static int Parent(int i)
        {
            return ((i - 1) / 2);
        }

        public bool isEmpty()
        {
            return m_count == 0;
        }

        private void Expand()
        {
            T[] bufferArray = new T[m_count * 2];
            for (int i = 0; i < m_count; ++i)
            {
                bufferArray[i] = m_heap[i];
            }
            m_heap = bufferArray;

            return;
        }
        private void HeapifyDown(int _parent)
        {
            int parent = _parent;
            T front = m_heap[parent];
            for (int lc = PriorityQueue<T>.LeftChild(parent);
                lc < this.m_count;
                lc = PriorityQueue<T>.LeftChild(parent))
            {
                int rc = PriorityQueue<T>.RightOfLeftChild(lc);
                int child = ((rc < m_count) && (m_comparer.Compare(m_heap[rc], m_heap[lc]) < 0)) ? rc : lc;

                // If parent is less than the one of its children, dont update anymore.
                if (m_comparer.Compare(front, m_heap[child]) <= 0)
                    break;

                // Switch the smaller one to the top
                m_heap[parent] = m_heap[child];
                parent = child;
            }

            m_heap[parent] = front;
        }

        private void HeapifyUp(int _child)
        {
            int child = _child;
            T rear = m_heap[child];
            for (int parent = PriorityQueue<T>.Parent(child);
                child > 0;
                parent = PriorityQueue<T>.Parent(child))
            {
                if (m_comparer.Compare(rear, m_heap[parent]) >= 0)
                    break;

                m_heap[child] = m_heap[parent];
                child = parent;
            }

            m_heap[child] = rear;
        }

        /// <summary>
        /// Pop the minimum element in heap top
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T heapTop = Top;
            m_heap[0] = m_heap[--m_count];

            if (m_count > 0)
                HeapifyDown(0);

            return heapTop;
        }

        /// <summary>
        /// Push from the rear of queue and call <see cref="HeapifyUp"/>
        /// </summary>
        /// <param name="_element"></param>
        public void Push(T _element)
        {
            if (m_count == m_heap.Length)
            {
                //Expand();
                System.Array.Resize(ref m_heap, m_count * 2);
            }
            // Push to the rear of queue.[11/29]
            m_heap[m_count] = _element;

            HeapifyUp(m_count++);

        }
    }
}
