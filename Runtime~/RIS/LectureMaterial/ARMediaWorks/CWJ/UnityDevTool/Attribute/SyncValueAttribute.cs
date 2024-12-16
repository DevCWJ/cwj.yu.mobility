using System;
using System.Reflection;
using UnityEngine;

namespace CWJ
{
    /// <summary>
    /// 값이 바뀔때 Undo나 바뀌는값의 저장이 필요할경우 UnityEditor.EditorUtility.SetDirty()꼭 해주기
    /// <para/>해당 Field가 Inspector에 보이지않을경우 실행되지 않음(HideInInspector라던가 SerializedField가 아니라던가)
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited =true)]
    public abstract class _Root_SyncValueAttribute : PropertyAttribute
    {
        public readonly string variableName;

        protected readonly BindingFlags defaultBindingFlags;

        public _Root_SyncValueAttribute(string variableName)
        {
            order = -444;
            this.variableName = variableName;
            defaultBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        }

        protected abstract bool GetMemberInfoAndReturnIsValid(Type targetType);
        protected abstract void SetValue(object targetObj, object value);

#if UNITY_EDITOR
        public bool IsValid(object targetObj)
        {
            return (!string.IsNullOrEmpty(variableName)) && GetMemberInfoAndReturnIsValid(targetObj?.GetType());
        }

        public void WhenChanged(object targetObj, FieldInfo fieldInfo)
        {
            CWJ.AccessibleEditor.EditorCallback.AddWaitForFrameCallback(() =>
            {
                try
                {
                    SetValue(targetObj, fieldInfo.GetValue(targetObj));
                }
                catch (ArgumentException e)
                {
                    Debug.LogError(e.ToString());
                }
            });
        }
#endif

    }
    /// <summary>
    /// 값이 바뀔때 Undo나 바뀌는값의 저장이 필요할경우 UnityEditor.EditorUtility.SetDirty()꼭 해주기
    /// <para/>해당 Field가 Inspector에 보이지않을경우 실행되지 않음(HideInInspector라던가 SerializedField가 아니라던가)
    /// </summary>
    public class SyncValue_FieldAttribute : _Root_SyncValueAttribute
    {
        public SyncValue_FieldAttribute(string fieldName) : base(fieldName) { }

        private System.Reflection.FieldInfo fi = null;

        protected override bool GetMemberInfoAndReturnIsValid(Type targetType)
        {
            fi = targetType?.GetField(variableName, defaultBindingFlags);
            return fi != null;
        }

        protected override void SetValue(object targetObj, object value)
        {
            fi.SetValue(targetObj, value);
        }
    }
    /// <summary>
    /// 값이 바뀔때 Undo나 바뀌는값의 저장이 필요할경우 UnityEditor.EditorUtility.SetDirty()꼭 해주기
    /// <para/>해당 Field가 Inspector에 보이지않을경우 실행되지 않음(HideInInspector라던가 SerializedField가 아니라던가)
    /// </summary>
    public class SyncValue_PropertyAttribute : _Root_SyncValueAttribute
    {
        public SyncValue_PropertyAttribute(string propertyName) : base(propertyName) { }

        private System.Reflection.PropertyInfo pi = null;

        protected override bool GetMemberInfoAndReturnIsValid(Type targetType)
        {
            pi = targetType?.GetProperty(variableName, defaultBindingFlags);
            return pi != null;
        }

        protected override void SetValue(object targetObj, object value)
        {
            pi.SetValue(targetObj, value);
        }
    }
}