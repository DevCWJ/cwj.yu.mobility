using System;

using UnityEngine;

namespace CWJ
{
    /// <summary>
    /// OnValueChanged에서 실행시키는 callback에서 Field를 Set해주는 경우 UnityEditor.EditorUtility.SetDirty()꼭 해주기
    /// <para/>해당 Field가 Inspector에 보이지않을경우 실행되지 않음(HideInInspector라던가 SerializedField가 아니라던가)
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class OnValueChangedAttribute : PropertyAttribute
    {
        public readonly string callbackName;

        public OnValueChangedAttribute(string callbackName)
        {
            this.callbackName = callbackName;
        }
    }
}