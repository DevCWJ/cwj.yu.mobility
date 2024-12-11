#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;

namespace CWJ.AccessibleEditor
{
    [Serializable]
    public sealed class CWJEditorHelper_ScriptableObject : Initializable_ScriptableObject
    {
        #region ApiCompatibilityLevel

        [Readonly] public ApiCompatibilityLevel lastApiLevel;

        public string ConvertToCWJApiName(ApiCompatibilityLevel api)
        {
            return "CWJ_" + (api.ToString());
        }

        public IEnumerable<string> GetAllCWJApiSymbols()
        {
            return EnumUtil.GetEnumArray<ApiCompatibilityLevel>().Select((s => ConvertToCWJApiName(s)));
        }
        #endregion

        #region Icon

        [Readonly, SerializeField] private Texture iconTexture;
        public Texture IconTexture
        {
            get
            {
                if (iconTexture == null)
                {
                    iconTexture = AssetDatabase.LoadAssetAtPath<Texture>(PathUtil.MyIconPath);
                }
                return iconTexture;
            }
        }
        #endregion
        #region EditorEvent

        [Readonly] public PlayModeStateChange CurPlayModeState;

        #endregion
        public override void OnReset(bool isNeedSave = false)
        {
            base.OnReset(isNeedSave);
            iconTexture = null;
        }
    }
}
#endif