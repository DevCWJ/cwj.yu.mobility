using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CWJ
{
	public class OffScreenIndicatorVR : OffScreenIndicatorAbstract
	{
		private GameObject indicatorsParentObj;
		public float cameraDistance = 1;
		public float radius = 0.375f;
		public float indicatorScale = 0.05f;

		public void CreateIndicatorsParent()
		{
			indicatorsParentObj = new GameObject("IndicatorsParentObject");
			indicatorsParentObj.transform.SetParentAndReset(playerCamera.transform);
			indicatorsParentObj.transform.localPosition = new Vector3(0, 0, cameraDistance);
		}

		void Update()
		{
			int arrIndicatorCnt = arrowIndicators.Count;
			for (int i = 0; i < arrIndicatorCnt; i++)
			{
				UpdateIndicatorPosition(arrowIndicators[i], i);
				arrowIndicators[i].UpdateEffects();
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
			GameObject newArrowObj = new GameObject("Indicator_" + target.name, typeof(ArrowIndicatorVR));
			var newArrowIndicator = newArrowObj.GetComponent<ArrowIndicatorVR>();
			newArrowIndicator.transform.SetParentAndReset(indicatorsParentObj.transform, true);
			newArrowIndicator.VR_scale = new Vector3(indicatorScale, indicatorScale, indicatorScale);
			newArrowIndicator.transform.localScale = newArrowIndicator.VR_scale;
			newArrowIndicator.indicator = indicatorSettings[indicatorIndex];
			newArrowIndicator.target = target;
			newArrowIndicator.indicatorIndex = indicatorIndex;

			var spriteRenderer = newArrowObj.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = newArrowIndicator.indicator.offScreenSprite;
			spriteRenderer.color = newArrowIndicator.indicator.offScreenColor;
			newArrowIndicator.arrowImg = spriteRenderer;
			spriteRenderer.material.renderQueue = 4000;
			if (newArrowIndicator.indicator.isVisibleDistance)
			{
				var distTxtObj = new GameObject("DistanceText",typeof(TextMesh));
				distTxtObj.transform.SetParentAndReset(newArrowObj.transform);
				TextMesh distanceText = distTxtObj.GetComponent<TextMesh>();
				distanceText.alignment = TextAlignment.Center;
				distanceText.anchor = TextAnchor.UpperCenter;
				Font font = newArrowIndicator.indicator.distTextFont;
				if (font == null) font = DefaultFont;
				distanceText.font = font;

				distanceText.characterSize = 0.1f;
				distanceText.fontSize = newArrowIndicator.indicator.distTextSize * 3;

				distanceText.font = null;
                ////distanceText.font = (newArrowIndicator.indicator.distTextFont ?? defaultFont);//이게 왜 안되지
                distanceText.color = newArrowIndicator.indicator.distTextColor;
                //distanceText.GetComponent<RectTransform>().sizeDelta = new Vector2(125, newArrowIndicator.indicator.distHeightOffset);
                //if (newArrowIndicator.indicator.isDistTextOutlines)
                //{
                //	distTxtObj.AddComponent<Outline>();
                //}
                newArrowIndicator.distanceText = distanceText;
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
			Vector3 camPos = playerCamera.transform.position;
			Vector3 planePos = indicatorsParentObj.transform.position;
			//z position에 따라 pPlane변경
			Vector3 heading = camPos - planePos;
			Ray zRay = new Ray(planePos, heading);
            planePos = zRay.GetPoint(-0.001f * id);

            Plane plane = new Plane(heading.normalized, planePos);
			
			Vector3 targetPos = arrowIndicator.target.position + arrowIndicator.indicator.targetOffset;
			Ray toTargetRay = new Ray(camPos, targetPos - camPos);
			Vector3 hitPoint;
			float distance;
			if (plane.Raycast(toTargetRay, out distance))
			{
				hitPoint = toTargetRay.GetPoint(distance);
				if (Vector3.Distance(planePos, hitPoint) > radius)
				{
					//offscreen
					arrowIndicator.onScreen = false;
					Ray rToArrow = new Ray(planePos, hitPoint - planePos);
					arrowIndicator.transform.position = rToArrow.GetPoint(radius);
				}
				else
				{
					//inscreen
					arrowIndicator.onScreen = true;
					arrowIndicator.transform.position = hitPoint;
				}

				Vector3 plPlane = indicatorsParentObj.transform.localPosition;
				Vector3 plHitPoint = arrowIndicator.transform.localPosition;

				//vr camera회전각 적용
				float angle = (90 - playerCamera.transform.localEulerAngles.z) * Mathf.Deg2Rad;

				if ((arrowIndicator.onScreen && arrowIndicator.indicator.onScreenRotates) || (!arrowIndicator.onScreen && arrowIndicator.indicator.offScreenRotates))
				{
					angle = Mathf.Atan2(plHitPoint.y - plPlane.y, plHitPoint.x - plPlane.x);
				}
				arrowIndicator.transform.localEulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg - 90);

#if CWJ_EDITOR_DEBUG_ENABLED
				if (enableDebug)
				{
					MeshUtil.DebugDrawPlane(Vector3.Normalize(camPos - plPlane), plPlane, radius);
					Debug.DrawRay(toTargetRay.origin, toTargetRay.direction);
					Debug.DrawLine(camPos, hitPoint, Color.white);
					Debug.DrawLine(hitPoint, plPlane, Color.magenta);
				}
#endif
			}
			else
			{
				toTargetRay = new Ray(targetPos, camPos - targetPos);
				if (plane.Raycast(toTargetRay, out distance))
				{
					hitPoint = toTargetRay.GetPoint(distance);
					Ray rToArrow = new Ray(planePos, hitPoint - planePos);
					arrowIndicator.transform.position = rToArrow.GetPoint(-radius);
					arrowIndicator.onScreen = false;

					Vector3 plPlane = indicatorsParentObj.transform.localPosition;
					Vector3 plHitPoint = arrowIndicator.transform.localPosition;
					float angle = (90 - playerCamera.transform.localEulerAngles.z) * Mathf.Deg2Rad;
					if (arrowIndicator.indicator.offScreenRotates)
					{
						angle = Mathf.Atan2(plHitPoint.y - plPlane.y, plHitPoint.x - plPlane.x);
					}
					arrowIndicator.transform.localEulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg - 90);
				}
				else //plane에 평행함
				{
					//마지막 indicator position 사용하기
				}
			}
			if (arrowIndicator.isVisibleDistance && arrowIndicator.indicator.isVisibleDistance)
			{
				var vrArrow = (arrowIndicator as ArrowIndicatorVR);
				var distText = vrArrow.distanceText;

				distText.text = "\n" + ((arrowIndicator.target.position - playerCamera.transform.position).magnitude.ToString("N2") + "m");
				Vector3 ang = distText.transform.eulerAngles;
				distText.transform.rotation = Quaternion.Euler(new Vector3(ang.x, ang.y, 0));
            } // TODO VR 테스트
		}
	}
}