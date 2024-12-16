#if UNITY_EDITOR

using UnityEngine;
using CWJ.AccessibleEditor;
using UnityEditor.SceneManagement;
using UnityEditor;

namespace CWJ
{
    public static class RuntimeDebuggingToolExtension
    {
        [UnityEditor.InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            CWJ_EditorEventHelper.ReloadedScriptEvent += AutoManageRdt;
            CWJ_EditorEventHelper.EditorWillSaveEvent += (_, _) => AutoManageRdt();
        }

        private static void AutoManageRdt()
        {
            if (CheckRdtModifiedByAutoManage())
            {
                AccessibleEditorUtil.ForceRecompile();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>isMarked</returns>
        public static bool CheckRdtModifiedByAutoManage()
        {
            if (!CWJ_EditorEventHelper.IsProjectOpened)
            {
                return false;
            }

            var rdt = UnityEngine.GameObject.FindObjectOfType<CWJ.RuntimeDebuggingTool>(true);
            bool isMarked = false;
            if (UnityEditor.EditorUserBuildSettings.development)
            {
                if (rdt == null)
                {
                    string name = nameof(RuntimeDebuggingTool);
                    var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath($"Assets/CWJ/Addon/{name}/__UseThisPrefabs/{name}.prefab", typeof(GameObject));
                    if (prefab != null)
                        rdt = (GameObject.Instantiate(prefab) as GameObject).GetComponent<RuntimeDebuggingTool>();
                    isMarked = true;
                }
                if (rdt != null)
                {
                    if (!rdt.gameObject.activeInHierarchy)
                    {
                        if (!rdt.gameObject.activeSelf)
                            rdt.gameObject.SetActive(true);
                        if(rdt.transform.root != rdt.transform)
                            rdt.transform.SetParent(null);
                        isMarked = true;
                    }
                    if (!rdt.isVisibleOnStart)
                    {
                        rdt.isVisibleOnStart = true;
                        isMarked = true;
                    }
                    if (!rdt.isDebuggingEnabled)
                    {
                        rdt.isDebuggingEnabled = true;
                        isMarked = true;
                    }
                    if (!rdt.enabled)
                    {
                        rdt.enabled = true;
                        isMarked = true;
                    }
                }
            }
            else
            {
                if (rdt != null && rdt.gameObject.activeInHierarchy)
                {
                    rdt.gameObject.SetActive(false);
                    isMarked = true;
                }
            }

            return isMarked;
        }
    }
}
#endif
