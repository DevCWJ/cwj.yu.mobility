using System;

using UnityEngine;

namespace CWJ
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DropdownAttribute : PropertyAttribute
    {
        public int selectedValue = 0;
        public readonly object[] dropdownList;

        public DropdownAttribute()
        {
            this.dropdownList = null;
        }

        public DropdownAttribute(params object[] list)
        {
            this.dropdownList = list;
        }
    }
}