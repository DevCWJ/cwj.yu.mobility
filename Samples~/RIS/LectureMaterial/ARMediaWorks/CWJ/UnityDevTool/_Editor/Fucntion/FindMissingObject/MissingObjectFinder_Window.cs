using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using UnityEditor;
#if UNITY_2021_3_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif
namespace CWJ.AccessibleEditor.Function
{
    public class MissingObjectFinder_Window : WindowBehaviour<MissingObjectFinder_Window, MissingObjectFinder_ScriptableObject>
    {
        public override string GetScriptableFirstName => "Missing objects finder";

        [MenuItem(MissingObjectFinder_Function.WindowTag_First_Path, priority = 100)]
        public static new void Open()
        {
            OnlyOpen();
        }

        private bool nonSerial;
        private bool serial;
        private bool inScene;
        private bool inPrefabs;

        private Vector2 scrollPos;

        protected override void _OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false, GUILayout.Width(position.width), GUILayout.Height(position.height));

            EditorGUI.indentLevel = 0;

            FoldoutCustomize(ref nonSerial, true, "NonSerialized", NonSerialized);

            GUILayout.Space(10);

            FoldoutCustomize(ref serial, true, "Serializable", Serializable);

            EditorGUILayout.EndScrollView();
        }

        private void NonSerialized()
        {
            int selectedCnt = Selection.transforms.Length;

            DrawLabelField($"Find Missing Objects {(selectedCnt > 0 ? "Among " + selectedCnt + " Selected" : "In This Scene")}");
            if (GUILayout.Button("Find"))
            {
                if (selectedCnt > 0)
                {
                    MissingObjectFinder_Function.QuickLaunch_OnlyFindSelected();
                }
                else
                {
                    MissingObjectFinder_Function.QuickLaunch_OnlyFindScene();
                }
            }
        }

        private void Serializable()
        {
            FoldoutCustomize(ref inScene, true, "In Scene", InScene);

            GUILayout.Space(5);

            FoldoutCustomize(ref inPrefabs, true, "In All Prefabs", InPrefabs);
        }

        private void CreateMissingObjectFinderForScene()
        {
            EditorGUIUtility.PingObject(Selection.activeObject = MissingObjectFindManager.Instance.gameObject);
        }

        private void InScene()
        {
            if (GUILayout.Button("Create Missing Object Find Manager"))
            {
                CreateMissingObjectFinderForScene();
            }
        }

        SerializedProperty _copyCompPrefabProp;
        SerializedProperty copyCompPrefabProp
        {
            get
            {
                if (_copyCompPrefabProp == null) _copyCompPrefabProp = SerializedObj.FindProperty(nameof(ScriptableObj.copyComp_Prefab));
                return _copyCompPrefabProp;
            }
        }

        SerializedProperty _missingObjPrefabProp;
        SerializedProperty missingObjPrefabProp
        {
            get
            {
                if (_missingObjPrefabProp == null) _missingObjPrefabProp = SerializedObj.FindProperty(nameof(ScriptableObj.missingObjs_Prefabs));
                return _missingObjPrefabProp;
            }
        }
        protected override void _OnEnable()
        {

        }

        private void InPrefabs()
        {
            if (ScriptableObj.missingObjs_Prefabs.LengthSafe() == 0)
            {
                DrawLabelField("Find Missing Objects In All Prefabs");
                if (GUILayout.Button("Find"))
                {
                    ScriptableObj.missingObjs_Prefabs = MissingObjectFinder_Function.GetMissingObjsInPrefab();
                }
                return;
            }

            GUILayout.BeginHorizontal();
            DrawLabelField("Past the Copied Component");

            if (ScriptableObj.copyComp_Prefab == null)
            {
                GUILayout.EndHorizontal();

                EditorGUILayout.HelpBox("copyComponent에 component를 넣으려면 : " +
                                        "\n1.빈오브젝트에 복사를 원하는 component 추가 후 프리팹으로 만듬" +
                                        "\n2.Project탭에서 프리팹을 클릭후 Inpsector창에서 해당 component를 드래그로 옮겨서 copyComponent에 할당" +
                                        "\n3.프리팹에 addComponent 하고 Paste", UnityEditor.MessageType.Error);
            }
            else
            {
                if (GUILayout.Button("Paste"))
                {
                    if (DisplayDialogUtil.DisplayDialogReflection($"Warning!\n프리팹들의 Missing Component가 지워집니다\nMissing compoent를 모두 지우고,\ncopyComponent를 붙여넣으시겠습니까?", "Ok", "Cancel"))
                    {
                        MissingObjectFinder_Function.PasteComponentInMissingPrefab(ScriptableObj.missingObjs_Prefabs, ScriptableObj.copyComp_Prefab);
                    }
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.HelpBox("주의! copyComponent를 붙여넣을땐 missingObject가 모두 삭제되어야함\nPaste버튼 누르면 missing component들 모두 삭제되고나서 붙여넣어짐", UnityEditor.MessageType.Warning);
            }

            BeginError(ScriptableObj.copyComp_Prefab == null);
            EditorGUILayout.PropertyField(copyCompPrefabProp, true, GUILayout.ExpandWidth(true));

            EndError();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            DrawLabelField("Missing Objects In All Prefabs");

            if (ScriptableObj.missingObjs_Prefabs[0] == null)
            {
                if (GUILayout.Button("Update missing objects in all prefabs"))
                {
                    ScriptableObj.missingObjs_Prefabs = MissingObjectFinder_Function.GetMissingObjsInPrefab();
                }
            }
            else
            {
                if (GUILayout.Button("Destroy All"))
                {
                    if (DisplayDialogUtil.DisplayDialogReflection($"Warning!\n모든 프리팹에서 Missing Component를 모두 삭제하시겠습니까?", "Ok", "Cancel"))
                    {
                        MissingObjectFinder_Function.DestroyMissingComp(ScriptableObj.missingObjs_Prefabs);
                        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                        if (prefabStage != null)
                        {
                            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(prefabStage.scene);
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(missingObjPrefabProp, true, GUILayout.ExpandWidth(true));

        }
    }

    public class MissingObjectFinder_Function
    {
        #region Menu겸 실행 메소드

        //[MenuItem(Tag_First_Path + Tag_OnlyFind_FindInScene)]
        public static void QuickLaunch_OnlyFindScene()
        {
            FindMissingComponent(FindUtil.GetRootGameObjects_New(false), false);
        }

        //[MenuItem(Tag_First_Path + Tag_OnlyFind_FindInSelected)]
        public static void QuickLaunch_OnlyFindSelected()
        {
            FindMissingComponent(Selection.gameObjects, true);
        }

        #endregion Menu겸 실행 메소드

        public static Transform[] FindTargetTranforms(GameObject[] gameObjects)
        {
            List<Transform> transfroms = new List<Transform>();

            int len = gameObjects.Length;

            for (int i = 0; i < len; ++i)
            {
#if UNITY_2019_1_OR_NEWER
                if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(gameObjects[i]) == 0)
                {
                    Debug.LogWarning(gameObjects[i].name);

                    continue;
                }
#endif
                transfroms.AddRange(gameObjects[i].GetComponentsInChildren<Transform>(true));
            }

            return transfroms.ToArray();
        }

        public static GameObject[] GetMissingObjsInScene(bool isShowLog = true)
        {
            return FindMissingComponent(FindUtil.GetRootGameObjects_New(false), false, isShowLog);
        }

        public static GameObject[] GetMissingObjsInPrefab()
        {
            string[] pathArray = AssetDatabase.FindAssets("t:prefab").ConvertAll(AssetDatabase.GUIDToAssetPath);

            GameObject[] prefabObjs = new GameObject[pathArray.Length];

            for (int i = 0; i < pathArray.Length; ++i)
            {
                prefabObjs[i] = AssetDatabase.LoadAssetAtPath<GameObject>(pathArray[i]);
            }

            return FindMissingComponent(prefabObjs, false);
        }

        public static void DestroyMissingComp(GameObject[] missingCompObjs, bool hasDisplayPopup = true)
        {
            for (int i = 0; i < missingCompObjs.Length; ++i)
            {
#if !UNITY_2019_1_OR_NEWER // TODO: 테스트 필요
                typeof(MissingObjectFindManager).PrintLogWithClassName("19.1 이하 버전은 테스트 필요. 디버깅할것", LogType.Error);
                //Component[] components = missingCompObjs[i].GetComponents<Component>();
                //SerializedObject missingObj = new SerializedObject((UnityEngine.Object)missingCompObjs[i]);
                //missingObj.Update();

                //SerializedProperty compPropArray = missingObj.FindProperty("m_Component");
                //int curObjMissingCnt = 0;
                //for (int j = 0; j < components.Length; j++)
                //{
                //    if (components[j] == null)
                //    {
                //        compPropArray.DeleteArrayElementAtIndex(j - curObjMissingCnt);
                //        ++curObjMissingCnt;

                //        EditorUtility.SetDirty(missingCompObjs[i]);
                //    }
                //}
                //missingObj.ApplyModifiedProperties();
#else 
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(missingCompObjs[i]);
#endif
            }

            UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

#if !UNITY_2019_1_OR_NEWER // TODO: 테스트 필요
            //if (hasDisplayPopup)
            //{
            //    DisplayDialogUtil.DisplayDialogReflection("모든 Missing Component를 삭제했습니다\n안전한 작업을 위해 재시작됩니다", ok: "Ok");
            //    EditorRestartFunction.EditorRestart(.1f);
            //}
#endif
        }

        public static void PasteComponentInMissingScene(GameObject[] pasteObjs, Component copiedComp)
        {
            DestroyMissingComp(pasteObjs, false);

            for (int i = 0; i < pasteObjs.Length; ++i)
            {
                pasteObjs[i].CopyComponent(copiedComp);
                Undo.RegisterCompleteObjectUndo(pasteObjs[i], "Paste Component in Missing");
            }

            DisplayDialogUtil.DisplayDialogReflection("복사한 copyComponent를 missing objects들에 붙여넣기 완료했습니다\n안전한 작업을 위해 재시작됩니다", ok: "Ok");

            //EditorRestartScript.EditorRestart(1);
        }

        public static void PasteComponentInMissingPrefab(GameObject[] pasteObjs, Component copiedComp)
        {
            DestroyMissingComp(pasteObjs, false);

            for (int i = 0; i < pasteObjs.Length; ++i)
            {
                //var path = AssetDatabase.GetAssetPath(pasteObjs[i]);
                //GameObject prefabObj = PrefabUtility.LoadPrefabContents(path);

                if (pasteObjs[i].GetComponent(copiedComp.GetType()))
                {
                    pasteObjs[i].CopyComponent(copiedComp);
                }
                //PrefabUtility.SaveAsPrefabAsset(prefabObj, path);

                //PrefabUtility.UnloadPrefabContents(prefabObj);
            }

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }

            DisplayDialogUtil.DisplayDialogReflection("복사한 copyComponent를 missing objects 프리팹들에게 붙여넣기 완료했습니다\n안전한 작업을 위해 재시작됩니다", ok: "Ok");
            //EditorRestartScript.EditorRestart(1);
        }

        private static GameObject[] FindMissingComponent(GameObject[] rootObjs, bool isSelectedFind, bool isShowLog = true)
        {
            List<GameObject> missingObjList = new List<GameObject>();

            for (int i = 0; i < rootObjs.Length; ++i)
            {
                Transform[] transforms = rootObjs[i].GetComponentsInChildren<Transform>(true);

                for (int j = 0; j < transforms.Length; j++)
                {
#if !UNITY_2019_1_OR_NEWER // TODO: 테스트 필요
                    Component[] components = transforms[j].GetComponents<Component>();

                    for (int k = 0; k < components.Length; k++)
                    {
                        if (components[k] == null)
                        {
                            if (!missingObjList.Contains(transforms[j].gameObject))
                            {
                                Debug.LogError(transforms[j].name + " MISSING exist");
                                missingObjList.Add(transforms[j].gameObject);
                            }
                        }
                    }
#else
                    if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(transforms[j].gameObject) > 0)
                    {
                        if (!missingObjList.Contains(transforms[j].gameObject))
                        {
                            missingObjList.Add(transforms[j].gameObject);
                        }
                    }
#endif
                }
            }

            GameObject[] missingObjs = missingObjList.ToArray();

            if (missingObjs.Length == 0)
            {
                if (isShowLog)
                    DisplayDialogUtil.DisplayDialogReflection($"[RESULT] Couldn't find missing object {(isSelectedFind ? "in these 'select objects'" : "")}", ok: "ok");
            }
            else
            {
                if (isShowLog)
                    DisplayDialogUtil.DisplayDialogReflection("[RESULT] Found " + missingObjs.Length + " objects with missing components\n" + string.Join("\n", Array.ConvertAll(missingObjs, (o) => o.name)), ok: "ok");

                Selection.objects = System.Array.ConvertAll(missingObjs, item => (UnityEngine.Object)item);
                Selection.selectionChanged();
                UnityEditor.EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
            }

            return missingObjs;
        }

        #region Validate Tag 관련

        //[MenuItem(Tag_First_Path + Tag_OnlyFind_FindInScene, true)]
        //private static bool Validate_OnlyFindScene()
        //{
        //    //return Selection.activeTransform == null;
        //    return false;
        //}

        //[MenuItem(Tag_First_Path + Tag_OnlyFind_FindInSelected, true)]
        //private static bool Validate_OnlyFindSelected()
        //{
        //    //return Selection.activeTransform != null;
        //    return false;
        //}

        public const string WindowTag_First_Path = "CWJ/Missing Object Finder";

        //private const string Tag_First_Path = "GameObject/CWJ/FindMissingObjs";
        //private const string Tag_OnlyFind_FindInScene = "OnlyFind/Find missing in this scene!";
        //private const string Tag_OnlyFind_FindInSelected = "OnlyFind/Find missing in selected objects!";

        //private const string Tag_CreateManager = "Create FindMissingManager";

        #endregion Validate Tag 관련
    }
}