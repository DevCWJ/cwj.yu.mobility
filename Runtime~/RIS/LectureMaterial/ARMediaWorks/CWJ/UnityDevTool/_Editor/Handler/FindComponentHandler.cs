using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;

using CWJ.AccessibleEditor;

using UnityObject = UnityEngine.Object;

namespace CWJ.EditorOnly
{
    /// <summary>
    /// 현재 부모클래스에 등록된 어트리뷰트는 찾아내지 않고있어서 
    /// </summary>
    public static class FindComponentHandler
    {
        private readonly static Dictionary<int, (FindCompEventHandler findCompFunc, FindCompsEventHandler findCompsFunc)>
        FindCompDictionary = new Dictionary<int, (FindCompEventHandler findCompFunc, FindCompsEventHandler findCompsFunc)>
        {
            {typeof(GetComponentAttribute).GetHashCode(),             (GetCompFunc,           GetCompsFunc        )},
            {typeof(GetComponentInChildrenAttribute).GetHashCode(),   (GetCompInChildFunc,    GetCompsInChildFunc )},
            {typeof(GetComponentInParentAttribute).GetHashCode(),     (GetCompInParentFunc,   GetCompsInParentFunc)},
            {typeof(FindObjectAttribute).GetHashCode(),               (FindObjectFunc,        FindObjectsFunc  )},
            {typeof(MustRequiredCompAttribute).GetHashCode(),         (GetOrAddCompFunc,      GetOrAddCompsFunc   )}
        };

        private readonly static Type GameObjectType = typeof(GameObject);

        private readonly static Dictionary<int, (FindCompEventHandler findCompFunc, FindCompsEventHandler findCompsFunc)>
        FindGoDictionary = new Dictionary<int, (FindCompEventHandler findCompFunc, FindCompsEventHandler findCompsFunc)>
        {
            {typeof(GetComponentAttribute).GetHashCode(),             (GetGoFunc,           GetGosFunc        )},
            {typeof(GetComponentInChildrenAttribute).GetHashCode(),   (GetGoInChildFunc,    GetGosInChildFunc )},
            {typeof(GetComponentInParentAttribute).GetHashCode(),     (GetGoInParentFunc,   GetGosInParentFunc)},
            {typeof(FindObjectAttribute).GetHashCode(),               (FindGoFunc,          FindGosFunc)},
            {typeof(MustRequiredCompAttribute).GetHashCode(),         (GetOrAddGoFunc,      GetOrAddGosFunc   )}
        };

        private static (FindCompEventHandler findCompFunc, FindCompsEventHandler findCompsFunc) GetFindCompEvent(Type elemType, _Root_FindCompAttribute findCompAttribute)
        {
            int attHashCode = findCompAttribute.GetType().GetHashCode();
            return elemType.Equals(GameObjectType) ? FindGoDictionary[attHashCode] : FindCompDictionary[attHashCode];
        }

        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            CWJ_EditorEventHelper.CompSelectedEvent += GetComponentLooper;

            CustomAttributeHandler.EditorSceneOpenedEvent += GetComponentLooper; //씬 열 때 + 프로젝트 열 때
            CustomAttributeHandler.EditorSceneClosedEvent += GetComponentLooper; //씬 닫을 때

            CustomAttributeHandler.ReloadedScriptEvent += GetComponentLooper; //스크립트 컴파일될때
            CustomAttributeHandler.EditorWillSaveAfterModifiedEvent += GetComponentLooper; //유니티 저장 시도를 할 때
            CustomAttributeHandler.ExitingEditModeEvent += GetComponentLooper; // 실행직전
            CustomAttributeHandler.BeforeBuildEvent += GetComponentLooper; //빌드되기전

            ////EditorEventSystem.AddComponentEvent += UpdateGetComponentBot<MonoBehaviour>;
            ////EditorEventSystem.RemoveComponentEvent += UpdateGetComponentBot<MonoBehaviour>;
            ////EditorEventSystem.TransformHierarchyChangedEvent += UpdateGetComponentBot<MonoBehaviour>;
        }

        private static void GetComponentLooper(MonoBehaviour comp, Type type)
        {
            var allBaseTypes = ReflectionUtil.GetAllBaseClassTypes(type).Reverse();

            BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            var allFieldInfos = allBaseTypes.SelectMany(t => t.GetFields(bindingFlags));

            foreach (var field in allFieldInfos)
            {
                if (field.IsAutoPropertyField() || !field.IsSerializeField())
                {
                    continue;
                }

                var findCompAtt = field.GetCustomAttribute<_Root_FindCompAttribute>(true);
                if (findCompAtt != null)
                {
                    FindComponentAndSetValue(comp, field, findCompAtt);
                }
            }
        }

        private static void FindComponentAndSetValue(MonoBehaviour component, FieldInfo fieldInfo, _Root_FindCompAttribute findCompAtt)
        {
            bool isModified = false;
            if (fieldInfo.FieldType.IsGenericList())
                isModified = UpdateList(component, fieldInfo, findCompAtt);
            else if (fieldInfo.FieldType.IsArray)
                isModified = UpdateArray(component, fieldInfo, findCompAtt);
            else
                isModified = UpdateSingle(component, fieldInfo, findCompAtt);

            if (isModified)
                EditorUtility.SetDirty(component);
        }

        private static Predicate<UnityObject> GetPredicateViaName(MonoBehaviour component, string predicateName)
        {
            if (string.IsNullOrEmpty(predicateName)) return null;
            return ReflectionUtil.ConvertMethodNameToFunc<UnityObject, bool>(component, predicateName).ConvertToPredicate();
        }

        //private static Action<object> GetAssignedAction(MonoBehaviour component, string actionName)
        //{
        //    if (string.IsNullOrEmpty(actionName)) return null;
        //    return ReflectionUtil.ConvertNameToAction<object>(component, actionName);
        //} // FieldInfo.SetValue는 너무 느리고 Action.Invoke()는 너무 빨라서 Invoke를 아랫줄에 적어도 SetValue가 완료되기전에 Invoke가 되어서 문제였음. 
        // 최선책은 아래와 같이 느린 MonoBehaviour.Invoke("",0)로 변경
        private static void InvokeAssignedAction(MonoBehaviour component, string actionName)
        {
            if (!string.IsNullOrEmpty(actionName))
                component.Invoke(actionName, 0);
        }

        private static bool UpdateList(MonoBehaviour component, FieldInfo fieldInfo, _Root_FindCompAttribute findCompAttribute)
        {
            IList list = fieldInfo.GetValue(component) as IList;
            int listCnt = list?.Count ?? 0;
            if (findCompAttribute.isFindOnlyWhenNull && listCnt > 0 && !list.IsAllNull()) return false;

            Type elemType = fieldInfo.FieldType.IsGenericList() ? fieldInfo.FieldType.GetGenericArguments()[0] : fieldInfo.FieldType.GetElementType();


            UnityObject[] values = GetFindCompEvent(elemType, findCompAttribute).
                    findCompsFunc(component, elemType, findCompAttribute.isIncludeInactive, findCompAttribute.isFindIncludeMe, GetPredicateViaName(component, findCompAttribute.predicateName));

            int newValueLength = values.Length;

            if (ArrayUtil.ArrayEqualsByDifferentType(values,list))
            {
                return false;
            }

            if (list == null)
            {
                list = (IList)Activator.CreateInstance(fieldInfo.FieldType);
            }

            if (newValueLength < listCnt) //To Remove
            {
                for (int i = list.Count - 1; i >= newValueLength; --i)
                {
                    list.RemoveAt(i);
                }
            }
            else if (newValueLength > listCnt) //To Add
            {
                for (int i = listCnt; i < newValueLength; ++i)
                {
                    list.Add(values[i]);
                }
            }

            for (int i = 0; i < newValueLength; i++)
            {
                list[i] = values[i];
            }

            fieldInfo.SetValue(component, list);

            InvokeAssignedAction(component, findCompAttribute.assignedCallbackName);
            return true;
        }



        private static bool UpdateArray(MonoBehaviour component, FieldInfo fieldInfo, _Root_FindCompAttribute findCompAttribute)
        {
            Array array = fieldInfo.GetValue(component) as Array;
            int arrayLength = array?.Length ?? 0;
            if (findCompAttribute.isFindOnlyWhenNull && arrayLength > 0 && !array.IsAllNull()) return false;

            Type elemType = fieldInfo.FieldType.GetElementType();

            UnityObject[] values = GetFindCompEvent(elemType, findCompAttribute).findCompsFunc
                (component, elemType, findCompAttribute.isIncludeInactive, findCompAttribute.isFindIncludeMe, GetPredicateViaName(component, findCompAttribute.predicateName));

            int newValueLength = values.LengthSafe();

            if (ArrayUtil.ArrayEqualsByDifferentType(values, array))
            {
                return false;
            }

            array = null;
            array = Array.CreateInstance(elemType, newValueLength);

            for (int i = 0; i < newValueLength; i++)
            {
                array.SetValue(values[i], i);
            }

            fieldInfo.SetValue(component, array);

            InvokeAssignedAction(component, findCompAttribute.assignedCallbackName);

            return true;
        }

        private static bool UpdateSingle(MonoBehaviour component, FieldInfo fieldInfo, _Root_FindCompAttribute findCompAttribute)
        {
            var lastValue = fieldInfo.GetValue(component) as UnityObject;
            if (findCompAttribute.isFindOnlyWhenNull && !lastValue.IsNullOrMissing()) return false;

            var newValue = GetFindCompEvent(fieldInfo.FieldType, findCompAttribute).findCompFunc
                (component, fieldInfo.FieldType, findCompAttribute.isIncludeInactive, findCompAttribute.isFindIncludeMe, GetPredicateViaName(component, findCompAttribute.predicateName));

            if (newValue.IsNullOrMissing())
            {
                if (lastValue.IsNullOrMissing()) return false;
            }
            else
            {
                if (newValue.Equals(lastValue)) return false;
            }

            fieldInfo.SetValue(component, newValue);

            InvokeAssignedAction(component, findCompAttribute.assignedCallbackName);

            return true;
        }

        private static bool IsAllNull(this IList list)
        {
            int cnt = list.Count;
            for (int i = 0; i < cnt; i++)
            {
                if (list[i] != null && !list[i].Equals(null))
                {
                    return false;
                }
            }
            return true;
        }

        private delegate UnityObject FindCompEventHandler(MonoBehaviour component, Type fieldType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate);
        private delegate UnityObject[] FindCompsEventHandler(MonoBehaviour component, Type underlyingType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate);
        private readonly static UnityObject[] EmptyUnityObjs = new UnityObject[0] { };
        private readonly static UnityEngine.Component[] EmptyComponents = new Component[0] { };


        private static UnityObject GetCompFunc(MonoBehaviour component, Type fieldType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate) => predicate == null ? component.GetComponent(fieldType) : component.GetComponents(fieldType).Find(predicate);
        private static UnityObject[] GetCompsFunc(MonoBehaviour component, Type underlyingType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate) => predicate == null ? component.GetComponents(underlyingType) : component.GetComponents(underlyingType).FindAll(predicate);
        private static UnityObject GetCompInChildFunc(MonoBehaviour component, Type fieldType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate)
            => Core_GetCompsInChildFunc(false, component, fieldType, isIncludeInactive, isFindIncludeMe, predicate).FirstOrDefault();
        private static UnityObject[] GetCompsInChildFunc(MonoBehaviour component, Type underlyingType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate)
            => Core_GetCompsInChildFunc(true, component, underlyingType, isIncludeInactive, isFindIncludeMe, predicate);

        static Component[] Core_GetCompsInChildFunc(bool isArray, MonoBehaviour component, Type underlyingType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<Component> predicate)
        {
            if (underlyingType == null || (!underlyingType.IsInterface && !typeof(Component).IsAssignableFrom(underlyingType)))
            {
                return EmptyComponents;
            }

            Transform trf = component.transform;

            if(isFindIncludeMe && predicate == null)
            {
                if (isArray)
                {
                    return trf.GetComponentsInChildren(underlyingType, isIncludeInactive);
                }
                else
                {
                    var c = trf.GetComponentInChildren(underlyingType, isIncludeInactive);
                    return c != null ? new Component[1] { c } : EmptyComponents;
                }
            }

            var components = trf.GetComponentsInChildren(underlyingType, isIncludeInactive);

            if (predicate != null)
            {
                components = components.FindAll(predicate);
            }

            if (!isFindIncludeMe && components.Length > 0 && components[0].transform == trf)
            {
                var compList = components.ToList();
                compList.RemoveAt(0);
                components = compList.ToArray();
            }

            return components;
        }

        private static UnityObject GetCompInParentFunc(MonoBehaviour component, Type fieldType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate)
            => Core_GetCompsInParentFunc(false, component, fieldType, isIncludeInactive, isFindIncludeMe, predicate).FirstOrDefault();
        private static UnityObject[] GetCompsInParentFunc(MonoBehaviour component, Type underlyingType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate)
            => Core_GetCompsInParentFunc(true, component, underlyingType, isIncludeInactive, isFindIncludeMe, predicate);

        static Component[] Core_GetCompsInParentFunc(bool isArray, MonoBehaviour component, Type underlyingType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<Component> predicate)
        {
            if (underlyingType == null || (!underlyingType.IsInterface && !typeof(Component).IsAssignableFrom(underlyingType)))
            {
                return EmptyComponents;
            }

            Transform trf = component.transform;

            if (isFindIncludeMe && predicate == null)
            {
                if (isArray)
                {
                    return trf.GetComponentsInParent(underlyingType, isIncludeInactive);
                }
                else
                {
                    var c = trf.GetComponentInParent(underlyingType
#if UNITY_2021_2_OR_NEWER
, isIncludeInactive
#endif
                    );

                    return c != null ? new Component[1] { c } : EmptyComponents;
                }
            }

            var components = trf.GetComponentsInParent(underlyingType, isIncludeInactive);

            if (predicate != null)
            {
                components = components.FindAll(predicate);
            }

            if (!isFindIncludeMe && components.Length > 0 && components[0].transform == trf)
            {
                var compList = components.ToList();
                compList.RemoveAt(0);
                components = compList.ToArray();
            }

            return components;
        }

        private static UnityObject FindObjectFunc(MonoBehaviour component, Type fieldType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate) => FindUtil.FindObjectOfType_NonGeneric(fieldType, isIncludeInactive, true, predicate);
        private static UnityObject[] FindObjectsFunc(MonoBehaviour component, Type underlyingType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate) => FindUtil.FindObjectsOfType_NonGeneric(underlyingType, isIncludeInactive, true, predicate);

        private static UnityObject GetOrAddCompFunc(MonoBehaviour component, Type fieldType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate) => GetOrAddComp(component, fieldType);
        private static UnityObject[] GetOrAddCompsFunc(MonoBehaviour component, Type underlyingType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate) => new UnityObject[] { GetOrAddCompFunc(component, underlyingType, isIncludeInactive: true, isFindIncludeMe: isFindIncludeMe, null) };

        private static UnityObject GetOrAddComp(MonoBehaviour target, Type type)
        {
            var c = target.GetComponent(type);
            if (c.IsNullOrMissing())
            {
                typeof(Editor).PrintLogWithClassName($"[{nameof(MustRequiredCompAttribute)}] '{target.gameObject.name}'에 AddComponent<{type.Name}>(); 완료.", isBigFont: false, obj: target, isPreventOverlapMsg: true, logType: LogType.Log);
                c = target.gameObject.AddComponent(type);
            }
            return c;
        }

#region GameObject

        private static UnityObject GetGoFunc(MonoBehaviour component, Type fieldType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate) => component.gameObject;
        private static UnityObject[] GetGosFunc(MonoBehaviour component, Type underlyingType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate) => new UnityObject[] { component.gameObject };
        private static UnityObject GetGoInChildFunc(MonoBehaviour component, Type fieldType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate)
            => Core_GetGosInChildFunc(false, component, isIncludeInactive, isFindIncludeMe, predicate).FirstOrDefault();
        private static UnityObject[] GetGosInChildFunc(MonoBehaviour component, Type underlyingType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate)
            => Core_GetGosInChildFunc(true, component, isIncludeInactive, isFindIncludeMe, predicate);
        
        static UnityObject[] Core_GetGosInChildFunc(bool isArray, MonoBehaviour component, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate)
        {
            if (component.transform.childCount == 0) return EmptyUnityObjs;

            Predicate<GameObject> baseCondition;
            if (!isIncludeInactive) baseCondition = ((o) => o != null && o.activeSelf);
            else baseCondition = ((o) => o != null);

            Predicate<GameObject> findCondition;
            if (predicate != null) findCondition = ((o) => baseCondition.Invoke(o) && predicate.Invoke(o));
            else findCondition = baseCondition;

            var trf = component.transform;
            int childCnt = trf.childCount;

            var childList = new List<UnityObject>(isArray ? (childCnt + 1) : 1);

            if (isFindIncludeMe && findCondition.Invoke(trf.gameObject))
                childList.Add(trf.gameObject);

            for (int i = 0; i < childCnt; i++)
            {
                var o = trf.GetChild(i).gameObject;
                if (findCondition.Invoke(o))
                {
                    childList.Add(o);
                    if (!isArray)
                    {
                        break;
                    }
                }
            }

            return childList.ToArray();
        }

        private static UnityObject GetGoInParentFunc(MonoBehaviour component, Type fieldType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate)
            => Core_GetGosInParentFunc(false, component, isIncludeInactive, isFindIncludeMe, predicate).FirstOrDefault();

        private static UnityObject[] GetGosInParentFunc(MonoBehaviour component, Type underlyingType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate)
            => Core_GetGosInParentFunc(true, component, isIncludeInactive, isFindIncludeMe, predicate);

        static UnityObject[] Core_GetGosInParentFunc(bool isArray, MonoBehaviour component, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate)
        {
            if (component.transform.parent == null) return EmptyUnityObjs;
            var trf = component.transform;

            Predicate<GameObject> baseCondition;
            if (!isIncludeInactive) baseCondition = ((o) => o != null && o.activeSelf);
            else baseCondition = ((o) => o != null);

            Predicate<GameObject> findCondition;
            if (predicate != null) findCondition = ((o) => baseCondition.Invoke(o) && predicate.Invoke(o));
            else findCondition = baseCondition;

            var parents = new List<UnityObject>();

            if (isFindIncludeMe && findCondition.Invoke(trf.gameObject))
                parents.Add(trf.gameObject);

            Transform parentTrf = trf.parent;

            while (parentTrf != null)
            {
                if (findCondition.Invoke(parentTrf.gameObject))
                {
                    parents.Add(parentTrf.gameObject);
                    if (!isArray)
                    {
                        break;
                    }
                }
                parentTrf = parentTrf.parent;
            }

            return parents.ToArray();
        }

        private static UnityObject FindGoFunc(MonoBehaviour component, Type fieldType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate) => FindUtil.FindObjectOfType_New<GameObject>(isIncludeInactive, true, predicate);
        private static UnityObject[] FindGosFunc(MonoBehaviour component, Type underlyingType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate) => FindUtil.FindObjectsOfType_New<GameObject>(isIncludeInactive, true, predicate);

        private static UnityObject GetOrAddGoFunc(MonoBehaviour component, Type fieldType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate) => predicate(component.gameObject) ? component.gameObject : null;
        private static UnityObject[] GetOrAddGosFunc(MonoBehaviour component, Type underlyingType, bool isIncludeInactive, bool isFindIncludeMe, Predicate<UnityObject> predicate) => predicate(component.gameObject) ? new UnityObject[] { component.gameObject } : null;
#endregion

    }
}