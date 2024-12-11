using System;
using System.Linq;

using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    public class AssetReferenceFinder : WindowBehaviour<AssetReferenceFinder, AssetReferenceData_ScriptableObject>
    {
        public override string GetScriptableFirstName => FirstName;

        public const string FirstName = "Find References";
        public const string MenuName = "CWJ/" + FirstName;
        public const string SelectObjMenu = "GameObject/" + FirstName;
        public const string SelectObjMenu2 = "Assets/" + FirstName;


        //[MenuItem(SelectObjMenu, true)]
        private static bool Validate_SelectionObjExists()
        {
            return Selection.activeObject != null || Selection.activeTransform != null;
        }

        //[MenuItem(MenuName, priority = 100)]
        //[MenuItem(SelectObjMenu, false, 0)]
        //[MenuItem(SelectObjMenu2, false, 20)]
        public new static void Open()
        {
            FindReferences(Selection.activeObject);
            OnlyOpen(minSize: new Vector2(400, 500), maxSize: new Vector2(1000, 425));
        }

        protected override void _OnEnable()
        {
            EditorApplication.hierarchyChanged += UpdateFindRef;
        }

        protected override void _OnDisable()
        {
            EditorApplication.hierarchyChanged -= UpdateFindRef;
        }

        protected override void _OnReloadedWhileOpened()
        {
            UpdateFindRef();
        }

        protected override void _OnSceneOpenedWhileOpened()
        {
            UpdateFindRef();
        }

        private void UpdateFindRef()
        {
            FindReferences(ScriptableObj.targetObj);
            Repaint();
        }

        private static void FindReferences(UnityObject targetObj)
        {
            var scriptableObj = ScriptableObj;
            scriptableObj.references.Clear();

            if (Selection.activeObject == null) return;

            scriptableObj.targetObj = targetObj;

            foreach (UnityObject obj in Resources.FindObjectsOfTypeAll<UnityObject>())
            {
                if (obj.GetType() == typeof(AssetReferenceData_ScriptableObject)) continue;
                if (obj != targetObj && (AssetDatabase.IsMainAsset(obj) || !EditorUtility.IsPersistent(obj) && obj is GameObject)) // EditorUtility.IsPersistent / AssetDatabase.Contains 차이 잘 모르겟
                {
                    foreach (UnityObject dependency in EditorUtility.CollectDependencies(new[] { obj }))
                    {
                        if (dependency == targetObj)
                        {
                            scriptableObj.references.Add(obj);
                        }
                    }
                }
            }
        }

        Vector2 scrollPos;
        bool isFoldoutInAsset, isFoldoutInScene;
        protected override void _OnGUI()
        {
            var scriptableObj = ScriptableObj;
            if (scriptableObj.targetObj == null) { return; }

            System.Type unityObjType = typeof(UnityObject);
            EditorGUILayout.BeginHorizontal();
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                scriptableObj.targetObj = EditorGUILayout.ObjectField("", scriptableObj.targetObj, unityObjType, true, GUILayout.ExpandWidth(true));
                if (changeScope.changed)
                {
                    UpdateFindRef();
                    return;
                }
            }

            if (GUILayout.Button("Update target's references", GUILayout.ExpandWidth(true)))
            {
                UpdateFindRef();
                return;
            }
            EditorGUILayout.EndHorizontal();

            var groups = from r in scriptableObj.references
                         group r by EditorUtility.IsPersistent(r) into g
                         select (isAsset: g.Key, list: g);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            foreach (var g in groups)
            {
                string title = $" ({g.list.Count()})";

                Action drawObjField = () =>
                {
                    foreach (UnityObject obj in g.list)
                    {
                        EditorGUILayout.ObjectField("", obj, unityObjType, true);
                    }
                };

                if (g.isAsset)
                {
                    FoldoutCustomize(ref isFoldoutInAsset, true, "In Assets" + title, drawObjField);
                }
                else
                {
                    FoldoutCustomize(ref isFoldoutInScene, true, "In this Scene" + title, drawObjField);
                }
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndScrollView();
        }
    }
}