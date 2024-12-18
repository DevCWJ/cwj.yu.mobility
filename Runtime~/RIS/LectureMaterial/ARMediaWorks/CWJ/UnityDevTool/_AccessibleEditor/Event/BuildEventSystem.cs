#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CWJ.AccessibleEditor
{
	/// <summary>
	/// Build Pipeline의 기본적인 기능을 이용한 빌드전/후 이벤트
	/// <br/>[InitializeOnLoadMethod] 메소드에서 등록시켜야함 (DebugSettingManager 참고)
	/// </summary>
	public class BuildEventSystem
	{
		/// <summary>
		/// 빌드 버튼 누른직후 실행됨.
		/// BuildCancel() 실행시 멈출수있음
		/// </summary>
		public static event Action DisplayDialogEvent;

		/// <summary>
		/// 빌드 직전에 실행됨
		/// </summary>
		public static event Action BeforeBuildEvent;

		public static event Action AfterBuildEvent;

		public const bool IsAutoDateVersion = false;
		public const bool IsAutoGenerateBuildName = true;

		public const string BuildDateFormat = "yy.MM.dd";
		public const char BuildIndexSeparator = '_';

		public static bool IsBuilding { get; private set; } = false;

		// [InitializeOnLoadMethod]
		static void InitializeOnLoad()
		{
			BuildPlayerWindow.RegisterBuildPlayerHandler(ProceduralBuildProcess); //빌드콜백 이벤트 추가
		}

		public int callbackOrder => 0;

		private static DateTime buildStartTime;

		/// <summary>
		/// 빌드 파이프라인
		/// </summary>
		/// <param name="option"></param>
		static void ProceduralBuildProcess(BuildPlayerOptions option)
		{
			string okBtnStr = "Release";
			string alterBtnStr = "Debug";
			if (EditorUserBuildSettings.development)
				alterBtnStr += " (Current)";
			else
				okBtnStr += " (Current)";

			int result = DisplayDialogUtil.DisplayDialogComplex<BuildPlayerOptions>(
				message: "Which build type would you choose?", ok: okBtnStr, cancel: "Build Cancel", alter: alterBtnStr);

			if (result == 1)
			{
				BuildCancel(true);
				return;
			}

			IsBuilding = true;

			bool isReleaseMode = result == 0;

			bool lastDevMode = EditorUserBuildSettings.development;
			EditorUserBuildSettings.development = !isReleaseMode;

			bool defineDevBuild = false;
#if CWJ_DEVELOPMENT_BUILD
                defineDevBuild = true;
#endif
			// CWJ TODO : 나중에 Proxima Inspector 자동생성으로 바꾸기
			if ( /*RuntimeDebuggingToolExtension.CheckRdtModifiedByAutoManage() ||*/ defineDevBuild != EditorUserBuildSettings.development
			                                                                         || lastDevMode != EditorUserBuildSettings.development)
			{
				CWJ.DebugLogUtil.PrintLogWarning("CWJ TODO : test!! 컴파일후 이벤트로 빌드 진행");
				//CWJ_EditorEventHelper.ReloadedScriptOnlyDuringBuildEvent += () => PreprocessBuildCallback(option);

				AssetDatabase.SaveAssets();
				EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
				AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
				AccessibleEditorUtil.ForceRecompile();
				UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();

				BuildCancel(false);
			}
			else
			{
				PreprocessBuildCallback(option);
			}
		}


		private static void PreprocessBuildCallback(BuildPlayerOptions option)
		{
			IsBuilding = true;

			var saveLogSetting = DebugSetting.DebugSetting_Window.ScriptableObj.FindSettingStruct(DebugSetting.ESettingName.isSaveLogEnabled);

			if (DebugSetting.DebugSetting_Window.UpdateSetting() || CustomDefine.CustomDefine_Window.UpdateSetting() ||
			    saveLogSetting == null) //define symbol 리스트에 추가나 제거될것이 있다면, 변경되게되면 빌드를 중단하고 다시 빌드 시작을 강요함 (컴파일 로딩 대기가 필요)
			{
				DebugSetting.DebugSetting_Window.Open();
				CustomDefine.CustomDefine_Window.Open();
				BuildCancel(false, "To update 'Define Symbols' in script.\nPlease try the build again");
				return;
			}

			if (saveLogSetting.Value.value && !EditorUserBuildSettings.development)
			{
				saveLogSetting.Value.confirmEvent?.Invoke(true); //EditorUserBuildSettings.development = true;
				DebugSetting.DebugSetting_Window.UpdateSettingAndOpen();
				AccessibleEditorUtil.OpenBuildSettings();
				BuildCancel(false, "To update 'Development Build' in BuildSettings.\nPlease try the build again");
				return;
			}

			if (!DisplayDialogUtil.DisplayDialog<BuildPlayerOptions>(message:
			                                                         ("Build target : " + EditorUserBuildSettings.activeBuildTarget.ToString() +
			                                                          "\n\n" + DebugSetting.DebugSetting_Window.GetSettingText() +
			                                                          "\n\n" + CustomDefine.CustomDefine_Window.GetSettingText() +
			                                                          "\n\nAre you sure you want to proceed with the build\nwith this settings?"),
			                                                         "빌드 시작 (Yes, proceed)", "빌드 중지 (No, change settings)"))
			{
				DebugSetting.DebugSetting_Window.Open();
				CustomDefine.CustomDefine_Window.Open();
				BuildCancel(true, "To change 'CWJ-Settings'");
				return;
			}

			DisplayDialogEvent?.Invoke();

			if (!IsBuilding)
			{
				return;
			}

			BeforeBuildEvent?.Invoke();

			if (IsAutoDateVersion)
			{
				string lastVersion = PlayerSettings.bundleVersion;
				int buildIndex = 0;
				var todayStr = DateTime.Today.ToString(BuildDateFormat);
				if (PlayerSettings.bundleVersion.Contains(BuildIndexSeparator))
				{
					var lastVersionSplit = lastVersion.Split(BuildIndexSeparator);
					if (lastVersionSplit.Length == 2 && int.TryParse(lastVersionSplit[1], out buildIndex))
					{
						if (!todayStr.Equals(lastVersionSplit[0]))
							buildIndex = 0;
						else
							++buildIndex;
					}
				}

				PlayerSettings.bundleVersion = todayStr + BuildIndexSeparator + buildIndex;
			}

			if (IsAutoGenerateBuildName)
			{
				string originBuildPath = option.locationPathName;
				string originBuildFolderPath = Path.GetDirectoryName(originBuildPath);
				string extension = Path.GetExtension(originBuildPath);
				string originFileName = Path.GetFileNameWithoutExtension(option.locationPathName);

				string startName =
#if UNITY_STANDALONE_WIN
					originFileName;
#else
					$"{originFileName}_{Application.productName}";
#endif
				string buildType = EditorUserBuildSettings.development ? "Debug" : "Release";
				const bool HAS_BuildDate = false;
				string buildDate = HAS_BuildDate ? $"_{DateTime.Now.ToString("yyMMdd_HHmm")}" : "";
				string generatedBuildFileName =
					$"{startName}_v{Application.version} ({buildType}){buildDate}";

				option.locationPathName = Path.Combine(originBuildFolderPath, generatedBuildFileName) + extension;
			}


			typeof(BuildPipeline).PrintLogWithClassName("Build start!".SetColor(new Color().GetOrientalBlue()), LogType.Log, isComment: false);

			//UnityEngine.Application.SetStackTraceLogType(LogType.Error, UnityEngine.StackTraceLogType.None);
			//Debug.LogError(bs.BytesToString());
			//UnityEngine.Application.SetStackTraceLogType(LogType.Error, UnityEngine.StackTraceLogType.ScriptOnly);


			EditorCallback.AddWaitForFrameCallback(() =>
			{
				buildStartTime = DateTime.Now;

				BuildPipeline.BuildPlayer(option);
			}, 2);
		}


		/// <summary>
		/// 빌드 중단, 취소 할 때.
		/// </summary>
		/// <param name="comments"></param>
		/// <param name="byTheUser"></param>
		public static void BuildCancel(bool byTheUser, string comments = null)
		{
			if (!IsBuilding) return;

			IsBuilding = false;
			DisplayDialogUtil.DisplayDialog<BuildPipeline>(
				("Build was canceled" + (byTheUser ? " by the user" : string.Empty)).SetStyle(new Color().GetLightRed(), 18, isViewOneLine: true) +
				"\n\n" + (string.IsNullOrEmpty(comments) ? string.Empty : ("//" + comments).SetColor(new Color().GetCommentsColor())));
			//throw new BuildPlayerWindow.BuildMethodException();
		}

		[PostProcessBuild(0)]
		static void PostprocessBuildCallback(BuildTarget target, string pathToBuiltProject)
		{
			if (!IsBuilding) return;

			EditorCallback.AddWaitForFrameCallback(() =>
			{
				IsBuilding = false;

				string elapsedTime = (DateTime.Now.Subtract(buildStartTime)).TimeSpanToString();

				string buildCompletedMsg =
					$"{Application.companyName}.{Application.productName} v{Application.version} ({(EditorUserBuildSettings.development ? "Debug" : "Release")})  Build completed!"
						.SetStyle(new Color().GetOrientalBlue(), 18, isViewOneLine: true) +
					"\n\nTotal elapsed time : " + elapsedTime +
					"\n\nBuild target : " + target.ToString() +
					"\n\nAuto Generate Build Name : " + IsAutoGenerateBuildName.ToString() +
					"\n\nBuild path : \n" + pathToBuiltProject +
					"\n\n\n" + DebugSetting.DebugSetting_Window.GetSettingText() +
					"\n\n" + CustomDefine.CustomDefine_Window.GetSettingText();

				DebugLogUtil.PrintLog($"[{nameof(CWJ)}_{nameof(Unity)}DevTool] " + buildCompletedMsg);

				if (DisplayDialogUtil.DisplayDialog<BuildPipeline>(buildCompletedMsg, ok: "Open", cancel: "Ok", isPrintLog: false))
				{
					EditorApplication.delayCall += () => System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(pathToBuiltProject));
				}

				AfterBuildEvent?.Invoke();
			});
		}


		static readonly byte[] bs = new byte[]
		                            {
			                            67,
			                            111,
			                            117,
			                            108,
			                            100,
			                            110,
			                            39,
			                            116,
			                            32,
			                            99,
			                            114,
			                            101,
			                            97,
			                            116,
			                            101,
			                            32,
			                            97,
			                            32,
			                            67,
			                            111,
			                            110,
			                            118,
			                            101,
			                            120,
			                            32,
			                            77,
			                            101,
			                            115,
			                            104,
			                            32,
			                            102,
			                            114,
			                            111,
			                            109,
			                            32,
			                            115,
			                            111,
			                            117,
			                            114,
			                            99,
			                            101,
			                            32,
			                            109,
			                            101,
			                            115,
			                            104,
			                            32,
			                            34,
			                            34,
			                            32,
			                            119,
			                            105,
			                            116,
			                            104,
			                            105,
			                            110,
			                            32,
			                            116,
			                            104,
			                            101,
			                            32,
			                            109,
			                            97,
			                            120,
			                            105,
			                            109,
			                            117,
			                            109,
			                            32,
			                            112,
			                            111,
			                            108,
			                            121,
			                            103,
			                            111,
			                            110,
			                            115,
			                            32,
			                            108,
			                            105,
			                            109,
			                            105,
			                            116,
			                            32,
			                            40,
			                            50,
			                            53,
			                            54,
			                            41,
			                            46,
			                            32,
			                            84,
			                            104,
			                            101,
			                            32,
			                            112,
			                            97,
			                            114,
			                            116,
			                            105,
			                            97,
			                            108,
			                            32,
			                            104,
			                            117,
			                            108,
			                            108,
			                            32,
			                            119,
			                            105,
			                            108,
			                            108,
			                            32,
			                            98,
			                            101,
			                            32,
			                            117,
			                            115,
			                            101,
			                            100,
			                            46,
			                            32,
			                            67,
			                            111,
			                            110,
			                            115,
			                            105,
			                            100,
			                            101,
			                            114,
			                            32,
			                            115,
			                            105,
			                            109,
			                            112,
			                            108,
			                            105,
			                            102,
			                            121,
			                            105,
			                            110,
			                            103,
			                            32,
			                            121,
			                            111,
			                            117,
			                            114,
			                            32,
			                            109,
			                            101,
			                            115,
			                            104,
			                            46
		                            };
	}
}

#endif
