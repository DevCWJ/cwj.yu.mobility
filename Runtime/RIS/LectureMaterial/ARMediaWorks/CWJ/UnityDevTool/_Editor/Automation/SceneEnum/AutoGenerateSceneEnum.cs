using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using CWJ.AccessibleEditor;

using UnityEditor;

using UnityEngine;

using static CWJ.SceneHelper.SceneEnumDefine;

namespace CWJ.SceneHelper.Editor
{
    /// <summary>
    /// SceneEnum 자동 생성/편집기
    /// <para/>ver10.7 [20.01.31]
    /// </summary>
    public class AutoGenerateSceneEnum
    {
        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
#if (!CWJ_SCENEENUM_ENABLED && !CWJ_SCENEENUM_DISABLED)
            CWJ_EditorEventHelper.ReloadedScriptEvent += Initialize;
#elif CWJ_SCENEENUM_ENABLED
            BuildEventSystem.BeforeBuildEvent += SyncSceneEnum;
            CWJ_EditorEventHelper.UnityProjectChangedEvent += SyncSceneEnum;
            CWJ_EditorEventHelper.ReloadedScriptSafeEvent += SyncSceneEnum;
            CWJ_EditorEventHelper.EditorWillSaveEvent += (target, isModified) => { if (isModified) SyncSceneEnum(); };
            CWJ_EditorEventHelper.PlayModeStateChangedEvent += (state) => { if (state == PlayModeStateChange.EnteredPlayMode) SyncSceneEnum(); };
            CWJ_EditorEventHelper.EditorSceneOpenedEvent += (s) => SyncSceneEnum();
            CWJ_EditorEventHelper.FolderChangedEvent += SyncSceneEnum;
            EditorBuildSettings.sceneListChanged += StartDelaySync;
#endif
        }

        private static void Initialize() //To prevent being 'whw' in old versions
        {
            if (ScriptableObj == null) return;
            ScriptableObj.OnReset();
            SyncSceneEnum();
            EditorCallback.AddWaitForFrameCallback(() => DefineSymbolUtil.AddCustomDefineSymbol(CWJ_SCENEENUM_ENABLED, true));
        }

        private static AutoGenerateSceneEnum_ScriptableObject _ScriptableObj = null;

        public static AutoGenerateSceneEnum_ScriptableObject ScriptableObj
        {
            get
            {
                if (_ScriptableObj == null)
                {
                    _ScriptableObj = ScriptableObjectStore.Instanced.GetScriptableObj<AutoGenerateSceneEnum_ScriptableObject>();
                }
                return _ScriptableObj;
            }
        }

        private static EditorCallback.EditorCallbackStruct delayCallback = null;

        //테스트 더 필요
        private static void StartDelaySync()
        {
            if (delayCallback != null)
            {
                EditorCallback.RemoveEditorCallback(delayCallback);
                delayCallback = null;
            }

            delayCallback = EditorCallback.AddWaitForSecondsCallback(SyncSceneEnum, 5); //Start sync after 5 seconds
        }

        public static void SyncSceneEnum()
        {
            if (delayCallback != null)
            {
                EditorCallback.RemoveEditorCallback(delayCallback);
                delayCallback = null;
            }

            bool isExists = File.Exists(PATH);

            bool isNeedUpdate = !isExists; //Do i need to do update 'SceneEnum.cs'?
            UpdateSceneNameList(ref isNeedUpdate);

            if (!isNeedUpdate)
            {
                return;
            }

#pragma warning disable
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);

            try
            {
                using (FileStream fileStream = isExists ? File.Open(PATH, FileMode.Truncate, FileAccess.Write) : File.Create(PATH))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8))
                    {
                        System.Func<bool, string, string[], int, string> getWriteEnumCode =
                        (bool isPublic, string name, string[] scenes, int indentLength) =>
                        $"{(isPublic ? "public" : "private")} enum " + name + "\n{ \n"
                        + string.Join(",\n", scenes.ModifyEach((s) => ConvertValidChar(s, true))) + "\n}";

                        StringBuilder stringBuilder = new StringBuilder();

                        stringBuilder.AppendLine($"namespace {nameof(CWJ)}.{nameof(SceneHelper)}");

                        stringBuilder.AppendLine("{");

                        stringBuilder.AppendLine(Summary_OnlyEnabled);
                        stringBuilder.AppendLine(getWriteEnumCode(true, EnumName_BuildSceneEnum, ScriptableObj?.enableScenes
                                                , 1));
                        //^Same as scene list at runtime after build

                        stringBuilder.AppendLine(Summary_OnlyDisabledClass);
                        stringBuilder.AppendLine("public class " + ClassName_IncludeDisabledScene);
                        stringBuilder.AppendLine("{");

                        stringBuilder.AppendLine(Summary_All_SortedByBuildSettings);
                        stringBuilder.AppendLine(getWriteEnumCode(true, EnumName_SortedByBuildSettings, ScriptableObj?.editorAllScenes
                                                , 2));
                        //^Just for comparison with >>Build Settings/Scene In Build<<

                        stringBuilder.AppendLine(Summary_All_SortedByBuildIndex);
                        stringBuilder.AppendLine(getWriteEnumCode(true, EnumName_SortedByBuildIndex, ScriptableObj?.disableScenes?.Length > 0 ? ArrayUtil.Merge(ScriptableObj?.enableScenes, ScriptableObj?.disableScenes) : ScriptableObj?.enableScenes
                                                , 2));
                        //^include disabled Scene Enum.. sorted by sceneBuildIndex

                        stringBuilder.AppendLine(Summary_OnlyDisabledEnum);
                        stringBuilder.AppendLine(getWriteEnumCode(true, EnumName_DisabledSceneEnum, ScriptableObj?.disableScenes
                                                , 2));

                        stringBuilder.AppendLine("}");

                        stringBuilder.AppendLine("}");

                        streamWriter.Write(stringBuilder.ToString().SetAutoInsertIndent(0));
                    }
                }
                AssetDatabase.Refresh();
            }
            catch
            {
            }
            finally
            {
                Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.ScriptOnly);
            }
#pragma warning restore
        }

        public static void UpdateSceneNameList(ref bool isNeedUpdate)
        {
            if (ScriptableObj == null)
            {
                isNeedUpdate = false;
                return;
            }

            var allSceneStructs = EditorBuildSettings.scenes.GroupBy(g => new { name = Path.GetFileNameWithoutExtension(g.path) })
                                                            .Select(n => new { n.Key.name, scene = n, length = n.Count() }).ToArray();

            List<string> allSceneNameList = new List<string>();
            List<string> enableNameList = new List<string>();
            List<string> disableNameList = new List<string>();

            for (int i = 0; i < allSceneStructs.Length; i++)
            {
                if (string.IsNullOrEmpty(allSceneStructs[i].name.RemoveAllSpaces()))
                {
                    continue;
                }

                if (allSceneStructs[i].length > 1)
                {
                    AccessibleEditorUtil.OpenBuildSettings();

                    AccessibleEditorUtil.SetProjectSearchField(allSceneStructs[i].name, typeName: "Scene");
                    UnityEngine.Object[] objects = System.Array.ConvertAll(allSceneStructs[i].scene.ToArray(), (s) => AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s.path));
                    Selection.objects = objects;
#if CWJ_SCENEENUM_ENABLED                      
                    DebugLogUtil.PrintLogWithClassName(typeof(SceneEnum),
                        new DuplicateNameException($"'{allSceneStructs[i].name}' scene is duplicated in the build settings!"), isPreventStackTrace: true);
#endif
                    isNeedUpdate = false;
                    return;
                }

                string sceneName = allSceneStructs[i].name;

                allSceneNameList.Add(sceneName);

                foreach (var item in allSceneStructs[i].scene)
                {
                    if (item.enabled)
                    {
                        enableNameList.Add(sceneName);
                    }
                    else
                    {
                        disableNameList.Add(sceneName);
                    }
                    break;
                }
            }

            if (enableNameList.Count == 0)
            {
                string activeSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                if (!string.IsNullOrEmpty(activeSceneName.RemoveAllSpaces()))
                {
                    enableNameList.Add(activeSceneName);
                    if (!allSceneNameList.Contains(activeSceneName))
                    {
                        allSceneNameList.Add(activeSceneName);
                    }
                    if (disableNameList.Contains(activeSceneName))
                    {
                        disableNameList.Remove(activeSceneName);
                    }
                }
            }

            string[] enableScenes = enableNameList.ToArray();

            string[] disableScenes = disableNameList.ToArray();

            string[] editorAllScenes = allSceneNameList.ToArray();

            bool isEnableScenesEquals = false, isAllScenesEquals = false;

            if (ScriptableObj.isInitialized)
            {
                isEnableScenesEquals = enableScenes.ArrayEquals(ScriptableObj.enableScenes);

                isAllScenesEquals = editorAllScenes.ArrayEquals(ScriptableObj.editorAllScenes);

                if (isEnableScenesEquals && isAllScenesEquals)
                {
                    isNeedUpdate |= false;
                    return;
                }
            }

            if (!isEnableScenesEquals)
            {
                ArrayUtil.Copy(out ScriptableObj.enableScenes, enableScenes);
            }
            if (!isAllScenesEquals)
            {
                ArrayUtil.Copy(out ScriptableObj.editorAllScenes, editorAllScenes);
            }

            ArrayUtil.Copy(out ScriptableObj.disableScenes, disableScenes);

            EditorUtility.SetDirty(ScriptableObj);

            ScriptableObj.isInitialized = true;

            isNeedUpdate = true;
            return;
        }

        const string SceneEnumName = "Auto Generate Scene Enum";

#if CWJ_SCENEENUM_DISABLED
        [MenuItem(nameof(CWJ) + "/" + SceneEnumName + "/Enable Automatically Sync " + SceneEnumName, priority = 100)]
#endif
        public static void EnableSyncSceneEnumAndOpenAsset()
        {
            AccessibleEditor.CustomDefine.CustomDefine_Window.ScriptableObj.isSceneEnumSync = true;
            AccessibleEditorUtil.OpenScriptViaName(ScriptName_SceneEnum);
            AccessibleEditor.CustomDefine.CustomDefine_Window.UpdateSettingAndOpen();
            SyncSceneEnum();
        }

#if CWJ_SCENEENUM_DISABLED
        [MenuItem(nameof(CWJ) + "/" + SceneEnumName + "/Only once update " + SceneEnumName, priority = 100)]
#endif
        public static void UpdateSceneEnumAndOpenAsset()
        {
            AccessibleEditorUtil.OpenScriptViaName(ScriptName_SceneEnum);
            SyncSceneEnum();
        }
    }
}