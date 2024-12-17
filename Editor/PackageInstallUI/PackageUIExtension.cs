#if UNITY_EDITOR
using System.IO;
using UnityEditor;
// using UnityEditor.PackageManager;
// using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace CWJ.Editor
{
	using static PackageDefine;
	using static PackageUtil;

	public static partial class PackageUIExtension
	{
		[InitializeOnLoadMethod]
		static void Init()
		{
			PackageManagerExtensions.RegisterExtension(new UpmExtension());
		}

		[MenuItem("CWJ/Package/" + MyPackageInAssetName + "/Import ThirdPartyPackage", false)]
		public static void ImportThirdPartyPackage()
		{
			ImportThirdPartyPackage(ThirdPartyPackageFileName, false, Path.Combine(RuntimeDownloadAssetsPath, "ThirdPartyPackage"));
		}

		private static void ImportThirdPartyPackage(string unityPackageFilename, bool originPath, string targetPath = null)
		{
			string packageFullPath = $"{PackageFullPath}/{UnityPackageFolderName}/{unityPackageFilename}";
			if (originPath)
				AssetDatabase.ImportPackage(packageFullPath, true);
			else
				ImportUnityPackage(packageFullPath, targetPath);
		}

		public static void ImportTmpEssentialPackage()
		{
			ImportThirdPartyPackage(TmpEssentialPackageFileName, true);
		}

		private static PackageInfo _CurPackage = null;

		class UpmExtension : IPackageManagerExtension
		{
			public VisualElement CreateExtensionUI()
			{
				VisualElement ExtentionRoot = new VisualElement();

				VisualElement labelLine = new VisualElement();
				ExtentionRoot.Add(labelLine);

				titleLbl = new Label { text = TitleStr };
				labelLine.Add(titleLbl);
				errorDescLbl = new Label { text = " " };
				labelLine.Add(errorDescLbl);

				VisualElement buttonsLine1 = new VisualElement();
				ExtentionRoot.Add(buttonsLine1);

				buttonsLine1.style.flexDirection = FlexDirection.Row;
				buttonsLine1.style.flexWrap = Wrap.Wrap;

				const int width = 200;

				importThirdPartyPackageBtn = new Button();
				importThirdPartyPackageBtn.text = "Import ThirdPartyPackage";
				importThirdPartyPackageBtn.clicked += ImportThirdPartyPackage;
				importThirdPartyPackageBtn.style.width = width;
				buttonsLine1.Add(importThirdPartyPackageBtn);

				importTmpEssentialPackageBtn = new Button();
				importTmpEssentialPackageBtn.text = "Import TextMeshPro Essential";
				importTmpEssentialPackageBtn.clicked += ImportTmpEssentialPackage;
				importTmpEssentialPackageBtn.style.width = width;
				buttonsLine1.Add(importTmpEssentialPackageBtn);

				changeApiCompatibilityBtn = new Button();
				changeApiCompatibilityBtn.text = "Change API Compatibility";
				changeApiCompatibilityBtn.clicked += ChangeApiCompatibility;
				changeApiCompatibilityBtn.style.width = width;
				buttonsLine1.Add(changeApiCompatibilityBtn);

				VisualElement buttonsLine2 = new VisualElement();
				ExtentionRoot.Add(buttonsLine2);
				buttonsLine2.style.flexDirection = FlexDirection.Row;
				buttonsLine2.style.flexWrap = Wrap.Wrap;
				//
				downloadRuntimePackageBtn = new Button();
				downloadRuntimePackageBtn.text = "Download Runtime Resources";
				downloadRuntimePackageBtn.clicked += DownloadRuntimeResources;
				downloadRuntimePackageBtn.style.width = width;
				buttonsLine2.Add(downloadRuntimePackageBtn);
				updateDescLbl = new Label { text = " " };
				buttonsLine2.Add(updateDescLbl);

				VisualElement lastLine = new VisualElement();
				ExtentionRoot.Add(lastLine);
				lastLine.style.flexDirection = FlexDirection.Row;
				lastLine.style.flexWrap = Wrap.Wrap;

				InjectUIElements(ExtentionRoot);

				return ExtentionRoot;
			}

			private Button importThirdPartyPackageBtn, importTmpEssentialPackageBtn, changeApiCompatibilityBtn, downloadRuntimePackageBtn;
			private Label titleLbl, updateDescLbl, errorDescLbl;

			public void OnPackageSelectionChange(PackageInfo packageInfo)
			{
				bool isTargetPackage = packageInfo != null && packageInfo.name == MyPackageName;
				_CurPackage = isTargetPackage ? packageInfo : null;

				if (isTargetPackage)
					updateDescLbl.text = "Checking for Updates...";
				updateDescLbl.visible = isTargetPackage;
				errorDescLbl.visible = isTargetPackage;
				importThirdPartyPackageBtn.visible = isTargetPackage;
				changeApiCompatibilityBtn.visible = isTargetPackage;
				importTmpEssentialPackageBtn.visible = isTargetPackage;
				downloadRuntimePackageBtn.SetEnabled(false);
				downloadRuntimePackageBtn.visible = isTargetPackage;
				titleLbl.visible = isTargetPackage;
				InjectSelectionChanged(isTargetPackage, _CurPackage);

				if (!isTargetPackage)
				{
					return;
				}

				errorDescLbl.text = $"오류가 난다면 아래 {importTmpEssentialPackageBtn.text} 버튼을 눌러주세요.\\n오류가 없을 시 {downloadRuntimePackageBtn.text} 를 눌러 다운로드 받으세요";

				importTmpEssentialPackageBtn.SetEnabled(!HasTmpEssential());


				bool needChangeApi = PlayerSettings.GetApiCompatibilityLevel(EditorUserBuildSettings.selectedBuildTargetGroup) !=
				                     ApiCompatibilityLevel.NET_Unity_4_8;

				importThirdPartyPackageBtn.SetEnabled(!needChangeApi);

				changeApiCompatibilityBtn.visible = needChangeApi;
				if (needChangeApi)
				{
					updateDescLbl.text = $"[{changeApiCompatibilityBtn.text}] 버튼을 눌러주세요.";
					if (EditorUtility.DisplayDialog(MyPackageInAssetName,
					                                $"{MyPackageName} 은 .NET Standard 2.1과 호환되지 않습니다.\nAPI 호환성 수준을 .NET Framework 4.x로 변경하시겠습니까?", "Ok"))
					{
						ChangeApiCompatibility();
					}
				}
				else
				{
					isUpdateChecking = true;
					dotCount = 3;
					EditorApplication.update += OnUpdateChecking;
					EditorApplication.delayCall += CheckForUpdates;
					EditorApplication.delayCall += CheckTmpEssential;
				}
			}

			private int dotCount = 3;

			private void OnUpdateChecking()
			{
				if (!isUpdateChecking)
				{
					EditorApplication.update -= OnUpdateChecking;
					return;
				}

				dotCount = (dotCount + 1) % 6; //0 ~ 5

				updateDescLbl.text = "Checking for Updates" + new string('.', dotCount);
			}

			private bool isUpdateChecking = false;

			// private void OnDownloadSampleClicked()
			// {
			// 	if (UnityEditor.EditorUtility.DisplayDialog($"{MyPackageInAssetName} Info",
			// 	                                            "다운로드 받으시겠습니까?", "Download", "Cancel"))
			// 	{
			// 		if (TrySelectDownloadPath(out string downloadFolderPath))
			// 		{
			// 			ImportSamplesToPath(current, downloadFolderPath);
			// 			UnityEditor.EditorUtility.DisplayDialog($"{MyPackageInAssetName} Info", "Download 완료", "OK");
			// 		}
			// 	}
			// }

			private void CheckForUpdates()
			{
				bool needUpdate = _CurPackage.CheckNeedUpdateByLastUpdate(out string latestVersion);
				updateDescLbl.text = (needUpdate ? ">> 현재 최신버전이 아닙니다 <<\n>> 하단에 [Update]버튼을 눌러주세요 <<" : "현재 최신 버전입니다");
				isUpdateChecking = false;
				downloadRuntimePackageBtn.SetEnabled(!needUpdate);
			}

			public void OnPackageAddedOrUpdated(PackageInfo packageInfo)
			{
				bool isTargetPackage = packageInfo != null && packageInfo.name == MyPackageName;
				_CurPackage = isTargetPackage ? packageInfo : null;
				if (isTargetPackage)
				{
					DownloadRuntimeResources();
				}
			}

			public void OnPackageRemoved(PackageInfo packageInfo)
			{
			}
		}
		//

		private static void CheckTmpEssential()
		{
			if (!HasTmpEssential())
			{
				EditorUtility.DisplayDialog(MyPackageInAssetName,
				                            $"{MyPackageName} 은 TextMeshPro가 필요합니다.\nTextMeshPro Essential.unitypackage 를 실행합니다.", "Next");
				ImportTmpEssentialPackage();
			}
		}


		/// <summary>
		/// 사실 Runtime~ 폴더를 unitypackage로 export하는거임
		/// </summary>
		[MenuItem("CWJ/Package/" + MyPackageInAssetName + "/Download RuntimeResources", false)]
		private static void DownloadRuntimeResources()
		{
			ExportAndImportPackageAnywhere(RuntimeIgnoreFolderName);
		}

		private static bool HasTmpEssential()
		{
			return File.Exists("Assets/TextMesh Pro/Resources/TMP Settings.asset");
		}


		private static void ChangeApiCompatibility()
		{
			BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			PlayerSettings.SetApiCompatibilityLevel(targetGroup, ApiCompatibilityLevel.NET_Unity_4_8);

			Debug.Log($"API Compatibility Level changed to: {ApiCompatibilityLevel.NET_Unity_4_8.ToString()} (.NET Framework)");
		}
	}
}
#endif
