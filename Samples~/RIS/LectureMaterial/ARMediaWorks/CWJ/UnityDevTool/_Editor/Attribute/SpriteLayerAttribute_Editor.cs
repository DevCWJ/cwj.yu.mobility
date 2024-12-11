using UnityEditor;

using UnityEngine;

using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(SpriteLayerAttribute))]
    public class SpriteLayerAttribute_Editor : PropertyDrawer
    {
        float height;
        bool isValidType = true;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return isValidType ? EditorGUI.GetPropertyHeight(property, label, true) : height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            isValidType = property.propertyType == SerializedPropertyType.Integer;

            if (!isValidType)
            {
                if (!_checkedType) PropertyTypeWarning(property);
                var info = EditorGUI_CWJ.PropertyField_CWJ(fieldInfo, position, property, label);
                height = info.height;
                return;
            }

            var spriteLayerNames = GetSpriteLayerNames();
            HandleSpriteLayerSelectionUI(position, property, label, spriteLayerNames);
        }

        private bool _checkedType;

        private void PropertyTypeWarning(SerializedProperty property)
        {
            typeof(SpriteLayerAttribute).PrintLogWithClassName($"Property {property.name.SetColor(new Color().GetBrown())}in object <color=brown>{property.serializedObject.targetObject.name.SetColor(new Color().GetBrown())}</color> is of wrong type{property.type}. Expected: Int", LogType.Warning, property.GetTargetObject(), isPreventOverlapMsg: true);
            _checkedType = true;
        }

        private void HandleSpriteLayerSelectionUI(Rect position, SerializedProperty property, GUIContent label, string[] spriteLayerNames)
        {
            EditorGUI.BeginProperty(position, label, property);

            // To show which sprite layer is currently selected.
            int currentSpriteLayerIndex;
            bool layerFound = TryGetSpriteLayerIndexFromProperty(out currentSpriteLayerIndex, spriteLayerNames, property);

            if (!layerFound)
            {
                // Set to default layer. (Previous layer was removed)
                typeof(SpriteLayerAttribute).PrintLogWithClassName($"Property {property.name.SetColor(new Color().GetBrown())} in object <color=brown>{property.serializedObject.targetObject}</color> is set to the default layer. Reason: previously selected layer was removed.", LogType.Log, property.GetTargetObject(), isPreventOverlapMsg: true);
                property.intValue = 0;
                currentSpriteLayerIndex = 0;
            }

            int selectedSpriteLayerIndex = EditorGUI.Popup(position, label.text, currentSpriteLayerIndex, spriteLayerNames);

            // Change property value if user selects a new sprite layer.
            if (selectedSpriteLayerIndex != currentSpriteLayerIndex)
            {
                property.intValue = SortingLayer.NameToID(spriteLayerNames[selectedSpriteLayerIndex]);
            }

            EditorGUI.EndProperty();
        }

        #region Util

        private bool TryGetSpriteLayerIndexFromProperty(out int index, string[] spriteLayerNames, SerializedProperty property)
        {
            // To keep the property's value consistent, after the layers have been sorted around.
            string layerName = SortingLayer.IDToName(property.intValue);

            // Return the index where on it matches.
            for (int i = 0; i < spriteLayerNames.Length; ++i)
            {
                if (spriteLayerNames[i].Equals(layerName))
                {
                    index = i;
                    return true;
                }
            }

            // The current layer was removed.
            index = -1;
            return false;
        }

        private string[] GetSpriteLayerNames()
        {
            string[] result = new string[SortingLayer.layers.Length];

            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = SortingLayer.layers[i].name;
            }

            return result;
        }

        #endregion Util
    }
}