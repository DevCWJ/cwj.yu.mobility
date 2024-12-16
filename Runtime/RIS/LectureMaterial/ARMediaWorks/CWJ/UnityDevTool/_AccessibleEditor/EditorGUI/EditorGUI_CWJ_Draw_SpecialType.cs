#if UNITY_EDITOR
using System;
using System.Reflection;

using UnityEditor;

using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace CWJ.AccessibleEditor
{
    public static partial class EditorGUI_CWJ
    {
        private static object DrawClassOrStructType(Type type, string name, object value, ref bool isValueChangedViaCode, int reflectObjInstanceID)
        {
            //Initialize가 안된 경우
            if (value == null || value.Equals(null))
            {
                if (type.IsTupleType()) 
                {
                    value = ReflectionUtil.TupleCreateInstance(type, value);
                }
                else
                {
                    value = TypeHelper.CreateInstance(type);
                    isValueChangedViaCode = true;
                    if (value == null)
                    {
                        EditorGUI_CWJ.DrawLabel_Exception(type, name);
                        return null;
                    }
                }
            }

            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance |
                                                    BindingFlags.Static |
                                                    BindingFlags.Public |
                                                    BindingFlags.NonPublic |
                                                    BindingFlags.DeclaredOnly);
            
            var cacheKey = new FoldoutCacheKey(reflectObjInstanceID, name);

            if (!FoldoutCacheDict.TryGetValue(cacheKey, out bool isFoldout))
            {
                FoldoutCacheDict.Add(cacheKey, isFoldout = false);
            }

            if (isFoldout = EditorGUILayout.Foldout(isFoldout, name, true))
            {
                ++EditorGUI.indentLevel;

                foreach (var fieldInfo in fieldInfos)
                {
                    if (fieldInfo.IsAutoPropertyField()) continue;

                    Type fieldType = fieldInfo.FieldType;
                    var variousTypeDrawer = GetDrawVariousTypeDelegate(fieldInfo.FieldType);
                    if (variousTypeDrawer == null) continue;

                    var result = DrawVariousFieldTypeWithAtt(fieldInfo, fieldInfo.Name, value, false, variousTypeDrawer);
                    if (!isValueChangedViaCode && result.isChanged)
                    {
                        isValueChangedViaCode = true;
                    }
                }
                //var newHashKey = (value.GetHashCode(), name);
                //FoldoutCacheDict.ChangeHashKey(cacheKey, newHashKey);
                --EditorGUI.indentLevel;
            }
            if(FoldoutCacheDict[cacheKey] != isFoldout)
            {
                FoldoutCacheDict[cacheKey] = isFoldout;
            }

            return value;
        }

        private static object DrawLayerMask(Type type, string name, object lastValue, ref bool isValueChangedViaCode)
        {
            var layerMaskValue = (LayerMask)lastValue;

            string[] allLayerNames = UnityEditorInternal.InternalEditorUtility.layers;
            int[] allLayerNumbers = allLayerNames.ConvertAll(s => LayerMask.NameToLayer(s));

            int maskWithoutEmpty = 0;
            for (int i = 0; i < allLayerNumbers.Length; i++)
            {
                if (((1 << allLayerNumbers[i]) & layerMaskValue.value) > 0)
                    maskWithoutEmpty |= (1 << i);
            }
            bool guiEnabled = GUI.enabled;
            if (!guiEnabled)
            {
                GUI.enabled = true;
            }
            int newMaskValue = UnityEditor.EditorGUILayout.MaskField(name, maskWithoutEmpty, allLayerNames);
            if (guiEnabled)
            {
                int mask = 0;
                for (int i = 0; i < allLayerNumbers.Length; i++)
                {
                    if ((newMaskValue & (1 << i)) > 0)
                        mask |= (1 << allLayerNumbers[i]);
                }
                layerMaskValue.value = mask;
            }
            else
            {
                GUI.enabled = false;
            }

            return layerMaskValue;
        }

        private static object DrawInterface(Type type, string name, object lastValue, ref bool isValueChangedViaCode)
        {
            var rect = EditorGUILayout.BeginHorizontal();
            var value = EditorGUILayout.ObjectField(name, (lastValue as Component), type, true);
            EditorGUILayout.EndHorizontal();

            if (!GUI.enabled) return value;
            if (!rect.Contains(Event.current.mousePosition)) return value;

            if (Event.current.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                Event.current.Use();
                return value;
            }
            else if (Event.current.type == EventType.DragPerform)
            {
                var dragObjs = DragAndDrop.objectReferences;

                if (dragObjs.Length == 0 || dragObjs[0] == null) return value;

                Type dragObjType = dragObjs[0].GetType();

                if (dragObjType == typeof(GameObject))
                {
                    foreach (var obj in dragObjs)
                    {
                        foreach (var comp in (obj as GameObject).GetComponents<Component>())
                        {
                            if (comp.GetType().GetInterfaces().IsExists(type))
                            {
                                isValueChangedViaCode = true;

                                Event.current.Use();
                                return comp;
                            }
                        }
                    }
                }
                else if (dragObjType == typeof(Component) && dragObjType.GetInterfaces().IsExists(type))
                {
                    isValueChangedViaCode = true;
                    Event.current.Use();
                    return dragObjs[0];
                }
            }

            return value;
        }

        private static readonly Type TypeOfTextAsset = typeof(TextAsset);

        private static object DrawUnityObject(Type type, string name, object lastValue, ref bool isValueChangedViaCode)
        {
            var newValue = EditorGUILayout.ObjectField(name, (lastValue as UnityObject), type, true);
            if (newValue == null) return null;

            Type newValueType = newValue.GetType();

            if (newValueType == null || !TypeOfTextAsset.IsAssignableFrom(newValueType))
            { 
                string newValueStr = newValue.ToString();
                int lastIndex = newValueStr.LastIndexOf('(');
                string valueTypeFullName = newValueStr.Substring(lastIndex + 1, newValueStr.Length - (lastIndex + 1) - 1);

                newValueType = (newValueType ?? type).Assembly?.GetType(valueTypeFullName);
            }

            if (newValueType != null && (type.Equals(newValueType) || type.IsAssignableFrom(newValueType)))
            {
                return newValue;
            }
            else
            {
                isValueChangedViaCode = true;
                return null;
            }
            // NOTE: object타입에서 type을 가져올 방법이 ToString 밖에 없을까?
            // 아래의 오류때문에 type비교는 꼭필요
            // 해당 오류 재연방법>>
            // 1. EditorGUILayout.ObjectField(name, value as UnityObject, type, true)를 바로 return 시키고 그 아래는 전부 주석처리. 
            // 2. 유효한 값이 들어있던 VisualizeFieldAttribute 배열의 자료형을 바꾸어줌.
            // (바꿀 자료형은 전/후 모두 UnityEngine에서 만든 Component내에서, 실행중일 필요없음)
            // ★결과: 자료형이 바뀌었음에도 이전 자료형일때 넣어놨던 값들이 그대로 남아있음. 그리고 GetType().Name하면 정상출력됨 ㅂㄷㅂㄷ
        }
    }
}
#endif