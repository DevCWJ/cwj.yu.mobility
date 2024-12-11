//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using UnityEngine;
//using UnityEditor;
//using UnityEditorInternal;
//using UnityObject = UnityEngine.Object;

//namespace CWJ.EditorOnly
//{
//    public class ReorderableListProcess
//    {
//        private UnityObject target;
//        private SerializedObject serializedObject;
//        private SerializedProperty[] props;

//        private Dictionary<string, ReorderableList> reorderablesByPropertyName = new Dictionary<string, ReorderableList>();

//        string EditorPrefsKey_isOpen(UnityObject target) => nameof(isReorderableFoldoutOpen) + "." + target.GetInstanceID();

//        bool isReorderableFoldoutOpen;

//        public ReorderableListProcess(UnityObject target, SerializedObject serializedObject)
//        {
//            this.target = target;
//            this.serializedObject = serializedObject;
//            isReorderableFoldoutOpen = EditorPrefs.GetBool(EditorPrefsKey_isOpen(target), false);
//            Type targetType = target.GetType();

//            SerializedProperty property = serializedObject.GetIterator();
//            bool next = property.NextVisible(true);
//            if (next)
//            {
//                do
//                {

//                } while (property.NextVisible(false));
//            }
//        }

//        string GetPropertyKeyName(SerializedProperty property)
//        {
//            return property.serializedObject.targetObject.GetInstanceID() + "/" + property.name;
//        }

//        public void Update()
//        {


//        }

//        public void DrawReorderable(SerializedProperty property)
//        {
//            EditorDrawUtility.DrawHeader(property);

//            if (property.isArray)
//            {
//                var key = GetPropertyKeyName(property);

//                if (!reorderablesByPropertyName.ContainsKey(key))
//                {
//                    ReorderableList reorderableList = new ReorderableList(property.serializedObject, property, true, true, true, true)
//                    {
//                        drawHeaderCallback = (Rect rect) =>
//                        {
//                            EditorGUI.LabelField(rect, string.Format("{0}: {1}", property.displayName, property.arraySize), EditorStyles.label);
//                        },

//                        drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
//                        {
//                            var element = property.GetArrayElementAtIndex(index);
//                            rect.y += 1.0f;
//                            rect.x += 10.0f;
//                            rect.width -= 10.0f;

//                            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, 0.0f), element, true);
//                        },

//                        elementHeightCallback = (int index) =>
//                        {
//                            return EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index)) + 4.0f;
//                        }
//                    };

//                    reorderablesByPropertyName[key] = reorderableList;
//                }

//                reorderablesByPropertyName[key].DoLayoutList();
//            }
//            else
//            {
//                string warning = "ReorderableListAttribute can be used only on arrays or lists";
//                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: AttributeUtility.GetTargetObject(property));
//                EditorGUILayout.PropertyField(property, true);
//            }
//        }
//    }
//}
