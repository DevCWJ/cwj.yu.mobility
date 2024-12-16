using System;

using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace CWJ.AccessibleEditor
{
    public static class ScriptExecutionOrder
    {
        public static void SetScriptExecutionOrder<T>(Int16 order) where T : ScriptableObject
        {
#if UNITY_EDITOR
            var scriptableObj = ScriptableObject.CreateInstance<T>();
            MonoScript monoScript = MonoScript.FromScriptableObject(scriptableObj);
            int curExecutionOrder;
            try
            {
                var monoimporter = new MonoImporter();
                curExecutionOrder = MonoImporter.GetExecutionOrder(monoScript);
            }
            catch
            {
                curExecutionOrder = order - 1;
            }

            if (curExecutionOrder != order)
            {
                try
                {
                    MonoImporter.SetExecutionOrder(monoScript, order);
                }
                catch (Exception e)
                {
                    Debug.LogError($"{typeof(T).Name} 가 오류로 ScriptExecutionOrder 수정못했음\n" + e.ToString());
                }
                finally
                {
                    UnityEngine.Object.DestroyImmediate(scriptableObj);
                }
            }
#endif
        }

        /// <summary>
        /// 스크립트 실행순서 설정. 사용예시는 아래에.
        /// </summary>
        /// <param name="order"> –32768 ~ 32767(int16) </param>
        public static void SetMonoBehaviourExecutionOrder<T>(Int16 order) where T: MonoBehaviour
        {
#if UNITY_EDITOR
            bool isError = false;
            var obj = new GameObject("Plz Destroy This Object");
            obj.SetActive(false);

            MonoBehaviourEventHelper.Editor_IsSilentlyCreateInstance = true;
            MonoBehaviourEventHelper.Editor_IsManagedByEditorScript = true;
            var comp = obj.AddComponent<T>();
            MonoBehaviourEventHelper.Editor_IsManagedByEditorScript = false;
            MonoBehaviourEventHelper.Editor_IsSilentlyCreateInstance = false;

            if (comp == null)
                isError = true;

            MonoScript monoScript = null;
            if (!isError)
                monoScript = MonoScript.FromMonoBehaviour(comp);
            GameObject.DestroyImmediate(obj);

            if(monoScript == null)
                isError = true;

            string errorStr = string.Empty;
            if (!isError)
            {
                int curExecutionOrder;
                try
                {
                    curExecutionOrder = MonoImporter.GetExecutionOrder(monoScript);
                }
                catch
                {
                    curExecutionOrder = order - 1;
                }

                if (curExecutionOrder != order)
                {
                    try
                    {
                        MonoImporter.SetExecutionOrder(monoScript, order);
                    }
                    catch (Exception e)
                    {
                        errorStr = e.ToString();
                        isError = true;
                    }
                }
            }

            if (isError)
                Debug.LogError($"{typeof(T).Name} 가 오류로 ScriptExecutionOrder 수정못했음\n" + errorStr);

#endif
        }
    }
}
//예시
//#if UNITY_EDITOR
//[UnityEditor.InitializeOnLoadMethod]
//public static void InitializeOnLoad()
//{
//    CWJ.AccessibleEditor.EditorEventSystem.ProjectOpenEvent += EditorEventSystem_ProjectOpenEvent;
//}

//private static void EditorEventSystem_ProjectOpenEvent()
//{
//    CWJ.AccessibleEditor.ScriptExecutionOrder.SetMonoBehaviourOrder<SingletonHelper>(-32000);
//}
//#endif