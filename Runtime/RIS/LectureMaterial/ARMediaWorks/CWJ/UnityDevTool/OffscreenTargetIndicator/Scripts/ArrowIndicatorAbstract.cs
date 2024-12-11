using UnityEngine;
using System.Collections;

namespace CWJ
{
	public abstract class ArrowIndicatorAbstract : MonoBehaviour
	{
		private Transform _transform;
		public new Transform transform
        {
            get
            {
                if (_transform == null)
                {
					_transform = GetComponent<Transform>();
                }
				return _transform;
            }
        }

		public IndicatorSetting indicator;
		public int indicatorIndex;
		public Transform target;
		public bool isVisibleDistance;
		protected bool _onScreen;
		protected bool _onScreenNextValue;
		protected Color transColor;
		protected bool fadingToOn = false;
		protected bool fadingToOff = false;
		protected bool fadingUp = false;
		protected float timeStartLerp;
		protected float elapsedTime;
		protected float lerpAmount;
		public abstract bool onScreen { get; set; }
		public abstract void UpdateEffects();
    }
}