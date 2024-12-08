
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

[InitializeOnLoad]
public class PackageInstaller
{
	private static readonly string[] gitUrls = new string[]
	                                           {
		                                           UniRxUrl, UniTaskUrl
	                                           };
	private const string UniRxUrl = "https://github.com/neuecc/UniRx.git?path=Assets/Plugins/UniRx/Scripts";
	private const string UniTaskUrl = "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask";

	static PackageInstaller()
	{
		EditorApplication.update += InstallPackages;
	}

	private static void InstallPackages()
	{
		// 유니티가 실행 중일 때만 동작하도록 설정
		if (!EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isCompiling)
		{
			// 설치 요청
			Debug.Log("Installing required packages...");
			foreach (var url in gitUrls)
			{
				Client.Add(url);
			}
			// 스크립트 등록 해제
			EditorApplication.update -= InstallPackages;
		}
	}
}

