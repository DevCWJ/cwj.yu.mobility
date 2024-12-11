using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityObject = UnityEngine.Object;

namespace CWJ.AccessibleEditor
{
#if UNITY_EDITOR

    public interface IConditionalAttEssential
    {
        (bool isEnabled, bool isConstantlyMatched, bool isPossitive) GetConstantResult();
        (bool isEnabled, bool isMatched, bool isPossitive) GetVariableResult();

    }

    public struct ConditionalAttributeData<T> : IConditionalAttEssential where T : _Root_ConditionalAttribute
    {
        public bool isEnabled;
        private bool IsPlayModeMatched()
        {
            return callSituation.Flags_Any(EPlayMode.Always)
                        || (Application.isPlaying ? callSituation.Flags_Any(EPlayMode.PlayMode) : callSituation.Flags_Any(EPlayMode.NotPlayMode));
        }
        private bool isConstantlyMatched;
        private bool forPredicateComparison;
        private EPlayMode callSituation;
        private Func<bool> predicate;

        public ConditionalAttributeData(MemberInfo memberInfo, object targetObj/*, out T attribute*/)
        {
            isEnabled = false;
            predicate = null; forPredicateComparison = false;
            isConstantlyMatched = false;
            callSituation = EPlayMode.Off;
            if (memberInfo != null)
            {
                T att = memberInfo.GetCustomAttribute<T>();

                //if (att == null)
                //{
                //    if (memberInfo.IsDefined(typeof(ReadonlyAttribute), true))
                //        att = memberInfo.GetCustomAttribute(typeof(ReadonlyAttribute), true) as T;
                //}


                //if (att == null)
                //{
                //    try
                //    {
                //        var memb = memberInfo.DeclaringType.GetField(memberInfo.Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                //        object[] atts = memb.GetCustomAttributes(true);
                //        att = atts[0] as T;
                //    }
                //    catch (Exception e)
                //    {
                //        Debug.LogError(e.ToString());
                //    }
                //} 나중에 고쳐야함...

                if (isEnabled = (att != null && !att.callSituation.Flags_Any(EPlayMode.Off)))
                {
                    callSituation = att.callSituation;
                    predicate = att.FindPredicate<T>(memberInfo, targetObj);
                    isConstantlyMatched = callSituation.Flags_Any(EPlayMode.Always) && predicate == null;
                    forPredicateComparison = att.boolForPredicateComparison;
                }
            }
        }
        
        public (bool isEnabled, bool isConstantlyMatched, bool isPossitive) GetConstantResult()
        {
            return (isEnabled, isConstantlyMatched, forPredicateComparison);
        }

        public bool IsMatched()
        {
            if (isEnabled)
            {
                if (isConstantlyMatched)
                {
                    return true;
                }
                else if (IsPlayModeMatched())
                {
                    return predicate == null || predicate.Invoke();
                }
            }
            return false;
        }

        public (bool isEnabled, bool isMatched, bool isPossitive) GetVariableResult()
        {
            return (isEnabled: isEnabled,
                    isMatched: IsMatched(),
                    isPossitive: forPredicateComparison);
        }
    }
#endif

    public class _Root_ConditionalAttribute : PropertyAttribute
    {
        public EPlayMode callSituation;
        public string predicateName;
        public bool boolForPredicateComparison;

        public _Root_ConditionalAttribute(EPlayMode callSituation, string predicateName, bool boolForPredicateComparison)
        {
            order = -444;

            this.boolForPredicateComparison = boolForPredicateComparison;
            this.callSituation = callSituation;
            this.predicateName = predicateName;
        }

#if UNITY_EDITOR
        public struct PredicateKey : IEqualityComparer<PredicateKey>
        {
            public object targetObj;
            public string predicateName;

            public PredicateKey(object targetObj, string predicateName)
            {
                this.targetObj = targetObj;
                this.predicateName = predicateName;
            }


            public bool Equals(PredicateKey x, PredicateKey y)
            {
                if (!x.predicateName.Equals(y.predicateName))
                    return false;
                return (x.targetObj == y.targetObj);
            }

            public int GetHashCode(PredicateKey obj)
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 31 + obj.targetObj.GetHashCode();
                    hash = hash * 31 + obj.predicateName.GetHashCode();
                    return hash;
                }
            }
        }
        static Dictionary<PredicateKey, Func<bool>> NameToPredicateDics = new Dictionary<PredicateKey, Func<bool>>();
        public Func<bool> FindPredicate<T>(MemberInfo memberInfo, object targetObj) where T : _Root_ConditionalAttribute
        {
            if (string.IsNullOrEmpty(predicateName) || targetObj.IsNullOrMissing()) return null;

            var predicateKey = new PredicateKey(targetObj, predicateName);
            Type targetObjType = predicateKey.targetObj?.GetType();
            if (targetObjType == null) return null;

            if (!NameToPredicateDics.TryGetValue(predicateKey, out Func<bool> predicate))
            {
                predicate = null;
                
                PropertyInfo propertyInfo = targetObjType.FindProperty(predicateName, inAllBaseType: true);
                if (propertyInfo != null && propertyInfo.GetMethod.ReturnType.Equals(typeof(bool))) //Property Get
                    predicate = (Func<bool>)propertyInfo.GetMethod.ConvertToDelegate(targetObj);

                if (predicate == null) // Smart Predicate
                {
                    if (propertyInfo != null)
                    {
                        predicate = GetSmartPredicateViaMemberInfo(propertyInfo, propertyInfo.PropertyType, propertyInfo.IsStatic(), targetObj);
                    }
                    else //Check fieldInfo
                    {
                        var fieldInfo = targetObjType.FindField(predicateName, inAllBaseType: true);
                        if (fieldInfo != null)
                            predicate = GetSmartPredicateViaMemberInfo(fieldInfo, fieldInfo.FieldType, fieldInfo.IsStatic, targetObj);
                    }
                }

                if (predicate == null) //MethodInfo 가져오는게 제일 비용이 커서 마지막에 둠. 자동 null체크
                {
                    MethodInfo methodInfo = targetObjType.FindMethodInfo<bool>(predicateName, inAllBaseType: true);
                    if (methodInfo != null && methodInfo.ReturnType.Equals(typeof(bool)))
                        predicate = (Func<bool>)methodInfo.ConvertToDelegate(targetObj);
                }

                if (predicate != null)
                {
                    NameToPredicateDics.Add(predicateKey, predicate);
                }
                else
                {
                    //Invalid PredicateName 
                    typeof(T).PrintLogWithClassName($"Not exists: {typeof(T).Name}'s parameter named {nameof(predicateName)}(:'{predicateName}')"
                                + ($"\nPlease add 'private bool {predicateName} => isBool;'\nor\n'[SerializeField] bool {predicateName};'\n").SetStyle(new UnityEngine.Color().GetCommentsColor(), isBold: true) + $" to script {(targetObj.GetType().Name).SetStyle(new UnityEngine.Color().GetClassNameColor(), isBold: true)}.cs" +
                                $"\nlocation:{targetObj.GetType().Name}.{memberInfo.Name}", obj: targetObj is UnityObject ? targetObj as UnityObject : null, logType: LogType.Error, isPreventOverlapMsg: true);
                }
            }

            //    typeof(T).PrintLogWithClassName($"Not exists: {typeof(T).Name}'s parameter named {nameof(predicateName)}(:'{predicateName}')"
            //    + ($"\nPlease add 'private bool {predicateName}() => isBool;'").SetStyle(new UnityEngine.Color().GetCommentsColor(), isBold: true) + $" to script {(targetObj.GetType().Name).SetStyle(new UnityEngine.Color().GetClassNameColor(), isBold: true)}.cs" +
            //    $"\nlocation:{targetObj.GetType().Name}.{memberInfo.Name}", obj: targetObj, logType: LogType.Error, isPreventOverlapMsg: true);
            //    return null;

            return predicate;
        }

        static Func<bool> GetSmartPredicateViaMemberInfo(FieldInfo fieldInfo, Type memberType, bool isStatic, object targetObj)
        {
            if (memberType == typeof(bool))
            {
                if (isStatic)
                    return () => (bool)fieldInfo.GetValue(null);
                else
                    return () => (bool)fieldInfo.GetValue(targetObj);
            }
            else if (memberType.IsClass/* || typeof(UnityObject).IsAssignableFrom(memberType)*/)
            {
                if (isStatic)
                    return () => !fieldInfo.GetValue(null).IsNullOrMissing();
                else
                    return () => !fieldInfo.GetValue(targetObj).IsNullOrMissing();
            }
            else if (memberType.IsArrayOrList())
            {
                if (isStatic)
                    return () => ((IList)fieldInfo.GetValue(null)).CountSafe() > 0;
                else
                    return () => ((IList)fieldInfo.GetValue(targetObj)).CountSafe() > 0;
            }
            return null;
        }

        static Func<bool> GetSmartPredicateViaMemberInfo(PropertyInfo propertyInfo, Type memberType, bool isStatic, object targetObj)
        {
            if (memberType == typeof(bool))
            {
                if (isStatic)
                    return () => (bool)propertyInfo.GetValue(null);
                else
                    return () => (bool)propertyInfo.GetValue(targetObj);
            }
            else if (memberType.IsClass/* || typeof(UnityObject).IsAssignableFrom(memberType)*/)
            {
                if (isStatic)
                    return () => !propertyInfo.GetValue(null).IsNullOrMissing();
                else
                    return () => !propertyInfo.GetValue(targetObj).IsNullOrMissing();
            }
            else if (memberType.IsArrayOrList())
            {
                if (isStatic)
                    return () => ((IList)propertyInfo.GetValue(null)).CountSafe() > 0;
                else
                    return () => ((IList)propertyInfo.GetValue(targetObj)).CountSafe() > 0;
            }
            return null;
        }
#endif
    }

    public static class AttributeUtil
    {
        public static HashSet<Type> GetAllRequireCompTypes(Type type)
        {
            var requireTypes = new HashSet<Type>();
            requireTypes.Add(type);
            foreach (var att in type.GetCustomAttributes(typeof(RequireComponent), true))
            {
                var requireCompAtt = (att as RequireComponent);
                if (requireCompAtt.m_Type0 != null)
                    requireTypes.AddRange(GetAllRequireCompTypes(requireCompAtt.m_Type0));
                if (requireCompAtt.m_Type1 != null)
                    requireTypes.AddRange(GetAllRequireCompTypes(requireCompAtt.m_Type1));
                if (requireCompAtt.m_Type2 != null)
                    requireTypes.AddRange(GetAllRequireCompTypes(requireCompAtt.m_Type2));
            };
            return requireTypes;
        }

        #region Reflection

        public static IEnumerable<FieldInfo> GetAllFields(object target, Func<FieldInfo, bool> predicate)
        {
            Type[] types = ReflectionUtil.GetAllBasePropertyTypes(target);
            int typeLength = types.Length;

            for (int i = typeLength - 1; i >= 0; i--)
            {
                IEnumerable<FieldInfo> fieldInfos = types[i]
                    .GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(predicate);

                foreach (var fieldInfo in fieldInfos)
                {
                    yield return fieldInfo;
                }
            }
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(object target, Func<PropertyInfo, bool> predicate)
        {
            Type[] types = ReflectionUtil.GetAllBasePropertyTypes(target);
            int typeLength = types.Length;

            for (int i = typeLength - 1; i >= 0; i--)
            {
                IEnumerable<PropertyInfo> propertyInfos = types[i]
                    .GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(predicate);

                foreach (var propertyInfo in propertyInfos)
                {
                    yield return propertyInfo;
                }
            }
        }

        public static IEnumerable<MethodInfo> GetAllMethods(object target, Func<MethodInfo, bool> predicate)
        {
            IEnumerable<MethodInfo> methodInfos = target?.GetType()?
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(predicate);

            return methodInfos;
        }

        public static PropertyInfo GetProperty(object target, string propertyName)
        {
            return GetAllProperties(target, p => p.Name.Equals(propertyName, StringComparison.InvariantCulture)).FirstOrDefault();
        }

        public static MethodInfo GetMethod(object target, string methodName)
        {
            return GetAllMethods(target, m => m.Name.Equals(methodName, StringComparison.InvariantCulture)).FirstOrDefault();
        }

        #endregion For Attribute

        [System.Flags]
        public enum NullCheckType
        {
            Nullable = 1 << 0,
            ValueType = 1 << 1,
            String = 1 << 2,
            Numeric = 1 << 3,
            Bool = 1 << 4,
            All = ~0
        }

        private delegate (bool isNull, string nullLog) GetNullInfoDelegate(Type fieldType, object value, bool hasLog, NullCheckType nullCheckType);

        private static GetNullInfoDelegate GetContextualNullInfoFunc(Type fieldType, NullCheckType nullCheckType)
        {
            if (fieldType.IsArray || fieldType.IsGenericList())
            {
                return _GetNullInfoArray;
            }
            if (nullCheckType.HasFlag(NullCheckType.String) && fieldType == typeof(string))
            {
                return _GetNullInfoString;
            }
            if (nullCheckType.HasFlag(NullCheckType.Bool) && fieldType == typeof(bool))
            {
                return _GetNullInfoBool;
            }
            if (nullCheckType.HasFlag(NullCheckType.Numeric) && fieldType.IsNumericType())
            {
                return _GetNullInfoNumeric;
            }
            if (nullCheckType.HasFlag(NullCheckType.ValueType) && fieldType.IsValueType)
            {
                return _GetNullInfoValueType;
            }
            if (nullCheckType.HasFlag(NullCheckType.Nullable))
            {
                return _GetNullInfoNullable;
            }
            return null;
        }

        private static (bool isNull, string nullLog) _GetNullInfoNullable(Type fieldType, object value, bool hasLog, NullCheckType nullCheckType)
        {
            if (value == null || value.Equals(null))
            {
                if (hasLog)
                {
                    return (true, "null");
                }
                else
                {
                    return (true, null);
                }
            }
            return (false, null);
        }

        private static (bool isNull, string nullLog) _GetNullInfoValueType(Type fieldType, object value, bool hasLog, NullCheckType nullCheckType)
        {
            if (Activator.CreateInstance(fieldType).Equals(value))
            {
                if (hasLog)
                {
                    return (true, "value type with default value");
                }
                else
                {
                    return (true, null);
                }
            }
            return (false, null);
        }

        private static (bool isNull, string nullLog) _GetNullInfoString(Type fieldType, object value, bool hasLog, NullCheckType nullCheckType)
        {
            if (string.IsNullOrEmpty((string)value))
            {
                if (hasLog)
                {
                    return (true, "string.Empty");
                }
                else
                {
                    return (true, null);
                }
            }
            return (false, null);
        }

        private static (bool isNull, string nullLog) _GetNullInfoNumeric(Type fieldType, object value, bool hasLog, NullCheckType nullCheckType)
        {
            if (float.Parse(value + "") == 0.0f)
            {
                if (hasLog)
                {
                    return (true, "0");
                }
                else
                {
                    return (true, null);
                }
            }
            return (false, null);
        }

        private static (bool isNull, string nullLog) _GetNullInfoBool(Type fieldType, object value, bool hasLog, NullCheckType nullCheckType)
        {
            if ((bool)value == false)
            {
                if (hasLog)
                {
                    return (true, "false");
                }
                else
                {
                    return (true, null);
                }
            }
            return (false, null);
        }

        private static (bool isNull, string nullLog) _GetNullInfoArray(Type fieldType, object value, bool hasLog, NullCheckType nullCheckType)
        {
            bool isArray = fieldType.IsArray;

            Type elemType = isArray ? fieldType.GetElementType() : fieldType.GetGenericArguments()[0];

            if (elemType != null)
            {
                bool isNull = true;

                IList list = (IList)value;
                string nullLog = "";
                if (list == null || list.Count == 0)
                {
                    nullLog = "empty " + (isArray ? "array" : "list");
                }
                else
                {
                    var getNullInfoFunc = GetContextualNullInfoFunc(elemType, nullCheckType);
                    (bool isNull, string nullLog) nullInfo = (true, "");
                    for (int i = 0; i < list.Count; i++)
                    {
                        nullInfo = getNullInfoFunc(elemType, list[i], hasLog, nullCheckType);
                        if (!nullInfo.isNull)
                        {
                            return (false, null);
                        }
                    }
                    nullLog = "has null elements (" + nullInfo.nullLog + ")";
                }

                if (isNull)
                {
                    return (true, hasLog ? nullLog : null);
                }
            }
            return (false, null);
        }

        private static (bool isNull, string nullLog) GetNullInfo(this FieldInfo field, object value, bool hasLog, NullCheckType nullCheckType)
        {
            Type fieldType = field.FieldType;

            return GetContextualNullInfoFunc(fieldType, nullCheckType)(fieldType, value, hasLog, nullCheckType);
        }

        public static bool IsNullWithErrorMsg(this FieldInfo field, object targetObj, out string nullLog, NullCheckType nullCheckType = NullCheckType.Nullable | NullCheckType.ValueType | NullCheckType.String)
        {
            var nullInfo = field.GetNullInfo(field.GetValue(targetObj), true, nullCheckType);
            nullLog = nullInfo.nullLog;
            return nullInfo.isNull;
        }

        public static bool IsNull(this FieldInfo field, object targetObj, NullCheckType nullCheckType = NullCheckType.Nullable | NullCheckType.ValueType | NullCheckType.String)
        {
            object value = field.GetValue(targetObj);
            return value == null ? true : field.GetNullInfo(value, false, nullCheckType).isNull;
        }

        public static T GetAttributeInTypes<T>(this Type[] types) where T : Attribute
        {
            Type t;
            return GetAttributeInTypes<T>(types, out t);
        }

        public static T GetAttributeInTypes<T>(this Type[] types, out Type type) where T : Attribute
        {
            type = null;
            for (int i = 0; i < types.Length; ++i)
            {
                T t = types[i].GetCustomAttribute<T>();
                if (t != null)
                {
                    type = types[i];
                    return t;
                }
            }
            return null;
        }

        public static T[] GetCustomAttributeArray<T>(this PropertyInfo propertyInfo) where T : Attribute
        {
            return propertyInfo.GetCustomAttributes<T>().ToArray();
        }

        public static T[] GetCustomAttributeArray<T>(this FieldInfo fieldInfo) where T : Attribute
        {
            return fieldInfo.GetCustomAttributes<T>(true).ToArray();
        }

        public static FieldInfo[] GetFieldsInAttribute<T>(this Type type
            , BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, bool inherit = true) where T : Attribute
        {
            return type.GetFields(bindingFlags).Where(field => field.IsDefined(typeof(T), inherit)).ToArray();
        }

        public static FieldInfo[] GetFieldsHasAnyAttributes(this Type type, Type[] attributeTypes
            , BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, bool inherit = true)
        {
            return type.GetFields(bindingFlags)
                .Where(field => field.HasAnyAttributes(attributeTypes)).ToArray();
        }

        public static bool HasAnyAttributes(this FieldInfo fieldInfo, Type[] attributeTypes)
        {
            return attributeTypes.IsExists(t => fieldInfo.IsDefined(t, true));
        }

        public static FieldInfo[] FindFieldsWithAttribute<T>(this FieldInfo[] fields, bool attributeInherit = true)
        {
            return fields.Where(field => field.IsDefined(typeof(T), attributeInherit)).ToArray();
        }

        public static PropertyInfo[] FindPropertiesWithAttribute<T>(this PropertyInfo[] properties, bool attributeInherit = true)
        {
            return properties.Where(property => property.IsDefined(typeof(T), attributeInherit)).ToArray();
        }

        public static MethodInfo[] FindMethodsWithAttribute<T>(this MethodInfo[] methods, bool attributeInherit = true)
        {
            return methods.Where(method => method.IsDefined(typeof(T), attributeInherit)).ToArray();
        }

        public enum EParameterColor
        {
            Null,
            White,
            Gray,
            Red,
            Pink,
            Orange,
            Yellow,
            Green,
            Blue,
            Indigo,
            Violet,
            Black
        }

        public static Color ToColor(this EParameterColor parameterColor)
        {
            switch (parameterColor)
            {
                case EParameterColor.Red:
                    return Color.red;

                case EParameterColor.Pink:
                    return new Color().GetPink();

                case EParameterColor.Orange:
                    return new Color().GetOrange();

                case EParameterColor.Yellow:
                    return new Color().GetYellow();

                case EParameterColor.Green:
                    return new Color().GetGreen();

                case EParameterColor.Blue:
                    return new Color().GetBlue();

                case EParameterColor.Indigo:
                    return new Color().GetIndigo();

                case EParameterColor.Violet:
                    return new Color().GetViolet();

                case EParameterColor.White:
                    return Color.white;

                case EParameterColor.Gray:
                    return Color.gray;

                case EParameterColor.Black:
                    return Color.black;

                default:
                    return Color.magenta;
            }
        }
    }
}