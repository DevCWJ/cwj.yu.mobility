using System.Linq;

using UnityEditor;
using UnityEditor.Animations;

using UnityEngine;
using CWJ.AccessibleEditor;
using System.Reflection;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(AnimatorParameterAttribute))]
    public class AnimatorParameterAttribute_Editor : PropertyDrawer_CWJ
    {

        public override float GetHeight(SerializedProperty property, GUIContent label)
        {
            return GetPropertyHeightDefault(property, label);
        }

        public override bool DrawGUI(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            var animatorController = GetAnimatorController(property);

            if (animatorController == null)
            {
                DefaultInspector(position, property, label);
                return false;
            }
            var parameters = animatorController.parameters;

            if (parameters.Length == 0)
            {
                typeof(AnimatorParameterAttribute).PrintLogWithClassName("AnimationParamater is 0", LogType.Warning, obj: property.GetTargetObject(), isPreventOverlapMsg: true);
                property.stringValue = string.Empty;
                DefaultInspector(position, property, label);
                return false;
            }

            var eventNames = parameters
                .Where(t => CanAddEventName(t.type))
                .Select(t => t.name).ToList();

            if (eventNames.Count == 0)
            {
                typeof(AnimatorParameterAttribute).PrintLogWithClassName(animatorParameterAttribute.parameterType + " Parameter is 0", LogType.Warning, obj: property.GetTargetObject(), isPreventOverlapMsg: true);
                property.stringValue = string.Empty;
                DefaultInspector(position, property, label);
                return false;
            }

            var eventNamesArray = eventNames.ToArray();

            var matchIndex = eventNames
                .FindIndex(eventName => eventName.Equals(property.stringValue));

            if (matchIndex != -1)
            {
                animatorParameterAttribute.selectedValue = matchIndex;
            }

            animatorParameterAttribute.selectedValue = EditorGUI.IntPopup(position, label.text, animatorParameterAttribute.selectedValue, eventNamesArray, SetOptionValues(eventNamesArray));

            property.stringValue = eventNamesArray[animatorParameterAttribute.selectedValue];
            return false;
        }

        private AnimatorController GetAnimatorController(SerializedProperty property)
        {
            var component = property.serializedObject.targetObject as Component;

            if (component == null)
            {
                typeof(AnimatorParameterAttribute).PrintLogWithClassName("[Missing] Couldn't cast targetObject", LogType.Error, obj: component.gameObject, isPreventOverlapMsg: true);
                return null;
            }

            var anim = component.GetComponent<Animator>();

            if (anim == null)
            {
                typeof(AnimatorParameterAttribute).PrintLogWithClassName("[Missing] Missing Aniamtor Component", LogType.Error, obj: component.gameObject, isPreventOverlapMsg: true);
                return null;
            }

            return anim.runtimeAnimatorController as AnimatorController;
        }

        private bool CanAddEventName(AnimatorControllerParameterType animatorControllerParameterType)
        {
            return !(animatorParameterAttribute.parameterType != AnimatorParameterAttribute.ParameterType.None
                     && (int)animatorControllerParameterType != (int)animatorParameterAttribute.parameterType);
        }

        private int[] SetOptionValues(string[] eventNames)
        {
            int[] optionValues = new int[eventNames.Length];

            for (int i = 0; i < eventNames.Length; ++i)
            {
                optionValues[i] = i;
            }

            return optionValues;
        }

        AnimatorParameterAttribute animatorParameterAttribute => attribute as AnimatorParameterAttribute;

        private void DefaultInspector(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, false);
        }
    }
}