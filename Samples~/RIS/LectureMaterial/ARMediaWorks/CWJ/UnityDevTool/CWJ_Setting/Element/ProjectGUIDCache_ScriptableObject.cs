#if UNITY_EDITOR

using UnityEngine;

namespace CWJ.AccessibleEditor
{
    public sealed class ProjectGUIDCache_ScriptableObject : Initializable_ScriptableObject
    {
        public override bool IsAutoReset => false;
        
        [Readonly] public string projectGUID = "";
        [Readonly] public string packageName = "";

        public bool IsUnityProjectChanged()
        {
            string curProjectGUID = UnityEditor.PlayerSettings.productGUID.ToString();
            string curPackageName = AccessibleEditorUtil.GetDetailPackageName();

            if (!curPackageName.Equals(packageName))
            {
                packageName = curPackageName;
                projectGUID = curProjectGUID;
                isInitialized = true;

                SaveScriptableObj();
                return true;
            }

            isInitialized = true;
            return false;
        }
    }
}

#endif