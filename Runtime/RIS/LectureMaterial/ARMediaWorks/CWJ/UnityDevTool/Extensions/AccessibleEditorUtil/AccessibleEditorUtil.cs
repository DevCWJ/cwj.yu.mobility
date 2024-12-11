using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

using UnityObject = UnityEngine.Object;

namespace CWJ.AccessibleEditor
{
    /// <summary>
    /// #if UNITY_EDITOR 하던 안하던 빌드 시 오류나지않음
    /// </summary>
    public static class InspectorHandler
    {
        public interface ISelectHandler
        {
#if UNITY_EDITOR
            /// <summary>
            /// <see cref="ISelectHandler"/>를 선언한 MonoBehaviour가 Inspector에서 보여질때 <see cref="CWJEditor_Inspector_OnEnable"/> 가 실행됨
            /// <para>Affecting : 복수 오브젝트</para>
            /// </summary>
            /// <param name="target">인스펙터에 그려지고있는 단일 대상</param>
            void CWJEditor_OnSelect(MonoBehaviour target);
#endif
        }

        public interface IOnGUIHandler
        {
#if UNITY_EDITOR
            /// <summary>
            /// <see cref="IOnGUIHandler"/>를 선언한 MonoBehaviour가 Inspector에 그려지고있을때 반복해서 실행 <see cref="CWJEditor_OnGUI"/> 가 실행됨
            /// <para>Affecting : 단일 오브젝트</para>
            /// </summary>
            void CWJEditor_OnGUI();
#endif
        }

        public interface IDeselectHandler
        {
#if UNITY_EDITOR
            /// <summary>
            /// <see cref="ISelectHandler"/>를 선언한 MonoBehaviour가 Hierarchy에서 클릭이 해제되면 <see cref="CWJEditor_OnDeselect"/> 가 실행됨
            /// <para>Affecting : 복수 오브젝트</para>
            /// </summary>
            /// <param name="target">인스펙터에 그려지고있던 단일 대상</param>
            void CWJEditor_OnDeselect(MonoBehaviour target);
#endif
        }

        public interface ICompiledHandler
        {
#if UNITY_EDITOR
            /// <summary>
            /// <see cref="ICompiledHandler"/>를 선언한 MonoBehaviour가 Inspector에서 보여지던 중에 컴파일 됐을때 <see cref="CWJEditor_OnCompile"/> 가 실행됨
            /// <para>Affecting : 단일 오브젝트</para>
            /// </summary>
            void CWJEditor_OnCompile();
#endif
        }

        public interface IDestroyHandler
        {
#if UNITY_EDITOR
            /// <summary>
            /// <see cref="IDestroyHandler"/>를 선언한 MonoBehaviour가 Inspector에서 보여지던 중에 삭제 됐을때 <see cref="CWJEditor_OnDestroy"/> 가 실행됨
            /// <para>Affecting : 복수 오브젝트 + 자식 오브젝트</para>
            /// </summary>
            void CWJEditor_OnDestroy();
#endif
        }
    }

    public static class AccessibleEditorUtil
    {

        public static bool IsAppFocused
        {
            get
            {
#if UNITY_EDITOR
                return UnityEditorInternal.InternalEditorUtility.isApplicationActive;
#else
                return Application.isFocused;
#endif
            }
        }

#if UNITY_EDITOR
        public static GameObject[] FindStaticsByFlags(StaticEditorFlags flagsFilter)
        {
            return FindUtil.FindGameObjects(true, true, predicate: (obj) => GameObjectUtility.AreStaticEditorFlagsSet(obj, flagsFilter));
        }

        public static IEnumerable<StaticEditorFlags> GetEditorStaticFlags(GameObject obj)
        {
            return System.Enum.GetValues(typeof(StaticEditorFlags)).Cast<StaticEditorFlags>()
               .Where(f => GameObjectUtility.GetStaticEditorFlags(obj).HasFlag(f));
        }

        /// <summary>
        /// 인스펙터에서 위로이동
        /// </summary>
        /// <param name="targetComp"></param>
        public static void ComponentMoveInInspector(Component targetComp, bool isUp)
        {
            if (isUp)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentUp(targetComp);
            }
            else
            {
                UnityEditorInternal.ComponentUtility.MoveComponentDown(targetComp);
            }
        }

        public static void ComponentMoveTopOrBottom(Component targetComp, bool isUp)
        {
            Func<bool> moveFunc = (isUp ? new Func<bool>(() => UnityEditorInternal.ComponentUtility.MoveComponentUp(targetComp)) : new Func<bool>(() => UnityEditorInternal.ComponentUtility.MoveComponentDown(targetComp)));
            
            while (moveFunc()) { }
        }
#endif

        public static void GizmosDrawCrossLine(Vector3 centerPoint, Color color, float crossLength = 1, float columnLength = 10)
        {
            Gizmos.color = color;
            Gizmos.DrawRay(centerPoint + (-1 * crossLength * Vector3.right), Vector3.right * 2 * crossLength);
            Gizmos.DrawRay(centerPoint + (-1 * crossLength * Vector3.forward), Vector3.forward * 2 * crossLength);
            Gizmos.DrawRay(centerPoint, Vector3.up * columnLength);
        }

        public static void DebugDrawCrossLine(Vector3 centerPoint, Color color, float duration = 0, float crossLength = 1, float columnLength = 10)
        {
            if (duration <= 0) duration = Time.deltaTime;
            Debug.DrawRay(centerPoint + (-1 * crossLength * Vector3.right), Vector3.right * 2 * crossLength, color, duration);
            Debug.DrawRay(centerPoint + (-1 * crossLength * Vector3.forward), Vector3.forward * 2 * crossLength, color, duration);
            Debug.DrawRay(centerPoint, Vector3.up * columnLength, color, duration);
        }

        public static string GetPersistentDataPath
        {
            get
            {
#if UNITY_EDITOR
                if (EditorUserBuildSettings.activeBuildTarget.Equals(BuildTarget.Android))
                {
                    return "/Android/data/" + Application.identifier + "/files";
                }
#endif
                return Application.persistentDataPath;
            }
        }

        public static string GetPathForUnity(string path)
        {
            string newPath = "";
            if (!string.IsNullOrEmpty(path) && path.StartsWith(Application.dataPath))
            {
                int dataPathLength = Application.dataPath.Length;
                newPath = "Assets" + path.Substring(dataPathLength, path.Length - dataPathLength);
            }
            else
            {
                Debug.LogError("Path Failed: Can't use assets folder path outside\n: " + path);
            }
            return newPath;
        }

        public static string SelectFilePath(string name, string extension, string title = "Choose Folder", string folder = "Assets/")
        {
            string newPath = "";
#if UNITY_EDITOR
            newPath = GetPathForUnity(EditorUtility.SaveFilePanel(title, folder, name, extension));
#endif
            return newPath;
        }

        public static string SelectFolderPath(string title = "Choose Folder", string folderPath = "Assets/", string folderName = "")
        {
            string newPath = "";
#if UNITY_EDITOR
            newPath = GetPathForUnity(EditorUtility.SaveFolderPanel(title, folderPath, folderName));
#endif
            return newPath;
        }

        [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
        public static void PingObj(GameObject pingObj, bool isChangeSelection = true)
        {
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(pingObj);
            if (isChangeSelection)
                Selection.activeObject = pingObj;
#endif
        }


        #region UNITY_EDITOR
#if UNITY_EDITOR


        [UnityEditor.MenuItem("CWJ/Generate UnityDevTool")]
        static void GenerateUnityDevToolPackage()
        {
            //HashSet<string> getIgnoreNames()
            //{
            //    string ext = ".asset";
            //    Type[] types = new Type[] { typeof(ScriptableObjectStore) };

            //    return new HashSet<string>(Resources.FindObjectsOfTypeAll<CWJScriptableObject>().Where(o => !types.IsExists(o.GetType())).Select(o => o.GetType().Name + ext));
            //}
            var ps = GetRecursivePath(new[] { "Assets/CWJ" });
            GenerateUnityPackage("CWJ_UnityDevTool"
                                , new[] { "Assets/CWJ" }
                                , PathUtil.MyVersion.Split('(')[0]
                                //, getIgnoreNamesFunc: getIgnoreNames
                                );
        }

        public static string[] GetRecursivePath(string[] folderPaths, string filter = "", Func<HashSet<string>> getIgnoreNamesFunc = null)
        {
            var assetPaths = UnityEditor.AssetDatabase.FindAssets(filter, folderPaths)
                                        .Select(guid => string.IsNullOrEmpty(guid) ? null : UnityEditor.AssetDatabase.GUIDToAssetPath(guid))
                                        .Where(path => !string.IsNullOrEmpty(path));
            var ignoreNameSet = getIgnoreNamesFunc?.Invoke() ?? null;

            if (ignoreNameSet != null && ignoreNameSet.Count > 0)
            {
                assetPaths = assetPaths.Where(p => !ignoreNameSet.Contains(Path.GetFileName(p.TrimEnd('\\'))));
                //Path.GetFileName(p.TrimEnd('\\')) : 경로에서 파일이름만 빼내기
            }

            if (assetPaths == null || assetPaths.Count() == 0)
            {
                return null;
            }
            return assetPaths.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folderPaths">relative path : new[] { "Assets/Plugins/CWJSDK", "Assets/Plugins/Android" }</param>
        /// <param name="version"></param>
        static void GenerateUnityPackage(string fileName, string[] exportPaths, string version
            , ExportPackageOptions exportOption= ExportPackageOptions.Recurse)
        {
            if(!UnityEditor.EditorUtility.DisplayDialog("CWJ - " + fileName, (fileName += "_" + version) + " Export를 진행하시겠습니까?", "예", "아니오"))
            {
                return;
            }

            fileName = $"{fileName}_{System.DateTime.Now.ToString("yyMMdd-HHmmss")}_from-{Application.productName}.unitypackage";
            UnityEditor.AssetDatabase.ExportPackage(exportPaths, fileName, exportOption);

            string projectPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1);
            projectPath = projectPath.Substring(0, projectPath.Length - 1);

            string folderPath = Application.dataPath + "/CWJ_UnityPackage/";
            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            string newPath = folderPath + fileName;
            System.IO.File.Move(projectPath + "/" + fileName, newPath);

            Debug.Log(fileName + " 를 생성했습니다\n" + newPath);
            System.Diagnostics.Process.Start(folderPath);
        }

        const string TypeStartTag = "PPtr<$";
        const string TypeEndTag = ">";

        public static string GetFriendlyTypeName(string propertyType)
        {
            if (propertyType.StartsWith(TypeStartTag))
            {
                return propertyType.Replace("PPtr<$", string.Empty).RemoveEnd(TypeEndTag);
            }
            return propertyType;
        }
        public static System.Type GetType(SerializedProperty property)
        {
            return
                Assembly.Load("UnityEngine.dll")
                    .GetType("UnityEngine." + property.type.Replace("PPtr<$", "").Replace(">", ""));
        }

        private static string GetSearchFilter(string objName, string typeName, string labelName, string assetBundleName)
        {
            return (!string.IsNullOrEmpty(typeName?.RemoveAllSpaces()) ? $"t:{typeName} " : "")
                    + (!string.IsNullOrEmpty(labelName?.RemoveAllSpaces()) ? $"l:{labelName} " : "")
                    + (!string.IsNullOrEmpty(assetBundleName?.RemoveAllSpaces()) ? $"b:{assetBundleName} " : "")
                    + objName;
        }

        private const string HierarchyWindowTypeName = "SceneHierarchyWindow";
        private const string ProjectBrowserTypeName = "ProjectBrowser";
        private const string GameViewWindowTypeName = "GameView";
        private const string BuildPlayerWindowTypeName = "BuildPlayerWindow";
        private const string InspectorWindowTypeName = "InspectorWindow";


        public static EditorWindow[] GetHierarchyWindow()
        {
            var hierarchys = Resources.FindObjectsOfTypeAll(ReflectionUtil.GetUnityEditorClassType(HierarchyWindowTypeName)) as SearchableEditorWindow[];
            return hierarchys;
        }

        private static void SetSearchableWindowSearchField(string WindowName, string objName, string typeName, string labelName, string assetBundleName)
        {
            System.Type windowType = ReflectionUtil.GetUnityEditorClassType(WindowName);
            //SearchableEditorWindow searchableWindow = EditorWindow.GetWindow(windowType) as SearchableEditorWindow;
            var searchableWindows = Resources.FindObjectsOfTypeAll(windowType);
            if (searchableWindows == null || searchableWindows.Length == 0) return;

            MethodInfo setSearchMethod = typeof(SearchableEditorWindow).GetMethod("SetSearchFilter", BindingFlags.NonPublic | BindingFlags.Instance);

            if (setSearchMethod == null) return;

            string filter = GetSearchFilter(objName, typeName, labelName, assetBundleName);
            SearchableEditorWindow.SearchMode filterMode = SearchableEditorWindow.SearchMode.All;

            object[] parameters =
#if UNITY_2018_3_OR_NEWER
            new object[] { filter, filterMode, false, false };
#else
            new object[] { filter, filterMode, false };
#endif

            foreach (var window in searchableWindows)
            {
                if (window == null) continue;
                setSearchMethod.Invoke(window, parameters);
            }
        }

        public static void SetHierarchySearchField(string objName, string typeName = "", string labelName = "", string assetBundleName = "")
        {
            SetSearchableWindowSearchField(HierarchyWindowTypeName, objName, typeName, labelName, assetBundleName);
        }

        public static void SetProjectSearchField(string objName, string typeName = "", string labelName = "", string assetBundleName = "")
        {
            System.Type projectBrowserType = ReflectionUtil.GetUnityEditorClassType(ProjectBrowserTypeName);
            //EditorWindow projectBrowser = EditorWindow.GetWindow(projectBrowserType);
            
            var projectBrowsers = Resources.FindObjectsOfTypeAll(projectBrowserType);
            
            if (projectBrowsers == null || projectBrowsers.Length == 0) return;

            MethodInfo setSearchMethod = projectBrowserType.GetMethod("SetSearch", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new System.Type[] { typeof(string) }, null);

            if (setSearchMethod == null) return;

            string filter = GetSearchFilter(objName, typeName, labelName, assetBundleName);

            EditorCallback.AddWaitForFrameCallback(() =>
            {
                foreach (var browser in projectBrowsers)
                {
                    if (browser == null) continue;
                    setSearchMethod.Invoke(browser, new object[] { filter });
                }
            });
        }

        public static EditorWindow GetMainGameView()
        {
            System.Type T = ReflectionUtil.GetUnityEditorClassType(GameViewWindowTypeName);
            System.Reflection.MethodInfo GetMainGameView = T.GetMethod("GetMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            return (EditorWindow)GetMainGameView.Invoke(null, null);
        }

        public static void OpenBuildSettings()
        {
            EditorWindow.GetWindow(ReflectionUtil.GetUnityEditorClassType(BuildPlayerWindowTypeName));
        }

        public static string GetDetailPackageName()
        {
            return GetSimplePackageName() + " | " + PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Standalone) + " | " + PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
        }

        public static string GetSimplePackageName()
        {
            return PlayerSettings.companyName + "." + PlayerSettings.productName;
        }

        public static string GetProjectUniqueIdentifier(bool includeProjectName =true)
        {
            string uniqueIdentifier = PlayerSettings.productGUID.ToString();
            if (includeProjectName)
            {
                uniqueIdentifier = "'" + GetSimplePackageName() + "'." + uniqueIdentifier;
            }
            return uniqueIdentifier;
        }

        public static string GetEditorRelativePath(this string path)
        {
            path = path.ToValidDirectoryPathByApp(false);
            if (path.StartsWith("Assets" + Path.DirectorySeparatorChar))
            {
                return path;
            }
            else
            {
                int index = path.IndexOf(Path.DirectorySeparatorChar + "Assets"+ Path.DirectorySeparatorChar);
                return index < 0 ? null : path.Substring(index, path.Length - index).Trim(Path.DirectorySeparatorChar);
            }
        }


        public static void PingAssetFile(string path, string log = null, LogType logType = LogType.Log)
        {
            EditorCallback.AddWaitForFrameCallback(() =>
            {
                var assetObj = AssetDatabase.LoadAssetAtPath<UnityObject>(path.GetEditorRelativePath());
                EditorGUIUtility.PingObject(assetObj);
                if (log != null)
                {
                    typeof(Ping).PrintLogWithClassName(log, logType, isBigFont: false, obj: assetObj);
                }
            });
        }

        /// <summary>
        /// scriptNameWithoutExtension : not ~.cs
        /// </summary>
        /// <param name="scriptNameWithoutExtension"></param>
        /// <returns></returns>
        public static bool TryGetScriptPath(string scriptNameWithoutExtension, out string path)
        {
            Debug.Log(scriptNameWithoutExtension);
            string[] paths = AssetDatabase.FindAssets(scriptNameWithoutExtension + " t:script").ConvertAll(AssetDatabase.GUIDToAssetPath);

            path = paths.FirstOrDefault(p => Path.GetFileNameWithoutExtension(p.TrimEnd('\\')).Equals(scriptNameWithoutExtension));
            return path != null;

            //return paths.Length > 0 ? paths[0] : null;

            //string[] paths = AssetDatabase.GetAllAssetPaths();
            //int length = paths.Length;
            //for (int i = 0; i < length; ++i)
            //{
            //    string path = paths[i];
            //    if (path.EndsWith(csFileName))
            //    {
            //        if ((MonoScript)AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript)) != null)
            //        {
            //            return path;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// scriptNameWithoutExtension : not ~.cs
        /// </summary>
        /// <param name="scriptNameWithoutExtension"></param>
        /// <returns></returns>
        public static MonoScript GetMonoScript(string scriptNameWithoutExtension)
        {
            if(TryGetScriptPath(scriptNameWithoutExtension, out string path))
            {
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                if (script != null)
                {
                    return script;
                }
            }
            return null;
        }

        private static void OpenScriptViaMono(MonoScript script)
        {
            if (script == null) return;
            AssetDatabase.OpenAsset(script);
        }

        public static void OpenScriptViaName(string csFileName)
        {
            OpenScriptViaMono(GetMonoScript(csFileName));
        }

        public static void OpenScriptViaPath(string path)
        {
            OpenScriptViaMono((MonoScript)AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript)));
        }

        private static CWJEditorHelper_ScriptableObject _EditorHelperObj = null;

        public static CWJEditorHelper_ScriptableObject EditorHelperObj
        {
            get
            {
                if (_EditorHelperObj == null)
                {
                    _EditorHelperObj = ScriptableObjectStore.Instanced.GetScriptableObj<CWJEditorHelper_ScriptableObject>();
                }

                return _EditorHelperObj;
            }
        }
        const string objName = "CWJ_TODO_ReloadedScriptEvent이후 이벤트 실행되어야 함 ";
        public static void ForceRecompile()
        {
            return;
            var obj = new GameObject(objName);
            obj.hideFlags = HideFlags.HideInInspector;
            void DestroyTempObj()
            {
                var obj= GameObject.Find(objName);
                 if (obj != null)
                {
                    GameObject.Destroy(obj);
                    Debug.LogError(obj.name);
                }
            }
            if (BuildEventSystem.IsBuilding)
                CWJ_EditorEventHelper.ReloadedScriptOnlyDuringBuildEvent += DestroyTempObj;
            else
                CWJ_EditorEventHelper.ReloadedScriptEvent += DestroyTempObj;
            var scene = obj.scene;
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
            Debug.LogError("Force Recompile");
        }

#endif
        #endregion UNITY_EDITOR
    }
}