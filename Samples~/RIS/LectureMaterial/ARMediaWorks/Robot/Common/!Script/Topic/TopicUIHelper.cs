using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CWJ.YU.Mobility
{
	public class TopicUIHelper : MonoBehaviour
	{
		[DrawHeaderAndLine("Topic UI")]
		[SerializeField] TextMeshProUGUI titleTxt;

		[SerializeField] TextMeshProUGUI titleNumberTxt;
		[SerializeField] TextMeshProUGUI subTitleTxt;
		[SerializeField] TextMeshProUGUI contextTxt;
		[SerializeField] Button prevBtn, nextBtn;

		string lastTitle, lastSubTitle, lastContext;

		public void SetTitleTxt(string title)
		{
			if (lastTitle == title) return;
			lastTitle = title;
			if (title.Contains(")"))
			{
				var splits = title.Split(')', 2);
				titleNumberTxt.SetText(splits[0]);
				title = splits[1];
			}

			titleTxt.SetText(title);
		}

		public void SetSubTitleTxt(string subTitle)
		{
			if (lastSubTitle == subTitle) return;
			subTitleTxt.SetText(lastSubTitle = subTitle);
		}

		public void SetContextTxt(string context)
		{
			if (lastContext == context) return;
			contextTxt.SetText(lastContext = context);
		}


		[DrawHeaderAndLine("Rotate UI")]
		[Tooltip("자동 삽입됨"), ReadonlyConditional(EPlayMode.NotPlayMode)]
		public RotateObjByDrag rotateAxes;

		[SerializeField] Button leftRotBtn, rightRotBtn;

		[SerializeField] Button initRotBtn;
		[SerializeField] Toggle holdRotToggle;
		[SerializeField] Image lockedObj, unlockedObj;
		[SerializeField] TextMeshProUGUI logTxt, logTxtMini;


		private bool hasRotationUI;

		private void Awake()
		{
			hasRotationUI = leftRotBtn && rightRotBtn;
			if (hasRotationUI)
			{
				leftRotBtn.AddLongPressLoopEvent(OnClickLeftLongPressLoop, 0.25f);
				leftRotBtn.AddShortPressUpEvent(OnClickLeftShortPressUp, 0.25f);
				rightRotBtn.AddLongPressLoopEvent(OnClickRightLongPressLoop, 0.25f);
				rightRotBtn.AddShortPressUpEvent(OnClickRightShortPressUp, 0.25f);
				initRotBtn.onClick.AddListener_New(ResetRotation);
				holdRotToggle.onValueChanged.AddListener_New(OnToggleChanged);
			}

			SetTarget(null);

			prevBtn.onClick.AddListener(ProjectManager.OnClickPrev);
			nextBtn.onClick.AddListener(ProjectManager.OnClickNext);
		}

		public void SetTarget(Transform targetTrf, bool isHoldDefault = true)
		{
			if (targetTrf)
				rotateAxes = targetTrf.GetComponentInChildren<RotateObjByDrag>(true);
			bool hasTargetTrf = rotateAxes && rotateAxes.AxesPivot;
			if (!hasTargetTrf)
				rotateAxes = null;

			if (hasRotationUI)
			{
				initRotBtn.gameObject.SetActive(hasTargetTrf);
				leftRotBtn.gameObject.SetActive(hasTargetTrf);
				rightRotBtn.gameObject.SetActive(hasTargetTrf);
				holdRotToggle.gameObject.SetActive(hasTargetTrf);
				if (hasTargetTrf)
				{
					holdRotToggle.SetIsOnWithoutNotify(!isHoldDefault);
					holdRotToggle.isOn = isHoldDefault;
				}

				logTxt.transform.parent.gameObject.SetActive(hasTargetTrf);
			}
		}

		bool isHoldRot;

		void OnToggleChanged(bool isOn)
		{
			holdRotToggle.targetGraphic = isOn ? lockedObj : unlockedObj;
			lockedObj.gameObject.SetActive(isOn);
			unlockedObj.gameObject.SetActive(!isOn);
			isHoldRot = isOn;

			if (!rotateAxes) return;
			rotateAxes.enabled = !isHoldRot;
			SendLogTxt("마우스를 통한 회전이 " + (isOn ? "잠겼습니다" : "풀렸습니다"));
		}

		StringBuilder _sb;
		Queue<string> logTxtQue = new Queue<string>();

		public void SendLogTxt(string log)
		{
			if (!hasRotationUI) return;
			if (log == null) log = string.Empty;
			logTxtQue.Enqueue(log);
			if (logTxtQue.Count > 3)
				logTxtQue.Dequeue();
			var arr = logTxtQue.Reverse().ToArray();

			if (_sb == null) _sb = new StringBuilder();
			_sb.Append("<b><size=15>");
			_sb.Append(arr[0]);
			_sb.AppendLine("</b></size>");
			if (arr.Length > 1)
			{
				_sb.Append("<size=12>");
				_sb.Append(arr[1]);
				_sb.AppendLine("</size>");
			}

			logTxt.SetText(_sb.ToString());
			_sb.Clear();

			if (arr.Length > 2)
				logTxtMini.SetText(arr[2]);
		}

		public void ResetRotation()
		{
			if (!rotateAxes) return;
			rotateAxes.ResetRotation();
			SendLogTxt("회전이 초기화됐습니다");
		}


		void OnClickLeftLongPressLoop()
		{
			if (!rotateAxes) return;
			rotateAxes.AxesPivot.Rotate(Vector3.up, Time.deltaTime * 20);
		}

		const float lrAngle = 15;

		void OnClickLeftShortPressUp()
		{
			if (!rotateAxes) return;

			rotateAxes.AxesPivot.Rotate(Vector3.up, lrAngle);
			SendLogTxt($"왼쪽으로 {lrAngle}' 회전했습니다");
		}

		void OnClickRightLongPressLoop()
		{
			if (!rotateAxes) return;
			rotateAxes.AxesPivot.Rotate(Vector3.down, Time.deltaTime * 20);
		}

		void OnClickRightShortPressUp()
		{
			if (!rotateAxes) return;
			rotateAxes.AxesPivot.Rotate(Vector3.down, lrAngle);
			SendLogTxt($"오른쪽으로 {lrAngle}' 회전했습니다");
		}
	}
}
