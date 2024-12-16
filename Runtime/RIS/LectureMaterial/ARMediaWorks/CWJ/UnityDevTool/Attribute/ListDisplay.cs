using System;
using UnityEngine;

namespace CWJ
{
	public class ListDisplayAttribute : PropertyAttribute
	{
		public bool AllowAdd = true;
		public bool AllowRemove = true;
		public bool AllowReorder = true;
		public bool AllowCollapse = true;
		public bool ShowEditButton = false;
		public bool InlineChildren = false;
		public Type UseAssetPopup = null;
		public string EmptyText = null;
	}
}
