#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace CWJ
{
    /// <summary>
    /// ScriptableObject의 생성자는 시도때도없이 호출되는 문제때문에
    /// <para>실제로 새롭게 생성되었을때만 호출되는 <see cref="OnConstruct"/> 를 가상함수로 둠.</para>
    /// </summary>
    [System.Serializable]
    public class CWJScriptableObject : ScriptableObject
    {
        /// <summary>
        /// 최초 생성시 실행됨
        /// </summary>
        public virtual void OnConstruct() { }

        public void SaveScriptableObj()
        {
            EditorUtility.SetDirty(this);
#if UNITY_2020_1_OR_NEWER
            AssetDatabase.SaveAssetIfDirty(this);
#else
            AssetDatabase.SaveAssets();
#endif
        }
    }
}

#endif