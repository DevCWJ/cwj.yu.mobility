using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CWJ.YU.Mobility
{
	[ExecuteAlways, RequireComponent(typeof(TMP_Text))]
	public class AnimationSyncText : MonoBehaviour
	{
		[GetComponent] public TMP_Text _tmpText;

		[Tooltip("-1 : 공백")]
		public int syncIndex = -1;

		[Multiline]
		public string[] syncTextList = new string[1] { string.Empty };

		private int listCnt;

		private void Awake()
		{
			_tmpText ??= GetComponent<TMP_Text>();
		}

		private void OnDisable()
		{
			lastSyncIndex = -2;
		}

		[NonSerialized] private int lastSyncIndex = -2;

		private void Update()
		{
			if (syncTextList.Length > syncIndex && lastSyncIndex != syncIndex)
			{
				lastSyncIndex = syncIndex;
				if (lastSyncIndex < 0)
					_tmpText.enabled = false;
				else
				{
					_tmpText.SetText(syncTextList[lastSyncIndex]);
					if (!_tmpText.enabled)
						_tmpText.enabled = true;
				}
			}
		}
	}
}
