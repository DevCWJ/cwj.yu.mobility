using UnityEngine;

namespace CWJ
{
	public class IntPopupAttribute : PropertyAttribute
	{
		public int[] Values { get; private set; }
		public string[] Names { get; private set; }

		public IntPopupAttribute(int[] values, string[] names)
		{
			Values = values;
			Names = names;
		}
	}
}
