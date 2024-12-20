using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using CWJ.SceneHelper;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace CWJ
{
	//[RequiredTag(new string[] { DescriptonManager.NormalTargetTag, DescriptonManager.LinePointObj_PointerTag })]
	public class DescriptonManager : CWJ.Singleton.SingletonBehaviour<DescriptonManager>, CWJ.SceneHelper.INeedSceneObj
	{
		/// <summary>
		/// ray hit 대상 오브젝트를 체크함
		/// </summary>
		public const string NormalTargetTag = "Desc_Target";

		/// <summary>
		/// 부모의 부모의 오브젝트이름을 가져와서 체크하게됨. 주의
		/// </summary>
		public const string LinePointObj_PointerTag = "Desc_Pointer";

		[VisualizeProperty] public SceneObjContainer sceneObjs { get; set; }

		public Description_LineDrawer theLineDrawer;

		public RectTransform panelRectTrf;

		public TextMeshProUGUI panelTitle;

		public TextMeshProUGUI panelDescription;

		public Image viewImg;

		public Scrollbar scrollBar;

		public Transform spherePointer;

		public bool isClickBgToClose = false;
		public bool isIn3DCanvas = false;
		public float maxDist = 200;
		public PanelPositionType panelPositionType;

		[Header("Auto Position Settings")]
		public float autoOffsetXPercent = 35.0f;

		public float autoOffsetYPercent = 15.0f;
		public float autoLimitPercent = 30.0f;

		public enum PanelPositionType
		{
			Auto = 0,
			LeftTop, Top, RightTop,
			Left, Medium, Right,
			LeftBottom, Bottom, RightBottom,
		}

		[Serializable]
		public struct MarginData
		{
			public float left;
			public float right;
			public float top;
			public float bottom;

			public MarginData(float left, float right, float top, float bottom)
			{
				this.left = left;
				this.right = right;
				this.top = top;
				this.bottom = bottom;
			}
		}


		[Header("Margin Settings")]
		public bool isMarginPercentage = true;

		[ShowConditional(EPlayMode.Always, predicateName = "isMarginPercentage")]
		public MarginData marginPercent = new MarginData(5, 5, 5, 5);

		[HideConditional(EPlayMode.Always, predicateName = "isMarginPercentage")]
		public MarginData marginPixel = new MarginData(10, 10, 10, 10);

		public VerticalLinePositionOnPanel verticalPositionOfLinePanelConnection;

		public enum VerticalLinePositionOnPanel
		{
			Top,
			Middle,
			Bottom,
		}

		private bool isDragging = false;

		private bool isMousePress = false;

		//public CWJ.Serializable.DictionaryVisualized<string, string> test_dataBase = new CWJ.Serializable.DictionaryVisualized<string, string>();

		[Header("Description Dictionary")]
		[Tooltip("스크립트 상단에 선언된 Tag와 적합할경우 띄워줌")]
		public CWJ.Serializable.DictionaryVisualized<Transform, DescriptionData> descDataDicByTag =
			new CWJ.Serializable.DictionaryVisualized<Transform, DescriptionData>();

		[Tooltip("이름이 같으면 띄워줌")]
		public CWJ.Serializable.DictionaryVisualized<string, DescriptionData> descDataDicByName =
			new CWJ.Serializable.DictionaryVisualized<string, DescriptionData>();

		[Serializable]
		public class DescriptionData
		{
			/// <summary>
			/// InvokeButton쓰고싶으면 생성자는 만들어둬야함
			/// </summary>
			public DescriptionData() { }

			public string _title;
			public string _content; //마지막에 적용된 값들. 안써도됨 테스트용

			public Func<Transform, (string title, string content, Sprite sprite)> getDataFunc;
			public bool useUpdateTxt { get; private set; }
			public Func<string> updateTxtFunc = null;

			public void SetTextDoTween(Func<string> updateTxtFunc)
			{
				if (updateTxtFunc != null)
				{
					useUpdateTxt = true;
					this.updateTxtFunc = updateTxtFunc;
				}
			}

			public DescriptionData(Func<Transform, (string title, string content, Sprite sprite)> function)
			{
				Subscribe(function);
			}

			public void Subscribe(Func<Transform, (string title, string content, Sprite sprite)> function)
			{
				this.getDataFunc = null;
				this.getDataFunc += function;
			}
		}

		[SerializeField] GameObject eventSystemObj;

		protected override void _Start()
		{
			IsOpenedPanel = false;
			if (EventSystem.current == null && FindObjectOfType<EventSystem>() == null)
			{
				eventSystemObj.gameObject.SetActive(true);
			}
		}


		KeyListener keyListener;

		protected override void _OnEnable()
		{
			if (keyListener == null)
			{
				keyListener = KeyEventManager_PC.GetKeyListener(KeyCode.Mouse0, false);
				keyListener.onTouchBegan.AddListener(OnLeftMouseDown);
				keyListener.onTouchEnded.AddListener(OnLeftMouseUp);
				keyListener.onTouchCanceled.AddListener(OnLeftMouseUp);
				keyListener.onUpdateEnded.AddListener(OnLoopChecker);
			}

			keyListener.enabled = true;
		}

		protected override void _OnDisable()
		{
			if (MonoBehaviourEventHelper.IS_QUIT) return;
			if (keyListener != null)
				keyListener.enabled = false;
		}

		protected override void _OnDestroy()
		{
			CloseDescription();
		}

		public void OpenDescription(Transform targetTrf, out DescriptionData descData)
		{
			if (descDataDicByTag.TryGetValue(targetTrf, out descData))
			{
				_OpenDescription(targetTrf, Vector3.zero, Vector3.zero, descData);
				return;
			}

			Debug.LogError("[DescriptionHlper] 없는 key " + targetTrf.gameObject.name, targetTrf.gameObject);
		}
#if UNITY_EDITOR
		bool isFocused = true;
		void OnApplicationFocus(bool isFocus)
		{
			isFocused = isFocus;
			if (isFocused && willOpenTarget)
			{
				var targetTrf = willOpenTarget;
				willOpenTarget = null;
				_OpenDescription(targetTrf, descCacheInfo.pointerPos, descCacheInfo.mousePos, descCacheInfo.descData);
			}
		}
#endif
		[InvokeButton]
		public void OpenDescription(Transform targetTrf, DescriptionData descData)
		{
			//Vector3 targetPos = targetTrf.position;
			_OpenDescription(targetTrf, Vector3.zero, Vector3.zero, descData);
		}

		private Tween updateTextTween;
		//[InvokeButton]
		//void TestUpdatePos(Transform targetTrf, DescriptionData descData)
		//{
		//    OpenDescriptionWithUpdateText(targetTrf, () => { return targetTrf.position.ToStringByDetailed(); }, descData);
		//}

		public void OpenDescriptionWithUpdateText(Transform targetTrf, Func<string> updateTxtCallback, DescriptionData descData)
		{
			descData.SetTextDoTween(updateTxtCallback);
			_OpenDescription(targetTrf, Vector3.zero, Vector3.zero, descData);
		}

		//public void OpenDescriptionWithUpdateText(Transform targetTrf, Func<Vector3> updatePosCallback, DescriptionData descData)
		//{
		//    descData.SetTextDoTween(() =>
		//    {
		//        var v = updatePosCallback.Invoke();
		//        return $"({v.x:F1}, {v.y:F1}, {v.z:F1})";
		//    });
		//    _OpenDescription(targetTrf, Vector3.zero, Vector3.zero, descData);
		//}

		[InvokeButton]
		public void CloseDescription()
		{
			if (updateTextTween != null && updateTextTween.IsActive())
			{
				updateTextTween.Kill();
				updateTextTween = null;
			}

			if (!MonoBehaviourEventHelper.IS_QUIT)
			{
				theLineDrawer.SetObjectToPointAt(null, panelRectTrf.position);
				IsOpenedPanel = false;
				spherePointer.gameObject.SetActive(false);
			}
		}

		Vector2 GetClickToSign(Vector3 clickPosition, int screenWidth, int screenHeight)
		{
			float signX = (clickPosition.x > screenWidth / 2.0f) ? -1 : 1;
			float signY = (clickPosition.y > screenHeight / 2.0f) ? -1 : 1;
			return new Vector2(signX, signY);
		}

		Vector3 ClampPosition(
			Vector3 position,
			Vector3 canvasPos,
			float canvasWidth,
			float canvasHeight,
			float panelWidth,
			float panelHeight,
			float marginLeft,
			float marginRight,
			float marginTop,
			float marginBottom,
			float? additionalMinX = null,
			float? additionalMaxX = null,
			float? additionalMinY = null,
			float? additionalMaxY = null)
		{
			float clampedX = Mathf.Clamp(
				position.x,
				canvasPos.x - canvasWidth / 2.0f + panelWidth / 2.0f + marginLeft,
				canvasPos.x + canvasWidth / 2.0f - panelWidth / 2.0f - marginRight
			);

			float clampedY = Mathf.Clamp(
				position.y,
				canvasPos.y - canvasHeight / 2.0f + panelHeight / 2.0f + marginBottom,
				canvasPos.y + canvasHeight / 2.0f - panelHeight / 2.0f - marginTop
			);

			// 추가적인 X 제한이 있는 경우
			if (additionalMinX.HasValue)
				clampedX = Mathf.Max(clampedX, additionalMinX.Value);
			if (additionalMaxX.HasValue)
				clampedX = Mathf.Min(clampedX, additionalMaxX.Value);

			// 추가적인 Y 제한이 있는 경우
			if (additionalMinY.HasValue)
				clampedY = Mathf.Max(clampedY, additionalMinY.Value);
			if (additionalMaxY.HasValue)
				clampedY = Mathf.Min(clampedY, additionalMaxY.Value);

			return new Vector3(clampedX, clampedY, position.z);
		}

		Vector3 GetPanelPosition(
			Vector3 clickPosition,
			int screenWidth,
			int screenHeight,
			float canvasWidth,
			float canvasHeight,
			Vector3 canvasPos,
			float panelWidth,
			float panelHeight)
		{
			Vector3 p = Vector3.zero;

			float marginLeft, marginRight, marginTop, marginBottom;
			if (isMarginPercentage)
			{
				marginLeft = (marginPercent.left / 100.0f) * screenWidth;
				marginRight = (marginPercent.right / 100.0f) * screenWidth;
				marginTop = (marginPercent.top / 100.0f) * screenHeight;
				marginBottom = (marginPercent.bottom / 100.0f) * screenHeight;
			}
			else
			{
				marginLeft = marginPixel.left;
				marginRight = marginPixel.right;
				marginTop = marginPixel.top;
				marginBottom = marginPixel.bottom;
			}

			if (panelPositionType == PanelPositionType.Auto)
			{
				Vector2 sign = GetClickToSign(clickPosition, screenWidth, screenHeight);

				p = new Vector3(
					clickPosition.x + sign.x * (screenWidth * autoOffsetXPercent / 100.0f),
					clickPosition.y + sign.y * (screenHeight * autoOffsetYPercent / 100.0f),
					0
				);

				// Auto 모드의 추가적인 제한 계산
				float lowPxLimit = (autoLimitPercent / 100.0f) * screenWidth;
				float highPxLimit = ((100.0f - autoLimitPercent) / 100.0f) * screenWidth;

				// ClampPosition 메소드를 사용하여 클램핑 (추가 제한 포함)
				p = ClampPosition(
					p,
					canvasPos,
					canvasWidth,
					canvasHeight,
					panelWidth,
					panelHeight,
					marginLeft,
					marginRight,
					marginTop,
					marginBottom,
					additionalMinX: lowPxLimit + (sign.x < 0 ? marginLeft : marginRight),
					additionalMaxX: highPxLimit - (sign.x < 0 ? marginRight : marginLeft),
					additionalMinY: sign.y < 0 ? marginBottom : marginTop,
					additionalMaxY: screenHeight - (sign.y < 0 ? marginTop : marginBottom)
				);
			}
			else
			{
				switch (panelPositionType)
				{
					case PanelPositionType.LeftTop:
						p.x = canvasPos.x - canvasWidth / 2.0f + panelWidth / 2.0f + marginLeft;
						p.y = canvasPos.y + canvasHeight / 2.0f - panelHeight / 2.0f - marginTop;
						break;

					case PanelPositionType.Top:
						p.x = canvasPos.x;
						p.y = canvasPos.y + canvasHeight / 2.0f - panelHeight / 2.0f - marginTop;
						break;

					case PanelPositionType.RightTop:
						p.x = canvasPos.x + canvasWidth / 2.0f - panelWidth / 2.0f - marginRight;
						p.y = canvasPos.y + canvasHeight / 2.0f - panelHeight / 2.0f - marginTop;
						break;

					case PanelPositionType.Left:
						p.x = canvasPos.x - canvasWidth / 2.0f + panelWidth / 2.0f + marginLeft;
						p.y = canvasPos.y;
						break;

					case PanelPositionType.Medium:
						p.x = canvasPos.x;
						p.y = canvasPos.y;
						break;

					case PanelPositionType.Right:
						p.x = canvasPos.x + canvasWidth / 2.0f - panelWidth / 2.0f - marginRight;
						p.y = canvasPos.y;
						break;

					case PanelPositionType.LeftBottom:
						p.x = canvasPos.x - canvasWidth / 2.0f + panelWidth / 2.0f + marginLeft;
						p.y = canvasPos.y - canvasHeight / 2.0f + panelHeight / 2.0f + marginBottom;
						break;

					case PanelPositionType.Bottom:
						p.x = canvasPos.x;
						p.y = canvasPos.y - canvasHeight / 2.0f + panelHeight / 2.0f + marginBottom;
						break;

					case PanelPositionType.RightBottom:
						p.x = canvasPos.x + canvasWidth / 2.0f - panelWidth / 2.0f - marginRight;
						p.y = canvasPos.y - canvasHeight / 2.0f + panelHeight / 2.0f + marginBottom;
						break;

					default:
						// 기본값으로 중앙에 위치하도록 설정
						p.x = canvasPos.x;
						p.y = canvasPos.y;
						break;
				}

				// ClampPosition 메소드 사용 (여백 고려)
				p = ClampPosition(
					p,
					canvasPos,
					canvasWidth,
					canvasHeight,
					panelWidth,
					panelHeight,
					marginLeft,
					marginRight,
					marginTop,
					marginBottom
				);
			}

			return p;
		}

		Vector2 GetLineStartPosition(Vector3 clickPosition, Vector3 panelPosition, Vector3 targetPos, int screenWidth, int screenHeight, float panelWidth,
		                             float panelHeight)
		{
			float yPos = 0.0f;
			Vector2 sign = GetClickToSign(clickPosition, screenWidth, screenHeight);
			switch (verticalPositionOfLinePanelConnection)
			{
				case VerticalLinePositionOnPanel.Bottom:
					yPos = panelPosition.y - panelHeight / 2.0f + 1;
					break;
				case VerticalLinePositionOnPanel.Middle:
					yPos = panelPosition.y;
					break;
				case VerticalLinePositionOnPanel.Top:
					yPos = panelPosition.y + panelHeight / 2.0f - 1;
					break;
			}

			// 화면 중앙을 기준으로 패널의 위치를 판단
			float centerX = screenWidth / 2.0f;
			float xPos;

			if (panelPosition.x > centerX)
				xPos = panelPosition.x - panelWidth / 2.0f;
			else
				xPos = panelPosition.x + panelWidth / 2.0f;

			return new Vector2(xPos, yPos);
		}

		[SerializeField, Readonly] private bool _isOpenedPanel = false;

		public bool IsOpenedPanel
		{
			get => _isOpenedPanel;
			private set
			{
				_isOpenedPanel = value;
				panelRectTrf.gameObject.SetActive(value);
				spherePointer.gameObject.SetActive(value);
				if (!value && viewImg)
					viewImg.transform.parent.gameObject.SetActive(false);
			}
		}

		RaycastHit[] hits = new RaycastHit[10];
		Transform willOpenTarget = null;
		(Vector3 pointerPos, Vector3 mousePos, DescriptionData descData) descCacheInfo;

		void _OpenDescription(Transform targetTrf, Vector3 pointerPos, Vector3 mousePos, DescriptionData descData)
		{
			if (!targetTrf || descData == null) return;
#if UNITY_EDITOR
			if (!isFocused)
			{
				willOpenTarget = targetTrf;
				descCacheInfo = (pointerPos, mousePos, descData);
				Debug.LogError("포커싱 빠져있어서 비활성화");
				return;
			}
#endif

			if (IsOpenedPanel)
			{
				Debug.LogError("Close");
				CloseDescription();
			}

			spherePointer.SetParent(targetTrf, true);
			if (pointerPos == Vector3.zero)
				spherePointer.localPosition = Vector3.zero;
			else
				spherePointer.position = pointerPos;
			Vector3 targetWorldPos = spherePointer.position;


			int screenWidth = Screen.width;
			int screenHeight = Screen.height;

			var canvasRectTrf = isIn3DCanvas ? sceneObjs.canvasOf3DRectTrf : sceneObjs.canvasOf2DRectTrf;
			float canvasWidth = canvasRectTrf.rect.width;
			float canvasHeight = canvasRectTrf.rect.height;

			var canvas = isIn3DCanvas ? sceneObjs.canvasOf3D : sceneObjs.canvasOf2D;
			if (pointerPos == Vector3.zero && mousePos == Vector3.zero)
			{
				// 타겟 오브젝트의 월드 좌표를 화면 좌표로 변환
				Vector3 targetScreenPos = sceneObjs.playerCamera.WorldToScreenPoint(targetTrf.position);
				targetScreenPos = new Vector3(targetScreenPos.x, targetScreenPos.y, 0);
				Ray ray = sceneObjs.playerCamera.ScreenPointToRay(targetScreenPos);
				int cnt = Physics.RaycastNonAlloc(ray, hits, maxDist);
				Debug.DrawRay(ray.origin, ray.direction * 200, Color.magenta, 3);
				for (int i = 0; i < cnt; i++)
				{
					var hit = hits[i];
					if (hit.collider.gameObject == targetTrf.gameObject) // if (hit.transform == targetTrf) 비교 절대금지 > transform은 rigidbody기준으로 반환함
					{
						// targetTrf가 RaycastHit에 포함되어 있으면 해당 정보를 사용하여 다시 호출
						_OpenDescription(targetTrf, hit.point, targetScreenPos, descData);
						return;
					}
				}

				mousePos = targetScreenPos;
				Debug.LogError("[DescriptionManager] RaycastHit에서 targetTrf를 찾지 못했습니다." + cnt, targetTrf);
			}

			CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
			float scaleFactor = 1.0f;

			if (canvasScaler != null && canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
				scaleFactor = canvasWidth / canvasScaler.referenceResolution.x;

			// 패널 크기 계산
			float panelWidth = panelRectTrf.rect.width * scaleFactor;
			float panelHeight = panelRectTrf.rect.height * scaleFactor;

			// 패널 위치 설정
			Vector3 panelPos = GetPanelPosition(
				mousePos,
				screenWidth,
				screenHeight,
				canvasWidth,
				canvasHeight,
				canvas.transform.position,
				panelWidth,
				panelHeight
			);

			// 패널의 위치를 anchoredPosition으로 설정 (피벗이 중앙으로 설정되어 있다고 가정)
			panelRectTrf.position = panelPos;

			// 선의 시작점 설정
			Vector2 startpointPosition =
				GetLineStartPosition(mousePos, panelRectTrf.position, targetWorldPos, screenWidth, screenHeight, panelWidth, panelHeight);
			theLineDrawer.SetObjectToPointAt(spherePointer, startpointPosition);

			IsOpenedPanel = true;

			if (descData != null)
			{
				if (descData.useUpdateTxt)
				{
					panelTitle.SetText(descData._title);

					DOFade(panelDescription, 1, 0);

					updateTextTween = DOTween.Sequence()
					                         .AppendInterval(0.4f)
					                         .Append(DOFade(panelDescription, 0.5f, 0.25f)) // 페이드 아웃
					                         .AppendCallback(() =>
					                         {
						                         string newText = descData.updateTxtFunc.Invoke();
						                         panelDescription.SetText(newText);
					                         })
					                         .Append(DOFade(panelDescription, 1, 0.1f)) // 페이드 인
					                         .SetLoops(-1) // 무한 반복
					                         .SetEase(Ease.Linear);
				}
				else
				{
					if (descData.getDataFunc != null)
					{
						var strData = descData.getDataFunc.Invoke(targetTrf);
						if (strData.sprite)
						{
							SetSpriteAndAdjustScale(viewImg, strData.sprite);
						}
						else
						{
							viewImg.transform.parent.gameObject.SetActive(false);
						}

						descData._title = strData.title;
						descData._content = strData.content;
					}

					panelTitle.SetText(descData._title);
					panelDescription.SetText(descData._content);
				}
			}


			StartCoroutine(ScrollBarDelay(pointerPos == Vector3.zero));
		}

		static void SetSpriteAndAdjustScale(Image image, Sprite sprite)
		{
			Debug.Assert(image, "Image 컴포넌트가 할당되지 않았습니다.");
			Debug.Assert(sprite, "할당할 Sprite가 null입니다.");

			// Image의 RectTransform 가져오기
			RectTransform imageRect = image.GetComponent<RectTransform>();
			if (!imageRect)
			{
				Debug.LogError("Image 컴포넌트에 RectTransform이 없습니다.");
				return;
			}

			// 부모 RectTransform 가져오기
			RectTransform parentRect = imageRect.parent.GetComponent<RectTransform>();
			if (!parentRect)
			{
				Debug.LogError("Image의 부모에 RectTransform이 없습니다.");
				return;
			}

			image.transform.parent.gameObject.SetActive(true);

			image.sprite = sprite;
			image.SetNativeSize();

			LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect.GetComponentInParent<LayoutGroup>().GetComponent<RectTransform>());

			ThreadDispatcher.Enqueue(() =>
			{
				// 부모 Rect 크기와 Image Rect 크기 비교
				float parentWidth = parentRect.rect.width;
				float parentHeight = parentRect.rect.height;
				float spriteWidth = sprite.rect.width;
				float spriteHeight = sprite.rect.height;

				// 스케일 조정이 필요한지 확인
				if (spriteWidth > parentWidth || spriteHeight > parentHeight)
				{
					// 가로와 세로 중 더 작은 비율을 선택하여 스케일 계산
					float scaleX = parentWidth / spriteWidth;
					float scaleY = parentHeight / spriteHeight;
					float scale = Mathf.Min(scaleX, scaleY);

					// 원본 비율을 유지하면서 스케일 조정
					imageRect.localScale = new Vector3(scale, scale, 1f);
				}
				else
				{
					// 스케일을 원래대로 복원
					imageRect.localScale = Vector3.one;
				}
			});

		}


		IEnumerator ScrollBarDelay(bool isSphereLocalZero)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(panelRectTrf);
			yield return null;
			panelDescription.gameObject.SetActive(false);
			if (scrollBar.gameObject.activeSelf)
			{
				scrollBar.value = 0.99f;
				yield return new WaitForEndOfFrame();
				scrollBar.value = 0.99f;
			}

			yield return null;
			panelDescription.gameObject.SetActive(true);

			if (isSphereLocalZero)
				spherePointer.localPosition = Vector3.zero;
		}

		public static TweenerCore<Color, Color, ColorOptions> DOFade(TMP_Text target, float endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> t = DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration);
			t.SetTarget(target);
			return t;
		}

		void OnLeftMouseDown()
		{
			isMousePress = true;
		}

		bool isUserClicked;

		void OnLeftMouseUp()
		{
			isUserClicked = false;
			if (!isDragging)
				isUserClicked = true;
			isMousePress = false;
			isDragging = false;
		}

		void OnLoopChecker()
		{
			if (isMousePress)
			{
				if (IsMouseMoving())
					isDragging = true;
			}

			if (!isUserClicked)
			{
				return;
			}

			isUserClicked = false;

			if (!sceneObjs.hasPlayerCam)
			{
				return;
			}

			if (Event.current != null && EventSystem.current.IsPointerOverGameObject())
			{
				return;
			}

			Vector3 clickPosition = Input.mousePosition;
			var ray = sceneObjs.playerCamera.ScreenPointToRay(clickPosition);
			bool isHit = Physics.Raycast(ray, out RaycastHit hit, maxDist);
			var hitTrf = hit.transform;
			if (isHit && hitTrf != null)
			{
				Transform targetTrf = null;
				if (hitTrf.CompareTag(NormalTargetTag))
					targetTrf = hitTrf;
				else if (hitTrf.CompareTag(LinePointObj_PointerTag))
					targetTrf = hitTrf.parent.parent;

				if (targetTrf != null)
				{
					if (descDataDicByTag.TryGetValue(targetTrf, out var descData))
					{
						if (descData != null)
						{
							_OpenDescription(targetTrf, hit.point, clickPosition, descData);
							return;
						}
					}
				}
				else
				{
					string hitName = hitTrf.name;
					if (descDataDicByName.Count > 0 && descDataDicByName.TryGetValue(hitName, out var descData))
					{
						if (descData != null)
						{
							_OpenDescription(hitTrf, hit.point, clickPosition, descData);
							return;
						}
					}
				}
			}

			if (isClickBgToClose)
				CloseDescription();
		}

		bool IsMouseMoving()
		{
			float moveX = Input.GetAxis("Mouse X");
			if (moveX < 0)
				moveX *= -1;
			if (moveX > 0.05f)
				return true;

			float moveY = Input.GetAxis("Mouse Y");
			if (moveY < 0)
				moveY *= -1;
			if (moveY > 0.05f)
				return true;

			return false;
		}
	}
}
