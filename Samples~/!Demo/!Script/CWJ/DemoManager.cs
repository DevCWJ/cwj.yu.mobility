using System;
using System.Linq;
using CWJ;
using CWJ.YU.Mobility;
using UnityEngine;

[RequiredTag(new string[] { DescriptonManager.NormalTargetTag, DescriptonManager.LinePointObj_PointerTag })]
public class DemoManager : MonoBehaviour
{
	[SerializeField] vThirdPersonCamera tpsCamera;
	[SerializeField] Topic[] topics_prefabs;

	bool escToggle;

	private void Start()
	{
		ProjectManager.OnSingletonCreated += (_) => InitInputEvent();
		//StartCoroutine(WebGLHelper.WebGLWindow.IE_MakeFullscreen());
	}

	// [InvokeButton]
	// public void Solution(int n)
	// {
	//     int[,] answer = new int[n, n];
	//     int lastX = 0, lastY = 0;
	//     int xDir = 1;
	//     int yDir = 0;
	//     int max = n * n;
	//     answer[0, 0] = 1;
	//     for (int i = 1; i < max;)
	//     {
	//         for (int j = 0; j < n - 1; ++j)
	//         {
	//             int y = lastY + yDir;
	//             int x = lastX + xDir;
	//             if (answer[y, x] != 0)
	//             {
	//                 break;
	//             }
	//             lastY = y;
	//             lastX = x;
	//             Debug.LogError($"{lastX}, {lastY}");
	//             answer[lastY, lastX] = ++i;
	//         }
	//
	//         (xDir, yDir) = (yDir, xDir);
	//         xDir *= -1;
	//     }
	//
	//     Debug.LogError(answer.ToStringByDetailed());
	// }
	//


	void InitInputEvent()
	{
		var rightMouseCallback = KeyEventManager_PC.GetKeyListener(KeyCode.Mouse1, true);
		rightMouseCallback.onTouchMoving.AddListener(() => SetLockCameraAndFreeCursor(true));
		rightMouseCallback.onTouchEnded.AddListener(() => SetLockCameraAndFreeCursor(false));
		var escCallback = KeyEventManager_PC.GetKeyListener(KeyCode.Escape, true);
		escCallback.onTouchBegan.AddListener(() => SetLockCameraAndFreeCursor(escToggle = !escToggle));
		escCallback.enabled = true;
		SetLockCameraAndFreeCursor(false);

		//WebGLHelper 테스트w
#if UNITY_WEBGL && !UNITY_EDITOR
		var fCallback = KeyEventManager_PC.GetKeyListener(KeyCode.F, true);
		fCallback.onTouchBegan.AddListener(WebGLHelper.WebGLWindow.SwitchFullscreen);
		fCallback.enabled = true;

		var nCallback = KeyEventManager_PC.GetKeyListener(KeyCode.G, true);
		nCallback.onTouchBegan.AddListener(() => WebGLHelper.WebGLWindow.GoUrl("https://www.google.com/"));
		nCallback.enabled = true;
#endif
	}


	void SetLockCameraAndFreeCursor(bool isEnabled)
	{
		//if (!isEnabled)
		//    tpsCamera.Init();
		tpsCamera.lockCamera = isEnabled;
		Cursor.visible = isEnabled;
		Cursor.lockState = isEnabled ? CursorLockMode.None : CursorLockMode.Locked;
	}


	private void Update()
	{
		if (!Input.GetKey(KeyCode.LeftShift))
		{
			return;
		}

		int wannaIndex = -1;
		for (int i = (int)KeyCode.Alpha0; i <= (int)KeyCode.Alpha9; ++i)
		{
			if (Input.GetKeyDown((KeyCode)i))
			{
				int pressKey = i - ((int)KeyCode.Alpha0);
				wannaIndex = pressKey > 0 ? pressKey - 1 : 9;
				break;
			}
		}

		if (wannaIndex >= 0)
		{
			Debug.Log("Start Topic Index: " + wannaIndex);
			ProjectManager.SetCurTopicIndex(wannaIndex);
		}
	}
}
