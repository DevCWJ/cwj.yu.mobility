using System;

using UnityEngine;

namespace CWJ
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class MinMaxRangeAttribute : PropertyAttribute
	{
		public readonly float Min;
		public readonly float Max;
		public MinMaxRangeAttribute(float min, float max)
		{
			Min = min;
			Max = max;
		}
	}

	[Serializable]
	public struct RangedFloat
	{
		public float Min;
		public float Max;

		public RangedFloat(float min, float max)
		{
			Min = min;
			Max = max;
		}
	}

	[Serializable]
	public struct RangedInt
	{
		public int min;
		public int max;

		public RangedInt(int min, int max)
		{
			this.min = min;
			this.max = max;
		}
	}

	public static class RangedExtensions
	{
		public static float GetRandomValue(this RangedFloat ranged)
        {
			return UnityEngine.Random.Range(ranged.Min, ranged.Max);
        }

		public static float LerpFromRange(this RangedFloat ranged, float t)
		{
			return Mathf.Lerp(ranged.Min, ranged.Max, t);
		}

		public static float LerpFromRangeUnclamped(this RangedFloat ranged, float t)
		{
			return Mathf.LerpUnclamped(ranged.Min, ranged.Max, t);
		}

		public static float LerpFromRange(this RangedInt ranged, float t)
		{
			return Mathf.Lerp(ranged.min, ranged.max, t);
		}

		public static float LerpFromRangeUnclamped(this RangedInt ranged, float t)
		{
			return Mathf.LerpUnclamped(ranged.min, ranged.max, t);
		}
	}
}
