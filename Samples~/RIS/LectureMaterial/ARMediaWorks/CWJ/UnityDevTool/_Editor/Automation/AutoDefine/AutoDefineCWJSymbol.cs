using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using CWJ.AccessibleEditor;

using UnityObject = UnityEngine.Object;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace CWJ.EditorOnly
{
    using static CWJ.AccessibleEditor.DebugSetting.CWJDefineSymbols;

    public static class AutoDefineCWJSymbol
    {
        private const string UnityDevToolFolder = "UnityDevTool";
        private const string AddonFolder = "Addon";


        private const string CWJVRDevFolder = "VRDevTool";
        private const string CWJVRDevScript = "VR_Manager";

        private const string SteamVRFolder = "SteamVR";
        private const string SteamVRScript = "SteamVR";

        private const string RuntimeNavFolder = "NavMeshComponents";
        private const string RuntimeNavScript = "NavMeshSurface";

        private const string UniRxFolder = "Plugins/UniRx";
        private const string UniRxScript = "Observer";

        //Packages
        private const string URPPackage = "com.unity.render-pipelines.universal";

        private const string NewInputSystemPackage = "com.unity.inputsystem";

        private const string EditorCoroutinePackage = "com.unity.editorcoroutines";

        static AddRequest packageAddRequest;
        static string packageIdentifier;
        static void AddPacakge(string identifier)
        {
            packageIdentifier = identifier;
            packageAddRequest = UnityEditor.PackageManager.Client.Add(packageIdentifier);

            EditorApplication.update += PackageProgress;
        }

        static void PackageProgress()
        {
            if (packageAddRequest.IsCompleted)
            {
                if (packageAddRequest.Status == StatusCode.Success)
                    Debug.Log("Installed: " + packageAddRequest.Result.packageId);
                else if (packageAddRequest.Status >= StatusCode.Failure)
                    DisplayDialogUtil.DisplayDialog<UnityEditor.Editor>($"'Package Manager'에서\n\n'{packageIdentifier}'를 Install 해주세요", isPreventOverlapMsg: true);

                EditorApplication.update -= PackageProgress;
            }
        }

        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            CWJ_EditorEventHelper.UnityProjectChangedEvent += OnProjectInitCheckFolder;
            CWJ_EditorEventHelper.ReloadedScriptEvent += OnReloadedCheckDefine;
            CWJ_EditorEventHelper.FolderChangedEvent += OnReloadedCheckDefine;
            CWJ_EditorEventHelper.EditorWillSaveEvent += (_, _) => OnReloadedCheckDefine();
        }

        private static void OnProjectInitCheckFolder()
        {
            CheckDefineSymbol(true);
        }

        private static void OnReloadedCheckDefine()
        {
            CheckDefineSymbol(false);
        }

        static List<string> _AddSymbolList = null;
        static List<string> _RemoveSymbolList = null;

        //ApiCompatibilityLevels in 19.3
        //public const string CWJ_NET_2_0 = nameof(CWJ_NET_2_0);
        //public const string CWJ_NET_2_0_Subset = nameof(CWJ_NET_2_0_Subset);
        //public const string CWJ_NET_4_6 = nameof(CWJ_NET_4_6);
        //public const string CWJ_NET_Web = nameof(CWJ_NET_Web);
        //public const string CWJ_NET_Micro = nameof(CWJ_NET_Micro);
        //public const string CWJ_NET_Standard_2_0 = nameof(CWJ_NET_Standard_2_0);
        private static void CheckApiName(bool isProjectInit)
        {
            var settingBackup = AccessibleEditorUtil.EditorHelperObj;

            if (settingBackup == null) return;

            _AddSymbolList = new List<string>();
            _RemoveSymbolList = new List<string>();

            var curBuildTarget = EditorUserBuildSettings.selectedBuildTargetGroup;
            var curApiLevel = PlayerSettings.GetApiCompatibilityLevel(curBuildTarget);

            if (!settingBackup.isInitialized || !settingBackup.lastApiLevel.Equals(curApiLevel))
            {
                string curCwjApiLevel = settingBackup.ConvertToCWJApiName(curApiLevel);

                _AddSymbolList.Add(curCwjApiLevel);
                if (isProjectInit)
                {
                    _RemoveSymbolList.AddRange(settingBackup.GetAllCWJApiSymbols());
                }
                else
                {
                    _RemoveSymbolList.Add(settingBackup.ConvertToCWJApiName(settingBackup.lastApiLevel));
                }

                settingBackup.lastApiLevel = curApiLevel;
                settingBackup.isInitialized = true;
                settingBackup.SaveScriptableObj();
            }
        }
        private static string[] CwjFolderNames = null;
        private static string[] RootFolderNames = null;

        private static void CheckDefineSymbol(bool isProjectInit)
        {
            CheckApiName(isProjectInit);

            if (DetectFolderChanged.ScriptableObj == null) return;

            CwjFolderNames = DetectFolderChanged.ScriptableObj.cwjFolderDatas.ToFolderNames();
            RootFolderNames = DetectFolderChanged.ScriptableObj.rootFolderDatas.ToFolderNames();

            //CWJ_UNITYDEVTOOL
            DefineSymbolWhenExists(CwjFolderNames?.IsExists(UnityDevToolFolder) ?? false,
#if !CWJ_UNITYDEVTOOL
            false
#else
            true
#endif
            , CWJ_UNITYDEVTOOL);


            //CWJ_EXISTS_CWJVR
            DefineSymbolWhenExists(IsExistsInCWJ(CWJVRDevFolder, CWJVRDevScript),
#if !CWJ_EXISTS_CWJVR
            false
#else
            true
#endif 
            , CWJ_EXISTS_CWJVR);


            //CWJ_EXISTS_STEAMVR
            DefineSymbolWhenExists(IsExistsInAssets(SteamVRFolder, SteamVRScript),
#if !CWJ_EXISTS_STEAMVR
            false
#else
            true
#endif
            , CWJ_EXISTS_STEAMVR);


            //CWJ_EXISTS_RUNTIMENAVMESH
            DefineSymbolWhenExists(IsExistsInAssets(RuntimeNavFolder, RuntimeNavScript),
#if !CWJ_EXISTS_RUNTIMENAVMESH
            false
#else
            true
#endif
            , CWJ_EXISTS_RUNTIMENAVMESH);

            //CWJ_EXISTS_NEWINPUTSYSTEM
            DefineSymbolWhenExists(IsPackageExists(NewInputSystemPackage),
#if !CWJ_EXISTS_NEWINPUTSYSTEM
            false
#else
            true
#endif
            , CWJ_EXISTS_NEWINPUTSYSTEM);

            //CWJ_EXISTS_UNIRX 
            DefineSymbolWhenExists(IsExistsInAssetsByRelativePath(UniRxFolder, UniRxScript),
#if !CWJ_EXISTS_UNIRX
            false
#else
            true
#endif
            , CWJ_EXISTS_UNIRX); // TODO : UniRx다운받게끔 하기 정안되겠으면 그냥 DisplayDialog로 처리


            //CWJ_EXISTS_EDITORCOROUTINE
            DefineSymbolWhenExists(IsPackageExists(EditorCoroutinePackage),
#if !CWJ_EXISTS_EDITORCOROUTINE
            false
#else
            true
#endif
            , CWJ_EXISTS_EDITORCOROUTINE
            , (isExists) =>
            {
                if (isExists) return;
#if UNITY_2019_1_OR_NEWER
                //UnityEditor.PackageManager.UI.Window.Open("Editor Coroutines");
                //AddPacakge(EditorCoroutinePackage);
#else
                // Unity Api에서 찾지못했음
#endif
            });

            //CWJ_EXISTS_URP
            DefineSymbolWhenExists(IsPackageExists(URPPackage),
#if !CWJ_EXISTS_URP
            false
#else
            true
#endif
            , CWJ_EXISTS_URP);



            //CWJ_EXISTS_RUNTIMEDEBUGGING
            DefineSymbolWhenExists(CwjFolderNames?.IsExists(AddonFolder) ?? false,
#if !CWJ_EXISTS_RUNTIMEDEBUGGING
            false
#else
            true
#endif
            , CWJ_EXISTS_RUNTIMEDEBUGGING);

            //CWJ_EXISTS_ADDON
            DefineSymbolWhenExists(CwjFolderNames?.IsExists(AddonFolder) ?? false,
#if !CWJ_EXISTS_ADDON
            false
#else
            true
#endif
            , CWJ_EXISTS_ADDON);

            //CWJ_DEVELOPMENT_BUILD
            DefineSymbolWhenExists(EditorUserBuildSettings.development,
#if !CWJ_DEVELOPMENT_BUILD
            false
#else
            true
#endif
            , CWJ_DEVELOPMENT_BUILD);

            //Done
            if (isProjectInit)
            {
                DefineSymbolUtil.AddSymbolsStack(_AddSymbolList);
                DefineSymbolUtil.RemoveSymbolsStack(_RemoveSymbolList);
            }
            else
            {
                if (_RemoveSymbolList.Count > 0)
                    DefineSymbolUtil.RemoveSymbolsFromAllTargets(false, _RemoveSymbolList);
                if (_AddSymbolList.Count > 0)
                    DefineSymbolUtil.AddSymbolsToAllTargets(false, _AddSymbolList);
            }
        }

        private static void DefineSymbolWhenExists(bool isWannaDefine, bool isSymbolDefined, string symbolName, Action<bool> callback = null)
        {
            if (isWannaDefine != isSymbolDefined)
            {
                if (isWannaDefine)
                    _AddSymbolList.Add(symbolName);
                else
                    _RemoveSymbolList.Add(symbolName);
            }
            callback?.Invoke(isWannaDefine);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packageName">packageName : 'com.x.y'</param>
        /// <returns></returns>
        private static bool IsPackageExists(string packageName)
        {
            return AssetDatabase.LoadAssetAtPath<UnityObject>("Packages/" + packageName) != null;
        }

        private static bool IsExistsInAssetsByRelativePath(string folderPath, string scriptName)
        {
            return Directory.Exists(PathUtil.Combine(Application.dataPath, folderPath)) && AccessibleEditorUtil.GetMonoScript(scriptName) != null;
        }

        private static bool IsExistsInAssets(string folderName, string scriptName)
        {
            return RootFolderNames?.IsExists(folderName) ?? false && AccessibleEditorUtil.GetMonoScript(scriptName) != null;
        }

        private static bool IsExistsInCWJ(string folderName, string scriptName)
        {
            return CwjFolderNames?.IsExists(folderName) ?? false && AccessibleEditorUtil.GetMonoScript(scriptName) != null;
        }
    }
}