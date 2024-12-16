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
			ImportUnityPackage(ThirdPartyPackageFileName);
		}

		private static void ImportUnityPackage(string unityPackageFilename)
		{
			string packageFullPath = $"{PackageFullPath}/{UnityPackageFolderName}/{unityPackageFilename}";
			AssetDatabase.ImportPackage(packageFullPath, true);
		}

		public static void ImportTmpEssentialPackage()
		{
			ImportUnityPackage(TmpEssentialPackageFileName);
		}

		class UpmExtension : IPackageManagerExtension
		{
			public VisualElement CreateExtensionUI()
			{
				VisualElement ExtentionRoot = new VisualElement();

				VisualElement labelLine = new VisualElement();
				ExtentionRoot.Add(labelLine);
				updateDescLbl = new Label { text = " " };
				labelLine.Add(updateDescLbl);
				errorDescLbl = new Label { text = "오류가 난다면 아래 Import ThirdPartyPackage 버튼을 눌러주세요." };
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
				importTmpEssentialPackageBtn = new Button();
				importTmpEssentialPackageBtn.text = "Import TextMeshPro Essential";
				importTmpEssentialPackageBtn.clicked += ImportTmpEssentialPackage;
				importTmpEssentialPackageBtn.style.width = width;
				buttonsLine2.Add(importTmpEssentialPackageBtn);

				VisualElement lastLine = new VisualElement();
				ExtentionRoot.Add(lastLine);
				lastLine.style.flexDirection = FlexDirection.Row;
				lastLine.style.flexWrap = Wrap.Wrap;

				InjectUIElements(ExtentionRoot);

				return ExtentionRoot;
			}

			private Button importThirdPartyPackageBtn, importTmpEssentialPackageBtn, changeApiCompatibilityBtn;
			private Label updateDescLbl, errorDescLbl;
			private PackageInfo current = null;

			public void OnPackageSelectionChange(PackageInfo packageInfo)
			{
				bool isTargetPackage = packageInfo != null && packageInfo.name == MyPackageName;
				current = isTargetPackage ? packageInfo : null;

				if (isTargetPackage)
					updateDescLbl.text = "Checking for Updates...";
				updateDescLbl.visible = isTargetPackage;
				errorDescLbl.visible = isTargetPackage;
				importThirdPartyPackageBtn.visible = isTargetPackage;
				changeApiCompatibilityBtn.visible = isTargetPackage;
				importTmpEssentialPackageBtn.visible = isTargetPackage;
				InjectSelectionChanged(isTargetPackage, current);

				if (!isTargetPackage)
				{
					return;
				}

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

			private void OnDownloadSampleClicked()
			{
				if (UnityEditor.EditorUtility.DisplayDialog($"{MyPackageInAssetName} Info",
				                                            "다운로드 받으시겠습니까?", "Download", "Cancel"))
				{
					if (TrySelectDownloadPath(out string downloadFolderPath))
					{
						ImportSamplesToPath(current, downloadFolderPath);
						UnityEditor.EditorUtility.DisplayDialog($"{MyPackageInAssetName} Info", "Download 완료", "OK");
					}
				}
			}

			private void OnImportThirdPartyPackageBtnClicked()
			{
				if (current == null) return;
				string packagePath = current.GetPackagePath();
				if (string.IsNullOrEmpty(packagePath))
				{
					Debug.LogError("패키지 경로를 찾을 수 없습니다.");
					return;
				}

				string samplesPath = Path.Combine(packagePath, "Samples~");
				if (!Directory.Exists(samplesPath))
				{
					Debug.LogError("Samples~ 폴더를 찾을 수 없습니다.");
					return;
				}
			}

			private void CheckForUpdates()
			{
				bool needUpdate = current.CheckNeedUpdateByLastUpdate(out string latestVersion);
				updateDescLbl.text = $"{TitleStr}\n" + (needUpdate ? ">> 현재 최신버전이 아닙니다. 하단에 [Update]버튼을 눌러주세요 <<" : "현재 최신 버전입니다.");
				isUpdateChecking = false;
			}

			public void OnPackageAddedOrUpdated(PackageInfo packageInfo)
			{
				bool isTargetPackage = packageInfo != null && packageInfo.name == MyPackageName;
				current = isTargetPackage ? packageInfo : null;
				if (isTargetPackage)
				{
					ImportRuntimeUnityPackage($"{MyPackageInAssetName}.Runtime.{current.version}.unitypackage");
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
		private static void DownloadRuntimeUnityPackage(string srcPath, string exportFilePath)
		{
			Debug.LogError(srcPath + " -> " + exportFilePath);
			try
			{
				AssetDatabase.ExportPackage(srcPath, exportFilePath, ExportPackageOptions.Recurse);
				AssetDatabase.Refresh();
				Debug.Log("Folder download successfully: " + exportFilePath);
			}
			catch (IOException ex)
			{
				Debug.LogError("Error download : " + ex.Message);
			}
		}

		[MenuItem("CWJ/Package/" + MyPackageInAssetName + "/Import RuntimePackage", false)]
		private static void ImportRuntimeUnityPackage(string fileName)
		{
			if (EditorUtility.DisplayDialog(MyPackageInAssetName,
			                                $"{MyPackageInAssetName} 실행에 필요한 Runtime.unitypackage파일을\n 다운로드 받으시겠습니까?.\n다소 시간이 소모됩니다."
			                              , "Download", "Cancel"))
			{
				if (TrySelectDownloadPath(out string downloadFolderPath) && downloadFolderPath != null)
				{
					string packageSrcPath = $"{PackageFullPath}/{IgnoreRuntimeFolderName}"; //경로
					string exportFilePath = downloadFolderPath + $"/{fileName}";
					DownloadRuntimeUnityPackage(packageSrcPath, exportFilePath);
					ImportUnityPackage(exportFilePath);
				}
			}
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
