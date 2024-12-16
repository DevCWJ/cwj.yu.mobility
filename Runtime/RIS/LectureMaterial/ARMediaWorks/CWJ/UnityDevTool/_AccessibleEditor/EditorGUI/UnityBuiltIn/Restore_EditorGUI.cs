#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.ComponentModel;
using System;
using System.Linq;

namespace CWJ.AccessibleEditor.UnityBuiltIn
{
    public static class Restore_EditorGUI
    {
        private static readonly FieldInfo[] _PrivateStaticFieldInfos = typeof(EditorGUI).GetFields(BindingFlags.NonPublic | BindingFlags.Static);
        private static object GetPrivateStaticFieldValue(string name) => _PrivateStaticFieldInfos.Find(f => f.Name.Equals(name)).GetValue(null);

        public static readonly float kIndentPerLevel = (float)GetPrivateStaticFieldValue(nameof(kIndentPerLevel)); //15

        public static readonly float kSpacing = (float)GetPrivateStaticFieldValue(nameof(kSpacing)); //5

        public static float kSingleLineHeight => EditorGUIUtility.singleLineHeight;

        public static readonly float kPrefixPaddingRight = (float)GetPrivateStaticFieldValue(nameof(kPrefixPaddingRight));

        public static readonly float kSpacingSubLabel = (float)GetPrivateStaticFieldValue(nameof(kSpacingSubLabel));

        public static float kControlVerticalSpacing => EditorGUIUtility.standardVerticalSpacing;

        public static readonly int kControlVerticalSpacingLegacy = (int)GetPrivateStaticFieldValue(nameof(kControlVerticalSpacingLegacy));

        private static float Get_kControlVerticalSpacing()
        {
            var svc = GetPrivateStaticFieldValue(nameof(kVerticalSpacingMultiField));
            return (float)svc.GetType().GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).GetValue(svc);
        }
        public static readonly float kVerticalSpacingMultiField = Get_kControlVerticalSpacing();

        private static float indent => EditorGUI.indentLevel * kIndentPerLevel;

        private static Func<float> GetContextWidth = null;
        public static float contextWidth
        {
            get
            {
                if (GetContextWidth == null)
                {
                    var pi = typeof(EditorGUIUtility).GetProperty(nameof(contextWidth), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetProperty);
                    GetContextWidth = (Func<float>)Delegate.CreateDelegate(typeof(Func<float>), null, pi.GetMethod);
                }

                return GetContextWidth.Invoke();
            }
        }

        private static PropertyInfo _gradientValuePropertyInfo = null;
        private static PropertyInfo GradientValuePropertyInfo
        {
            get
            {
                if (_gradientValuePropertyInfo == null)
                {
                    _gradientValuePropertyInfo = typeof(SerializedProperty).GetProperty("gradientValue",
                                                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty,
                                                null,
                                                typeof(Gradient),
                                                new Type[0],
                                                null
                                                );
                }
                return _gradientValuePropertyInfo;
            }
        }


        public static Gradient GetGradientValue(this SerializedProperty property)
        {
            if (GradientValuePropertyInfo == null) return null;
            Gradient gradientValue = GradientValuePropertyInfo.GetValue(property, null) as Gradient;
            return gradientValue;
        }

        public static void SetGradientValue(this SerializedProperty property, Gradient value)
        {
            if (GradientValuePropertyInfo == null) return;
            GradientValuePropertyInfo.SetValue(property, value);
        }

        internal static bool LabelHasContent(GUIContent label)
        {
            if (label == null)
            {
                return true;
            }
            // @TODO: find out why checking for GUIContent.none doesn't work
            return label.text != string.Empty || label.image != null;
        }

        // Apply the indentLevel to a control rect
        public static Rect IndentedRect(Rect source)
        {
            float x = indent;
            return new Rect(source.x + x, source.y, source.width - x, source.height);
        }

        private static Func<Rect, int, GUIContent, int, Rect> _MultiFieldPrefixLabelFunc = null;
        private static Func<Rect, int, GUIContent, int, Rect> MultiFieldPrefixLabelFunc
        {
            get
            {
                if (_MultiFieldPrefixLabelFunc == null)
                {
                    var methods = typeof(EditorGUI).GetMethods(BindingFlags.Static | BindingFlags.NonPublic);
                    var m = methods.FirstOrDefault(m => m.Name.Equals(nameof(MultiFieldPrefixLabel)));
                    //var mi = typeof(EditorGUI).GetMethod(nameof(MultiFieldPrefixLabel), BindingFlags.Static | BindingFlags.NonPublic);
                    _MultiFieldPrefixLabelFunc = (Func<Rect, int, GUIContent, int, Rect>)Delegate.CreateDelegate(typeof(Func<Rect, int, GUIContent, int, Rect>), null, m);
                }

                return _MultiFieldPrefixLabelFunc;
            }
        }
        public static Rect MultiFieldPrefixLabel(Rect totalPosition, int id, GUIContent label, int columns)
        {
            if (MultiFieldPrefixLabelFunc == null) return IndentedRect(totalPosition);
            return MultiFieldPrefixLabelFunc.Invoke(totalPosition, id, label, columns);
        }

        private static MethodInfo _ScrollableTextAreaInternalMethod_ = null;
        private static MethodInfo ScrollableTextAreaInternalMethod
        {
            get
            {
                if (_ScrollableTextAreaInternalMethod_ == null)
                {
                    var paramTypes = new Type[] { typeof(Rect), typeof(string), typeof(Vector2).MakeByRefType(), typeof(GUIStyle) };
                    _ScrollableTextAreaInternalMethod_ = typeof(EditorGUI).GetMethod(nameof(ScrollableTextAreaInternal), BindingFlags.Static | BindingFlags.NonPublic, null, paramTypes, null);
                }

                return _ScrollableTextAreaInternalMethod_;
            }
        }

        public static string ScrollableTextAreaInternal(Rect position, string text, ref Vector2 scrollPosition, GUIStyle style)
        {
            var parameters = new object[] { position, text, scrollPosition, style };
            object result = ScrollableTextAreaInternalMethod.Invoke(null, parameters);
            scrollPosition = (Vector2)parameters[2];
            return result != null ? (string)result : null;
        }

        public static readonly GUIContent mixedValueContent = EditorGUIUtility.TrTextContent("\u2014", "Mixed Values");
    }
} 
#endif