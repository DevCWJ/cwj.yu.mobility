using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CWJ.AccessibleEditor
{
    public class DebugLogUtilEditor : MonoBehaviour
    {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            CWJ_EditorEventHelper.CompSelectedEvent += (arg1, arg2) => DebugLogUtil.LogPool.Clear();
            CWJ_EditorEventHelper.EditorWillSaveEvent += (arg1, arg2) => DebugLogUtil.LogPool.Clear();
            CWJ_EditorEventHelper.EditorSceneOpenedEvent += (arg1) => DebugLogUtil.LogPool.Clear();
        }
#endif
    }

}