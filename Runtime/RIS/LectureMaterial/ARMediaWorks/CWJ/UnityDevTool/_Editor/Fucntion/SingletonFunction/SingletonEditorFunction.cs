using CWJ.AccessibleEditor;

using UnityEditor;

using UnityEngine;

namespace CWJ.Singleton.Core
{
    public class SingletonEditorFunction
    {
        private const string MenuItem_SingletonEditorFunction = nameof(CWJ) + "/" + nameof(Singleton);

        [MenuItem(MenuItem_SingletonEditorFunction + "/Find", priority = 100)]
        public static void FindSingleton()
        {
            //var interfaces = FindUtil.FindInterfaces<ISingleton>(includeInactive: true, includeDontDestroyOnLoadObjs: true);
            var singletons = FindUtil.FindObjectsOfType_New<SingletonCore>(true, true);
            var names = new System.Text.StringBuilder();
            var objs = new System.Collections.Generic.List<GameObject>();

            for (int i = 0; i < singletons.Length; ++i)
            {
                if (singletons[i] == null) continue;
                names.AppendLine(singletons[i].GetType().FullName);
                objs.Add(singletons[i].gameObject);
            }

            string message = "";
            if (singletons.Length == 0)
            {
                message = "현재 씬에서 사용중인 Singleton class가 없습니다";
            }
            else
            {
                message = $"현재 씬에서 사용중인 Singleton class 목록 : {singletons.Length}개\n" + names.ToString();
                UnityEditor.Selection.objects = objs.ToArray();
                //UnityEditor.EditorGUIUtility.PingObject(UnityEditor.Selection.activeInstanceID);
            }
            AccessibleEditorUtil.SetHierarchySearchField("", typeName: nameof(SingletonCore));
            DisplayDialogUtil.DisplayDialog(typeof(SingletonCore), message);
        }
    }
}