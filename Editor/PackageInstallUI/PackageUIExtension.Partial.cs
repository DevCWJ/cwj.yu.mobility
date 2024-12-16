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
using System;

namespace CWJ.Editor
{
	public partial class PackageUIExtension
	{
		private static Label sampleDescLbl;
		private static Button openDemoUrlBtn;

		private static void InjectUIElements(VisualElement root)
		{
			var partialLine = new VisualElement();
			root.Add(partialLine);
			openDemoUrlBtn = new Button();
			openDemoUrlBtn.text = "데모_PC빌드.exe in GoogleDrive";
			openDemoUrlBtn.clicked += OnOpenDemoUrlBtnClicked;
			openDemoUrlBtn.style.width = 200;
			partialLine.Add(openDemoUrlBtn);

			sampleDescLbl = new Label { text = " " };
			partialLine.Add(sampleDescLbl);
		}

		private const string _DescOfDemoImport =
			"\n Samples에서 데모씬을 import 받을수있습니다.\n[Import이후 데모씬 위치 : \nAssets/Samples/CWJ.YU.Mobility/{0}/데모용_Scene&Resources/YU_Demo.unity]\n[조작: [Left Shift]를 누른채 숫자1~9 or 0키 입력 : 토픽 1~9번 or 10번 활성화]";

		private static void OnOpenDemoUrlBtnClicked()
		{
			Application.OpenURL("https://drive.google.com/drive/folders/1m_9dOMbPEMA4S9hDls5PpNCcNXUULWh1?usp=sharing");
		}

		private static void InjectSelectionChanged(bool isTargetPackage, PackageInfo curPackageInfo)
		{
			if (isTargetPackage)
				sampleDescLbl.text = string.Format(_DescOfDemoImport, curPackageInfo.version);
			sampleDescLbl.visible = isTargetPackage;
			openDemoUrlBtn.visible = isTargetPackage;

			if (isTargetPackage)
			{
			}
		}
	}
}

#endif
