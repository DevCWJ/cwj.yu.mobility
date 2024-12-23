using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CWJ.SceneHelper;

namespace CWJ.YU.Mobility
{
    using static Define;

    public class ProjectManager : CWJ.Singleton.SingletonBehaviour<ProjectManager>
    {
        //public const string RootPrefabName = "[CWJ.YU.Root]";
        //[VisualizeField] public static Transform Project_RootObj;

        //public static void InitWorld()
        //{
        //    if (Project_RootObj == null)
        //    {
        //        var rootObj = FindUtil.GetRootGameObjects_New(false).FirstOrDefault(g => g != null && g.name.Equals(RootPrefabName));
        //        if (rootObj == null)
        //        {
        //            rootObj = Instantiate(Resources.Load<GameObject>(RootPrefabName));
        //            rootObj.SetActive(false);
        //        }
        //        Project_RootObj = rootObj.transform;
        //    }
        //    if (Project_RootObj.parent != null)
        //        Project_RootObj.SetParent(null, true);
        //    if (Project_RootObj.position != Vector3.zero)
        //        Project_RootObj.position = Vector3.zero;
        //    if (Project_RootObj.rotation != Quaternion.identity)
        //        Project_RootObj.rotation = Quaternion.identity;
        //    if (!Project_RootObj.gameObject.activeSelf)
        //        Project_RootObj.gameObject.SetActive(true);
        //}

        //public Transform topicParentTrf;

        [SerializeField] private CWJ.Serializable.DictionaryVisualized<int, Topic> topicDics = new();
        [VisualizeProperty] public static int CurTopicIndex { get; private set; }

        public static void OnClickPrev() { Instance.topicDics[CurTopicIndex].Previous(); }
        public static void OnClickNext() { Instance.topicDics[CurTopicIndex].Next(); }

        public bool TryAddToDict(Topic topic)
        {
            if (!topicDics.TryGetValue(topic.topicIndex, out var existsTopic))
            { //Topic 최초 추가
                topicDics.Add(topic.topicIndex, topic);
                return true;
            }
            else if (!existsTopic)
            {
                topicDics[topic.topicIndex] = topic;
                return true;
            }
            return false;
        }


        public static bool isDuringSetTopic { get; private set; } = false;


        /// <summary>
        /// use this
        /// </summary>
        /// <param name="topicIndex"></param>
        public static void SetCurTopicIndex(int topicIndex)
        {
            EnsureSingletonObject();

            MultiThreadHelper.UIEnqueue(() =>
            {
                __UnsafeFastIns._SetCurTopicIndex(topicIndex);
            });

        }
        void _SetCurTopicIndex(int topicIndex)
        {
            if(!TryGetTopic(topicIndex, out var targetTopic))
            {
                return;
            }

            isDuringSetTopic = true;

            transform.parent.SetParent(targetTopic.transform, true);

            foreach (Topic t in topicDics.Values)
            {
                t.Init();
            }

            CurTopicIndex = topicIndex;


            targetTopic.Next();
            MultiThreadHelper.Enqueue(() =>
            {
                isDuringSetTopic = false;
                SceneControlManager.UpdateSceneObj(true);
            });

            #if UNITY_EDITOR
            CWJ.AccessibleEditor.AccessibleEditorUtil.PingObj(targetTopic.gameObject);
            #endif
        }

        public bool TryGetTopic(int topicIndex, out Topic topic)
        {
            topic = null;
            if (topicIndex < 0) return false;
            if (topicDics.TryGetValue(topicIndex, out topic) && topic)
            {
                return true;
            }

            return UpdateCacheAndTryGetTopic(topicIndex, out topic);
        }

        private bool UpdateCacheAndTryGetTopic(int topicIndex, out Topic targetT)
        {
            // string topicObjName = string.Format(TopicObjNameFormat, topicIndex + 1);
            var allOfTopics = FindObjectsOfType<Topic>(true);

            targetT = null;
            foreach (Topic topic in allOfTopics)
            {
                if (topicDics.TryGetValue(topicIndex, out var exists))
                {
                    if (!exists || exists.gameObject != topic.gameObject)
                    {
                        topicDics[topic.topicIndex] = topic;
                        if (exists)
                            exists.gameObject.SetActive(false);
                    }
                }
                else
                {
                    topicDics.Add(topic.topicIndex, topic);
                }

                if (topicIndex == topic.topicIndex)
                    targetT = topic;
            }


            if (targetT && targetT != null)
            {
                targetT.transform.Reset();

                return true;
            }

            Debug.LogError("씬에 배치되지않았거나 제작되지 않은 Topic : " + (topicIndex + 1));
            return false;
        }

        public static event System.Action<ProjectManager> OnSingletonCreated;

        static bool EnsureSingletonObject()
        {
            if (!IsExists)
            {
                var src = Resources.Load<GameObject>(SingletonObjName);
                if (src)
                {
                    var s = Instantiate(src);
                    var pm = s.GetComponentInChildren<ProjectManager>();
                    pm.UpdateInstanceForcibly();
                    if (OnSingletonCreated != null)
                        MultiThreadHelper.Enqueue(() => OnSingletonCreated.Invoke(pm));
                }
                return false;
            }
            return true;
        }

    }
}
