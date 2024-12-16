#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEditor;

using UnityEngine;

namespace CWJ.AccessibleEditor
{
    public static partial class EditorGUI_CWJ
    {
        #region DrawLine
        private static GUIStyle _LineGUILabelStyle = null;
        public static GUIStyle LineGUILabelStyle
        {
            get
            {
                if (_LineGUILabelStyle == null)
                {
                    _LineGUILabelStyle = new GUIStyle(EditorGUICustomStyle.LargeLabelStyle);
                    _LineGUILabelStyle.alignment = TextAnchor.MiddleCenter;
                }
                return _LineGUILabelStyle;
            }
        }

        private static Rect _GetDefaultLineRect(float thickness, (float top, float bottom)? margin, Rect refRect, bool isSameToRefRectY, Vector2 textSize)
        {
            float marginTop = margin?.top ?? 0;
            float marginBottom = margin?.bottom ?? 0;

            Rect lineRect;
            if (refRect == default(Rect))
            {
                lineRect = EditorGUILayout.GetControlRect(GUILayout.Height(marginTop + marginBottom + Math.Max(textSize.y, thickness)));
                lineRect.x -= 2;
                lineRect.width += 6;
            }
            else
            {
                if (isSameToRefRectY)
                {
                    lineRect = refRect;
                }
                else
                {
                    lineRect = EditorGUILayout.GetControlRect(GUILayout.Height(marginTop + marginBottom + Math.Max(textSize.y, thickness)));
                    lineRect.x = refRect.x;
                    lineRect.width = refRect.width;
                }
            }

            lineRect.height = thickness;
            lineRect.y += marginTop;
            return lineRect;
        }

        public static void DrawLine(float thickness = 1, (float top, float bottom)? margin = null, Rect refRect = default(Rect), bool isSameToRefRectY = true)
        {
            DrawLineWithColor(EditorGUICustomStyle.DefaultLineColor, thickness, margin, refRect, isSameToRefRectY);
        }

        public static void DrawLineWithColor(Color lineColor, float thickness = 1, (float top, float bottom)? margin = null, Rect refRect = default(Rect), bool isSameToRefRectY = true)
        {
            if (!GUI.enabled)
            {
                lineColor.a *= .5f;
            }
            Rect rect = _GetDefaultLineRect(thickness, margin, refRect, isSameToRefRectY, Vector2.zero);
            rect.y = rect.yMin;
            EditorGUI.DrawRect(rect, lineColor);
        }

        public static void DrawLineAndHeader(GUIContent headerContent, float thickness = 1, (float top, float bottom)? margin = null, Rect refRect = default(Rect), bool isSameToRefRectY = true)
        {
            DrawLineAndHeaderWithColor(headerContent, EditorGUICustomStyle.DefaultTextColor, EditorGUICustomStyle.DefaultLineColor, thickness, margin, refRect);
        }
        public static void DrawLineAndHeaderWithColor(GUIContent headerContent, Color textColor, Color lineColor, float thickness = 1, (float top, float bottom)? margin = null, Rect refRect = default(Rect), bool isSameToRefRectY = true)
        {
            Vector2 textSize = headerContent != null ? LineGUILabelStyle.CalcSize(headerContent) : Vector2.zero;
            Rect lineRect = _GetDefaultLineRect(thickness, margin, refRect, isSameToRefRectY, textSize);

            float separatorWidth = (lineRect.width - textSize.x) * .5f - 5.0f;

            float textPosY = lineRect.y - 1; //딱 1만큼 중심에서 차이남 왜지.. 폰트때문일거같은데
            lineRect.y += (textSize.y * .5f) - (lineRect.height * .5f);

            if (!GUI.enabled)
            {
                lineColor.a *= .5f;
            }
            EditorGUI.DrawRect(new Rect(lineRect.xMin, lineRect.yMin, separatorWidth, lineRect.height), lineColor);
            LineGUILabelStyle.normal.textColor = textColor;
            EditorGUI.LabelField(new Rect(lineRect.xMin + separatorWidth + 5.0f, textPosY, textSize.x, textSize.y), headerContent, LineGUILabelStyle);
            EditorGUI.DrawRect(new Rect(lineRect.xMin + separatorWidth + 10.0f + textSize.x, lineRect.yMin, separatorWidth, lineRect.height), lineColor);
        }
        #endregion

        public static void DrawHeader(string header)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
        }

        private static List<Color> BackupColors = new List<Color>();

        public static void BeginErrorGUI(Color color)
        {
            BackupColors.Add(GUI.color);

            GUI.color = color;
        }

        public static void EndErrorGUI()
        {
            if (BackupColors.Count == 0) return;
            int index = BackupColors.Count - 1;

            GUI.color = BackupColors[index];

            BackupColors.RemoveAt(index);
        }

        public static bool DrawHeader(SerializedProperty property)
        {
            HeaderAttribute headerAttr = property.GetFieldInfo().GetCustomAttribute<HeaderAttribute>();
            if (headerAttr != null)
            {
                DrawHeader(headerAttr.header);
                return true;
            }

            return false;
        }

        public static void DrawBigFoldout(ref bool isExpand, GUIContent content, Action<bool> foldoutAction, GUIStyle boxStyle = null)
        {
            bool prevGUIEnabled = GUI.enabled;
            if (!prevGUIEnabled) GUI.enabled = true;

            var r = EditorGUILayout.BeginVertical(boxStyle ?? EditorGUICustomStyle.InspectorBox);

#if UNITY_2019_3_OR_NEWER
            isExpand = EditorGUILayout.BeginFoldoutHeaderGroup(isExpand, content, EditorGUICustomStyle.FoldoutHeader_Big);
#else
            if (isExpand = EditorGUILayout.Foldout(isExpand, content, true, EditorGUICustomStyle.FoldoutHeader_Big))
            {
#endif
            DrawLine(refRect: r, isSameToRefRectY: false);

            if (!prevGUIEnabled) GUI.enabled = false;

            foldoutAction?.Invoke(isExpand);
#if UNITY_2019_3_OR_NEWER
            EditorGUILayout.EndFoldoutHeaderGroup();
#else
            }
#endif

            EditorGUILayout.EndVertical();
        }

        public static void DrawHelpBox(this Type attributeType, string message, MessageType type, UnityEngine.Object context = null, bool logToConsole = true)
        {
            EditorGUILayout.HelpBox(message, type);

            if (logToConsole)
            {
                if (type == MessageType.Warning)
                {
                    attributeType.PrintLogWithClassName(message, LogType.Warning, obj: context, isPreventOverlapMsg: true, isPreventStackTrace: true);
                }
                else if (type == MessageType.Error)
                {
                    attributeType.PrintLogWithClassName(message, LogType.Error, obj: context, isPreventOverlapMsg: true, isPreventStackTrace: true);
                }
                else
                {
                    attributeType.PrintLogWithClassName(message, LogType.Log, obj: context, isPreventOverlapMsg: true, isPreventStackTrace: true);
                }
            }
        }

    }
}
#endif