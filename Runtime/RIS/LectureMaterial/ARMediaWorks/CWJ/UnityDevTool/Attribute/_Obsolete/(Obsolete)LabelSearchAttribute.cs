//(Obsolete)
//폐기되었지만 지우면 안됨
//using UnityEngine;
//using System.Collections.Generic;
//using System.Reflection;
//#if UNITY_EDITOR
//using UnityEditor;
//using System.Linq;

//#endif

//public class LabelSearchAttribute : PropertyAttribute
//{
//    public bool init = false;

//    public bool search = true;

//    public string labelName;

//    public bool canPrintLabelName = false;

//    public bool foldout = false;

//    public Direction direction = Direction.ASC;

//    public int limit = 2147483647;

//    public static Dictionary<string, System.Type> assetTypes = new Dictionary<string, System.Type>();

//    public LabelSearchAttribute(string labelName)
//    {
//        this.labelName = labelName;
//    }

//    public LabelSearchAttribute(string labelName, int limit)
//    {
//        if (Mathf.Sign(limit) == 1)
//        {
//            this.limit = limit;
//        }

//        this.labelName = labelName;
//    }

//    public LabelSearchAttribute(string labelName, Direction direction)
//    {
//        this.labelName = labelName;
//        this.direction = direction;
//    }

//    public LabelSearchAttribute(string labelName, int limit, Direction direction)
//    {
//        this.labelName = labelName;

//        if (Mathf.Sign(limit) == 1)
//        {
//            this.limit = limit;
//        }

//        this.direction = direction;
//    }

//    public enum Direction
//    {
//        ASC,
//        DESC
//    }
//}

//#if UNITY_EDITOR

//[CustomPropertyDrawer(typeof(LabelSearchAttribute))]
//public class LabelSearchDrawer : PropertyDrawer
//{
//    private const int CONTENT_HEIGHT = 16;

//    SerializedProperty GetArrayProperty(SerializedProperty property)
//    {
//        string[] variableName = property.propertyPath.Split('.');
//        return property.serializedObject.FindProperty(variableName[0]);
//    }

//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        if (!labelSearchAttribute.init)
//        {
//            labelSearchAttribute.init = true;
//            return;
//        }

//        if (labelSearchAttribute.canPrintLabelName)
//        {
//            label.text += string.Format(" ( Label = {0} )", labelSearchAttribute.labelName);
//        }

//        SerializedProperty serializedProperty = GetArrayProperty(property);
//        if (serializedProperty.isArray)
//        {
//            EditorGUI.indentLevel = 0;
//            labelSearchAttribute.foldout = EditorGUI.Foldout(position, labelSearchAttribute.foldout, label);
//            if (labelSearchAttribute.search)
//            {
//                DrawArrayProperty(position, serializedProperty, label);

//            }
//            else
//            {
//                DrawCachedArrayProperty(position, serializedProperty, label);
//            }
//        }
//        else
//        {
//            if (labelSearchAttribute.search)
//            {
//                DrawSingleProperty(position, property, label);

//            }
//            else
//            {
//                DrawCachedSingleProperty(position, property, label);
//            }
//        }

//        labelSearchAttribute.search = false;
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        float height = 0;

//        if (property.isArray && labelSearchAttribute.foldout)
//        {
//            height = (property.arraySize + 1) * CONTENT_HEIGHT;
//        }

//        return base.GetPropertyHeight(property, label) + height;
//    }

//    LabelSearchAttribute labelSearchAttribute
//    {
//        get { return (LabelSearchAttribute)attribute; }
//    }

//    void DrawCachedSingleProperty(Rect position, SerializedProperty property, GUIContent label)
//    {
//        property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue,
//            GetType(property), false);
//    }

//    void DrawCachedArrayProperty(Rect position, SerializedProperty property, GUIContent label)
//    {
//        if (labelSearchAttribute.foldout)
//        {
//            position.y += CONTENT_HEIGHT;
//            EditorGUI.indentLevel = 2;
//            System.Type type = GetType(property.GetArrayElementAtIndex(0));
//            EditorGUI.LabelField(position, "Size", property.arraySize.ToString());

//            for (int i = 0; i < property.arraySize; i++)
//            {
//                position.y += CONTENT_HEIGHT;
//                position.height = CONTENT_HEIGHT;
//                GUIContent content =
//                    EditorGUIUtility.ObjectContent(property.GetArrayElementAtIndex(i).objectReferenceValue, type);
//                content.image = AssetPreview.GetMiniTypeThumbnail(type);

//                EditorGUI.LabelField(position, new GUIContent(ObjectNames.NicifyVariableName("Element" + i)), content);
//            }
//        }
//    }

//    void DrawSingleProperty(Rect position, SerializedProperty property, GUIContent label)
//    {
//        System.Type type = GetType(property);

//        property.objectReferenceValue = null;

//        foreach (string path in GetAllAssetPath())
//        {
//            System.Type assetType = null;
//            Object asset = null;

//            if (!LabelSearchAttribute.assetTypes.TryGetValue(path, out assetType))
//            {
//                asset = AssetDatabase.LoadMainAssetAtPath(path);

//                if (asset == null)
//                {
//                    continue;
//                }

//                assetType = asset.GetType();
//                LabelSearchAttribute.assetTypes.Add(path, assetType);
//            }

//            if (type != assetType)
//            {
//                continue;
//            }

//            if (asset == null)
//            {
//                asset = AssetDatabase.LoadMainAssetAtPath(path);

//                if (asset == null)
//                {
//                    continue;
//                }
//            }

//            if (!string.IsNullOrEmpty(AssetDatabase.GetLabels(asset).FirstOrDefault(l => l.Equals(labelSearchAttribute.labelName))))
//            {
//                property.objectReferenceValue = asset;
//                break;
//            }
//        }

//        property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, type,
//            false);
//    }

//    void DrawArrayProperty(Rect position, SerializedProperty property, GUIContent label)
//    {
//        int size = 0;

//        EditorGUI.indentLevel = 2;

//        if (labelSearchAttribute.foldout)
//        {
//            position.y += CONTENT_HEIGHT;
//            EditorGUI.LabelField(position, "Size", property.arraySize.ToString());
//        }

//        property.arraySize = 0;
//        property.InsertArrayElementAtIndex(0);
//        System.Type type = GetType(property.GetArrayElementAtIndex(0));

//        foreach (string path in GetAllAssetPath())
//        {
//            System.Type assetType = null;
//            UnityEngine.Object asset = null;

//            if (!LabelSearchAttribute.assetTypes.TryGetValue(path, out assetType))
//            {
//                asset = AssetDatabase.LoadMainAssetAtPath(path);
//                assetType = asset.GetType();
//                LabelSearchAttribute.assetTypes.Add(path, assetType);
//            }

//            if (type != assetType)
//            {
//                continue;
//            }

//            if (asset == null)
//            {
//                asset = AssetDatabase.LoadMainAssetAtPath(path);
//            }

//            if (!string.IsNullOrEmpty(AssetDatabase.GetLabels(asset).FirstOrDefault(l => l.Equals(labelSearchAttribute.labelName))))
//            {
//                property.arraySize = ++size;
//                property.GetArrayElementAtIndex(size - 1).objectReferenceValue = asset;
//                if (labelSearchAttribute.foldout)
//                {
//                    position.y += CONTENT_HEIGHT;
//                    position.height = CONTENT_HEIGHT;
//                    GUIContent content =
//                        EditorGUIUtility.ObjectContent(property.GetArrayElementAtIndex(size - 1).objectReferenceValue,
//                            type);
//                    content.image = AssetPreview.GetMiniTypeThumbnail(type);

//                    EditorGUI.ObjectField(position, new GUIContent(ObjectNames.NicifyVariableName("Element" + i)), property.GetArrayElementAtIndex(i).objectReferenceValue, type, false);
//                    EditorGUI.LabelField(position,
//                        new GUIContent(ObjectNames.NicifyVariableName("Element" + (size - 1))), content);
//                }
//            }

//            if (labelSearchAttribute.limit <= property.arraySize)
//            {
//                break;
//            }
//        }

//    }

//    string[] GetAllAssetPath()
//    {
//        string[] allAssetPath = AssetDatabase.GetAllAssetPaths();

//        System.Array.Sort(allAssetPath);

//        if (labelSearchAttribute.direction.Equals(LabelSearchAttribute.Direction.DESC))
//        {
//            System.Array.Reverse(allAssetPath);
//        }
//        return allAssetPath;
//    }

//    System.Type GetType(SerializedProperty property)
//    {
//        return
//            Assembly.Load("UnityEngine.dll")
//                .GetType("UnityEngine." + property.type.Replace("PPtr<$", "").Replace(">", ""));
//    }
//}

//#endif