using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CWJ
{
	public class OffScreenIndicatorCanvas : OffScreenIndicatorAbstract
	{
		public GameObject indicatorsParentObj;
		public float cameraDistance = 5;
		public int circleRadius = 100;
		public int border = 10;
		public int indicatorSize = 100;
		private float realBorder;
		private Vector2 referenceResolution;
		private float screenScaleX;
		private float screenScaleY;
		private bool screenScaled = false;

		void Start()
		{
			if (indicatorsParentObj == null || indicatorsParentObj.GetComponent<Canvas>() == null)
			{
				Debug.LogError("OffScreenIndicator Canvas field requieres a Canvas GameObject", gameObject);
				Debug.Break();
			}

			//scale 얻기위해 referenceResolution 참조
			if (indicatorsParentObj.GetComponent<CanvasScaler>().uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
			{
				referenceResolution = indicatorsParentObj.GetComponent<CanvasScaler>().referenceResolution;
				Vector2 screenResolution = new Vector2(Screen.width, Screen.height);
				screenScaleX = screenResolution.x / referenceResolution.x;
				screenScaleY = screenResolution.y / referenceResolution.y;
				screenScaled = true;
				screenScaled = false;
#if CWJ_EDITOR_DEBUG_ENABLED
				//Debug.Log("ReferenceResolution = " + referenceResolution.ToString());
				//Debug.Log("ScreenResolution = " + screenResolution.ToString());
				//Debug.Log("ScreenScaleX = " + screenScaleX.ToString());
				//Debug.Log("ScreenScaleY = " + screenScaleY.ToString());
#endif
			}
			else
			{
				screenScaled = false;
			}
			//화면 크기에 따라 indicator 크기도 다름

			if (screenScaled)
			{
				indicatorSize = Mathf.RoundToInt(indicatorSize * screenScaleX);
			}
			realBorder = (indicatorSize / 2f) + border;
		}

		void LateUpdate()
		{
			foreach (ArrowIndicatorAbstract arrowIndicator in arrowIndicators)
			{
				UpdateIndicatorPosition(arrowIndicator);
				arrowIndicator.UpdateEffects();
			}
		}

		public override void AddTargetIndicator(Transform target, int indicatorIndex)
		{
			if (indicatorIndex >= indicatorSettings.Length)
			{
				Debug.LogError($"Indicator ID not valid. Check {nameof(OffScreenIndicatorManager)} indicatorSettings", gameObject);
				return;
			}
			if (ExistsIndicator(target))
			{
				Debug.LogError("Target already added: " + target.name, gameObject);
				return;
			}

			GameObject newArrowObj = new GameObject("Indicator_" + target.name, typeof(ArrowIndicatorCanvas));
			newArrowObj.transform
				.SetParentAndReset(indicatorsParentObj.transform, true);
			var newArrowIndicator = newArrowObj.GetComponent<ArrowIndicatorCanvas>();
			newArrowIndicator.indicator = indicatorSettings[indicatorIndex];
			newArrowIndicator.target = target;
			newArrowIndicator.indicatorIndex = indicatorIndex;

			Image img = newArrowObj.AddComponent<Image>();
			img.sprite = newArrowIndicator.indicator.offScreenSprite;
			img.color = newArrowIndicator.indicator.offScreenColor;
			newArrowObj.AddComponent<Outline>();
			newArrowIndicator.arrowImg = img;

			newArrowObj.GetComponent<RectTransform>().sizeDelta = new Vector2(indicatorSize, indicatorSize);

			if (newArrowIndicator.indicator.isVisibleDistance)
			{
				var distTxtObj = new GameObject("DistanceText");
				distTxtObj.transform.SetParentAndReset(newArrowObj.transform);
				Text distText = distTxtObj.AddComponent<Text>();
				distText.alignment = TextAnchor.LowerCenter;
				distText.fontSize = newArrowIndicator.indicator.distTextSize;
				Font font = newArrowIndicator.indicator.distTextFont;
				if (font == null) font = DefaultFont;
				distText.font = font;

				//distText.font = (newArrowIndicator.indicator.distTextFont ?? defaultFont);//이게 왜 안되지
				distText.color = newArrowIndicator.indicator.distTextColor;
				distText.GetComponent<RectTransform>().sizeDelta = new Vector2(125, newArrowIndicator.indicator.distHeightOffset);
				if (newArrowIndicator.indicator.isDistTextOutlines)
				{
					distTxtObj.AddComponent<Outline>();
				}
				newArrowIndicator.distanceText = distText;
				newArrowIndicator.isVisibleDistance = true;
			}

			if (!newArrowIndicator.indicator.showOffScreen)
			{
				newArrowIndicator.gameObject.SetActive(false);
			}
			newArrowIndicator.onScreen = false;
			arrowIndicators.Add(newArrowIndicator);
		}

		protected override void UpdateIndicatorPosition(ArrowIndicatorAbstract arrowIndicator, int id = 0)
		{
			Vector3 targetScreenPos = playerCamera.WorldToScreenPoint(arrowIndicator.target.localPosition + arrowIndicator.indicator.targetOffset);
			
			Vector3 heading = arrowIndicator.target.position - playerCamera.transform.position;

			bool behindCamera = Vector3.Dot(playerCamera.transform.forward, heading) < 0;
			float angle;

			if (targetScreenPos.x > Screen.width - realBorder || targetScreenPos.x < realBorder || targetScreenPos.y > Screen.height - realBorder || targetScreenPos.y < realBorder || behindCamera)
			{
				//offscreen
				arrowIndicator.onScreen = false;
				angle = Mathf.Atan2(targetScreenPos.y - (Screen.height / 2), targetScreenPos.x - (Screen.width / 2));
				float xCut, yCut;
				//양측
				if (targetScreenPos.x - Screen.width / 2 > 0)
				{
					//Right
					xCut = Screen.width / 2 - realBorder;
					yCut = xCut * Mathf.Tan(angle);
				}
				else
				{
					//Left
					xCut = -Screen.width / 2 + realBorder;
					yCut = xCut * Mathf.Tan(angle);
				}
				//아래위
				if (yCut > Screen.height / 2 - realBorder)
				{
					//Up
					yCut = Screen.height / 2 - realBorder;
					xCut = yCut / Mathf.Tan(angle);
				}
				if (yCut < -Screen.height / 2 + realBorder)
				{
					//Down
					yCut = -Screen.height / 2 + realBorder;
					xCut = yCut / Mathf.Tan(angle);
				}
				if (behindCamera)
				{
					xCut = -xCut;
					yCut = -yCut;
				}
				if (screenScaled)
				{
					xCut /= screenScaleX;
					yCut /= screenScaleY;
				}
				arrowIndicator.transform.localPosition = new Vector3(xCut, yCut, 0);
			}
			else
			{
				//inscreen
				arrowIndicator.onScreen = true;
				float xScaled = targetScreenPos.x - (Screen.width / 2);
				float yScaled = targetScreenPos.y - (Screen.height / 2);
				if (screenScaled)
				{
					xScaled /= screenScaleX;
					yScaled /= screenScaleY;
				}
				arrowIndicator.transform.localPosition = new Vector3(xScaled, yScaled, 0);
			}

			//rotate
			if ((arrowIndicator.onScreen && arrowIndicator.indicator.onScreenRotates) || (!arrowIndicator.onScreen && arrowIndicator.indicator.offScreenRotates))
			{
				if (behindCamera)
				{
					angle = Mathf.Atan2(-(targetScreenPos.y - (Screen.height / 2)), -(targetScreenPos.x - (Screen.width / 2)));
				}
				else
				{
					angle = Mathf.Atan2(targetScreenPos.y - (Screen.height / 2), targetScreenPos.x - (Screen.width / 2));
				}
			}
			else
			{
				angle = 90 * Mathf.Deg2Rad;
			}
			arrowIndicator.transform.localEulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg - 90);

			if (arrowIndicator.isVisibleDistance && arrowIndicator.indicator.isVisibleDistance)
			{
				var distText = (arrowIndicator as ArrowIndicatorCanvas).distanceText;
				distText.text = (heading.magnitude.ToString("N2") + "m");

				Quaternion distanceTextRot = Quaternion.identity;

				if (OffScreenIndicatorManager.Instance.canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                {
					Vector3 ang = distText.transform.eulerAngles;
					distanceTextRot = Quaternion.Euler(new Vector3(ang.x, ang.y, 0));
				}

				distText.transform.rotation = distanceTextRot;
			}
		}
	}
}