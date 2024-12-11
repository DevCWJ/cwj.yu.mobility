using System;

using UnityEngine;

namespace CWJ.Serializable
{
    /// <summary>
    /// key가 string밖에안됨
    /// </summary>
    [Obsolete]
	public class SerializableDictionaryAttribute : PropertyAttribute
	{
		public bool isAllowAdd = true;
		public bool isAllowRemove = true;
		public bool isAllowCollapse = true;
        public bool isReadonly = false;
		public bool isShowEditButton = false;
		public bool isInlineChildren = false;
		public string addLabel = null;
		public string emptyText = null;

        public SerializableDictionaryAttribute(bool allowAdd=true, bool allowRemove = true, bool allowCollapse = true, bool isReadonly= false, bool showEditButton = false, bool inlineChildren = false, string addLabel = null, string emptyText = null)
        {
            isAllowAdd = allowAdd;
            isAllowRemove = allowRemove;
            isAllowCollapse = allowCollapse;
            this.isReadonly = isReadonly;
            if (this.isReadonly)
            {
                isAllowAdd = false; isAllowRemove = false;
            }
            isShowEditButton = showEditButton;
            isInlineChildren = inlineChildren;
            this.addLabel = addLabel;
            this.emptyText = emptyText;
        }
    }
}
