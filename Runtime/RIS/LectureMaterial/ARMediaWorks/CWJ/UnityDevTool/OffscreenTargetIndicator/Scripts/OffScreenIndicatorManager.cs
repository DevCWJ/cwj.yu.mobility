using UnityEngine;
using System.Collections;

namespace CWJ
{
	/// <summary>
	/// canvas에 거리 표시용 text UI 추가함. 
	/// [20.07.13]
	/// </summary>
	public class OffScreenIndicatorManager : CWJ.Singleton.SingletonBehaviour<OffScreenIndicatorManager>, CWJ.Singleton.IDontAutoCreatedWhenNull
	{
#if CWJ_EDITOR_DEBUG_ENABLED
		public bool enableDebug = true;
#endif
		public bool isAutoStart = false;
		public bool isVRSupported = true;
		[HideConditional(nameof(isVRSupported))] public Camera vrCamera;
		[HideConditional(nameof(isVRSupported))] public float VR_cameraDistance = 1;
		[HideConditional(nameof(isVRSupported))] public float VR_radius = 0.375f;
		[HideConditional(nameof(isVRSupported))] public float VR_indicatorScale = 0.05f;

		[Space]
		
		public Camera nonVrCamera;
		public Canvas canvas;
		public int Canvas_circleRadius = 5; 
		public int Canvas_border = 50; //최소 50
		[DrawLine]
		public int Canvas_indicatorSize = 100; 

		public IndicatorSetting[] indicatorSettings;
		[ReadonlyConditional(EPlayMode.PlayMode)] public FixedTarget[] targetsToAdd;

		private OffScreenIndicatorAbstract manager;

        protected override void _Start()
        {
			if (!isAutoStart) return;

			if (isVRSupported)
			{
#if CWJ_EXISTS_CWJVR
                CWJ.VR.VR_Manager.Instance.AddEnabledEvent(Init);
#endif
            }
			else
			{
				Init(false);
			}
		}

		[InvokeButton]
        public void Init(bool isVrEnabled)
		{
			if (isVrEnabled)
			{
				manager = gameObject.GetOrAddComponent<OffScreenIndicatorVR>();
				manager.playerCamera = vrCamera;
				var vrManager = (manager as OffScreenIndicatorVR);
				vrManager.cameraDistance = VR_cameraDistance;
				vrManager.radius = VR_radius;
				vrManager.indicatorScale = VR_indicatorScale;
				vrManager.CreateIndicatorsParent();
			}
			else
			{
				manager = gameObject.GetOrAddComponent<OffScreenIndicatorCanvas>();
				manager.playerCamera = nonVrCamera;
				var canvasManager = (manager as OffScreenIndicatorCanvas);
				canvasManager.indicatorsParentObj = canvas.gameObject;
				canvasManager.circleRadius = Canvas_circleRadius;
				canvasManager.border = Canvas_border;
				canvasManager.indicatorSize = Canvas_indicatorSize;
			}

			manager.indicatorSettings = indicatorSettings;
#if CWJ_EDITOR_DEBUG_ENABLED
			manager.enableDebug = enableDebug;
#endif
			manager.CheckFields();

			foreach (FixedTarget target in targetsToAdd)
			{
				AddTargetIndicator(target.target, target.indicatorIndex);
			}
		}

		[InvokeButton]
		public void AddTargetIndicator(Transform target, int indicatorIndex)
		{
			manager?.AddTargetIndicator(target, indicatorIndex);
		}

		[InvokeButton]
		public void RemoveTargetIndicator(Transform target)
		{
			manager?.RemoveTargetIndicator(target);
		}

		[InvokeButton]
		public void RemoveAllTargetIndicator()
		{
			manager?.RemoveAllTargetIndicator();
		}
	}

	[System.Serializable]
	public class IndicatorSetting
	{
		public Sprite onScreenSprite;
		public Color onScreenColor = Color.white;
		public bool onScreenRotates = false;
		public Sprite offScreenSprite;
		public Color offScreenColor = Color.white;
		public bool offScreenRotates = true;
		public Vector3 targetOffset = new Vector3(0, 0, 0);

		public Transition transition = Transition.Fading;
		public float transitionDuration = 0.5f;

		public bool isVisibleDistance = true;
		public int distTextSize = 25;
		public Font distTextFont = null;
		[Tooltip("100+Canvas_indicatorSize")]
		public float distHeightOffset = 100;
		public Color distTextColor = Color.white;
		public bool isDistTextOutlines = true;

		[System.NonSerialized]
		public bool showOnScreen;
		[System.NonSerialized]
		public bool showOffScreen;

		public enum Transition
		{
			None,
			Fading,
			Scaling
		}
	}

	[System.Serializable]
	public class FixedTarget
	{
		public Transform target;
		public int indicatorIndex;
	}
}