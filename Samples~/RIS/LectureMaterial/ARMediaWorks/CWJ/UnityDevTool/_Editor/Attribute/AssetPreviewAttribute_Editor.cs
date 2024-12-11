using UnityEditor;

using UnityEngine;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(AssetPreviewAttribute))]
    public class AssetPreviewAttribute_Editor : PropertyDrawer
    {
        private float height = 0;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var info = EditorGUI_CWJ.PropertyField_CWJ(fieldInfo, position, property, label);
            height = info.height;
            Texture2D previewTexture = null;

            if (property.propertyType == SerializedPropertyType.ObjectReference
                && property.objectReferenceValue != null)
            {
                previewTexture = AssetPreview.GetAssetPreview(property.objectReferenceValue);
            }

            if (previewTexture != null)
            {
                AssetPreviewAttribute assetPreviewAttribute = attribute as AssetPreviewAttribute;
                int width = Mathf.Clamp(assetPreviewAttribute.width, 0, previewTexture.width);
                int height = Mathf.Clamp(assetPreviewAttribute.height, 0, previewTexture.height);

                GUILayout.Label(previewTexture, GUILayout.MaxWidth(width), GUILayout.MaxHeight(height));
            }
            else
            {
                typeof(AssetPreviewAttribute).DrawHelpBox(property.name + " doesn't have an asset preview", MessageType.Warning, context: property.GetTargetObject(), false);
            }
        }
    }
}