using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CWJ
{
    [System.Serializable]
    public class SortedQueue<T> : IEnumerable<T>
    {
        private const int InitialCapacity = 4;
        private const int GrowFactor = 2;
        private const int MinGrow = 1;
        private int _capacity = InitialCapacity;
        [UnityEngine.SerializeField,Readonly]
        private int _head = 0;
        [UnityEngine.SerializeField, Readonly]
        private int _tail = 0;
        [UnityEngine.SerializeField,Readonly]
        private int _size = 0;
        [UnityEngine.SerializeField]
        private T[] _array = new T[InitialCapacity];
#if UNITY_EDITOR
        [UnityEngine.SerializeField]
        private T[] _viewArray;
        private void Editor_CopyToViewArray()
        {
            _viewArray = new T[_size];
            if (_size > 0)
                Array.Copy(_array, _head, _viewArray, 0, _size);
        }
#endif

        public int Count { get { return _size; } }
        public int Capacity { get { return _capacity; } }
        protected IComparer<T> comparer { get; private set; }

        public SortedQueue()
            : this(8) { }
        protected SortedQueue(int capapcity)
            : this(Comparer<T>.Default, capapcity) { }

        protected SortedQueue(IComparer<T> comparer, int capapcity)
        {
            _size = 0;
            this.comparer = comparer;
            SetCapacity(capapcity);
        }

        static bool IsInit;
        protected static T nullValue;

        protected SortedQueue(IEnumerable<T> collection, int length, IComparer<T> comparer, bool isNeedSort)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            if (comparer == null) throw new ArgumentNullException("comparer");
            
            if(!IsInit)
            {
                nullValue = default(T);
                IsInit = true;
            }

            _size = length;
            _capacity = length * GrowFactor + MinGrow;
            _array = new T[_capacity];
            _head = 0;
            _tail = 0;
            foreach (var item in collection)
            {;
                    _array[_tail++] = item;
            }
            this.comparer = comparer;

            if (isNeedSort)
            {
                Array.Sort(_array, _head, _size, comparer);
            }

#if UNITY_EDITOR
            Editor_CopyToViewArray();
#endif
        }


        public SortedQueue(ICollection<T> collection)
            : this(collection, collection.Count, Comparer<T>.Default, true) { }
        public SortedQueue(ICollection<T> collection, IComparer<T> comparer)
            : this(collection, collection.Count, comparer, true) { }

        public void Enqueue(T item)
        {
            int arrayLength = _array.Length;
            if (_size == arrayLength)
            {
                int newCapacity = arrayLength * 2;
                if (newCapacity < 8)
                    newCapacity = 8;

                SetCapacity(newCapacity);
            }

            _array[_tail] = item;
            _tail = (_tail + 1) % _array.Length;
            ++_size;

            Array.Sort(_array, _head, _size, comparer);

#if UNITY_EDITOR
            Editor_CopyToViewArray();
#endif
        }

        public T Peek()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException("SortedQueue is empty");
            }
            return _array[_head];
        }
        public bool TryPeek(int queueIndex, out T result)
        {
            if (_size == 0 || queueIndex >= _size)
            {
                result = nullValue;
                return false;
            }
            int i = (_head + queueIndex) % _size;
            if (i >= _array.Length)
            {
                result = nullValue;
                return false;
            }
            result = _array[i];
            return true;
        }

        public bool TryDequeue(out T result)
        {
            if (_size == 0)
            {
                result = nullValue;
                return false;
            }
            result = Dequeue();
            return true;
        }

        public T Dequeue()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException("SortedQueue is empty");
            }
            T result = _array[_head];
            _array[_head] = nullValue;
            _head = (_head + 1) % _array.Length;
            --_size;
            if (_size == 0)
            {
                _head = 0;
                _tail = 0;
            }
#if UNITY_EDITOR
            Editor_CopyToViewArray();
#endif
            return result;
        }

        public T Push()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException("SortedQueue is empty");
            }

            T result = _array[--_tail];
            _array[_tail] = nullValue;
            --_size;
            if (_size == 0)
            {
                _head = 0;
                _tail = 0;
            }

#if UNITY_EDITOR
            Editor_CopyToViewArray();
#endif
            return result;
        }
        public void Clear()
        {
            if (_head < _tail)
            {
                Array.Clear(_array, _head, _size);
            }
            else
            {
                Array.Clear(_array, 0, _array.Length);
            }

            _head = 0;
            _tail = 0;
            _size = 0;
#if UNITY_EDITOR
            Editor_CopyToViewArray();
#endif
        }

        public bool Contains(T item)
        {
            int num = _head;
            int size = _size;
            EqualityComparer<T> equlity = EqualityComparer<T>.Default;
            while (size-- > 0)
            {
                var v = _array[num];
                if ((item == null && v == null)
                    || (item.Equals(nullValue) && v.Equals(nullValue)))
                {
                    return true;
                }
                else if (v != null && equlity.Equals(v, item))
                {
                    return true;
                }

                num = (num + 1) % _array.Length;
            }

            return false;
        }
        private void SetCapacity(int capacity)
        {
            T[] newArray = new T[capacity];
            if (_size > 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(_array, _head, newArray, 0, _size);
                }
                else
                {
                    Array.Copy(_array, _head, newArray, 0, _array.Length - _head);
                    Array.Copy(_array, 0, newArray, _array.Length - _head, _tail);
                }
            }

            _array = newArray;
            _head = 0;
            _tail = ((_size != capacity) ? _size : 0);
        }

        public T[] ToArray()
        {
            T[] array = new T[_size];
            if (_size == 0)
            {
                return array;
            }

            if (_head < _tail)
            {
                Array.Copy(_array, _head, array, 0, _size);
            }
            else
            {
                Array.Copy(_array, _head, array, 0, _array.Length - _head);
                Array.Copy(_array, 0, array, _array.Length - _head, _tail);
            }

            return array;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _array.Skip(_head).Take(_size).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
