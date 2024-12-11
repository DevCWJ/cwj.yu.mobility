using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace CWJ
{
	/// <summary>
	/// <para/>아래와 같은 컴포넌트를 클릭할때
	/// <br/>클릭 사운드, 애니메이션을 실행해줌
	/// <br/>[UIFeedbackHelper 타겟 조건]
	/// <br/>1. Selectable를 상속받는 UI컴포넌트
	/// <br/>2. IPointerClickHandler 를 상속받는 컴포넌트
	/// <br/>3. 3D오브젝트는 아직 지원안함 (무분별하게 검출할까봐)
	/// <para/>[24.12.06]
	/// </summary>
	public class UIFeedbackHelper : CWJ.Singleton.SingletonBehaviour<UIFeedbackHelper>
	{
		[DrawHeader("클릭 사운드 사용여부")]
		public bool useClickSound = true;

		[GetComponent, SerializeField] private AudioSource audioSource;

		[SerializeField] private AudioClip pointerDownSound, pointerUpSound, notInteractableSound, inputFieldEndEditSound;
		public bool CanClickSound => useClickSound && audioSource;


		[DrawHeader("클릭 피드백 애니메이션 사용여부")]
		public bool useTweenAnim = false;

		public bool CanTweenAnim => useTweenAnim;


		protected override void _Reset()
		{
			audioSource = gameObject.GetOrAddComponent<AudioSource>();
			audioSource.playOnAwake = false;
		}

		protected override void _Start()
		{
			if (!HasInstance)
				UpdateInstance();
		}

		private void Update()
		{
			// 마우스 왼쪽버튼 다운 혹은 모바일 터치 시작 시 판단
			if (Input.GetMouseButtonDown(0))
			{
				var curEventSys = EventSystem.current;
				if (curEventSys)
					CheckObjAndInitEventEntry(curEventSys);
			}
		}

		void OnPointerDownFeedback(ClickableUICache cache, bool isInteractable)
		{
			_PlaySoundFx(isInteractable ? pointerDownSound : notInteractableSound);
			_PlayDoTween(cache, isInteractable, 0);
		}

		private void OnPointerUpOrClickFeedback(ClickableUICache clickableUICache, bool isClickSuccess)
		{
			if (clickableUICache.targetObj)
			{
				if (!clickableUICache.hasSelectableUI || clickableUICache.selectableUI.enabled)
				{
					bool isInteractable = clickableUICache.GetIsInteractableWhenPointerUp(out bool isInteractableWhenPointerDown);
					if (isInteractableWhenPointerDown)
					{
						_PlayDoTween(clickableUICache, isInteractable, isClickSuccess ? 2 : 1);
						if (isInteractable && !isClickSuccess)
						{
							_PlaySoundFx(pointerUpSound);
						}
					}

					return;
				}

				ThreadDispatcher.LateUpdateQueue((s) =>
				{
					if (!s) //버튼이 눌리며 Destroy가 실행중인 경우 (DestroyImmediate가 아닐것으로 추측)
					{
						Debug.Log("삭제됏구나?(딜레이체크)");
						_PlaySoundFx(pointerUpSound);
					}
				}, clickableUICache.targetObj);
			}
			else
			{ //버튼누른 신호는 들어왔지만 DestroyImmediate된경우
				Debug.Log("삭제됏구나?(즉시실행)");
				_PlaySoundFx(pointerUpSound);
			}
		}

		private void OnEndEdit(string text)
		{
			_PlaySoundFx(inputFieldEndEditSound);
		}

		private static HashSet<int> _RegisteredSet = new HashSet<int>();

		private void CheckObjAndInitEventEntry(EventSystem curEventSystem)
		{
			GameObject hoveredObj = curEventSystem.currentSelectedGameObject;

			if (!hoveredObj)
			{
				PointerEventData pointerData = new(curEventSystem) { position = Input.mousePosition };
				List<RaycastResult> totalResults = new List<RaycastResult>();
				EventSystem.current.RaycastAll(pointerData, totalResults);
				if (totalResults.Count == 0)
				{
					return;
				}

				hoveredObj = totalResults[0].gameObject;
			}

			GameObject curSelectObj = null;
			var selectable = hoveredObj.GetComponentInParent<Selectable>();
			if (!selectable)
			{
				if (!hoveredObj.TryGetComponentInParent<IPointerClickHandler>(out _, out curSelectObj))
				{
					return;
				}
			}
			else
			{
				curSelectObj = selectable.gameObject;
			}

			int objId = curSelectObj.GetInstanceID();

			// 이미 등록된 오브젝트는 할필요없음
			if (!_RegisteredSet.Add(objId))
			{
				return;
			}

			if (curSelectObj.TryGetComponent(out TMP_InputField ipf))
			{
				ipf.onEndEdit ??= new TMP_InputField.SubmitEvent();
				ipf.onEndEdit.AddListener(OnEndEdit);
			}

			ClickableUICache clickableUICache = new ClickableUICache(curSelectObj, selectable, curSelectObj.transform.localScale);
			OnPointerDownFeedback(clickableUICache, clickableUICache.GetIsInteractableWhenPointerDown()); //PointerDown은 이미 이벤트가 불린이후라 즉시 실행해줌
		}


		private void _PlaySoundFx(AudioClip soundClip)
		{
#if UNITY_EDITOR
			if (useClickSound && !audioSource)
			{
				Debug.LogError("useClickSound 가 활성화 되어있지만 audioSource가 없음");
			}
#endif
			if (!CanClickSound || !soundClip) return;
			audioSource.PlayOneShot(soundClip);
		}


		/// <summary>
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="isInteractable"></param>
		/// <param name="isPointerUpOrDown">0 : pointerDown,<br/>1: pointerUp(Cancel Click),<br/>2: pointerClick Done(Success Click)</param>
		private void _PlayDoTween(ClickableUICache uiCache, bool isInteractable, int isPointerUpOrDown)
		{
			if (!CanTweenAnim) return;

			uiCache.PlayDoTween(isInteractable, isPointerUpOrDown);
		}

		public class ClickableUICache
		{
			public readonly GameObject targetObj;
			public readonly Selectable selectableUI;
			public readonly bool hasSelectableUI;
			public Vector3 localScale;
			private Sequence sequence;

			public ClickableUICache(GameObject targetObj, Selectable selectableUI, Vector3 localScale)
			{
				this.targetObj = targetObj;
				this.selectableUI = selectableUI;
				hasSelectableUI = selectableUI;
				this.localScale = localScale;
				this.sequence = null;

				if (!targetObj.TryGetComponent(out EventTrigger evtTriggerComp))
					evtTriggerComp = targetObj.AddComponent<EventTrigger>();

				var entryByEventTypes = new (bool hasEntry, EventTriggerType evtType, EventTrigger.Entry entry)[]
				                        {
					                        (false, EventTriggerType.PointerDown, null),
					                        (false, EventTriggerType.PointerUp, null),
					                        (false, EventTriggerType.PointerClick, null)
				                        };
				int entryCount = entryByEventTypes.Length;


				foreach (var entry in evtTriggerComp.triggers)
				{
					for (int i = 0; i < entryCount; i++)
					{
						(bool hasEntry, var eventID, _) = entryByEventTypes[i];
						if (!hasEntry && entry.eventID == eventID)
						{
							entry.callback ??= new EventTrigger.TriggerEvent();
							entryByEventTypes[i] = (true, eventID, entry);
							if (entryByEventTypes.All(e => e.hasEntry))
							{
								break;
							}
						}
					}
				}

				for (int i = 0; i < entryCount; i++)
				{
					(bool hasEntry, var eventID, var entry) = entryByEventTypes[i];
					if (!hasEntry)
					{
						entry = new EventTrigger.Entry
						        {
							        eventID = eventID,
							        callback = new EventTrigger.TriggerEvent()
						        };
						evtTriggerComp.triggers.Add(entry);
					}

					if (eventID == EventTriggerType.PointerDown)
						entry.callback.AddListener(OnPointerDown);
					else if (eventID == EventTriggerType.PointerUp)
						entry.callback.AddListener(OnPointerUp);
					else if (eventID == EventTriggerType.PointerClick)
						entry.callback.AddListener(OnPointerClick);
				}
			}

			void OnPointerDown(BaseEventData eventData)
			{
				if (targetObj)
				{
					if (!hasSelectableUI || selectableUI.enabled)
						__UnsafeFastIns.OnPointerDownFeedback(this, GetIsInteractableWhenPointerDown());
				}
			}

			void OnPointerUp(BaseEventData eventData)
			{
				__UnsafeFastIns.OnPointerUpOrClickFeedback(this, false);
			}

			void OnPointerClick(BaseEventData eventData)
			{
				__UnsafeFastIns.OnPointerUpOrClickFeedback(this, true);
			}


			private bool _isInteractableWhenPointerDown;

			public bool IsInteractable => !hasSelectableUI || selectableUI.interactable;

			/// <summary>
			/// Selectable컴포넌트가 아니거나 Selectable컴포넌트라면 interactable true인경우
			/// </summary>
			public bool GetIsInteractableWhenPointerDown()
			{
				_isInteractableWhenPointerDown = IsInteractable;
				return _isInteractableWhenPointerDown;
			}

			public bool GetIsInteractableWhenPointerUp(out bool isInteractableWhenPointerDown)
			{
				isInteractableWhenPointerDown = _isInteractableWhenPointerDown;
				return IsInteractable;
			}


			public void PlayDoTween(bool isInteractable, int isPointerUpOrDown)
			{
				if (!targetObj) return;

				Vector3 originalLocalScale = localScale;
				Transform targetTrf = targetObj.transform;

				sequence?.Kill();
				sequence = DOTween.Sequence(targetObj);

				if (!isInteractable)
				{
					sequence.Append(
						targetTrf.DOShakeScale(0.2f, strength: 0.1f, vibrato: 10, randomness: 90, fadeOut: true)
						         .SetEase(Ease.OutCubic));
				}
				else
				{
					if (isPointerUpOrDown == 0) // PointerDown
					{
						sequence.Append(
							targetTrf.DOScale(originalLocalScale * 0.95f, 0.1f)
							         .SetEase(Ease.OutQuad));
					}
					else if (isPointerUpOrDown == 1) // PointerUp
					{
						sequence.Append(
							targetTrf.DOScale(originalLocalScale, 0.1f)
							         .SetEase(Ease.OutQuad));
					}
					else if (isPointerUpOrDown == 2) // PointerClick
					{
						sequence.Append(
							        targetTrf.DOScale(originalLocalScale * 1.05f, 0.08f)
							                 .SetEase(Ease.OutQuad))
						        .Append(
							        targetTrf.DOScale(originalLocalScale, 0.12f)
							                 .SetEase(Ease.OutBack));
					}
				}

				sequence.Play();
			}
		}

	}
}

