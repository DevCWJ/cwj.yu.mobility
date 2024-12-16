#if UNITY_2020_1_OR_NEWER
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using System.Reflection;

namespace CWJ.Serializable
{
    [Serializable]
    public class DictionaryVisualized<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        /// <summary>
        /// Serializable KeyValue struct used as items in the dictionary. This is needed
        /// since the KeyValuePair in System.Collections.Generic isn't serializable.
        /// </summary>
        [Serializable]
        struct SerializedKeyValueStruct
        {
            public TKey Key;
            public TValue Value;
            public SerializedKeyValueStruct(TKey Key, TValue Value)
            {
                this.Key = Key;
                this.Value = Value;
            }
        }
        public DictionaryVisualized(int capacity)
        {
            keyValues = new SerializedKeyValueStruct[capacity];
            SetCapacity(capacity);
            Init();
        }
        public DictionaryVisualized()
        {
            keyValues = new SerializedKeyValueStruct[0];
            Init();
        }
        readonly static Type dicType = typeof(Dictionary<TKey, TValue>);
        static MethodInfo _SetCapacityMethod = null;
        static MethodInfo SetCapacityMethod
        {
            get
            {
                if (_SetCapacityMethod == null)
                    _SetCapacityMethod = dicType.GetMethod("Initialize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                return _SetCapacityMethod;
            }
        }
        static FieldInfo _ComparerField = null;
        static FieldInfo ComparerField
        {
            get
            {
                if (_ComparerField == null)
                {
                    _ComparerField = dicType.GetField("_comparer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (_ComparerField == null)
                        _ComparerField = dicType.GetField("comparer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                }

                return _ComparerField;
            }
        }
        void SetCapacity(int capacity)
        {
            if (capacity == 0 || capacity == this.Count)
            {
                return;
            }

            SetCapacityMethod.Invoke(this, new object[] { capacity });
        }


        void SetComparer(IEqualityComparer<TKey> equalityComparer)
        {
            if (equalityComparer == null)
            {
                return;
            }
            ComparerField.SetValue(this, equalityComparer);
        }


        public DictionaryVisualized(IEqualityComparer<TKey> equalityComparer)
        {
            SetComparer(equalityComparer);
            keyValues = new SerializedKeyValueStruct[0];
            Init();
        }
        void Init()
        {
            conflictDatas = null;
            conflictKeyOriginIndexes = null;
            conflictKeyWarningIndexes = null;
            nullKeyIndexes = null;
        }



        public DictionaryVisualized(IDictionary<TKey, TValue> dict)
        {
            Init();
            int cnt = dict.Count;
            keyValues = new SerializedKeyValueStruct[cnt];
            if (cnt > 0)
            {
                SetCapacity(cnt);
                int i = 0;
                foreach (var item in dict)
                {
                    this.Add(item.Key, item.Value);

                    keyValues[i] = new SerializedKeyValueStruct(item.Key, item.Value);
                    ++i;
                }
            }
        }

        [SerializeField]
        SerializedKeyValueStruct[] keyValues = null;

        [NonSerialized]
        ConflictKeyValue[] conflictDatas = null;

        struct ConflictKeyValue
        {
            public int index;
            public bool isKeyNull;
            public SerializedKeyValueStruct keyValue;

            public ConflictKeyValue(int index, bool isNull, SerializedKeyValueStruct keyValue)
            {
                this.index = index;
                this.isKeyNull = isNull;
                this.keyValue = keyValue;
            }
        }

        [SerializeField, HideInInspector]
        int[] conflictKeyOriginIndexes;
        [SerializeField, HideInInspector]
        int[] conflictKeyWarningIndexes;
        [SerializeField, HideInInspector]
        int[] nullKeyIndexes;



        public bool IsReadOnly => ((IDictionary<TKey, TValue>)this).IsReadOnly;

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            ConvertToKeyValue();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            UpdateFromKeyValue();
        }

        /// <summary>
        /// Serialize dictionary into keyValueList representation.
        /// </summary>
        void ConvertToKeyValue()
        {
            int dictCnt = this.Count;
            int conflictLength = conflictDatas.LengthSafe();

            if (keyValues != null)
                keyValues = null;

            keyValues = new SerializedKeyValueStruct[dictCnt];

            int i = 0;
            foreach (var kvp in this)
            {
                keyValues[i] = new SerializedKeyValueStruct(kvp.Key, kvp.Value);
                ++i;
            }

            if (conflictLength > 0)
            {
                var keyValueList = keyValues.ToList();
                for (i = 0; i < conflictLength; ++i)
                {
                    keyValueList.Insert(conflictDatas[i].index, conflictDatas[i].keyValue);
                }
                keyValues = keyValueList.ToArray();
            }
        }

        /// <summary>
        /// Deserialize dictionary from list while checking for key-collisions.
        /// </summary>
        void UpdateFromKeyValue()
        {
            if (keyValues == null)
            {
                return;
            }

            if (conflictDatas == null && keyValues.Length > 0)
            { //remove conflict data in keyValues
                var keyValueList = keyValues.ToList();
                void RemoveAtConflictIndex(int[] arr)
                {
                    for (int i = 0; i < arr.Length; ++i)
                        keyValueList.RemoveAt(arr[i]);
                };
                if (conflictKeyWarningIndexes.LengthSafe() > 0) RemoveAtConflictIndex(conflictKeyWarningIndexes);
                if (nullKeyIndexes.LengthSafe() > 0) RemoveAtConflictIndex(nullKeyIndexes);
                keyValues = keyValueList.ToArray();
            }
            Init();

            int keyValueCnt = keyValues.Length;
            Clear();
            
            SetCapacity(keyValueCnt);
            var conflictDataList = new List<ConflictKeyValue>();

            for (int i = 0; i < keyValueCnt; i++)
            {
                bool isNull = keyValues[i].Key == null || keyValues[i].Key.Equals(null);
                if (!isNull && !this.ContainsKey(keyValues[i].Key))
                {
                    this.Add(keyValues[i].Key, keyValues[i].Value);
                }
                else
                {
                    var conflictData = new ConflictKeyValue(i, isNull, keyValues[i]);
                    conflictDataList.Add(conflictData);
                }
            }

            conflictDatas = conflictDataList.ToArray();
            int conflictCnt = conflictDatas.Length;

            if (conflictCnt > 0)
            {
                var conflictOriginKeyList = new HashSet<TKey>();
                var conflictOriginIndexList = new List<int>(conflictCnt);
                var conflictWarningIndexList = new List<int>(conflictCnt);
                var nullIndexList = new List<int>(conflictCnt);

                foreach (var conflictData in conflictDatas)
                {
                    int conflictIndex = conflictData.index;

                    if (conflictData.isKeyNull)
                    {
                        nullIndexList.Add(conflictIndex);
                    }
                    else
                    {
                        if (conflictOriginKeyList.Add(conflictData.keyValue.Key))
                        {
                            conflictOriginIndexList.Add(keyValues.FindIndex(kv => kv.Key.Equals(conflictData.keyValue.Key)));
                        }
                        conflictWarningIndexList.Add(conflictIndex);
                    }
                }
                conflictKeyOriginIndexes = conflictOriginIndexList.ToArray();
                conflictKeyWarningIndexes = conflictWarningIndexList.ToArray();
                nullKeyIndexes = nullIndexList.ToArray();
            }
        }



        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentException("The array cannot be null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
            if (array.Length - arrayIndex < this.Count)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            foreach (var pair in this)
            {
                array[arrayIndex] = pair;
                arrayIndex++;
            }
        }


    }
}
#endif