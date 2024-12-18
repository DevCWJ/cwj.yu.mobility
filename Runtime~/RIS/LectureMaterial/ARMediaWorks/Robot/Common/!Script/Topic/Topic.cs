using System;
using CWJ.SceneHelper;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CWJ.YU.Mobility
{
	using static Define;

	public class Topic : MonoBehaviour, IHaveSceneObj
	{
		public TopicUIHelper topicUI;
		[SerializeField] Canvas _canvasOf3D;
		public Transform fbxRootLocateTrf;

		public Camera playerCamera => null;
		public Canvas canvasOf3D => _canvasOf3D;
		public Canvas canvasOf2D => null;

		[Tooltip("0부터 시작함"), HelpBox(nameof(topicIndex) + "는 숫자0부터 시작함")]
		public int topicIndex = 0;

		public string topicTitle;

		private void Awake()
		{
			if (ProjectManager.IsExists)
				OnSingletonCreated(ProjectManager.Instance);
			ProjectManager.OnSingletonCreated += OnSingletonCreated;
		}

		void OnSingletonCreated(ProjectManager pm)
		{
			pm.TryAddToDict(this);
		}

		private void OnDestroy()
		{
			ProjectManager.OnSingletonCreated -= OnSingletonCreated;
		}


		private IEnumerator Start()
		{
			yield return null;

			Debug.Assert(!string.IsNullOrEmpty(topicTitle) && topicTitle.Contains(')')
			           , $"ERR : Topic[{topicIndex}] - {nameof(topicTitle)} null!\n{topicTitle}", gameObject);

			Debug.Assert((int.Parse(topicTitle.Trim().Split(')', 2)[0].Replace("#", string.Empty)) - 1) == topicIndex
			           , $"ERR : Topic[{topicIndex}] - {nameof(topicIndex)}가 {nameof(topicTitle)}과 다름. 확인할것." + $"\n{topicIndex} != {topicTitle}");

			if (!ProjectManager.isDuringSetTopic)
			{
				try
				{
					ProjectManager.SetCurTopicIndex(topicIndex);
				}
				catch (System.Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
			}
			else
			{
				gameObject.SetActive(false);
				yield break;
			}
		}

		//private void OnEnable()
		//{
		//    if (!ProjectManager.isDuringSetTopic)
		//        ProjectManager.Instance.SetTopic(topicIndex);
		//}

		[System.Serializable]
		public class Scenario
		{
			[SerializeField] string devMemo;
			public string subTitle;
			public bool isInit;

			[Multiline]
			public string context;

			public LinePoint_EzDrawer lineConfigure;
			public Transform rotateTargetTrf;
			public Transform[] activeSyncTrfs;
			public InputXYContainer[] xyInputArr;
			public DescriptionSettings[] descriptionSettings = new DescriptionSettings[0];
			public DescriptionSettingsByAnimEvent[] descriptionSettingsByAnimEvt = new DescriptionSettingsByAnimEvent[0];

			public MonoBehaviour[] activeSyncMonoBehavs;
			public UnityEvent enableEvent = new UnityEvent();
			public UnityEvent disableEvent = new UnityEvent();


			public void Enable(Topic topic)
			{
				enableEvent?.Invoke();
				activeSyncTrfs ??= Array.Empty<Transform>();
				activeSyncMonoBehavs ??= Array.Empty<MonoBehaviour>();

				if (activeSyncTrfs.Length > 0)
					activeSyncTrfs.ForEach(t =>
					{
						if (t && !t.gameObject.activeSelf) t.gameObject.SetActive(true);
					});
				if (activeSyncMonoBehavs.Length > 0)
					activeSyncMonoBehavs.ForEach(m =>
					{
						if (m && !m.enabled) m.enabled = true;
					});
				topic.topicUI.SetTarget(rotateTargetTrf ? rotateTargetTrf : activeSyncTrfs.FirstOrDefault(t => t.GetComponentInChildren<RotateObjByDrag>()));

				Debug.Assert(subTitle != null, "subTitle is null");
				topic.topicUI.SetSubTitleTxt(subTitle);

				Debug.Assert(context != null, "context is null");
				topic.topicUI.SetContextTxt(context);

				if (lineConfigure)
					lineConfigure.Draw();

				if (xyInputArr.LengthSafe() > 0)
					xyInputArr.ForEach(x => x.Enable());

				var descManager = DescriptonManager.Instance;
				descManager.CloseDescription();

				for (int i = 0; i < descriptionSettings.Length; i++)
				{
					var setting = descriptionSettings[i];
					if (setting == null || !setting.targetTrf) continue;
					if (setting.positionChaser)
					{
						DescriptonManager.DescriptionData descData = new();
						descData._title = setting.title;
						var posChaser = setting.positionChaser;
						descManager.OpenDescriptionWithUpdateText(setting.targetTrf, () => { return OnUpdatePosInDesc(posChaser); }, descData);
					}
					else
					{
						if (!descManager.descDataDicByTag.TryGetValue(setting.targetTrf, out var descData) || descData == null)
						{
							descData = new DescriptonManager.DescriptionData(setting.OnClickDescTarget);
							descManager.descDataDicByTag.TryAdd(setting.targetTrf, descData);
						}
						else
						{
							descData.Subscribe(setting.OnClickDescTarget);
						}
					}
				}
			}

			string OnUpdatePosInDesc(PositionChaser positionChaser)
			{
				var v = positionChaser.GetChaserUsersidePosition();
				return $"{v.x:F1} -> x위치\n" +
				       $"{v.y:F1} -> y위치\n" +
				       $"{v.z:F1} -> z위치\n";
			}

			public void Disable(Topic topic, bool isForAllInit)
			{
				disableEvent?.Invoke();

				if (rotateTargetTrf)
				{
					topic.topicUI.ResetRotation();
					if (!isForAllInit)
						topic.topicUI.SetTarget(null);
				}

				if (activeSyncTrfs.LengthSafe() > 0)
					activeSyncTrfs.ForEach(t =>
					{
						if (t && t.gameObject.activeSelf) t.gameObject.SetActive(false);
					});

				if (activeSyncMonoBehavs.LengthSafe() > 0)
					activeSyncMonoBehavs.ForEach(m =>
					{
						if (m && m.enabled) m.enabled = false;
					});

				if (lineConfigure)
					lineConfigure.DisableDraw();


				if (xyInputArr.LengthSafe() > 0)
					xyInputArr.ForEach(x => x.Disable());
			}

			[System.Serializable]
			public class DescriptionSettingsByAnimEvent : DescriptionSettings
			{
				public AnimatorHandler animatorHandler;
				public string triggerName;
				public bool whenStart = false;

				void SetDescDataAndOpen()
				{
					var descManager = DescriptonManager.Instance;
					if (!descManager.descDataDicByTag.TryGetValue(targetTrf, out var descData) || descData == null)
					{
						descData = new DescriptonManager.DescriptionData(OnClickDescTarget);
						descManager.descDataDicByTag.TryAdd(targetTrf, descData);
					}
					else
					{
						descData.Subscribe(OnClickDescTarget);
					}

					descManager.OpenDescription(targetTrf, out _);
				}

				public void OnAnimEventCallback(bool isStart)
				{
					if (whenStart == isStart)
					{
						SetDescDataAndOpen();
					}
				}
			}

			[System.Serializable]
			public class DescriptionSettings
			{
				public Transform targetTrf;
				public PositionChaser positionChaser;
				public string title;

				[Multiline]
				public string content;

				public Sprite imgSprite;
				protected string targetName = "";

				public (string title, string content, Sprite sprite) OnClickDescTarget(Transform target)
				{
					if (target != targetTrf) return (null, null, null);

					targetName = target.name.Trim();
					if (targetName.Contains("//"))
						targetName = targetName.Split("//")[0].Trim();

					string _t = title.Replace("{0}", targetName);
					string _c = content.Replace("{0}", targetName);

					return (_t, _c, imgSprite);
				}
			}

			[System.Serializable]
			public class InputXYContainer
			{
				public Transform targetTrf;
				public TMP_InputField xInput, yInput;
				LinePoint_EzDrawer configureEditor;

				public void Enable()
				{
					if (xInput)
					{
						if (targetTrf)
						{
							_localXyPos = targetTrf.localPosition;

							if (!int.TryParse(Mathf.FloorToInt(_localXyPos.x * 0.1f).ToString(), out int x))
								x = 0;
							xInput.SetTextWithoutNotify(x.ToString());
							if (!int.TryParse(Mathf.FloorToInt(_localXyPos.y * 0.1f).ToString(), out int y))
								y = 0;
							yInput.SetTextWithoutNotify(y.ToString());
						}

						xInput.transform.parent.gameObject.SetActive(true);
						xInput.onEndEdit.RemoveAllListeners();
						yInput.onEndEdit.RemoveAllListeners();
						xInput.onEndEdit.AddListener(OnXValueChanged);
						yInput.onEndEdit.AddListener(OnYValueChanged);
						//xInput.onValidateInput += OnXValueChanged;
						//yInput.onValidateInput += OnYValueChanged;
					}
				}

				public void Disable()
				{
					if (xInput)
					{
						xInput.onEndEdit.RemoveListener(OnXValueChanged);
						yInput.onEndEdit.RemoveListener(OnYValueChanged);
						xInput.transform.parent.gameObject.SetActive(false);
						//xInput.onValidateInput -= OnXValueChanged;
						//yInput.onValidateInput -= OnYValueChanged;
					}
				}

				public void Init(LinePoint_EzDrawer configureEditor)
				{
					this.configureEditor = configureEditor;
				}

				void ChangePos(Vector3 localPos)
				{
					if (_localXyPos != localPos)
					{
						_localXyPos = localPos;
						configureEditor.ChangePointWithAngleLine(targetTrf, _localXyPos);
					}
					//xInput.SetTextWithoutNotify(targetTrf.localPosition.x * 0.1f + "");
					//yInput.SetTextWithoutNotify(targetTrf.localPosition.y * 0.1f + "");
				}

				Vector3 _localXyPos;

				void OnXValueChanged(string input)
				{
					if (int.TryParse(input, out int num))
						ChangePos(new Vector3(num * 10, _localXyPos.y, 0));
				}

				void OnYValueChanged(string input)
				{
					if (int.TryParse(input, out int num))
						ChangePos(new Vector3(_localXyPos.x, num * 10, 0));
				}

				//char OnXValueChanged(string input, int charIndex, char addedChar)
				//{
				//    if (UGUIUtil.TmpIpf_ValidateAndTryConvert(xInput, int.TryParse, ref input, ref charIndex, ref addedChar, out int resultNum))
				//    {
				//        ChangePos(new Vector3(resultNum * 10, _localXyPos.y, 0));
				//    }

				//    return addedChar;
				//}

				//char OnYValueChanged(string input, int charIndex, char addedChar)
				//{
				//    if (UGUIUtil.TmpIpf_ValidateAndTryConvert(yInput, int.TryParse, ref input, ref charIndex, ref addedChar, out int resultNum))
				//    {
				//        ChangePos(new Vector3(_localXyPos.x, resultNum * 10, 0));
				//    }
				//    return addedChar;
				//}
			}

			public void Init(Topic topic)
			{
				if (isInit)
				{
					Disable(topic, false);
					return;
				}

				isInit = true;

				for (int i = 0; i < xyInputArr.Length; i++)
				{
					xyInputArr[i].Init(lineConfigure);
				}

				if (activeSyncTrfs.Length > 0 && !rotateTargetTrf)
				{
					for (int i = 0; i < activeSyncTrfs.Length; i++)
					{
						var t = activeSyncTrfs[i];
						if (t && t.TryGetComponent<FbxInteractor>(out var fbxInteractor) && fbxInteractor.rotateObjByDrag)
						{
							rotateTargetTrf = fbxInteractor.rotateObjByDrag.transform;
							break;
						}
					}
				}

				if (descriptionSettingsByAnimEvt.Length > 0)
				{
					foreach (var setting in descriptionSettingsByAnimEvt)
					{
						if (setting != null && setting.animatorHandler)
						{
							setting.animatorHandler.AddAnimEvent_TriggerName(setting.triggerName, setting.OnAnimEventCallback);
							CheckAndAddCollider(setting.targetTrf);
						}
					}
				}

				if (descriptionSettings.Length > 0)
				{
					foreach (var setting in descriptionSettings)
					{
						if (setting != null)
						{
							CheckAndAddCollider(setting.targetTrf);
						}
					}
				}

				void CheckAndAddCollider(Transform target)
				{
					//Debug.LogError("??", target);
					if (target && !target.GetComponent<Collider>())
					{
						var col = target.gameObject.AddComponent<SphereCollider>();
						col.isTrigger = true;
						col.radius = 0.015f;
						col.center = Vector3.zero;
					}
				}

				Disable(topic, true);
			}
		}

		[SerializeField, Readonly] int curScenarioIndex = -1;
		public Scenario[] scenarios;

		public void Init()
		{
			curScenarioIndex = -1;
			for (int i = scenarios.Length - 1; i >= 0; --i)
			{
				scenarios[i].Init(this);
			}

			if (gameObject.activeSelf)
				gameObject.SetActive(false);
		}

		void _ChangeScenario(int toScenarioIndex)
		{
			if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
				Debug.Assert(topicTitle != null, $"[{toScenarioIndex}]TopicTitle is null");
				topicUI.SetTitleTxt(topicTitle);
			}

			int scenarioLength = scenarios.Length;

			if (0 <= curScenarioIndex && curScenarioIndex < scenarioLength)
			{
				scenarios[curScenarioIndex]?.Disable(this, false);
			}

			if (0 <= toScenarioIndex && toScenarioIndex < scenarioLength)
			{
				curScenarioIndex = toScenarioIndex;
				scenarios[curScenarioIndex]?.Enable(this);
			}
			else
			{
				if (toScenarioIndex < 0)
				{
					if (topicIndex - 1 >= 0)
						ProjectManager.SetCurTopicIndex(topicIndex - 1);
				}
				else if (toScenarioIndex >= scenarioLength)
				{
					ProjectManager.SetCurTopicIndex(topicIndex + 1);
				}
			}
		}

		[InvokeButton]
		public void Previous()
		{
			_ChangeScenario(curScenarioIndex - 1);
		}

		[InvokeButton]
		public void Next()
		{
			_ChangeScenario(curScenarioIndex + 1);
		}

		public void InstantActiveFi()
		{
		}
	}
}
