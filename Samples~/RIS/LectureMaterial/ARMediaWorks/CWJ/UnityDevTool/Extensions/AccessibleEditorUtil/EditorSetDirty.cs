using UnityEngine;

#if UNITY_EDITOR

using UnityEditor.SceneManagement;
using UnityEditor;

#endif

namespace CWJ.AccessibleEditor
{
    /// <summary>
    /// <para>bool변수로 저장을 했었는지 체크하는것으로 중복저장/중복추가를 피할것</para>
    /// <para>내부 변수의 값이 바뀐적없는 프리팹 오브젝트는 게임이 실행될때, 끝날때 프리팹에 저장되어있는 값으로 돌아옴</para>
    /// 프리팹인 녀석 값이 초기화되는것을 막기위해 만든것이 EditorFix임
    /// </summary>
    public static class EditorSetDirty
    {
        public static bool isPlayingOrWillChangePlaymode
        {
            get
            {
#if UNITY_EDITOR
                return EditorApplication.isPlayingOrWillChangePlaymode;
#else
                return Application.isPlaying;
#endif
            }
        }

        public static void SetCreateObjDirty(Object o)
        {
#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(o, "Undo " + o.name);
            EditorUtility.SetDirty(o);
#endif
        }

        public static void SetObjectDirty(Object o)
        {
#if UNITY_EDITOR
            Undo.RecordObject(o, "Undo " + o.name);
            EditorUtility.SetDirty(o);
#endif
        }

        public static void SetObjectDirty<T>(T o, string title = "Undo") where T: Component
        {
#if UNITY_EDITOR
            Undo.RegisterFullObjectHierarchyUndo(o, title + " " + o.name);

            EditorUtility.SetDirty(o);
            if (!isPlayingOrWillChangePlaymode)
            {
                EditorSceneManager.MarkSceneDirty(o.gameObject.scene);
            }
#endif
        }

        public static void SetObjectDirty(Component comp)
        {
#if UNITY_EDITOR
            Undo.RecordObject(comp, "Undo " + comp.gameObject.name + "<" + comp.name + ">");

            EditorUtility.SetDirty(comp);
            if (!isPlayingOrWillChangePlaymode)
            {
                EditorSceneManager.MarkSceneDirty(comp.gameObject.scene);
            }
#endif
        }
    }
}