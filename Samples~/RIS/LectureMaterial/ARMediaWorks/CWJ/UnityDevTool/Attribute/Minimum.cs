using UnityEngine;

namespace CWJ
{
	public class MinimumAttribute : PropertyAttribute
	{
		public float MinimumValue { get; private set; }

		public MinimumAttribute(float minimum) => MinimumValue = minimum;
		public MinimumAttribute(int minimum) => MinimumValue = minimum;
	}
}
