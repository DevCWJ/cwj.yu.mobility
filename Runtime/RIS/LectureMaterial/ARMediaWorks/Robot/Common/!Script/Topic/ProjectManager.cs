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
        public void SetTopic(int topicIndex, Topic lastTopic = null)
        {
            if(!TryGetTopic(topicIndex, out var targetTopic))
            {
                return;
            }

            isDuringSetTopic = true;

            transform.parent.SetParent(targetTopic.transform, true);

            foreach (Topic t in topicDics.Values.ToArray())
            {
                t.Init();
            }

            CurTopicIndex = topicIndex;


            targetTopic.Next();
            ThreadDispatcher.Enqueue(() =>
            {
                isDuringSetTopic = false;
                SceneControlManager.UpdateSceneObj(true);
            });

            CWJ.AccessibleEditor.AccessibleEditorUtil.PingObj(targetTopic.gameObject);
        }

        public bool TryGetTopic(int topicIndex, out Topic topic)
        {
            topic = null;
            if (topicIndex < 0) return false;
            if (topicDics.TryGetValue(topicIndex, out topic) && topic)
            {
                return true;
            }
            topic = InstantiateTopic(topicIndex);
            if (!topic)
            {
                return false;
            }
            topicDics[topicIndex] = topic;

            return true;
        }

        Topic InstantiateTopic(int topicIndex)
        {
            var src = Resources.Load<GameObject>(string.Format(TopicObjName, topicIndex + 1));
            if (!src)
            {
                Debug.LogError("아직 제작되지 않은 Topic : " + (topicIndex + 1));
                return null;
            }
            var obj = Instantiate(src);
            obj.transform.Reset();
            return obj ? obj.GetComponent<Topic>() : null;
        }

        public static event System.Action<ProjectManager> OnSingletonCreated;

        public static bool TryGetOrCreateSingleton(Transform tmpParent)
        {
            if (!IsExists)
            {
                var src = Resources.Load<GameObject>(SingletonObjName);
                if (src)
                {
                    var s = Instantiate(src, tmpParent);
                    s.transform.SetParent(null);
                    var pm = s.GetComponentInChildren<ProjectManager>();
                    pm.UpdateInstanceForcibly();
                    if (OnSingletonCreated != null)
                        ThreadDispatcher.Enqueue(() => OnSingletonCreated.Invoke(pm));
                }
                return false;
            }
            return true;
        }

    }
}
