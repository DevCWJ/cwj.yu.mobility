using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CWJ
{
	[DisallowMultipleComponent]
	public abstract class OffScreenIndicatorAbstract : MonoBehaviour
	{
#if CWJ_EDITOR_DEBUG_ENABLED
		public bool enableDebug;
#endif
		public Camera playerCamera;

		protected List<ArrowIndicatorAbstract> arrowIndicators = new List<ArrowIndicatorAbstract>();

		public IndicatorSetting[] indicatorSettings = new IndicatorSetting[0];

		public abstract void AddTargetIndicator(Transform target, int indicatorIndex);

		public virtual void RemoveTargetIndicator(Transform target)
		{
			int index = arrowIndicators.FindIndex(x => x.target == target);

			if (index < 0)
			{
				Debug.LogWarning("Target not exists: " + target.name, gameObject);
				return;
			}

			ArrowIndicatorAbstract oldArrowTarget = arrowIndicators[index];
			arrowIndicators.RemoveAt(index);
			Destroy(oldArrowTarget.gameObject);
		}

		public virtual void RemoveAllTargetIndicator()
		{
			int cnt = arrowIndicators.Count;

			for (int i = 0; i < cnt; i++)
			{
				var arrIndicator = arrowIndicators[0];
				arrowIndicators.RemoveAt(0);
				Destroy(arrIndicator.gameObject);
			}
		}

		Font defaultFont = null;
		protected Font DefaultFont
		{
			get
			{
				if (defaultFont == null)
				{
					defaultFont = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
				}
				return defaultFont;
			}
		}

		protected abstract void UpdateIndicatorPosition(ArrowIndicatorAbstract arrowIndicator, int id = 0);

		void Awake()
		{
			arrowIndicators = new List<ArrowIndicatorAbstract>();
		}

		protected bool ExistsIndicator(Transform target)
		{
			bool exists = false;
			foreach (ArrowIndicatorAbstract arrowIndicator in arrowIndicators)
			{
				if (arrowIndicator.target == target)
				{
					exists = true;
				}
			}
			return exists;
		}

		public void CheckFields()
		{
			foreach (IndicatorSetting indicator in indicatorSettings)
			{
				if (indicator.onScreenSprite == null)
				{
					indicator.showOnScreen = false;
				}
				else
				{
					indicator.showOnScreen = true;
				}

				if (indicator.offScreenSprite == null)
				{
					indicator.showOffScreen = false;
				}
				else
				{
					indicator.showOffScreen = true;
				}
				if (!indicator.showOnScreen && !indicator.showOffScreen)
				{
					Debug.LogError("You should add at least one Sprite for offScreen or onScreen.", gameObject);
					Debug.Break();
				}
			}
		}
	}
}