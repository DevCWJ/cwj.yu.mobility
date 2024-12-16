using System;
using System.IO;
using System.Reflection;

using UnityEditor;

using UnityEngine;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(CreateAssetAttribute), true)]
    public class CreateAssetAttribute_Editor : PropertyDrawer
    {
        private static Type createPropertyType;
        private static string lastDirectory = Application.dataPath;

        private static void OnCreate(object val)
        {
            string assetTypeName = createPropertyType.ToString();
            string assetShortName = assetTypeName.Substring(assetTypeName.LastIndexOf('.') + 1, assetTypeName.Length - assetTypeName.LastIndexOf('.') - 1);

            if (!Directory.Exists(lastDirectory)) lastDirectory = Application.dataPath;

            string userFilePath = EditorUtility.SaveFilePanel("Save " + assetShortName, lastDirectory, assetShortName, "asset");

            if (string.IsNullOrEmpty(userFilePath)) return;

            string userFileDirectory = Path.GetDirectoryName(userFilePath);

            if (userFileDirectory.Length < Application.dataPath.Length)
            {
                typeof(UnityEditor.Editor).DisplayDialog("Invalid Path\nThe path must be within the Unity Assets folder");
                return;
            }

            lastDirectory = userFileDirectory;

            string filePathRelative = userFilePath.Replace(Application.dataPath, "Assets");

            CreateScriptableObjectAsset(val, filePathRelative);
        }

        private static void OnCreateInBrowser(object val)
        {
            string assetTypeName = createPropertyType.ToString();
            string assetShortName = assetTypeName.Substring(assetTypeName.LastIndexOf('.') + 1, assetTypeName.Length - assetTypeName.LastIndexOf('.') - 1);

            string projectPathRelative = GetActiveFolderPath();

            if (string.IsNullOrEmpty(projectPathRelative)) projectPathRelative = "Assets";

            lastDirectory = projectPathRelative.Replace("Assets", Application.dataPath);

            string filePathRelative = projectPathRelative + Path.AltDirectorySeparatorChar + assetShortName + ".asset";

            string uniquePathRelative = AssetDatabase.GenerateUniqueAssetPath(filePathRelative);

            CreateScriptableObjectAsset(val, uniquePathRelative);
        }

        private static void CreateScriptableObjectAsset(object val, string userFilePathRelative)
        {
            AssetDatabase.Refresh();

            ScriptableObject asset = ScriptableObject.CreateInstance(createPropertyType);

            AssetDatabase.CreateAsset(asset, userFilePathRelative);

            var prop = (val as SerializedProperty);
            prop.objectReferenceValue = asset;
            prop.serializedObject.ApplyModifiedProperties();

            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        private static string GetActiveFolderPath()
        {
            string defaultPath = "Assets";

            Type projectWindowUtilType = typeof(ProjectWindowUtil);
            if (projectWindowUtilType == null) return defaultPath;

            MethodInfo getFolderMethodInfo = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.NonPublic | BindingFlags.Static);
            if (getFolderMethodInfo == null) return defaultPath;

            string projectPath = (string)getFolderMethodInfo.Invoke(null, null);

            return projectPath;
        }
        private float height = 0;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (position.Contains((Event.current.mousePosition)))
            {
                bool leftClickOnEmpty = Event.current.type == EventType.MouseUp && null == property.objectReferenceValue;
                bool contextClick = Event.current.type == EventType.ContextClick;

                if (contextClick || leftClickOnEmpty)
                {
                    createPropertyType = fieldInfo.FieldType;

                    if (createPropertyType.BaseType == typeof(ScriptableObject))
                    {
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("Create"), false, OnCreate, property);
                        menu.AddItem(new GUIContent("CreateInBrowser"), false, OnCreateInBrowser, property);
                        menu.ShowAsContext();
                    }
                }
            }

            var info = EditorGUI_CWJ.PropertyField_CWJ(fieldInfo, position, property, label);
            height = info.height;
        }
    }
}