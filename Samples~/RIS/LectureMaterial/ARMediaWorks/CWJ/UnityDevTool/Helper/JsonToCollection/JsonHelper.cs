using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CWJ
{
    public static class JsonHelper
    {
        #region Dictionary
        [Serializable]
        public sealed class PipeGroupCollection_Dic : ISerializationCallbackReceiver
        {
            public int state;
            [SerializeField] private PipeGroupData[] response_data = null;

            private Dictionary<int, PipeGroupData> dictionary;

            public IReadOnlyDictionary<int, PipeGroupData> Dictionary => dictionary;

            public PipeGroupCollection_Dic(IEnumerable<PipeGroupData> collection)
            {
                dictionary = new Dictionary<int, PipeGroupData>();
                foreach (var item in collection)
                {
                    dictionary.Add(item.group_id, item);
                }
            }

            public void OnBeforeSerialize()
            {
                if (dictionary == null) return;
                response_data = new PipeGroupData[dictionary.Count];
                int i = -1;
                foreach (var item in dictionary)
                {
                    response_data[++i] = item.Value;
                }
                VisualizeByGroupId();
            }

            public void OnAfterDeserialize()
            {
                dictionary = new Dictionary<int, PipeGroupData>(response_data.Length);
                foreach (var item in response_data)
                {
                    dictionary.Add(item.group_id, item);
                }
                response_data = null;
            }

            [System.Diagnostics.Conditional("UNITY_EDITOR")]
            public void VisualizeByGroupId()
            {
                for (int i = 0; i < response_data.Length; i++)
                {
                    response_data[i].Init();
                }
            }
        }
        #endregion


        #region HashSet
        [Serializable]
        public sealed class PipeGroupCollection_Hs : ISerializationCallbackReceiver
        {
            public int state;
            [SerializeField] private PipeGroupData[] response_data = null;

            private HashSet<PipeGroupData> hashSet;

            public IReadOnlyCollection<PipeGroupData> HashSet => hashSet;


            public PipeGroupCollection_Hs(IEnumerable<PipeGroupData> collection)
            {
                hashSet = new HashSet<PipeGroupData>(collection);
            }

            public void OnBeforeSerialize()
            {
                if (hashSet == null) return;
                response_data = new PipeGroupData[hashSet.Count];
                hashSet.CopyTo(response_data);
                VisualizeByGroupId();
            }

            public void OnAfterDeserialize()
            {
                hashSet = new HashSet<PipeGroupData>(response_data);
                response_data = null;
            }

            [System.Diagnostics.Conditional("UNITY_EDITOR")]
            public void VisualizeByGroupId()
            {
                for (int i = 0; i < response_data.Length; i++)
                {
                    response_data[i].Init();
                }
            }
        }
        #endregion


        #region Data Struct
        [Serializable]
        public struct PipeGroupData : IEqualityComparer<PipeGroupData>
        {
            private bool isInit;
#if UNITY_EDITOR
            [SerializeField, HideInInspector] private string editor_name;
#endif

            public void Init()
            {
                if (isInit) return;
#if UNITY_EDITOR
                editor_name = group_id.ToString();
#endif

                // Initialize

                isInit = true;
            }

            public int group_id;

            public string latitude;

            public string longitude;

            public bool Equals(PipeGroupData x, PipeGroupData y)
            {
                return x.group_id == y.group_id;
            }

            public int GetHashCode(PipeGroupData obj)
            {
                return obj.group_id.GetHashCode();
            }
        }
        #endregion



        //JsonUtility 특성상 field이름을 통해 구조체를 변환시켜주기때문에 이걸 쓰려면 "response_data"를 "array"로 이름바꿔줘야할듯
        //[Serializable]
        //public class SerializeHashSet<T> : ISerializationCallbackReceiver
        //{
        //    [SerializeField] protected T[] array = null;

        //    protected HashSet<T> hashSet = new HashSet<T>();

        //    public IReadOnlyCollection<T> HashSet => hashSet;

        //    public SerializeHashSet(IEnumerable<T> collection)
        //    {
        //        hashSet = new HashSet<T>(collection);
        //    }

        //    public void OnBeforeSerialize()
        //    {
        //        array = new T[hashSet.Count];
        //        hashSet.CopyTo(array);
        //    }

        //    public void OnAfterDeserialize()
        //    {
        //        hashSet = new HashSet<T>(array);
        //        array = null;
        //    }
        //}

        //[Serializable]
        //public class GroupDataHashSet : SerializeHashSet<PipeGroupData>
        //{
        //    public GroupDataHashSet(IEnumerable<PipeGroupData> collection) : base(collection)
        //    {
        //    }
        //}
    }

}