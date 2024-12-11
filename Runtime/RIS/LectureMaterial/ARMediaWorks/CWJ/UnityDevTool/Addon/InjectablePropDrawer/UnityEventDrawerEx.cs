#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;
using UnityEngine.Events;

namespace CWJ.AccessibleEditor
{
    [InjectablePropertyDrawer(typeof(UnityEventBase), true)]
    public class UnityEventDrawerCustom : UnityEventDrawer, CWJ.AccessibleEditor.ICustomPropertyDrawer
    {
        const BindingFlags kBfAll = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        static readonly FieldInfo s_FiReorderableList = typeof(UnityEventDrawer).GetField("m_ReorderableList", kBfAll);
        
        static readonly FieldInfo s_FiRuntimeCalls = UnityEventUtil.T_InvokableCallList.GetField("m_RuntimeCalls", kBfAll);
        static GUIStyle s_CachedStyleToggle;
        static GUIStyle s_CachedStyleBg;

        static bool s_FoldStatus
        {
            get => UnityEventDrawerExSettings.instance.foldStatus;
            set => UnityEventDrawerExSettings.instance.foldStatus = value;
        }

        public float height { get; set; } = 0;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }

        /// <summary>
        /// Gets the height of the property.
        /// </summary>
        /// <returns>The property height.</returns>
        /// <param name="property">Property.</param>
        /// <param name="label">Label.</param>
        float _GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Get the ReorderableList for default drawer.
            ReorderableList ro = s_FiReorderableList.GetValue(this) as ReorderableList;
            if (ro == null)
            {
                base.GetPropertyHeight(property, label);
                ro = s_FiReorderableList.GetValue(this) as ReorderableList;
            }

            // If persistent calls is empty, display it compactry.
            bool isEmpty = property.FindPropertyRelative("m_PersistentCalls").FindPropertyRelative("m_Calls").arraySize == 0;
            ro.elementHeight = isEmpty
                ? 16
                : 16 * 2 + 11;

            // If drawer is folded, skip drawing runtime calls.
            return s_FoldStatus
                ? base.GetPropertyHeight(property, label) + GetRuntimeCalls(property).Count * 17
                : base.GetPropertyHeight(property, label);
        }

        public (bool isExpanded, float height) Draw(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            // Draw background and toggle.
            var RuntimeCalls = GetRuntimeCalls(property);
            float height = s_FoldStatus
                ? RuntimeCalls.Count * 17 + 16
                : 16;
#if UNITY_2019_3_OR_NEWER
            height += 4;
#endif
            var r = new Rect(position.x + 2, position.y + position.height - height, position.width - 4, height);
            DrawRuntimeCallToggle(r, RuntimeCalls.Count);

            // Draw UnityEvent using default drawer.
            base.OnGUI(position, property, label);

            if (s_FoldStatus)
            {
                // Draw runtime calls.
                r = new Rect(r.x + 16, r.y + 15, r.width - 16, 16);
                EditorStyles.objectField.fontSize = 9;
                foreach (var invokableCall in RuntimeCalls)
                {
                    var fi = invokableCall.GetMemberInfo("Delegate", MemberTypes.Field) as FieldInfo;
                    Delegate del = fi.GetValue(invokableCall) as Delegate;

                    // Draw delegate.
                    DrawDelegate(r, del);
                    r.y += r.height + 1;
                }
                EditorStyles.objectField.fontSize = 11;
            }

            return (s_FoldStatus, _GetPropertyHeight(property, label));
        }

        /// <summary>
        /// Raises the GU event.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property.</param>
        /// <param name="label">Label.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var info = CWJ.AccessibleEditor.EditorGUI_CWJ.PropertyField_CWJ(fieldInfo, position, property, label, drawFunc: Draw);
            height = info.height;
        }


        /// <summary>
        /// Draws the runtime call toggle.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="count">Runtime call count.</param>
        static void DrawRuntimeCallToggle(Rect position, int count)
        {
            // Cache style.
            if (s_CachedStyleBg == null)
            {
                s_CachedStyleBg = new GUIStyle("ProgressBarBack");
                s_CachedStyleToggle = new GUIStyle("OL Toggle") { fontSize = 9 };
                s_CachedStyleToggle.onNormal.textColor =
                    s_CachedStyleToggle.normal.textColor =
                        s_CachedStyleToggle.onActive.textColor =
                            s_CachedStyleToggle.active.textColor = EditorStyles.label.normal.textColor;
            }

            // Draw background.
            GUI.Label(position, "", s_CachedStyleBg);

            // Draw foldout with label.
            string text = string.Format("Show runtime calls ({0})", count);
            bool prevGuiEnabled = GUI.enabled;
            if (!prevGuiEnabled) GUI.enabled = true;
            s_FoldStatus = GUI.Toggle(new Rect(position.x, position.y, position.width - 80, 14), s_FoldStatus, text, s_CachedStyleToggle);
            if (!prevGuiEnabled) GUI.enabled = false;
        }

        /// <summary>
        /// Draws the delegate.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="del">Delegate.</param>
        static void DrawDelegate(Rect position, Delegate del)
        {
            try
            {
                Rect r = new Rect(position.x, position.y, position.width * 0.3f, position.height);
                MethodInfo method = del.Method;
                object target = del.Target;

                // Draw the target if possible.
                var obj = target as UnityEngine.Object;
                if (obj)
                {
                    EditorGUI.ObjectField(r, obj, obj.GetType(), true);
                }
                else if (target != null)
                {
                    EditorGUI.LabelField(r, string.Format("{0} ({1})", target.ToString(), target.GetType()), EditorStyles.miniLabel);
                }
                else
                {
                    EditorGUI.LabelField(r, "null", EditorStyles.miniLabel);
                }

                // Draw the method name.
                r.x += r.width;
                r.width = position.width - r.width;
                EditorGUI.LabelField(r, method.ReflectedType + "." + method.Name, EditorStyles.miniLabel);
            }
            catch
            {
                EditorGUI.LabelField(position, "null delegate", EditorStyles.miniLabel);
            }
        }

        /// <summary>
        /// Gets the runtime call list from SerializedProperty.
        /// </summary>
        /// <returns>The runtime call list.</returns>
        /// <param name="property">SerializedProperty.</param>
        public static IList GetRuntimeCalls(SerializedProperty property)
        {
            var propertyInstance = property.GetInstance();

            return propertyInstance != null
                ? s_FiRuntimeCalls.GetValue(UnityEventUtil.F_Calls.GetValue(propertyInstance)) as IList
                : new List<object>() as IList;
        }


    }

    [Serializable]
    public class UnityEventDrawerExSettings : ScriptableSingleton<UnityEventDrawerExSettings>
    {
        public bool foldStatus = true;
    }
}

#endif