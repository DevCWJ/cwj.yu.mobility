using System;
using System.Collections.Generic;
using UnityEngine;

namespace CWJ
{
    [System.Serializable]
    public class ListStack<T> : List<T>
        where T : Component
    {
        private short listIndex { get; set; }
        private void Init()
        {
            listIndex = -1;
        }
        public ListStack()
        {
            Init();
        }

        public ListStack(int capacity)
        {
            if (capacity > 0)
                this.Capacity = capacity;
            Init();
        }

        public void Push(T target)
        {
            if (Contains(target))
            {
                this.RemoveStack(target);
            }
            this.Add(target);

            ++listIndex;
        }

        public T Pop()
        {
            if (listIndex < 0)
            {
                return null;
            }

            if (listIndex >= this.Count)
            {
                return null;
            }

            T popObj = null;
            try
            {
                popObj = this[listIndex];
                RemoveAt(listIndex);
                if (listIndex > -1)
                    --listIndex;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            return popObj;
        }

        public T Peek()
        {
            if (this.Count == 0)
            {
                return null;
            }
            return this[Count - 1];
        }

        public void SetActiceTrueAndPush(T target)
        {
            this.Add(target);
            target.gameObject.SetActive(true);
        }

        public void RemoveStack(T target)
        {
            if (Remove(target))
            {
                if (listIndex > -1)
                    --listIndex;
            }
        }


        public void ClearStack()
        {
            Init();

            this.Clear();
        }



        public bool AllContains(IList<T> list)
        {
            int len = list.Count;
            for (int i = 0; i < len; ++i)
            {
                if (this.Contains(list[i]))
                    return true;
            }

            return false;
        }

        public void TargetActive(bool isActive, params T[] targets)
        {
            int targetLen = targets == null ? 0 : targets.Length;
            int listLen = this.Count;

            int inspectionCompeteNumber = 0;

            for (var i = 0; i < listLen; ++i)
            {
                bool isFind = false;

                for (var j = inspectionCompeteNumber; j < targetLen; ++j)
                {
                    if ((this[i] == targets[j]))
                    {
                        T tmp = targets[j];
                        targets[j] = targets[inspectionCompeteNumber];
                        targets[inspectionCompeteNumber] = tmp;

                        ++inspectionCompeteNumber;

                        isFind = true;
                        continue;
                    }
                }

                bool isActiveInspection = (targetLen == 0 || isFind) ? isActive : !isActive;

                this[i].gameObject.SetActive(isActiveInspection);
            }
        }

        public void OnlyTargetActive(bool isActive, params T[] targets)
        {
            int len = targets == null ? 0 : targets.Length;

            for (var i = 0; i < len; ++i)
            {
                this.Find(item => item == targets[i])?.gameObject.SetActive(isActive);
            }
        }

        public void Shuffle()
        {
            int count = this.Count;
            int last = count - 1;
            for (int i = 0; i < last; ++i)
            {
                int r = UnityEngine.Random.Range(i, count);
                T tmp = this[i];
                this[i] = this[r];
                this[r] = tmp;
            }
        }
    }
}
