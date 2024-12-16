using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CWJ
{
	/// <summary>
	/// import받은 모델링의 root에 있을 컴포넌트
	/// </summary>
	public class FbxInteractor : MonoBehaviour
	{
		public const string NameTag = "_Interactor";
		[SerializeField, Readonly] bool isInit;
		public bool IsInit => isInit;
		public AnimatorHandler animHandler;
		[SerializeField] Transform modelTrf;

		[SerializeField] RectTransform btnParentTrf;
		[SerializeField] Button animPlayBtn_prefab;
		[SerializeField] List<Button> animPlayBtnList = new();
		[SerializeField] Button animResetBtn;

		public RotateObjByDrag rotateObjByDrag;

		[SerializeField] bool isResetWhenDisable = true;


		private void Awake()
		{
			animHandler.isSetIdleOnDisable = isResetWhenDisable;
			InitAnimBtnClickEvent();
		}

		void InitAnimBtnClickEvent()
		{
			animPlayBtn_prefab.gameObject.SetActive(false);

			if (animHandler)
			{
				if (animResetBtn)
					animResetBtn.onClick.AddListener(OnClickResetBtn);
				foreach (Button triggerBtn in animPlayBtnList)
				{
					string triggerName = triggerBtn.gameObject.name;
					triggerBtn.onClick.AddListener(() => OnClickPlayBtn(triggerName));
				}
			}
		}

		void OnClickResetBtn()
		{
			if (!animHandler) return;
			rotateObjByDrag.ResetRotation();
			animHandler.SetOff();
		}

		void OnClickPlayBtn(string triggerName)
		{
			if (!animHandler) return;
			animHandler.SetTrigger(triggerName);
		}


		private void OnEnable()
		{
			rotateObjByDrag.ResetRotation();
		}

#if UNITY_EDITOR
		[InvokeButton]
		void Editor_ClearFbxRoot()
		{
			if (!IsInit || !modelTrf)
			{
				Debug.LogError("설정한적 없으므로 직접 지우면됨");
				return;
			}

			if (PrefabUtility.IsAnyPrefabInstanceRoot(gameObject))
			{
				modelTrf.GetComponent<FbxAutoSetupHelper>().prefabFilePathCache = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
				// Undo 기능 등록 (되돌리기 가능하게)
				Undo.RegisterCompleteObjectUndo(gameObject, "Unpack Prefab Completely");

				PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.UserAction);
				Debug.Log($"프리팹이 완전히 언팩되었습니다: {gameObject.name}");
			}

			if (modelTrf)
			{
				modelTrf.SetParent(null, true);
			}

			UnityEditor.EditorApplication.delayCall += () => DestroyImmediate(gameObject);
		}


		public void SetupInteractor(Transform fbxModelingTrf, string[] clipNames)
		{
			if (isInit || modelTrf)
			{
				Debug.LogError("이미 작업했엇음. 다시하려면 InvokeButton > " + nameof(Editor_ClearFbxRoot) + "실행하기", this);
				return;
			}

			((RectTransform)transform).SetAnchorWithoutMoving(RectAnchor.MIDDLE_CENTER);

			modelTrf = fbxModelingTrf;
			gameObject.name = (fbxModelingTrf.gameObject.name + NameTag);

			fbxModelingTrf.SetParent(null, true);
			fbxModelingTrf.localScale = Vector3.one; // 캔버스의 자식이 될 것이므로 스케일 유지
			fbxModelingTrf.SetParent(rotateObjByDrag.AxesPivot, true);

			// 모델의 위치 및 회전 설정
			fbxModelingTrf.localPosition = Vector3.zero;
			fbxModelingTrf.localRotation = Quaternion.identity;

			rotateObjByDrag.AxesPivot.localRotation = Quaternion.identity;
			rotateObjByDrag.AxesPivot.localPosition = Vector3.zero;

			// 모든 MeshRenderer의 Bounds를 합산하여 모델의 크기를 계산 (월드 좌표계)
			Bounds combinedBounds = new Bounds(fbxModelingTrf.position, Vector3.zero);
			foreach (var renderer in fbxModelingTrf.GetComponentsInChildren<MeshRenderer>())
			{
				combinedBounds.Encapsulate(renderer.bounds);
			}

			// World Space Canvas에서의 스케일 조정 (캔버스 스케일이 100배 크다고 가정)
			const float ScaleOffset = 100f;

			Vector3 fbxMaxSize = combinedBounds.size * ScaleOffset;
			float maxDist = Mathf.Max(fbxMaxSize.x, fbxMaxSize.y, fbxMaxSize.z);

			var boxCollider = rotateObjByDrag.AxesPivot.GetComponent<BoxCollider>();
			boxCollider.size = fbxMaxSize;
			Vector3 center = (combinedBounds.center - fbxModelingTrf.position) * ScaleOffset;
			boxCollider.center = center;
			boxCollider.isTrigger = true;

			var maxRectSize = _FbxInteractorSpawner.Instance.fbxMaxRectSize;
			((RectTransform)transform).sizeDelta = new Vector2(Mathf.Min(maxRectSize.x, maxDist), Mathf.Min(maxRectSize.y, maxDist));

			// 사용자를 바라보게 하기 위해 180도 회전
			rotateObjByDrag.transform.localEulerAngles = new Vector3(0, 180f, 0);
			//((RectTransform)rotateObjByDrag.AxesPivot).anchoredPosition3D = -center;
			//AxesPivot을 FbxInteractor의 중심점으로 이동시킨후 복귀 (AxesPivot는 rotateObjByDrag의 직계child)

			if (animPlayBtnList == null)
				animPlayBtnList = new List<Button>();
			else
				animPlayBtnList.Clear();

			var triggerNameList = new List<string>();

			if (modelTrf.TryGetComponent<Animator>(out var myAnimator))
			{
				animHandler = myAnimator.gameObject.GetOrAddComponent<AnimatorHandler>();
				AnimatorController animatorController = myAnimator.runtimeAnimatorController as AnimatorController;
				if (animatorController != null)
				{
					animPlayBtn_prefab.gameObject.SetActive(true);
					foreach (var parameter in animatorController.parameters)
					{
						if (parameter.type == AnimatorControllerParameterType.Trigger && parameter.name != "off")
						{
							AddAnimPlayBtn(parameter.name);
						}
					}

					void AddAnimPlayBtn(string triggerName)
					{
						var newPlayBtn = Instantiate(animPlayBtn_prefab, btnParentTrf);
						newPlayBtn.gameObject.name = triggerName;
						newPlayBtn.GetComponentInChildren<TextMeshProUGUI>().SetText(triggerName);
						animPlayBtnList.Add(newPlayBtn);
						triggerNameList.Add(triggerName);
					}

					animResetBtn.transform.SetAsLastSibling();
					animPlayBtn_prefab.gameObject.SetActive(false);
					UnityEditor.EditorApplication.delayCall += () => LayoutRebuilder.ForceRebuildLayoutImmediate(btnParentTrf);
				}
				else
				{
					Debug.LogError("AnimatorController를 가져올 수 없습니다.", this);
				}
			}

			isInit = true;
		}


#endif
	}
}
