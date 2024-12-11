#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;

namespace CWJ.AccessibleEditor
{
    //상속받는 곳에서 필요한 att [CanEditMultipleObjects, CustomEditor(typeof(클래스))]
    //클래스 : InspectorBehaviour<클래스> 필수 (클래스이름은 ~_Inspector)
    public class InspectorBehaviour<T> : Editor where T : Object
    {
        protected T Target;

        protected T[] Targets;

        private static readonly string[] PropertyToExclude = new string[] { "m_Script" };

        private static List<Color> BackupColors = new List<Color>();

        protected static void BeginError(bool error)
        {
            BeginError(error, new Color().GetDarkRed());
        }

        protected static void BeginError(bool error, Color color)
        {
            BackupColors.Add(GUI.color);

            GUI.color = (error ? color : BackupColors[0]);
        }

        protected static void EndError()
        {
            if (BackupColors.Count == 0) return;
            int index = BackupColors.Count - 1;

            GUI.color = BackupColors[index];

            BackupColors.RemoveAt(index);
        }

        protected static Rect Reserve()
        {
            Rect rect = EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(GUIContent.none);
            EditorGUILayout.EndVertical();

            return rect;
        }

        private void OnEnable()
        {
            Target = target as T;
            Targets = targets.Select(t => t as T).ToArray();
            _OnEnable();
        }

        protected virtual void _OnEnable()
        {
        }

        public override sealed void OnInspectorGUI()
        {
            if (target == null || serializedObject == null) return;

            BackupColors.Clear();

            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.Separator();

                serializedObject.Update();

                EditorGUILayout.HelpBox("Help: Contact to CWJ ^-^", MessageType.Info);

                DrawInspector();

                serializedObject.ApplyModifiedProperties();

                EditorGUILayout.Separator();
            }
            if (EditorGUI.EndChangeCheck())
            {
                GUI.changed = true;
                Repaint();
                Dirty();
            }

            if (GUI.changed)
            {
                OnGuiChanged();
            }
        }

        protected virtual void OnGuiChanged()
        {
        }

        protected virtual void OnSceneGUI()
        {
            if (target == null) return;

            Target = target as T;

            DrawScene();
        }

        protected virtual void DrawInspector()
        {
            if (target == null || serializedObject == null) return;
            DrawPropertiesExcluding(serializedObject, PropertyToExclude);
        }

        protected void OnDestroy()
        {
            _OnDestroy();

            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (target == null && CWJ_EditorEventHelper.PlayModeState != PlayModeStateChange.ExitingPlayMode)
                {
                    OnDestroyInEditMode();
                }
            }
        }

        protected virtual void _OnDestroy()
        {
        }

        protected virtual void OnDestroyInEditMode()
        {
        }

        protected virtual void DrawScene()
        {
        }

        protected void Each(System.Action<T> update, bool dirty = true)
        {
            if (target == null || serializedObject == null) return;
            if (dirty)
            {
                Undo.RecordObjects(Targets, "Inspector");
            }

            foreach (T t in Targets)
            {
                update(t);
            }

            if (dirty)
            {
                Dirty();
            }
        }

        protected bool Any(System.Func<T, bool> check)
        {
            if (target == null || serializedObject == null) return false;
            foreach (T t in Targets)
            {
                if (check(t))
                {
                    return true;
                }
            }

            return false;
        }

        protected bool All(System.Func<T, bool> check)
        {
            if (target == null || serializedObject == null) return false;
            foreach (T t in Targets)
            {
                if (!check(t))
                {
                    return false;
                }
            }

            return true;
        }

        protected void SwitchingToggle(ref bool a_prevValue, ref bool a_curValue,
                                       ref bool b_PrevValue, ref bool b_CurValue)
        {
            if (a_prevValue != a_curValue)
            {
                if (a_curValue)
                {
                    b_PrevValue = b_CurValue = false;
                }
                a_prevValue = a_curValue;
            }

            if (b_PrevValue != b_CurValue)
            {
                if (b_CurValue)
                {
                    a_prevValue = a_curValue = false;
                }
                b_PrevValue = b_CurValue;
            }
        }

        protected void WriteSubTitle(string content, int fontSize = 0)
        {
            GUIStyle headerStyle = new GUIStyle();
            headerStyle.alignment = TextAnchor.MiddleCenter;
            if (fontSize > 0)
            {
                headerStyle.fontSize = fontSize;
            }
            else
            {
                headerStyle.fontSize = 17;
            }
            headerStyle.fontStyle = FontStyle.Bold;
            GUILayout.Label(content, headerStyle);
        }

        protected bool DrawExpand(ref bool expand, string propertyPath, string overrideTooltip = null, string overrideText = null)
        {
            SerializedProperty property = serializedObject?.FindProperty(propertyPath);
            if (property == null) return false;
            Rect rect = Reserve();
            GUIContent guiContent = new GUIContent();
            guiContent.text = !string.IsNullOrEmpty(overrideText) ? overrideText : property.displayName;
            guiContent.tooltip = !string.IsNullOrEmpty(overrideTooltip) ? overrideTooltip : property.tooltip;

            EditorGUI.BeginChangeCheck();

            EditorGUI_CWJ.PropertyField_CWJ(property.GetFieldInfo(), rect, property, guiContent);

            bool isChanged = EditorGUI.EndChangeCheck();

            expand = EditorGUI.Foldout(new Rect(rect.position, new Vector2(25.0f, rect.height)), expand, string.Empty);

            return isChanged;
        }

        protected bool DrawMinMax(string propertyPath, float min, float max, string overrideTooltip = null, string overrideText = null)
        {
            SerializedProperty property = serializedObject?.FindProperty(propertyPath);
            if (property == null) return false;
            Vector2 value = property.vector2Value;
            GUIContent guiContent = new GUIContent();
            guiContent.text = !string.IsNullOrEmpty(overrideText) ? overrideText : property.displayName;
            guiContent.tooltip = !string.IsNullOrEmpty(overrideTooltip) ? overrideTooltip : property.tooltip;

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.MinMaxSlider(guiContent, ref value.x, ref value.y, min, max);

            if (EditorGUI.EndChangeCheck())
            {
                property.vector2Value = value;

                return true;
            }

            return false;
        }

        protected bool DrawEulerAngles(string propertyPath, string overrideTooltip = null, string overrideText = null)
        {
            SerializedProperty property = serializedObject?.FindProperty(propertyPath);
            if (property == null) return false;
            bool mixed = EditorGUI.showMixedValue;
            GUIContent guiContent = new GUIContent();
            guiContent.text = !string.IsNullOrEmpty(overrideText) ? overrideText : property.displayName;
            guiContent.tooltip = !string.IsNullOrEmpty(overrideTooltip) ? overrideTooltip : property.tooltip;

            EditorGUI.BeginChangeCheck();

            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;

            Vector3 oldEulerAngles = property.quaternionValue.eulerAngles;
            Vector3 newEulerAngles = EditorGUILayout.Vector3Field(guiContent, oldEulerAngles);

            if (oldEulerAngles != newEulerAngles)
            {
                property.quaternionValue = Quaternion.Euler(newEulerAngles);
            }

            EditorGUI.showMixedValue = mixed;

            return EditorGUI.EndChangeCheck();
        }

        protected bool Draw(string propertyPath, string overrideTooltip = null, string overrideText = null)
        {
            SerializedProperty property = serializedObject?.FindProperty(propertyPath);
            if (property == null) return false;
            GUIContent guiContent = new GUIContent();
            guiContent.text = !string.IsNullOrEmpty(overrideText) ? overrideText : property.displayName;
            guiContent.tooltip = !string.IsNullOrEmpty(overrideTooltip) ? overrideTooltip : property.tooltip;

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(property, guiContent, true);

            return EditorGUI.EndChangeCheck();
        }

        protected void Dirty()
        {
            if (target == null || serializedObject == null) return;
            int targetLength = targets.Length;
            for (int i = targetLength - 1; i >= 0; i--)
            {
                EditorUtility.SetDirty(targets[i]);
            }

            serializedObject?.Update();
        }
    }
}

#endif