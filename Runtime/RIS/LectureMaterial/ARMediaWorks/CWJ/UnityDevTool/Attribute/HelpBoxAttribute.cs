using System;

using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace CWJ
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public class HelpBoxAttribute : PropertyAttribute
    {
        public readonly string text;

        public readonly MessageType messageType;

        public HelpBoxAttribute(string text, MessageType messageType = MessageType.Info)
        {
            this.text = text;
            this.messageType = messageType;
        }
    }

#if !UNITY_EDITOR
    public enum MessageType
    {
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }
#endif

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CWJInfoBoxAttribute : Attribute
    {
        public readonly string info = null;

        public CWJInfoBoxAttribute(string infoText = null)
        {
            this.info = infoText;
        }
    }
}