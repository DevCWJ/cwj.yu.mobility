#if UNITY_EDITOR
using System.Reflection;

using UnityEditor;
using UnityEngine;
using CWJ.Serializable;
using CWJ.AccessibleEditor;
namespace CWJ.EditorOnly
{
    public class DictionaryDisplayDrawer : PropertyControl
    {
        public const string _invalidTypeWarning = "Invalid type for DictionaryDisplay on field {0}: DictionaryDisplay can only be applied to IEditableDictionary fields";

        private static IconButton _addButton = new IconButton(IconButton.CustomAdd, "Add an item to this dictionary");
        private static IconButton _editButton = new IconButton(IconButton.Edit, "Edit this item");
        private static IconButton _removeButton = new IconButton(IconButton.Remove, "Remove this item from the dictionary");

        private IEditableDictionary _dictionary;
        private DictionaryControl _dictionaryControl = new DictionaryControl();
        private GUIContent _label;

        private static string GetOpenPreference(SerializedProperty property)
        {
            return property.serializedObject.targetObject.GetType().Name + "." + property.propertyPath + ".IsOpen";
        }
#pragma warning disable 0612 
        public override void Setup(SerializedProperty property, FieldInfo fieldInfo)
        {
            Setup(property, TypeHelper.GetAttribute<SerializableDictionaryAttribute>(fieldInfo));
        }

        public void Setup(SerializedProperty property, SerializableDictionaryAttribute attribute)
        {
            _dictionary = GetObject<IEditableDictionary>(property);

            if (_dictionary == null)
            {
                Debug.LogWarningFormat(_invalidTypeWarning, property.propertyPath);
            }
            else
            {
                _dictionaryControl.Setup(property, _dictionary);

                if (attribute != null)
                {
                    _dictionaryControl.isReadonly = attribute.isReadonly;

                    if (attribute.isInlineChildren)
                        _dictionaryControl.MakeDrawableInline();

                    if (attribute.isAllowAdd)
                        _dictionaryControl.MakeAddable(_addButton, attribute.addLabel == null ? new GUIContent("Add Item") : (attribute.addLabel == "" ? GUIContent.none : new GUIContent(attribute.addLabel)));

                    if (attribute.isAllowRemove)
                        _dictionaryControl.MakeRemovable(_removeButton);

                    if (attribute.isAllowCollapse) 
                        _dictionaryControl.MakeCollapsable(GetOpenPreference(property));

                    if (attribute.isShowEditButton)
                        _dictionaryControl.MakeEditable(_editButton);

                    if (attribute.emptyText != null)
                        _dictionaryControl.MakeEmptyLabel(new GUIContent(attribute.emptyText));
                }
            }
        }
#pragma warning restore
        public override float GetHeight(SerializedProperty property, GUIContent label)
        {
            return _dictionary != null ? _dictionaryControl.GetHeight() : EditorGUI.GetPropertyHeight(property, label);
        }

        public override void Draw(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_dictionary != null)
            {
                // During drawing the text of label changes so Unity must internally pool or reuse it for each element. In
                // any case, making a copy of it fixes the problem.

                if (_label == null)
                    _label = new GUIContent(label);

                _dictionaryControl.Draw(position, _label);
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }

    [CustomPropertyDrawer(typeof(SerializableDictionaryAttribute))]
    public class DictionaryDisplayAttributeDrawer : ControlDrawer<DictionaryDisplayDrawer>
    {
        private float height = 0;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var info = EditorGUI_CWJ.PropertyField_CWJ(fieldInfo, position, property, label, drawFunc: DrawDictionaryBase);
            height = info.height;
        }

        public (bool isExpanded, float height) DrawDictionaryBase(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            var control = GetControl(property);
            control.Draw(position, property, label);
            return (true, control.GetHeight(property, label));
        }

    }
}

#endif