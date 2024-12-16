#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
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
			string packageFullPath = PackageFullPath;
			Debug.LogError(packageFullPath);
			AssetDatabase.ImportPackage(packageFullPath + $"/{UnityPackageFolderName}/{ThirdPartyPackageFileName}", true);
		}

		class UpmExtension : IPackageManagerExtension
		{
			public VisualElement CreateExtensionUI()
			{
				VisualElement ExtentionRoot = new VisualElement();

				VisualElement labelLine = new VisualElement();
				ExtentionRoot.Add(labelLine);
				descLbl = new Label { text = " " };
				labelLine.Add(descLbl);

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

				VisualElement lastLine = new VisualElement();
				ExtentionRoot.Add(lastLine);
				lastLine.style.flexDirection = FlexDirection.Row;
				lastLine.style.flexWrap = Wrap.Wrap;

				InjectUIElements(ExtentionRoot);

				return ExtentionRoot;
			}

			private Button changeApiCompatibilityBtn, importThirdPartyPackageBtn;
			private Label descLbl;
			private PackageInfo current = null;

			public void OnPackageSelectionChange(PackageInfo packageInfo)
			{
				current = packageInfo;
				bool isTargetPackage = current != null && current.name == MyPackageName;

				descLbl.visible = isTargetPackage;

				importThirdPartyPackageBtn.visible = isTargetPackage;
				changeApiCompatibilityBtn.visible = isTargetPackage;

				InjectSelectionChanged(isTargetPackage, current);

				if (!isTargetPackage)
				{
					return;
				}

				bool needChangeApi = PlayerSettings.GetApiCompatibilityLevel(EditorUserBuildSettings.selectedBuildTargetGroup) !=
				                     ApiCompatibilityLevel.NET_Unity_4_8;

				changeApiCompatibilityBtn.visible = needChangeApi;
				importThirdPartyPackageBtn.SetEnabled(!needChangeApi);
				if (needChangeApi)
				{
					descLbl.text = $"[{changeApiCompatibilityBtn.text}] 버튼을 눌러주세요.";
				}
				else
				{
					CheckForUpdates();
				}
			}

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
				descLbl.text = $"{DescStr}\n" + (needUpdate ? ">> 현재 최신버전이 아닙니다. 하단에 [Update]버튼을 눌러주세요 <<" : "현재 최신 버전입니다.");
			}

			public void OnPackageAddedOrUpdated(PackageInfo packageInfo) { }

			public void OnPackageRemoved(PackageInfo packageInfo) { }
		}
		//


		private static void ChangeApiCompatibility()
		{
			BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			PlayerSettings.SetApiCompatibilityLevel(targetGroup, ApiCompatibilityLevel.NET_Unity_4_8);

			Debug.Log($"API Compatibility Level changed to: {ApiCompatibilityLevel.NET_Unity_4_8.ToString()} (.NET Framework)");
		}
	}
}
#endif
