using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly.Inspector
{
    public partial class CWJ_Inspector_Core : Editor
    {

        protected Texture disabledImg = null;
        public Texture DisabledImg
        {
            get
            {
                if (disabledImg == null)
                {
                    disabledImg = EditorGUIUtility.IconContent("d_VisibilityOff").image;
                }
                return disabledImg;
            }
        }

        protected Texture enabledImg = null;
        public Texture EnabledImg
        {
            get
            {
                if (enabledImg == null)
                {
                    enabledImg = EditorGUIUtility.IconContent("ViewToolOrbit On").image;
                }
                return enabledImg;
            }
        }

        protected Texture enabled_nullImg = null;
        public Texture Enabled_nullImg
        {
            get
            {
                if (enabled_nullImg == null)
                {
                    enabled_nullImg = EditorGUIUtility.IconContent("d_VisibilityOn").image;
                }
                return enabled_nullImg;
            }
        }

    }
}