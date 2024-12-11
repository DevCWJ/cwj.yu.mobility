using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Linq;

namespace CWJ
{
	public class StringPopupAttribute : PropertyAttribute
	{
		public readonly string[] options;
		public readonly string arrayFieldName;
		public readonly bool hasIndexInName;
		public FieldInfo arrFieldInfo { get; private set; }

		public StringPopupAttribute(string arrayFieldName, bool hasIndexInName = true)
		{
			this.arrayFieldName = arrayFieldName;
			this.hasIndexInName = hasIndexInName;
			options = null;
			arrFieldInfo = null;
			isInit = false;
			order = -1;
		}
		public StringPopupAttribute(string[] constOptions)
		{
			this.options = constOptions;
			arrayFieldName = null;
			arrFieldInfo = null;
		}

		bool isInit = false;
		public bool GetOptionNames(UnityEngine.Object obj, out string[] names)
		{
			names = null;

			if (options != null)
            {
				names = options;
            }
            else
            {
				if (!isInit)
				{
					isInit = true;
					var type = obj?.GetType();
					if (type == null || string.IsNullOrEmpty(arrayFieldName))
					{
						return false;
					}

					arrFieldInfo = type.GetField(arrayFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
				}

				var value = arrFieldInfo?.GetValue(obj);
				if (value == null)
					return false;

				var list = ((IList)value);
				if (list == null)
					return false;

				names = new string[list.Count];
				int i = 0;
				foreach (var item in list)
				{
					names[i++] = item.ToString();
				}
			}

			if (hasIndexInName)
				names = names.Select((s, index) => $"[{index}]: {s}").ToArray();

			return true;
		}


	}
}
