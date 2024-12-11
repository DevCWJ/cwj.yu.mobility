using UnityEngine;

using CWJ.Singleton;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace CWJ.AccessibleEditor.Function
{
    public class MissingObjectFindManager : SingletonBehaviour<MissingObjectFindManager>, IDontSaveInBuild
    {
        public GameObject[] missingObjs = new GameObject[0];
        public Component copyComp;

        protected override void _Awake()
        {
#if UNITY_EDITOR
            DisplayDialogUtil.DisplayDialogReflection("MissingObjectFindManager는 사용후 삭제해주세요", ok: "ok");

            EditorGUIUtility.PingObject(Selection.activeObject = gameObject);
            Selection.selectionChanged();
            UnityEditor.EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
#else
            Destroy(this);
#endif
        }
    }
}