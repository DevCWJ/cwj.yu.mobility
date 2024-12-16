using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CWJ.Serializable;
using UnityEngine;
using DG.Tweening;

namespace CWJ
{
	public class EnlargeCloneManager : CWJ.Singleton.SingletonBehaviour<EnlargeCloneManager>
	{
		// 복제된 오브젝트를 관리하는 딕셔너리 (키: int, 값: GameObject)

#if UNITY_EDITOR
		[SerializeField] DictionaryVisualized
#else
        System.Collections.Generic.Dictionary
#endif
			<int, List<GameObject>> _cloneCache = new();


		/// <summary>
		/// localspace 기준 위치, 회전, 크기 조정
		/// original 의 자식에 들어간 상태의 위치값을 전해주면됨
		/// </summary>
		/// <param name="original"></param>
		/// <param name="scaleMultiplier"></param>
		/// <param name="targetPos"></param>
		/// <param name="targetRotEuler"></param>
		/// <param name="keyOptionable"></param>
		[InvokeButton]
		public static void CloneAndMultiplierObj(Transform original, float scaleMultiplier, Vector3 targetPos, Vector3 targetRotEuler, int keyOptionable = 0)
		{
			CloneAndSetLocation(original.gameObject, original.transform.localScale * scaleMultiplier
			                  , targetPos, targetRotEuler, keyOptionable, true);
		}


		[InvokeButton]
		public static void CloneAndSetLocation(GameObject original, Vector3 targetScl, Vector3 targetPos, Vector3 targetRotEuler
		                                     , int optionalIndex = 0, bool isLocalSpace = true)
		{
			if (!original)
			{
				Debug.LogError("원본 오브젝트가 지정되지 않았습니다.");
				return;
			}

			GameObject cloneObject = FindOrCreateCloneCache(original, out List<GameObject> cloneObjectList, optionalIndex);

			if (cloneObject.transform.parent != original.transform.parent)
				cloneObject.transform.SetParent(original.transform.parent, true);


			// 복제 오브젝트의 초기 위치, 회전, 스케일 설정
			cloneObject.transform.localPosition = original.transform.localPosition;
			cloneObject.transform.localRotation = original.transform.localRotation;
			cloneObject.transform.localScale = original.transform.localScale;

			if (!cloneObject.activeSelf)
			{
				cloneObject.SetActive(true);
			}

			Vector3 curOriginalScale = original.transform.localScale; //startScale

			// DoTween 시퀀스 생성
			Sequence sequence = DOTween.Sequence();
			sequence.SetTarget(cloneObject);
			sequence.Append(cloneObject.transform.DOScale(curOriginalScale * 0.8f, 0.03f).SetEase(Ease.InQuad));
			sequence.Append(cloneObject.transform.DOScale(curOriginalScale * 1.15f, 0.15f).SetEase(Ease.OutQuad));
			sequence.Append(cloneObject.transform.DOScale(curOriginalScale, 0.07f).SetEase(Ease.InQuad));
			sequence.AppendInterval(0.1f);

			if (isLocalSpace)
			{
				targetPos = original.transform.localPosition + original.transform.localRotation * targetPos;
				targetRotEuler = (original.transform.localRotation * Quaternion.Euler(targetRotEuler)).eulerAngles;
				// Local Space 기준 애니메이션
				sequence.Append(cloneObject.transform.DOLocalMove(targetPos, 0.7f));
				sequence.Join(cloneObject.transform.DOLocalRotate(targetRotEuler, 0.7f, RotateMode.FastBeyond360));
			}
			else
			{
				// World Space 기준 애니메이션
				sequence.Append(cloneObject.transform.DOMove(targetPos, 0.7f));
				sequence.Join(cloneObject.transform.DORotate(targetRotEuler, 0.7f, RotateMode.FastBeyond360));
			}


			Vector3 finalScale = isLocalSpace ? targetScl
				                     : MultipliedBy(targetScl, DivideBy(original.transform.lossyScale, curOriginalScale));

			if (finalScale != curOriginalScale)
			{
				float additive = curOriginalScale.z <= finalScale.z ? 0.1f : -0.1f;
				sequence.Join(cloneObject.transform.DOScale(finalScale + (Vector3.one * additive), 0.6f).SetEase(Ease.OutQuad));
				sequence.Append(cloneObject.transform.DOScale(finalScale, 0.1f).SetEase(Ease.InQuad));
			}

			Debug.LogError(finalScale.ToStringByDetailed());
			// 스케일 애니메이션: 먼저 1.1배로 커졌다가 목표 스케일로 줄어듦

			// 시퀀스 실행
			sequence.Play();
			//sequence.OnKill
		}

		public static bool TryFindCloneCache(GameObject original, out GameObject cloneObj, int optionalIndex = 0)
		{
			return _TryFindCloneCache(original, out _, optionalIndex, out cloneObj);
		}

		private static bool _TryFindCloneCache(GameObject original, out List<GameObject> cloneObjectList, int optionalIndex, out GameObject cloneObj)
		{
			int key = original.GetInstanceID();
			if (!__UnsafeFastIns._cloneCache.TryGetValue(key, out cloneObjectList))
			{
				__UnsafeFastIns._cloneCache.Add(key, cloneObjectList = new List<GameObject>());
			}
			else
			{
				int cnt = cloneObjectList.Count;
				if (cnt > 0)
				{
					for (int i = cnt - 1; i >= 0; --i)
					{
						var c = cloneObjectList[i];
						if (!c)
						{
							cloneObjectList.RemoveAt(i);
							continue;
						}

						if (c.name.EndsWith($"_{optionalIndex})"))
						{
							cloneObj = c;
							return true;
						}
					}
				}
			}

			cloneObj = null;
			return false;
		}

		private static GameObject FindOrCreateCloneCache(GameObject original, out List<GameObject> cloneObjectList, int optionalIndex)
		{
			if (!_TryFindCloneCache(original, out cloneObjectList, optionalIndex, out var cloneObj))
			{
				cloneObj = GameObject.Instantiate(original);
				cloneObj.gameObject.SetActive(false);
				cloneObj.gameObject.name = $"//{original.name} (Clone_{optionalIndex})";
				cloneObjectList.Add(cloneObj);
				var evt = original.GetMonoBehaviourEvent();
				evt.onDisabledEvent.AddListener(OnDisableOriginalObj);
				evt.onDestroyEvent.AddListener(OnDestroyOriginalObj);
			}

			return cloneObj;
		}

		public static void OnDestroyOriginalObj(Transform originalTrf)
		{
			RemoveCloneList(originalTrf.gameObject.GetInstanceID());
		}

		// public static void OnEnableOriginalObj(Transform originalTrf)
		// {
		//
		// }

		public static void OnDisableOriginalObj(Transform originalTrf)
		{
			if (__UnsafeFastIns._cloneCache.TryGetValue(originalTrf.gameObject.GetInstanceID(), out var cloneObjectList))
			{
				cloneObjectList.ForEach(o =>
				{
					if (o)
						o.SetActive(false);
				});
			}
		}

		public static void RemoveCloneList(int key)
		{
			if (__UnsafeFastIns._cloneCache.Remove(key, out var cloneObjectList))
			{
				cloneObjectList.ForEach(Destroy);
			}
		}

		public static void ClearAllCache()
		{
			foreach (var clone in __UnsafeFastIns._cloneCache.Values)
			{
				clone.ForEach(Destroy);
			}

			__UnsafeFastIns._cloneCache.Clear();
		}

		public static Vector3 MultipliedBy(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		public static Vector3 DivideBy(Vector3 a, Vector3 b)
		{
			return new Vector3(
				b.x != 0 ? a.x / b.x : 0,
				b.y != 0 ? a.y / b.y : 0,
				b.z != 0 ? a.z / b.z : 0
			);
		}
	}
}
