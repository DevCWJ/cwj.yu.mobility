using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace CWJ.YU.Mobility
{
	public class Topic6_1_Solution : MonoBehaviour
	{
		public XyInputFieldSet xyIpfPack;
		public Ipf_IntValidator ipfValidator;

		/// <summary>
		/// 사용자가 입력한 끝점(x,y)
		/// </summary>
		public Transform targetPointTrf;

		/// <summary>
		/// 0,0과 사용자가 입력한 끝점사이의 팔꿈치 관절위치의 점
		/// </summary>
		public Transform midPointTrf;

		public TextMeshProUGUI targetPointText;

		/// <summary>
		/// 좌표(0,0,0)의 자식에있는 local space lineRenderer
		/// </summary>
		public LineRenderer blueLineRendererFromZero;

		/// <summary>
		/// midPointTrf의 자식에있는 local space lineRenderer
		/// </summary>
		public LineRenderer blueLineRendererFromMid; //

		/// <summary>
		/// midPointTrf의 자식에있는 local space lineRenderer
		/// </summary>
		public LineRenderer redLineRenderer; //(0,0) 제로포인트에서 midPointTrf까지 이어진 방향

		public TextMeshProUGUI angle1Text;
		public TextMeshProUGUI angle2Text;

		[Tooltip("각도1 적힌 버튼")]
		public Button elbowUpwardBtn;

		[Tooltip("각도2 적힌 버튼")]
		public Button elbowDownwardBtn;


		[SerializeField] private float unitXDistance, unitYDistance;
		private DG.Tweening.Sequence lastPosSequence;


		private bool isAbleElbowAdd = false;

		[InvokeButton]
		public void SetEnableElbowAddFunc()
		{
			isAbleElbowAdd = true;
			_SetActiveElbowAddFunc(true);
		}

		public void SetDisableElbowAddFunc()
		{
			isAbleElbowAdd = false;
			_SetActiveElbowAddFunc(false);
		}

		void _SetActiveElbowAddFunc(bool isActiveFunc)
		{
			angle1Text.gameObject.SetActive(false);
			midPointTrf.gameObject.SetActive(false);
			if (!isActiveFunc)
			{
				targetPointText.SetText("(x,y)");
				LineRendererSetColor(Color.blue);
			}
		}


		private void Start()
		{
			blueLineRendererFromZero.useWorldSpace = false;
			xyIpfPack.Init($"끝점", ipfValidator, Color.black);
			xyIpfPack.SubscribeChangeVector2(SetPosition);
			elbowUpwardBtn.onClick.AddListener(OnClickElbowUpwardBtn);
			elbowDownwardBtn.onClick.AddListener(OnClickElbowDownwardBtn);
		}

		private void OnEnable()
		{
			SetDisableElbowAddFunc();
			string lastX = xyIpfPack.xIpf.text;
			if (!string.IsNullOrEmpty(lastX))
			{
				SetPosition(lastUserInputPos, false);
			}
		}

		void LineRendererSetColor(Color color)
		{
			blueLineRendererFromZero.startColor = color;
			blueLineRendererFromZero.endColor = color;
			blueLineRendererFromMid.startColor = color;
			blueLineRendererFromMid.endColor = color;
		}

		void OnClickElbowUpwardBtn()
		{
			LineRendererSetColor(Color.blue);

			//팔꿈치 상향 케이스
			CalculateElbowIK(isUpwardElbow: true);
		}

		void OnClickElbowDownwardBtn()
		{
			LineRendererSetColor(new Color().GetOrange());

			//팔꿈치 하향 케이스
			CalculateElbowIK(isUpwardElbow: false);
		}


		void CalculateElbowIK(bool isUpwardElbow)
		{
			// 목표 위치 (사용자가 입력한 끝점)
			// Vector2 targetPos = ConvertUnityPosToUsersidePos(targetPointTrf.localPosition);

			var targetEndPoint = lastUserInputPos;
			float distanceToTarget = GetDistance(targetEndPoint);

			if (distanceToTarget <= 0.0f)
			{
				Debug.LogError("목표 거리가 0.");
				return;
			}

			float baseAngle = GetAngle(targetEndPoint);

			// θ₁ 계산: 상향일 경우 -절반, 하향일 경우 +기존각도의 + 0.25~0.4(랜덤)배율 만큼 더하거나 빼줌
			float baseAngleHalf = baseAngle * UnityEngine.Random.Range(0.25f, 0.4f);
			float angle1 = baseAngle + ((isUpwardElbow ? -1 : 1) * baseAngleHalf);

			float angle1Rad = angle1 * Mathf.Deg2Rad; // 라디안으로 변환

			// 중간 점(midPoint) 계산: 거리의 절반, θ₁ 방향
			Vector2 midPoint = new Vector2(
				(distanceToTarget * 0.5f) * Mathf.Cos(angle1Rad),
				(distanceToTarget * 0.5f) * Mathf.Sin(angle1Rad)
			);

			// 로컬 스페이스에 맞게 변환
			Vector3 midPointLocal = ConvertUsersidePosToUnityPos(midPoint);
			midPointTrf.localPosition = new Vector3(midPointLocal.x, midPointLocal.y, 0);

			// 라인렌더러 설정 (로컬스페이스 기준)
			// 첫 번째 관절: 제로포인트 -> 중간점 (zeroPointTrf 기준)
			blueLineRendererFromZero.SetPosition(1, midPointLocal); // zeroPointTrf -> midPointTrf

			Vector3 targetPosLocal = ConvertUsersidePosToUnityPos(targetEndPoint);
			blueLineRendererFromMid.SetPosition(1, targetPosLocal - midPointLocal); // midPointTrf -> targetPointTrf

			Vector2 direction = midPoint.normalized; // 중간점 방향 벡터
			Vector2 extendedPoint = direction * unitXDistance; // 연장된 위치 (로컬 기준)
			redLineRenderer.SetPosition(1, new Vector3(extendedPoint.x, extendedPoint.y, 0)); // midPointTrf 시작 ~ 연장 위치

			midPointTrf.gameObject.SetActive(true);

			// angle1Text 위치 설정 (중간점 방향으로 고정된 거리)
			Vector2 angle1TextPos = direction * (unitYDistance * 2); // 고정 거리
			angle1Text.rectTransform.localPosition = new Vector3(angle1TextPos.x, angle1TextPos.y * 0.5f, 0);

			// 각도 텍스트 갱신
			angle1Text.SetText($"θ₁={angle1:F1}°");
			angle1Text.gameObject.SetActive(true);

			Vector2 redLineDirection = extendedPoint.normalized; // 빨간선 방향 벡터
			Vector2 midToTargetDirection = (targetEndPoint - midPoint).normalized; // 중간점에서 목표점 방향 벡터

			// θ₂ = 빨간선 방향과 목표점 방향 사이의 각도
			float cosTheta2 = Vector2.Dot(redLineDirection, midToTargetDirection); // 두 벡터의 내적
			float angle2 = Mathf.Acos(cosTheta2) * Mathf.Rad2Deg; // 내적으로 각도 계산 (라디안 -> 도 단위 변환)

			// 방향 확인 (외적)
			Vector3 crossProduct = Vector3.Cross(new Vector3(redLineDirection.x, redLineDirection.y, 0),
			                                     new Vector3(midToTargetDirection.x, midToTargetDirection.y, 0));
			if (crossProduct.z < 0) // 방향에 따라
				angle2 = -angle2;

			// angle2Text 위치 설정 midPoint자식으로 있음
			angle2Text.rectTransform.localPosition = extendedPoint;
			angle2Text.rectTransform.pivot = new Vector2(isUpwardElbow ? 0 : 1, angle2Text.rectTransform.pivot.y);
			angle2Text.SetText($"θ₂={angle2:F1}°");


			// Debug.Log($"팔꿈치 {(isUpwardElbow ? "상향" : "하향")} 케이스 계산 완료");
		}

		Vector2 ConvertUnityPosToUsersidePos(Vector3 objPos)
		{
			return new Vector2(objPos.x / unitXDistance, objPos.y / unitYDistance);
		}

		Vector3 ConvertUsersidePosToUnityPos(Vector2 userSidePos)
		{
			return new Vector3(userSidePos.x * unitXDistance, userSidePos.y * unitYDistance, 0);
		}

		Vector2 lastUserInputPos = Vector2.zero;

		void SetPosition(Vector2 usersidePos)
		{
			lastUserInputPos = usersidePos;
			SetPosition(usersidePos, true);
		}

		public void SetPosition(Vector2 usersidePos, bool useTween)
		{
			_SetActiveElbowAddFunc(false);
			lastPosSequence?.Kill();

			Vector3 willLocalPosition = ConvertUsersidePosToUnityPos(usersidePos);

			if (useTween && Application.isPlaying)
			{
				lastPosSequence = DOTween.Sequence().SetTarget(this)
				                         .SetAutoKill(true)
				                         .Append(targetPointTrf.DOLocalMove(willLocalPosition, 2))
				                         .SetEase(Ease.InOutQuad)
				                         .OnUpdate(OnUpdateCurPosToUI)
				                         .OnComplete(() =>
				                         {
					                         OnCompletePosUpdate(willLocalPosition);
				                         });
			}
			else
			{
				OnCompletePosUpdate(willLocalPosition);
			}

			//Debug.LogError(inputVector.ToStringByDetailed());
		}


		void OnCompletePosUpdate(Vector3 targetLocalPos)
		{
			lastPosSequence = null;
			targetPointTrf.localPosition = targetLocalPos;
			OnUpdateCurPosToUI();
			Vector2 angle1Pos = targetLocalPos.normalized * (unitYDistance * 2);
			angle1Text.rectTransform.localPosition = new Vector3(angle1Pos.x, angle1Pos.y * 0.5f, 0);
			var v = ConvertUnityPosToUsersidePos(targetLocalPos);
			angle1Text.SetText($"θ = {GetAngle(v):F1}°");

			angle1Text.gameObject.SetActive(true);

			if (isAbleElbowAdd)
			{
				_SetActiveElbowAddFunc(true);
			}
		}


		void OnUpdateCurPosToUI()
		{
			var localPos = targetPointTrf.localPosition;
			var usersidePos = ConvertUnityPosToUsersidePos(localPos);
			targetPointText.SetText($"({usersidePos.x:F1}, {usersidePos.y:F1})");
			blueLineRendererFromZero.SetPosition(1, localPos);
		}

		float GetDistance(Vector2 usersidePos)
		{
			return Mathf.Sqrt(Mathf.Pow(usersidePos.x, 2) + Mathf.Pow(usersidePos.y, 2));
		}

		float GetAngle(Vector2 usersidePos)
		{
			var angle = Mathf.Atan2(usersidePos.y, usersidePos.x) * Mathf.Rad2Deg;
			return angle;
		}
	}
}
