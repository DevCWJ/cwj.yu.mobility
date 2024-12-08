using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

[InitializeOnLoad]
public class PackageInstaller
{
	private static readonly string[] gitUrls = new string[]
	                                           {
		                                           UniRxUrl,
		                                           UniTaskUrl
	                                           };

	private const string UniRxUrl = "https://github.com/neuecc/UniRx.git?path=Assets/Plugins/UniRx/Scripts";
	private const string UniTaskUrl = "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask";

	[MenuItem("CWJ/Install CWJ Packages")]
	public static void InstallPackageMenu()
	{
		Debug.Log("Installing UnityDevTool and Required packages...");
		foreach (var url in gitUrls)
		{
			Client.Add(url);
		}
	}

	private static AddRequest currentRequest;
	private static int currentIndex;

	static PackageInstaller()
	{
		EditorApplication.update += InstallPackages;
	}

	private static void InstallPackages()
	{
		// Unity 에디터가 실행 중일 때만 동작
		if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling)
			return;

		// 설치 요청이 없으면 새 요청 시작
		if (currentRequest == null && currentIndex < gitUrls.Length)
		{
			Debug.Log($"Installing package: {gitUrls[currentIndex]}");
			currentRequest = Client.Add(gitUrls[currentIndex]);
		}

		// 요청 상태 확인
		if (currentRequest != null && currentRequest.IsCompleted)
		{
			if (currentRequest.Status == StatusCode.Success)
			{
				Debug.Log($"Successfully installed: {gitUrls[currentIndex]}");
			}
			else
				if (currentRequest.Status >= StatusCode.Failure)
				{
					Debug.LogError($"Failed to install: {gitUrls[currentIndex]} - {currentRequest.Error.message}");
				}

			// 다음 패키지로 이동
			currentRequest = null;
			currentIndex++;
		}

		// 모든 패키지 설치 완료 시 업데이트 종료
		if (currentIndex >= gitUrls.Length)
		{
			Debug.Log("All packages installed.");
			EditorApplication.update -= InstallPackages;
		}
	}
}
