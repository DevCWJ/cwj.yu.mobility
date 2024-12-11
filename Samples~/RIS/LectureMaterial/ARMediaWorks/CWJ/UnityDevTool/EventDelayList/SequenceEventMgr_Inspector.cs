#if UNITY_EDITOR

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

namespace CWJ
{
    [CustomEditor(typeof(SequenceEventMgr))]
    public class SequenceEventMgr_Inspector : UnityEditor.Editor
    {
        private ReorderableList list;
        private SequenceEventMgr scriptedSequence;

        private void OnEnable()
        {
            scriptedSequence = target as SequenceEventMgr;
            list = new ReorderableList(serializedObject, serializedObject.FindProperty("eventList"), true, true, true, true);
        }

        private void DrawList()
        {
            list.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Event List");
            };

            list.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = list.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 2;

                    EditorGUI.PropertyField(
                         new Rect(rect.x, rect.y, rect.width - 30, EditorGUI.GetPropertyHeight(element, true)),
                         element.FindPropertyRelative("unityEvent"), GUIContent.none);

                    EditorGUI.PropertyField(
                         new Rect(rect.x + rect.width - 25, rect.y, 25, EditorGUIUtility.singleLineHeight),
                         element.FindPropertyRelative("delay"), GUIContent.none);
                };

            list.elementHeightCallback = (int index) =>
            {
                var elementHeight = scriptedSequence.eventList[index].unityEvent.GetPersistentEventCount();
                if (elementHeight >= 1)
                {
                    elementHeight--;
                }
                return (EditorGUIUtility.singleLineHeight * 5) + elementHeight * (EditorGUIUtility.singleLineHeight * 2.7f);
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawList();
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif