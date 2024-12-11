using System;
using System.IO;
using System.Reflection;

using UnityEditor;

using UnityEngine;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    public static class ForcedFieldSerialization
    {
        private static SerializationCache_ScriptableObject _ScriptableObj = null;

        public static SerializationCache_ScriptableObject ScriptableObj
        {
            get
            {
                if (_ScriptableObj == null)
                {
                    _ScriptableObj = ScriptableObjectStore.Instanced.GetScriptableObj<SerializationCache_ScriptableObject>();
                }
                return _ScriptableObj;
            }
        }

         [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            CWJ_EditorEventHelper.ReloadedScriptEvent += WriteSerializationCode;

            CWJ_EditorEventHelper.PlayModeStateChangedEvent += (state) => WriteSerializationCode(); //실행모드 바뀔때

            BuildEventSystem.BeforeBuildEvent += WriteSerializationCode;

            CWJ_EditorEventHelper.EditorWillSaveEvent += (_, __) => WriteSerializationCode();
        }

        /// <summary>
        /// serialize되지 않은 field의 값이 수정될때 마다 ScriptableObject에 보관시킴
        /// <para/>field의 수정된값이 날아가는 타이밍때마다 code에 수정된값을 적용시킬 함수
        /// </summary>
        /// <param name="scriptType"></param>
        /// <param name="fieldInfo"></param>
        /// <param name="value"></param>
        /// <param name="isIncludeBaseClass"></param>
        public static void AddSerializationCache(FieldInfo fieldInfo, object value, bool isIncludeBaseClass)
        {
            if (value == null) return;

            if (ScriptableObj == null) return;

            var cacheObj = ScriptableObj;
            var codeContainer = new CodeContainer(fieldInfo, value, isIncludeBaseClass);

            if (string.IsNullOrEmpty(codeContainer.name))
            {
                return;
            }

            cacheObj.codeContainerCache[codeContainer.name] = codeContainer;

            EditorUtility.SetDirty(cacheObj);
        }

        //적당한때에 serialize되지 않은 field들의 initializer code부분이 수정됨
        static void WriteSerializationCode()
        {
            if (ScriptableObj == null) return;
            var cacheObj = ScriptableObj;
            int length = cacheObj?.codeContainerCache?.Count ?? 0;

            if (length == 0)
            {
                return;
            }

            cacheObj.WriteCodeContainer();
        }





        
       
    }
}