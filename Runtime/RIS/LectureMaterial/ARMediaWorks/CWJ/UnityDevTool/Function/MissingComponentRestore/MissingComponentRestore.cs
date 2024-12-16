using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

using CWJ.AccessibleEditor;
#if UNITY_2021_3_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif
#endif

namespace CWJ
{
    /// <summary>
    /// restore시키길 원하는 missing 컴포넌트의 바로 아래에 두기
    /// <para/>씬의 프리팹인 경우 Unpack 되므로 주의하기 (프리팹은 프리팹 수정 스테이지에서 사용하기) =>아직 프리팹은 테스트안해봤음 걍 씬에서 쓰기
    /// <para/>Moving file failed 오류 팝업시 cancel 버튼 누르기 (씬저장 관련 오류같음. 확인필요.)
    /// <br/>cs파일 이름을 클래스 이름과 동일하게 바꾸기
    /// <para/>211017
    /// </summary>
    public class MissingComponentRestore : Singleton.SingletonBehaviour<MissingComponentRestore>, Singleton.IDontSaveInBuild, Singleton.IDontAutoCreatedWhenNull
#if UNITY_EDITOR
        , InspectorHandler.IOnGUIHandler
#endif
    {
#if UNITY_EDITOR

        [SerializeField, GetComponent, Readonly] GameObject missingObject;
        bool hasMissingComp = false;
        string[] compsName;
        [ShowConditional(nameof(hasMissingComp)), StringPopup(nameof(compsName), true), SerializeField] int _1_missingCompIndex;

        bool isSelectedMissingComp = false;
        [ShowConditional(nameof(isSelectedMissingComp)), ErrorIfNull, SerializeField] MonoScript _2_replaceScript;

        bool canUseMissingCompRestore = false;
        [DrawHeaderAndLine("Please Read Description")]
        [Readonly, ResizableTextArea, SerializeField] string description;
        public void CWJEditor_OnGUI()
        {
            //➩➪➫➬➭➮➯➱
            if (EditorApplication.isPlayingOrWillChangePlaymode || missingObject == null)
            {
                canUseMissingCompRestore = false;
                return;
            }

            var prefabInfo = GetPrefabInfo();
            if (prefabInfo.isPrefabObj)
            {
                description = ($"- {missingObject.name} 프리팹을 \nUnpack Prefab Completely 시켜줘야함");
                return;
            }

            Component[] comps = missingObject.GetComponents<Component>();

            if (hasMissingComp = HasMissingComp(missingObject, comps))
            {
                compsName = comps.Where(c => c != this).Select(c => IsMissingComp(c) ? ">> Missing (Mono Script) <<" : c?.GetType()?.Name).ToArray();
            }
            else
            {
                description = ($"- {missingObject.name} 오브젝트에\n Missing Component가 없음.");
            }

            if (hasMissingComp & !(isSelectedMissingComp = hasMissingComp && IsMissingComp(comps[_1_missingCompIndex])))
            {
                description = ("- missingCompIndex 드롭다운을 이용해\n  missingComponent를 선택해주세요.");
            }

            if (isSelectedMissingComp & !(canUseMissingCompRestore = isSelectedMissingComp && _2_replaceScript != null))
            {
                description = ($"- Missing Component를 대체할\n  (Missing Component에서 이름을 바꾼)\n  Script를 {nameof(_2_replaceScript)}에 넣어주세요.");
            }

            if (canUseMissingCompRestore)
                description = $"InvokeButton에서 \n'{nameof(RestoreMissingComp)}()'버튼을 클릭해주세요";
        }

        bool HasMissingComp(GameObject obj, Component[] comps)
        {
            if (comps.Length == 0) return false;

            if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(obj) > 0)
                return true;

            return  comps.IsExists(c => c.IsNullOrMissing() || c.GetType().IsNullOrMissing());
        }

        bool IsMissingComp(Component comp)
        {
            return (comp.IsNullOrMissing() || comp.GetType().IsNullOrMissing());
        }

        (bool isPrefabStage, bool isPrefabObj) GetPrefabInfo()
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            bool isPrefabStage = prefabStage != null;
            bool isPrefabObj = isPrefabStage ? (PrefabUtility.GetOutermostPrefabInstanceRoot(missingObject).Equals(prefabStage.prefabContentsRoot)) :
                                    PrefabUtility.IsPartOfAnyPrefab(missingObject);
            return (isPrefabStage, isPrefabObj);
        }

        [InvokeButton(isNeedUndoNSave:false), ReadonlyConditional(nameof(canUseMissingCompRestore), forPredicateComparison:false)]
        void RestoreMissingComp()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

            Component[] comps = missingObject.GetComponents<Component>();
            if (!HasMissingComp(missingObject, comps))
            {
                typeof(MissingComponentRestore).DisplayDialog($"Missing component doesn't exist in GameObject '{missingObject.name}'", logObj: missingObject);
                return;
            }

            // 0. Determining whether a missingObject is a prefab

            var prefabInfo = GetPrefabInfo();
            bool isPrefabStage = prefabInfo.isPrefabStage;
            bool isPrefabObj = prefabInfo.isPrefabObj;

            if (isPrefabObj && !isPrefabStage)
            {
                Undo.RegisterCompleteObjectUndo(missingObject, "MissingObject Modified");
                PrefabUtility.UnpackPrefabInstance(PrefabUtility.GetOutermostPrefabInstanceRoot(missingObject), PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                //^ 1. 씬의 프리팹이면 좀 귀찮아서 걍 Unpack시킴ㅋㅋ
            }

            string missingCompGUID;

            string missingCompNestMetaDataPath = isPrefabObj && isPrefabStage ? PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(missingObject)
                                                                                : missingObject.gameObject.scene.path;

            // 2. Find missing component's GUID
            bool isSuccess = false;
            string objOriginName;
            if (FindMissingCompGUID(missingCompNestMetaDataPath, _1_missingCompIndex, out missingCompGUID, out objOriginName))
            {
                missingObject.name = objOriginName;
                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(missingObject.scene, missingObject.scene.path);

                var newScriptGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_2_replaceScript));

                var regeneratedAssets = new List<(string assetName, List<string> updatedPaths)>(1);
                var skippedAssets = new List<(string assetName, string skipReason)>(1);

                AssetDatabase.StartAssetEditing();

                // 3. Replace GUID using GUIDRegenerator
                if (GUIDRegenerator.ReplaceGUID(missingCompGUID, newScriptGUID, ref regeneratedAssets, ref skippedAssets, isForMissingComp: true))
                {
                    GUIDRegenerator.PrintLog(regeneratedAssets, skippedAssets);
                    Debug.Log("Success!!");
                    isSuccess = true;
                }
                else
                {
                    Debug.LogError("Error!!");
                }

                AssetDatabase.StopAssetEditing();

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                missingObject.name = objOriginName;
                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(missingObject.scene, missingObject.scene.path);
            }


            if (!isSuccess)
            {
                _OnValidate();
            }
        }

         const string NameFieldMatch = "  m_Name: ";
         const string CompHeaderMatch = "  m_Component:";
         const string SplitSign = "--- ";
         const string CompFieldMatch = "  - component: {fileID: ";
         readonly int CompFieldMatchLength = CompFieldMatch.Length;
         const string ScriptFieldMatch = "  m_Script: {";

        bool FindMissingCompGUID(string metaDataPath, int missingCompIndex, out string missingCompGUID, out string objOriginName)
        {
            missingCompGUID = null;

            // 0. Rename an object with the missing component to a unique name

            objOriginName = missingObject.name;
            string objUniqueName = objOriginName + "_" + missingObject.GetInstanceID();
            missingObject.name = objUniqueName;
            
            if (!UnityEditor.SceneManagement.EditorSceneManager.SaveScene(missingObject.scene, missingObject.scene.path))
            {
                Debug.LogError("현재 씬을 저장 할 수 있어야합니다");
                return false;
            }

            string[] sceneMetaData = System.IO.File.ReadAllLines(metaDataPath);

            int metaDataLength = sceneMetaData.Length;

            // 1. Find object by name in scene's meta data

            int nameLineIndex = sceneMetaData.FindIndex(s => s.Equals((NameFieldMatch + objUniqueName)));

            if (nameLineIndex < 0)
            {
                Debug.LogError("Not found nameLineIndex");
                return false;
            }

            // 2. Find the component line number of the object you found. and FileID is identified by adding the index of the missing component to the line number.

            int compHeaderLineIndex = sceneMetaData.LastIndexOf(s => s.Equals(CompHeaderMatch), nameLineIndex, 0, breakMatch: (s) => s.StartsWith(SplitSign));
            if (compHeaderLineIndex < 0)
            {
                Debug.LogError("Not found compHeaderLineIndex");
                return false;
            }

            int missingCompFileIDLineIndex = compHeaderLineIndex + 1 + missingCompIndex;
            string missingCompFileIdLine = sceneMetaData[missingCompFileIDLineIndex];
            if (!missingCompFileIdLine.StartsWith(CompFieldMatch))
            {
                Debug.LogError("Not found missingCompFileIdLineIndex");
                return false;
            }

            // 3. Find the line number where the data about the missing component by the missing component's FileID.

            string missingCompFileID = "&" + missingCompFileIdLine.Substring(CompFieldMatchLength, missingCompFileIdLine.Length - CompFieldMatchLength - 1);

            int missingCompHeaderLineIndex = sceneMetaData.IndexOf(s => s.StartsWith(SplitSign) && s.EndsWith(missingCompFileID), startIndex: 0, endIndex: metaDataLength);

            if (missingCompHeaderLineIndex < 0)
            {
                Debug.LogError("Not found missingCompHeaderLineIndex");
                return false;
            }

            // 4. Extract the GUID from the 'm_Script' field of the missing component

            int missingCompScriptFieldLineIndex = sceneMetaData.IndexOf(s => s.StartsWith(ScriptFieldMatch) && s.Contains("guid: "), startIndex: missingCompHeaderLineIndex, metaDataLength);

            if (missingCompScriptFieldLineIndex < 0)
            {
                Debug.LogError("Not found missingCompScriptFieldLineIndex");
                return false;
            }

            missingCompGUID = sceneMetaData[missingCompScriptFieldLineIndex];
            missingCompGUID = (missingCompGUID.Split(new string[] { "guid: " }, System.StringSplitOptions.RemoveEmptyEntries)[1]).Split(',')[0];

            Debug.Log($"Missing Component's target script path is " + AssetDatabase.GUIDToAssetPath(missingCompGUID) + "\n" + missingCompGUID);

            return true;
        }

#else

        protected override void _Awake()
        {
            Destroy(this);
        }

#endif
        }
}


//조금은 불편했어도 이름도 잘바뀌고 Scene Save오류도 안나고 깔끔하게 잘되던 구버전 MissingComponentRestore
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;

//using UnityEngine;
//#if UNITY_EDITOR
//using UnityEditor;
//using UnityEditor.Experimental.SceneManagement;
//using CWJ.AccessibleEditor;
//#endif

//namespace CWJ
//{
//    /// <summary>
//    /// restore시키길 원하는 missing 컴포넌트의 바로 아래에 두기
//    /// <para/>씬의 프리팹인 경우 Unpack 되므로 주의하기 (프리팹은 프리팹 수정 스테이지에서 사용하기
//    /// </summary>
//    public class MissingComponentRestore : Singleton.SingletonBehaviour<MissingComponentRestore>
//#if UNITY_EDITOR
//        , InspectorHandler.IOnGUIHandler
//#endif
//    {
//#if UNITY_EDITOR
//        protected override void _Reset()
//        {
//            if (!hideFlags.HasFlag(HideFlags.DontSaveInBuild))
//                hideFlags |= HideFlags.DontSaveInBuild;
//        }

//        [SerializeField] GameObject missingObject;
//        [SerializeField] MonoScript replaceScript;

//        [ResizableTextArea, SerializeField]
//        string description;
//        public void CWJEditor_OnGUI()
//        {
//            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

//            var strBuilder = new System.Text.StringBuilder(369);

//            if (!HasMissingComp(missingObject))
//            {
//                strBuilder.AppendLine($"- {missingObject.name} 오브젝트에 Missing Component가 없음.\n");
//            }
//            else
//            {
//                if (GetMissingCompIndex() == -1)
//                {
//                    strBuilder.AppendLine("- Missing Component의 아래에 이 Component를 위치시켜 주세요.\n");
//                }
//            }

//            if (replaceScript == null)
//            {
//                strBuilder.AppendLine($"- Missing Component를 대체할(Missing Component에서 이름을 바꾼)\n\tScript를 {nameof(replaceScript)}에 넣어주세요.\n");
//            }

//            description = strBuilder.ToString();
//            if (string.IsNullOrEmpty(description))
//            {
//                description = $"{nameof(InvokeButtonAttribute)}로 '{nameof(RestoreMissingComp)}()'를 실행해주세요";
//            }
//        }

//        bool HasMissingComp(GameObject obj)
//        {
//            if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(obj) > 0)
//                return true;

//            var comps = obj.GetComponents<Component>();
//            if (comps.Length == 0) return false;

//            return comps.IsExists(c => c.IsNullOrMissing() || c.GetType().IsNullOrMissing());
//        }

//        int GetMissingCompIndex()
//        {
//            int missingCompIndex = -1;
//            var comps = missingObject.GetComponents<Component>();
//            var thisCompIndex = comps.IndexOf(this);
//            if (comps[thisCompIndex - 1].IsNullOrMissing() || comps[thisCompIndex - 1].GetType().IsNullOrMissing())
//            {
//                missingCompIndex = thisCompIndex - 1;
//            }
//            return missingCompIndex;
//        }

//        [InvokeButton]
//        public void RestoreMissingComp()
//        {
//            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

//            if (!HasMissingComp(missingObject))
//            {
//                typeof(MissingComponentRestore).DisplayDialog($"Missing component doesn't exist in GameObject '{missingObject.name}'", logObj: missingObject);
//                return;
//            }

//            int missingCompIndex = GetMissingCompIndex();

//            if (missingCompIndex == -1)
//            {
//                typeof(MissingComponentRestore).DisplayDialog($"Please move this {nameof(MissingComponentRestore)} under the Missing Componet.", logObj: missingObject);
//                return;
//            }

//            // 0. Determining whether a missingObject is a prefab

//            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
//            bool isPrefabStage = prefabStage != null;
//            bool isPrefabObj = isPrefabStage ? (PrefabUtility.GetOutermostPrefabInstanceRoot(missingObject).Equals(prefabStage.prefabContentsRoot)) :
//                                    PrefabUtility.IsPartOfAnyPrefab(missingObject);

//            if (isPrefabObj && !isPrefabStage)
//            {
//                Undo.RegisterCompleteObjectUndo(missingObject, "MissingObject Modified");
//                PrefabUtility.UnpackPrefabInstance(PrefabUtility.GetOutermostPrefabInstanceRoot(missingObject), PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
//                //^ 1. 씬의 프리팹이면 좀 귀찮아서 걍 Unpack시킴ㅋㅋ
//            }

//            string missingCompGUID;

//            string missingCompNestMetaDataPath = isPrefabObj && isPrefabStage ? PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(missingObject)
//                                                                                : missingObject.gameObject.scene.path;

//            // 2. Find missing component's GUID
//            bool isSuccess = false;

//            if (FindMissingCompGUID(missingCompNestMetaDataPath, missingCompIndex, out missingCompGUID))
//            {
//                var newScriptGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(replaceScript));

//                var regeneratedAssets = new List<(string assetName, List<string> updatedPaths)>(1);
//                var skippedAssets = new List<(string assetName, string skipReason)>(1);

//                AssetDatabase.StartAssetEditing();

//                // 3. Replace GUID using GUIDRegenerator
//                if (GUIDRegenerator.ReplaceGUID(missingCompGUID, newScriptGUID, ref regeneratedAssets, ref skippedAssets, isForMissingComp: true))
//                {
//                    GUIDRegenerator.PrintLog(regeneratedAssets, skippedAssets);
//                    Debug.Log("Success!!");
//                    isSuccess = true;
//                }
//                else
//                {
//                    Debug.LogError("Error!!");
//                }

//                AssetDatabase.StopAssetEditing();
//                AssetDatabase.SaveAssets();
//                AssetDatabase.Refresh();
//            }

//            if (!isSuccess)
//            {
//                _OnValidate();
//            }
//        }

//        const string NameFieldMatch = "  m_Name: ";
//        const string CompHeaderMatch = "  m_Component:";
//        const string SplitSign = "--- ";
//        const string CompFieldMatch = "  - component: {fileID: ";
//        readonly int CompFieldMatchLength = CompFieldMatch.Length;
//        const string ScriptFieldMatch = "  m_Script: {";

//        bool FindMissingCompGUID(string metaDataPath, int missingCompIndex, out string missingCompGUID)
//        {
//            missingCompGUID = null;

//            // 0. Rename an object with the missing component to a unique name

//            string objOriginName = missingObject.name;
//            string objUniqueName = objOriginName + "_" + missingObject.GetInstanceID();
//            missingObject.name = objUniqueName;

//            if (!UnityEditor.SceneManagement.EditorSceneManager.SaveScene(missingObject.scene, missingObject.scene.path))
//            {
//                missingObject.name = objOriginName;
//                Debug.LogError("현재 씬을 저장 할 수 있어야합니다");
//                return false;
//            }

//            string[] sceneMetaData = System.IO.File.ReadAllLines(metaDataPath);

//            missingObject.name = objOriginName;
//            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(missingObject.scene, missingObject.scene.path);

//            int metaDataLength = sceneMetaData.Length;

//            // 1. Find object by name in scene's meta data

//            int nameLineIndex = sceneMetaData.FindIndex(s => s.Equals((NameFieldMatch + objUniqueName)));

//            if (nameLineIndex < 0)
//            {
//                Debug.LogError("Not found nameLineIndex");
//                return false;
//            }

//            // 2. Find the component line number of the object you found. and FileID is identified by adding the index of the missing component to the line number.

//            int compHeaderLineIndex = sceneMetaData.LastIndexOf(s => s.Equals(CompHeaderMatch), nameLineIndex, 0, breakMatch: (s) => s.StartsWith(SplitSign));
//            if (compHeaderLineIndex < 0)
//            {
//                Debug.LogError("Not found compHeaderLineIndex");
//                return false;
//            }

//            int missingCompFileIDLineIndex = compHeaderLineIndex + 1 + missingCompIndex;
//            string missingCompFileIdLine = sceneMetaData[missingCompFileIDLineIndex];
//            if (!missingCompFileIdLine.StartsWith(CompFieldMatch))
//            {
//                Debug.LogError("Not found missingCompFileIdLineIndex");
//                return false;
//            }

//            // 3. Find the line number where the data about the missing component by the missing component's FileID.

//            string missingCompFileID = "&" + missingCompFileIdLine.Substring(CompFieldMatchLength, missingCompFileIdLine.Length - CompFieldMatchLength - 1);

//            int missingCompHeaderLineIndex = sceneMetaData.IndexOf(s => s.StartsWith(SplitSign) && s.EndsWith(missingCompFileID), startIndex: 0, endIndex: metaDataLength);

//            if (missingCompHeaderLineIndex < 0)
//            {
//                Debug.LogError("Not found missingCompHeaderLineIndex");
//                return false;
//            }

//            // 4. Extract the GUID from the 'm_Script' field of the missing component

//            int missingCompScriptFieldLineIndex = sceneMetaData.IndexOf(s => s.StartsWith(ScriptFieldMatch) && s.Contains("guid: "), startIndex: missingCompHeaderLineIndex, metaDataLength);

//            if (missingCompScriptFieldLineIndex < 0)
//            {
//                Debug.LogError("Not found missingCompScriptFieldLineIndex");
//                return false;
//            }

//            missingCompGUID = sceneMetaData[missingCompScriptFieldLineIndex];
//            missingCompGUID = (missingCompGUID.Split(new string[] { "guid: " }, System.StringSplitOptions.RemoveEmptyEntries)[1]).Split(',')[0];

//            Debug.Log($"Missing Component's target script path is " + AssetDatabase.GUIDToAssetPath(missingCompGUID) + "\n" + missingCompGUID);

//            return true;
//        }

//        [InvokeButton]
//        void MoveCompUpOrDown(bool upOrDown)
//        {
//            AccessibleEditor.AccessibleEditorUtil.ComponentMoveInInspector(this, upOrDown);
//        }

//        [InvokeButton]
//        void MoveCompTopOrBottom(bool topOrBottom)
//        {
//            AccessibleEditor.AccessibleEditorUtil.ComponentMoveTopOrBottom(this, topOrBottom);
//        }

//#else

//        protected override void _Awake()
//        {
//            Destroy(this);
//        }

//#endif
//    }
//}
