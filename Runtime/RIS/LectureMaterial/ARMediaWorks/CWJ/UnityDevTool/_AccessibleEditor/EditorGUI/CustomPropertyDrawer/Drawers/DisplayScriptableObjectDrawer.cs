#if UNITY_EDITOR
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    public class DisplayScriptableObjectDrawer : PropertyDrawer_CWJ
    {
        Type GetFieldType(FieldInfo fi)
        {
            if (fi == null) return null;
            Type type = fi.FieldType;
            if (type.IsArray) type = type.GetElementType();
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) type = type.GetGenericArguments()[0];
            return type;
        }

        static bool AreAnySubPropertiesVisible(SerializedProperty property)
        {
            var data = (ScriptableObject)property.objectReferenceValue;
            using (var serializedObject = new SerializedObject(data))
            {
                using (var prop = serializedObject.GetIterator())
                {
                    while (prop.NextVisible(true))
                    {
                        if (!ReflectionUtil.IsPropName_m_Script(prop))
                            return true;
                    }
                }

            }
            return false;
        }

        public override float GetHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = EditorGUIUtility.singleLineHeight;
            if (property.isExpanded && property.objectReferenceValue != null && AreAnySubPropertiesVisible(property))
            {
                var data = property.objectReferenceValue as ScriptableObject;
                if (data == null) return EditorGUIUtility.singleLineHeight;
                using (var serializedObject = new SerializedObject(data))
                {
                    using (SerializedProperty prop = serializedObject.GetIterator())
                    {
                        if (prop.NextVisible(true))
                        {
                            do
                            {
                                if (ReflectionUtil.IsPropName_m_Script(prop))
                                {
                                    continue;
                                }
                                var subProp = serializedObject.FindProperty(prop.name);
                                float height = EditorGUI.GetPropertyHeight(subProp, null, true) + EditorGUIUtility.standardVerticalSpacing;
                                totalHeight += height;
                            }
                            while (prop.NextVisible(false));
                        }
                    }
                }

                totalHeight += EditorGUIUtility.standardVerticalSpacing;
            }

            return totalHeight;
        }

        const string CreateNewBtn = "New";

        const int buttonWidth = 40;

        const string SaveDefaultPath = "Assets/ScriptableObj";

        static string savePath = SaveDefaultPath;
        const string ignoreTypeName = "TMPro.TMP_FontAsset";
        public override bool DrawGUI(FieldInfo fInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            EditorGUI.BeginProperty(position, label, property);
            var type = GetFieldType(fInfo);

            if (type == null || ignoreTypeName.Equals(type.FullName))
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return property.isExpanded = false;
            }

            ScriptableObject propertySO = null;
            if (!property.hasMultipleDifferentValues && property.serializedObject.targetObject != null && property.serializedObject.targetObject is ScriptableObject)
            {
                propertySO = (ScriptableObject)property.serializedObject.targetObject;
            }

            var propertyRect = Rect.zero;
            var guiContent = new GUIContent(property.displayName);
            var foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            if (property.objectReferenceValue != null && AreAnySubPropertiesVisible(property))
            {
                property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, guiContent, true);
            }
            else
            {
                foldoutRect.x += 12;
                EditorGUI.Foldout(foldoutRect, property.isExpanded, guiContent, true, EditorStyles.label);
            }
            var indentedPosition = EditorGUI.IndentedRect(position);
            var indentOffset = indentedPosition.x - position.x;
            propertyRect = new Rect(position.x + (EditorGUIUtility.labelWidth - indentOffset), position.y, position.width - (EditorGUIUtility.labelWidth - indentOffset), EditorGUIUtility.singleLineHeight);

            if (propertySO != null || property.objectReferenceValue == null)
            {
                propertyRect.width -= buttonWidth;
            }

            EditorGUI.ObjectField(propertyRect, property, type, GUIContent.none);

            if (GUI.changed) property.serializedObject.ApplyModifiedProperties();

            var buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, EditorGUIUtility.singleLineHeight);

            if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue != null)
            {
                var data = (ScriptableObject)property.objectReferenceValue;

                if (property.isExpanded)
                {
                    GUI.Box(new Rect(0, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1, Screen.width, position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing), "");

                    EditorGUI.indentLevel++;
                    using (SerializedObject serializedObject = new SerializedObject(data))
                    {
                        // Iterate over all the values and draw them
                        using (SerializedProperty prop = serializedObject.GetIterator())
                        {
                            float y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                            if (prop.NextVisible(true))
                            {
                                do
                                {
                                    if (ReflectionUtil.IsPropName_m_Script(prop))
                                    {
                                        continue;
                                    }
                                    float height = EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName), true);
                                    EditorGUI.PropertyField(new Rect(position.x, y, position.width - buttonWidth, height), prop, true);
                                    y += height + EditorGUIUtility.standardVerticalSpacing;
                                }
                                while (prop.NextVisible(false));
                            }
                            if (GUI.changed)
                                serializedObject.ApplyModifiedProperties();
                        }
                    }

                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if (GUI.Button(buttonRect, CreateNewBtn))
                {
                    if (property.serializedObject.targetObject is MonoBehaviour)
                    {
                        MonoScript ms = MonoScript.FromMonoBehaviour((MonoBehaviour)property.serializedObject.targetObject);
                        savePath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(ms));
                    }
                    ScriptableObject obj = null;
                    savePath = EditorUtility.SaveFilePanelInProject("Save ScriptableObject", type.Name + ".asset", "asset", "Enter a file name for the ScriptableObject.", savePath);
                    if (savePath.Length > 0)
                    {
                        obj = ScriptableObject.CreateInstance(type);
                        AssetDatabase.CreateAsset(obj, savePath);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.ImportAsset(savePath, ImportAssetOptions.ForceUpdate);
                        EditorGUIUtility.PingObject(obj);
                        AssetDatabase.Refresh();
                    }
                    if (obj != null)
                    {
                        property.objectReferenceValue = obj;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                    GUIUtility.ExitGUI();
                }
            }
            property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();

            return property.isExpanded;
        }
        //public static T _GUILayout<T>(string label, T objectReferenceValue, ref bool isExpanded) where T : ScriptableObject
        //{
        //    return _GUILayout<T>(new GUIContent(label), objectReferenceValue, ref isExpanded);
        //}

        //public static T _GUILayout<T>(GUIContent label, T objectReferenceValue, ref bool isExpanded) where T : ScriptableObject
        //{
        //    Rect position = EditorGUILayout.BeginVertical();

        //    var propertyRect = Rect.zero;
        //    var guiContent = label;
        //    var foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
        //    if (objectReferenceValue != null)
        //    {
        //        isExpanded = EditorGUI.Foldout(foldoutRect, isExpanded, guiContent, true);

        //        var indentedPosition = EditorGUI.IndentedRect(position);
        //        var indentOffset = indentedPosition.x - position.x;
        //        propertyRect = new Rect(position.x + EditorGUIUtility.labelWidth - indentOffset, position.y, position.width - EditorGUIUtility.labelWidth - indentOffset, EditorGUIUtility.singleLineHeight);
        //    }
        //    else
        //    {
        //        // So yeah having a foldout look like a label is a weird hack 
        //        // but both code paths seem to need to be a foldout or 
        //        // the object field control goes weird when the codepath changes.
        //        // I guess because foldout is an interactable control of its own and throws off the controlID?
        //        foldoutRect.x += 12;
        //        EditorGUI.Foldout(foldoutRect, isExpanded, guiContent, true, EditorStyles.label);

        //        var indentedPosition = EditorGUI.IndentedRect(position);
        //        var indentOffset = indentedPosition.x - position.x;
        //        propertyRect = new Rect(position.x + EditorGUIUtility.labelWidth - indentOffset, position.y, position.width - EditorGUIUtility.labelWidth - indentOffset - 60, EditorGUIUtility.singleLineHeight);
        //    }

        //    EditorGUILayout.BeginHorizontal();
        //    objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent(" "), objectReferenceValue, typeof(T), false) as T;

        //    if (objectReferenceValue != null)
        //    {

        //        EditorGUILayout.EndHorizontal();
        //        if (isExpanded)
        //        {
        //            DrawScriptableObjectChildFields(objectReferenceValue);
        //        }
        //    }
        //    else
        //    {
        //        if (GUILayout.Button(CreateBtn, GUILayout.Width(buttonWidth)))
        //        {
        //            string selectedAssetPath = "Assets";
        //            var newAsset = CreateAssetWithSavePrompt(typeof(T), selectedAssetPath);
        //            if (newAsset != null)
        //            {
        //                objectReferenceValue = (T)newAsset;
        //            }
        //        }
        //        EditorGUILayout.EndHorizontal();
        //    }
        //    EditorGUILayout.EndVertical();
        //    return objectReferenceValue;
        //}

        //static void DrawScriptableObjectChildFields<T>(T objectReferenceValue) where T : ScriptableObject
        //{
        //    // Draw a background that shows us clearly which fields are part of the ScriptableObject
        //    EditorGUI.indentLevel++;
        //    EditorGUILayout.BeginVertical(GUI.skin.box);

        //    var serializedObject = new SerializedObject(objectReferenceValue);
        //    // Iterate over all the values and draw them
        //    SerializedProperty prop = serializedObject.GetIterator();
        //    if (prop.NextVisible(true))
        //    {
        //        do
        //        {
        //            // Don't bother drawing the class file
        //            if (prop.name == mScript) continue;
        //            EditorGUILayout.PropertyField(prop, true);
        //        }
        //        while (prop.NextVisible(false));
        //    }
        //    if (GUI.changed)
        //        serializedObject.ApplyModifiedProperties();
        //    serializedObject.Dispose();
        //    EditorGUILayout.EndVertical();
        //    EditorGUI.indentLevel--;
        //}

        //public static T DrawScriptableObjectField<T>(GUIContent label, T objectReferenceValue, ref bool isExpanded) where T : ScriptableObject
        //{
        //    Rect position = EditorGUILayout.BeginVertical();

        //    var propertyRect = Rect.zero;
        //    var guiContent = label;
        //    var foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
        //    if (objectReferenceValue != null)
        //    {
        //        isExpanded = EditorGUI.Foldout(foldoutRect, isExpanded, guiContent, true);

        //        var indentedPosition = EditorGUI.IndentedRect(position);
        //        var indentOffset = indentedPosition.x - position.x;
        //        propertyRect = new Rect(position.x + EditorGUIUtility.labelWidth - indentOffset, position.y, position.width - EditorGUIUtility.labelWidth - indentOffset, EditorGUIUtility.singleLineHeight);
        //    }
        //    else
        //    {
        //        // So yeah having a foldout look like a label is a weird hack 
        //        // but both code paths seem to need to be a foldout or 
        //        // the object field control goes weird when the codepath changes.
        //        // I guess because foldout is an interactable control of its own and throws off the controlID?
        //        foldoutRect.x += 12;
        //        EditorGUI.Foldout(foldoutRect, isExpanded, guiContent, true, EditorStyles.label);

        //        var indentedPosition = EditorGUI.IndentedRect(position);
        //        var indentOffset = indentedPosition.x - position.x;
        //        propertyRect = new Rect(position.x + EditorGUIUtility.labelWidth - indentOffset, position.y, position.width - EditorGUIUtility.labelWidth - indentOffset - 60, EditorGUIUtility.singleLineHeight);
        //    }

        //    EditorGUILayout.BeginHorizontal();
        //    objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent(" "), objectReferenceValue, typeof(T), false) as T;

        //    if (objectReferenceValue != null)
        //    {
        //        EditorGUILayout.EndHorizontal();
        //        if (isExpanded)
        //        {

        //        }
        //    }
        //    else
        //    {
        //        if (GUILayout.Button(CreateBtn, GUILayout.Width(buttonWidth)))
        //        {
        //            string selectedAssetPath = "Assets";
        //            var newAsset = CreateAssetWithSavePrompt(typeof(T), selectedAssetPath);
        //            if (newAsset != null)
        //            {
        //                objectReferenceValue = (T)newAsset;
        //            }
        //        }
        //        EditorGUILayout.EndHorizontal();
        //    }
        //    EditorGUILayout.EndVertical();
        //    return objectReferenceValue;
        //}
    }
}
#endif