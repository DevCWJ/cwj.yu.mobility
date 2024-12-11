using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using System.Linq.Expressions;
#endif
using System.Reflection;
using System.Text;

using CWJ.Singleton.Core;

#if UNITY_EDITOR
using UnityEditor;
#endif

//using System.CodeDom;
//using Microsoft.CSharp;

using UnityEngine;
using UnityEngine.Events;

namespace CWJ
{
    public static class ReflectionUtil
    {
        #region Util

        //attribute에서 매개변수에서 받은 string함수이름을 함수로 전환하려고 만듬(attribute는 Func, Delegate를 받을수없으므로)
        public static Func<TParam1, TResult> ConvertMethodNameToFunc<TParam1, TResult>(object monoObj, string methodName)
        {
            if (string.IsNullOrEmpty(methodName)) return null;
            var methodInfos = monoObj.GetType().GetAllClassMethods(inAllBaseType: false, bindingFlags: BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
                                                    , predicate: m =>
                                                    {
                                                        if (!m.Name.Equals(methodName) || m.ReturnType != typeof(TResult)) return false;
                                                        var paramInfos = m.GetParameters();
                                                        int length = paramInfos?.Length ?? 0;
                                                        if (length != 1 || paramInfos[0].ParameterType != typeof(TParam1)) return false;
                                                        return true;
                                                    });
            if (methodInfos == null || methodInfos.Length == 0 || methodInfos[0] == null) return null;

            return (Func<TParam1, TResult>)methodInfos[0].ConvertToDelegate(monoObj);
            //return (Func<TParam1, TResult>)Delegate.CreateDelegate(typeof(Func<TParam1, TResult>), methodInfos[0]);
        }

        public static Predicate<T> ConvertToPredicate<T>(this Func<T, bool> func)
        {
            if (func == null) return null;
            return new Predicate<T>(func);
        }

        public static UnityAction ConvertToUnityAction(this MethodInfo methodInfo, object target)
        => ConvertToUnityAction(methodInfo.Name, target);

        public static UnityAction ConvertToUnityAction(string methodName, object target)
        {
            return (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), target, methodName, ignoreCase: false, throwOnBindFailure: true);
        }

        public static UnityAction<T0> ConvertToUnityAction<T0>(string methodName, object target)
        {
            return (UnityAction<T0>)Delegate.CreateDelegate(typeof(UnityAction<T0>), target, methodName, ignoreCase: false, throwOnBindFailure: true);
        }

#if UNITY_EDITOR
        public static Delegate ConvertToDelegate(this MethodInfo methodInfo, object targetObj)
        {
            if (methodInfo == null) return null;

            var paramTypes = methodInfo.GetParameters().Select(p => p.ParameterType);

            Func<Type[], Type> getMethodType;
            if (methodInfo.ReturnType.Equals(typeof(void))) // is Action
            {
                getMethodType = Expression.GetActionType;
            }
            else
            {
                getMethodType = Expression.GetFuncType;
                paramTypes = paramTypes.Concat(new[] { methodInfo.ReturnType });
            }

            if (methodInfo.IsStatic)
                return Delegate.CreateDelegate(getMethodType.Invoke(paramTypes.ToArray()), methodInfo);
            else
                return Delegate.CreateDelegate(getMethodType.Invoke(paramTypes.ToArray()), targetObj, methodInfo.Name);
        }
#else
        public static Delegate ConvertToDelegate(this MethodInfo methodInfo, object targetObj)
        {
            if (methodInfo == null) return null;

            var paramTypes = methodInfo.GetParameters().Select(p => p.ParameterType).ToArray();

            // Action과 Func을 구분하여 Delegate를 생성
            if (methodInfo.ReturnType == typeof(void)) // Action
            {
                return methodInfo.IsStatic
                    ? Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(paramTypes), methodInfo)
                    : Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(paramTypes), targetObj, methodInfo);
            }
            else // Func
            {
                var funcParamTypes = paramTypes.Concat(new[] { methodInfo.ReturnType }).ToArray();
                return methodInfo.IsStatic
                    ? Delegate.CreateDelegate(typeof(Func<>).MakeGenericType(funcParamTypes), methodInfo)
                    : Delegate.CreateDelegate(typeof(Func<>).MakeGenericType(funcParamTypes), targetObj, methodInfo);
            }
        }
#endif


        public static Action ConvertNameToAction(object monoObj, string methodName)
        {
            if (string.IsNullOrEmpty(methodName)) return null;
            var methodInfos = monoObj.GetType().GetAllClassMethods(inAllBaseType: false, bindingFlags: BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
                                                    , predicate: m =>
                                                    {
                                                        if (!m.Name.Equals(methodName)) return false;
                                                        var paramInfos = m.GetParameters();
                                                        int length = paramInfos?.Length ?? 0;
                                                        if (length != 0) return false;
                                                        return true;
                                                    });

            if (methodInfos == null || methodInfos.Length == 0 || methodInfos[0] == null) return null;

            return (Action)Delegate.CreateDelegate(typeof(Action), null, methodInfos[0]);
        }

        public static Action<TParam1> ConvertNameToAction<TParam1>(object monoObj, string methodName)
        {
            if (string.IsNullOrEmpty(methodName)) return null;
            var methodInfos = monoObj.GetType().GetAllClassMethods(inAllBaseType: false, bindingFlags: BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
                                                    , predicate: m =>
                                                    {
                                                        if (!m.Name.Equals(methodName)) return false;
                                                        var paramInfos = m.GetParameters();
                                                        int length = paramInfos?.Length ?? 0;
                                                        if (length != 1 || paramInfos[0].ParameterType != typeof(TParam1)) return false;
                                                        return true;
                                                    });
            if (methodInfos == null || methodInfos.Length == 0 || methodInfos[0] == null) return null;

            return (Action<TParam1>)Delegate.CreateDelegate(typeof(Action<TParam1>), methodInfos[0].IsStatic ? null : monoObj, methodInfos[0]);
        }

        public static bool IsStatic(this PropertyInfo property, bool isNonPublic = true) => property?.GetAccessors(isNonPublic)?[0].IsStatic ?? false;

        private static readonly Type _ObsoleteAttType = typeof(ObsoleteAttribute);

        public static bool IsObsolete(this MemberInfo member)
        {
            return Attribute.IsDefined(member, _ObsoleteAttType);
        }

        public static bool IsNumericType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsConst(this FieldInfo fieldInfo) => fieldInfo.IsLiteral && !fieldInfo.IsInitOnly;

        public static bool IsProtected(this FieldInfo fieldInfo) => !fieldInfo.IsPrivate && !fieldInfo.IsPublic && fieldInfo.IsFamily;

        public static bool IsReadonly(this FieldInfo fieldInfo) => fieldInfo.IsInitOnly;

        private static readonly Type _SerializeFieldAttType = typeof(SerializeField);
        private static readonly Type _SerializeRefAttType = typeof(SerializeReference);

        private static readonly Type _UnityObjType = typeof(UnityEngine.Object);

        public static bool IsSerializeField(this FieldInfo field) =>
            (!field.IsStatic && !field.IsNotSerialized)
            &&
            (
                field.IsDefined(_SerializeRefAttType)
               || (
                        (field.IsPublic || field.IsDefined(_SerializeFieldAttType))
                        && (field.FieldType.IsSerializable || field.FieldType.IsSubclassOf(_UnityObjType))
            )
            );

        private const string PropertyFieldName = "k__BackingField";
        public static bool IsAutoPropertyField(this FieldInfo fieldInfo) => fieldInfo.Name.EndsWith(PropertyFieldName);

        public const string NameOf_m_Script = "m_Script";
#if UNITY_EDITOR
        public static bool IsPropName_m_Script(this SerializedProperty prop) => prop.name.Equals(NameOf_m_Script);
#endif

        public const BindingFlags AllOfFieldsBindingFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Default | BindingFlags.Instance;

        /// <summary>
        /// 유니티 버전에 따라 Inspected field가 달라질수 있으므로 정확한 이름 파악은 <see cref="GetSerializedFieldNames(Type)"/> 를 사용하기.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inAllBaseType"></param>
        /// <param name="lastBaseType"></param>
        /// <returns></returns>
        public static FieldInfo[] GetSerializedFieldInfos(this Type type, bool inAllBaseType = true, Type lastBaseType = null)
        {
            IEnumerable<FieldInfo> fields = Enumerable.Empty<FieldInfo>();
            IEnumerable<Type> types = Enumerable.Empty<Type>();

            types = types.Append(type);

            var bindingFlags = AllOfFieldsBindingFlag;

            if (!inAllBaseType)
                bindingFlags |= BindingFlags.DeclaredOnly;

            if (inAllBaseType)
            {
                if (lastBaseType == null)
                    lastBaseType = typeof(MonoBehaviour);

                do
                {
                    types = types.Append(type = type.BaseType);
                }
                while (type != lastBaseType);
            }

            foreach (var t in types.Reverse())
            {
                fields = fields.Concat(t.GetFields(bindingFlags));
            }

            return fields.ToArray().GetSerializedFieldInfos();
        }

        public static FieldInfo[] GetSerializedFieldInfos(this FieldInfo[] fieldInfos)
        {
            return (from field in fieldInfos
                    where field.IsSerializeField()
                    select field).ToArray();
        }

#if UNITY_EDITOR
        /// <summary>
        /// It's more accurate than the <see cref="GetSerializedFieldInfos(Type, bool, Type)"/>.
        /// <para/>But it's only available in the unity editor. and slow
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string[] GetSerializedFieldNames(this Type type)
        {
            var comp = UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).GetRootGameObjects()[0].AddComponent(type);
            IEnumerable<string> fieldNames = Enumerable.Empty<string>();

            using (var obj = new SerializedObject(comp))
            {
                using (var iterator = obj.GetIterator())
                {
                    if (iterator.NextVisible(true))
                    {
                        do
                        {
                            fieldNames = fieldNames.Append(iterator.name);
                        }
                        while (iterator.NextVisible(false));
                    }
                }
            }
            GameObject.DestroyImmediate(comp);
            string[] fieldNameArray = fieldNames.ToArray();
            ArrayUtil.Remove(ref fieldNameArray, NameOf_m_Script);
            return fieldNameArray;
        }
#endif

        /// <summary>
        /// 테스트 필요
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static bool IsBaseField(this FieldInfo fieldInfo, bool ignoreNullExp = false)
        {
            if (fieldInfo == null)
            {
                if (ignoreNullExp)
                {
                    return false;
                }
                throw new NullReferenceException();
            }

            Type declaring = fieldInfo.DeclaringType;
            Type reflected = fieldInfo.ReflectedType;

            return declaring != reflected && declaring.IsAssignableFrom(reflected);
        }

        /// <summary>
        /// <paramref name="targetType"/> 클래스에서 선언된 Field인지
        /// </summary>
        /// <param name="member"></param>
        /// <param name="targetType"></param>
        /// <param name="ignoreNullExp"></param>
        /// <returns></returns>
        public static bool IsDeclaredTargetType<T>(this T member, Type targetType, bool ignoreNullExp = false) where T : MemberInfo
        {
            if (member == null || targetType == null)
            {
                if (ignoreNullExp)
                {
                    return false;
                }
                throw new NullReferenceException();
            }

            return targetType.Equals(member.DeclaringType);
        }


        private static readonly Type[] ValueTupleTypes = new Type[] { typeof(ValueTuple<>), typeof(ValueTuple<,>), typeof(ValueTuple<,,>), typeof(ValueTuple<,,,>),
                                                                 typeof(ValueTuple<,,,,>), typeof(ValueTuple<,,,,,>), typeof(ValueTuple<,,,,,,>), typeof(ValueTuple<,,,,,,,>)};
        public static bool IsValueTupleType(this Type type)
        {
            return type.IsGenericType
                    && ValueTupleTypes.IsExists(type.GetGenericTypeDefinition());
        }

        private static readonly Type[] TupleTypes = new Type[] { typeof(Tuple<>), typeof(Tuple<,>), typeof(Tuple<,,>), typeof(Tuple<,,,>),
                                                                 typeof(Tuple<,,,,>), typeof(Tuple<,,,,,>), typeof(Tuple<,,,,,,>), typeof(Tuple<,,,,,,,>)};
        public static bool IsTupleType(this Type type)
        {
            return type.IsGenericType
                    && TupleTypes.IsExists(type.GetGenericTypeDefinition());
        }

        public static object TupleCreateInstance(Type type, object target)
        {
            //ValueTuple타입은 null허용 x
            if (!type.IsTupleType()) return null;
            Type[] genericTypes = type.GetGenericArguments();

            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);
            object[] values = new object[fieldInfos.Length];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = (target == null ? null : fieldInfos[i].GetValue(target));
            }

            return type.GetConstructor(genericTypes)?.Invoke(values);
        }

        public const string SystemNameSpace = "System";
        public const string SystemNameSpaceDot = "System.";
        public const string Dot = ".";

        public static string[] ToCsFriendlyTypeNames(this Type[] systemTypes)
        {
            string[] typeNames = new string[systemTypes.Length];

#if !NET_STANDARD_2_0 && !NET_STANDARD_2_1
            //Cannot use C# Dotnet 2.0, 2.1 without Editor Folder
            using (var provider = new Microsoft.CSharp.CSharpCodeProvider())
            {
                for (int i = 0; i < systemTypes.Length; i++)
                {
                    if (string.Equals(systemTypes[i].Namespace, ReflectionUtil.SystemNameSpace))
                    {
                        string csFriendlyName = provider.GetTypeOutput(new System.CodeDom.CodeTypeReference(systemTypes[i]));
                        if (csFriendlyName.IndexOf(ReflectionUtil.Dot) == -1)
                        {
                            typeNames[i] = csFriendlyName;
                            continue;
                        }
                    }

                    typeNames[i] = systemTypes[i].Name;
                }
            }
#else
            Type csCodeProviderType = GetTypeForcibly("Microsoft.CSharp.CSharpCodeProvider");
            if (csCodeProviderType == null)
            {
                return systemTypes.ConvertAll(t => t.Name);
            }
            ConstructorInfo providerCtor = csCodeProviderType.GetConstructor(new Type[] { });
            object providerObj = providerCtor.Invoke(null);

            Type codeTypeRefType = GetTypeForcibly("System.CodeDom.CodeTypeReference");
            ConstructorInfo codeTypeRefCtor = codeTypeRefType.GetConstructor(new Type[] { typeof(Type) });
            for (int i = 0; i < systemTypes.Length; i++)
            {
                object codeTypeRefObj = codeTypeRefCtor.Invoke(new object[] { systemTypes[i] });
                typeNames[i] = InvokeMethodForcibly(providerObj, true, false, csCodeProviderType, "GetTypeOutput", new object[] { codeTypeRefObj }).ToString();
            }
#endif

            return typeNames;
        }

        public static string ToCsFriendlyTypeName(this Type systemType)
        {
            return ToCsFriendlyTypeNames(new Type[] { systemType })[0];
        }

        //Field만 취급
        /// <summary>
        /// class/struct의 field들을 원하는 형태의 string으로 변환
        /// <para>구조체 내부의 모든 field들을 쿼리로 전환할때 용이</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format"></param>
        /// <param name="isEnumAsInt"></param>
        /// <param name="ignoreNames"></param>
        /// <returns></returns>
        public static string ConvertFieldToCustomFormat(object obj, string format = "name = value, ", bool isEnumAsInt = true, params string[] ignoreNames)
        {
            Type type = obj?.GetType();
            if (type == null) return string.Empty;

            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);
            int fieldLength = fieldInfos?.Length ?? 0;
            if (fieldLength == 0) return string.Empty;

            int nameIndex = format.IndexOf("name");
            int valueIndex = format.IndexOf("value");

            string separator = format.Substring(valueIndex + 5, format.Length - (valueIndex + 5));

            bool isNameVisible = nameIndex >= 0;
            string nameValueSep = isNameVisible ? format.Substring(nameIndex + 4, valueIndex - (nameIndex + 4)) : string.Empty;

            Func<FieldInfo, string> toText = null;
            if (isEnumAsInt)
            {
                toText = (f) =>
                {
                    object value = f?.GetValue(obj);
                    return (isNameVisible ? f.Name + nameValueSep : string.Empty) + (f.FieldType.IsEnum ? ((int)Enum.Parse(f.FieldType, value.ToString())).ToString() : value.ToString());
                };
            }
            else
            {
                toText = (f) =>
                {
                    return (isNameVisible ? f.Name + nameValueSep : string.Empty) + f?.GetValue(obj) + string.Empty;
                };
            }

            return string.Join(separator, Array.ConvertAll(fieldInfos.FindAll((f) => !ignoreNames.IsExists(f.Name)), (f) => toText(f)));
        }

        public enum PropertyType
        {
            Get,
            Set,
            GetSet
        }

        public static PropertyType GetPropertyType(this PropertyInfo property)
        {
            if (property.GetSetMethod() == null)
            {
                if (property.GetMethod != null)
                {
                    return PropertyType.Get;
                }
                else if (property.SetMethod != null)
                {
                    return PropertyType.Set;
                }
            }
            return PropertyType.GetSet;
        }

        public enum EConvertType
        {
            Log,
            Script,
            Custom
        }

        /// <summary>
        /// class/struct의 field들을 정해진 형태의 string으로 변환
        /// <para>로그 확인 또는 Editor코드에서 스크립트를 동적으로 작성할때 편리</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <param name="indentBlankChar"></param>
        /// <param name="isColorText"></param>
        /// <returns></returns>
        public static string GetAllDataToText(string name, object obj, EConvertType convertType, string indentBlankChar = "null", bool isColorText = false)
        {
            if (convertType == EConvertType.Custom && indentBlankChar.Equals("null") && isColorText == false)
            {
                convertType = EConvertType.Script;
            }

            if (convertType == EConvertType.Log)
            {
                indentBlankChar = "    ";
            }
            else if (convertType == EConvertType.Script)
            {
                indentBlankChar = "\t";
                isColorText = false;
            }

            Type type = obj?.GetType();
            if (type == null) return string.Empty;
            string assignLine = isColorText ? type.Name.SetColor(new Color().GetClassNameColor()) : type.Name;
            assignLine = $"{assignLine} {name}";
            return ((isColorText ? assignLine.SetBold() : assignLine) + "\r\n" +
                _GetAllVariablesToText(obj, isColorText)).SetAutoInsertIndent(0, indentBlankChar);
        }

        static string _GetAllVariablesToText(object obj, bool isColorText)
        {
            Type type = obj?.GetType();
            if (type == null) return "null";

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;
            PropertyInfo[] propertyInfos = (type.IsValueTupleType() || type.IsTupleType()) ? new PropertyInfo[0] : type.GetProperties(bindingFlags);
            FieldInfo[] fieldInfos = type.GetFields(bindingFlags);

            Color typeColor = new Color().GetOrientalBlue();
            Color classColor = new Color().GetClassNameColor();
            Color enumColor = new Color().GetOrange();

            Func<Type, string> getColorText = (memberType) =>
            {
                if (isColorText)
                {
                    if (memberType.IsCustomComplex())
                    {
                        return memberType.Name.SetColor(classColor);
                    }
                    else if (memberType.IsEnum)
                    {
                        return memberType.Name.SetColor(enumColor);
                    }
                    return memberType.Name.SetColor(typeColor);
                }
                else
                {
                    return memberType.Name;
                }
            };

            Func<FieldInfo, string> getFieldAssignCode = (field) =>
            {
                string assignCode = getColorText(field.FieldType) + " " + field.Name;
                return isColorText ? assignCode.SetBold() : assignCode;
            };

            Func<PropertyInfo, string> getPropertyAssignCode = (property) =>
            {
                string assignCode = (getColorText(property.PropertyType) + $"({property.GetPropertyType()}) " + property.Name);
                return isColorText ? assignCode.SetBold() : assignCode;
            };

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine();

            for (int i = 0; i < propertyInfos.Length; i++)
            {
                if (propertyInfos[i].GetMethod == null) continue;

                string assignCode = getPropertyAssignCode(propertyInfos[i]);

                if (propertyInfos[i].PropertyType.IsCustomComplex())
                {
                    stringBuilder.AppendLine(assignCode + " = \r\n" + _GetAllVariablesToText(propertyInfos[i].GetValue(obj), isColorText));
                }
                else
                {
                    stringBuilder.AppendLine($"{assignCode} = {propertyInfos[i].GetValue(obj)};");
                }
            }

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                string assignCode = getFieldAssignCode(fieldInfo);
                if (fieldInfo.FieldType.IsCustomComplex())
                {
                    if (fieldInfo.FieldType.IsClassOrStructOrTuple())
                        stringBuilder.Append(assignCode + " = \r\n" + _GetAllVariablesToText(fieldInfo.GetValue(obj), isColorText));
                    else
                        stringBuilder.Append(assignCode + " = " + StringUtil.ToReadableString(fieldInfo.GetValue(obj)) + ";");
                }
                else
                {
                    stringBuilder.AppendLine($"{assignCode} = {fieldInfo.GetValue(obj)};");
                }
            }

            return "{" + stringBuilder.ToString().TrimEnd("\r\n") + "\r\n}";
        }

        public static object GetValue(this MemberInfo memberInfo, object obj)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(obj);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(obj);
                default:
                    throw new NotImplementedException();
            }
        }


        public static object GetDefaultValue(this ParameterInfo parameterInfo, Type validType = null)
        {
            if (parameterInfo == null) return null;
            if (parameterInfo.HasDefaultValue)
            {
                return parameterInfo.DefaultValue;
            }
            else
            {
                validType ??= TypeUtil.GetValidType(parameterInfo.ParameterType);

                // 배열 타입 처리
                if (validType.IsArray)
                {
                    var elementType = validType.GetElementType(); // 배열의 요소 타입 가져오기

                    if (elementType != null)
                    {
                        return Array.CreateInstance(elementType, 0); // 빈 배열 생성
                    }

                    return null;
                }

                // UnityEngine.Object 타입 및 자식 타입을 체크
                if (typeof(UnityEngine.Object).IsAssignableFrom(validType))
                {
                    return null; // UnityEngine.Object와 그 파생 클래스는 생성하지 않음
                }

                if (validType.IsValueType)
                {
                    return Activator.CreateInstance(validType);
                }

                if (validType == typeof(string))
                {
                    return null;
                }
                if (Nullable.GetUnderlyingType(validType) != null)
                {
                    return null; // Nullable 타입의 기본값은 null
                }

                // 추상 클래스나 인터페이스에 대한 기본값 처리
                if (validType.IsAbstract || validType.IsInterface)
                {
                    return null;
                }

                // 특수 타입 처리 (Action, Func, Predicate, Coroutine 등)
                if (validType == typeof(UnityEngine.Coroutine) ||
                    typeof(Delegate).IsAssignableFrom(validType) || // Action, Func, Predicate 포함
                    validType.FullName?.StartsWith("System.Func") == true ||
                    validType.FullName?.StartsWith("System.Action") == true)
                {
                    return null; // 특수 타입은 null로 처리
                }

                try
                {
                    return Activator.CreateInstance(validType);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"타입 {validType} :  기본 생성자가 없거나 인스턴스를 생성할 수 없음\n{ex.Message}");
                    // 기본 생성자가 없거나 인스턴스를 생성할 수 없는 경우 null 반환
                    return null;
                }
            }
        }

        /// <summary>
        /// UnityEngine 컴포넌트외의 직접 코딩한 Type들만 반환
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static Type GetCustomType(this MonoBehaviour comp)
        {
            Type type = comp?.GetType();
            return (type != null && !type.FullName.StartsWith(nameof(UnityEngine) + ".")) ? type : null;
        }

        public static Type GetCWJType(this MonoBehaviour comp)
        {
            Type type = comp?.GetType();
            return (type != null && type.FullName.StartsWith(nameof(CWJ) + ".")) ? type : null;
        }

        public static T[] GetAllInterfacesInAssembly<T>() where T : class
        {
            if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");

            T[] instances = (from t in Assembly.GetExecutingAssembly().GetTypes()
                             where !t.IsGenericType && t.IsClass && t.GetInterfaces().IsExists(typeof(T))
                                      && t.GetConstructor(Type.EmptyTypes) != null //생성자를 제한시켰을경우엔 찾지못함
                             select Activator.CreateInstance(t) as T).ToArray();

            return instances;
        }

        public static IEnumerable<TStruct> ToStructs<TStruct>(this object[] structObj) where TStruct : struct
        {
            //return objs.OfType<TStruct>();
            return structObj.Cast<TStruct>();
        }

        /// <summary>
        /// 바로 이전 메소드정보는 1
        /// </summary>
        /// <param name="prevIndex"></param>
        /// <param name="isWithClassName"></param>
        /// <returns></returns>
        public static string GetPrevMethodName(int prevIndex = 1, bool isWithClassName = true)
        {
            string prevMethodName = new System.Diagnostics.StackFrame(prevIndex, true).GetMethod().Name;

            return isWithClassName ? string.Concat(GetPrevClassName(prevIndex), ".", prevMethodName, "()") : prevMethodName;
        }

        public static Type GetPrevClassType(int prevIndex = 1)
        {
            return new System.Diagnostics.StackTrace().GetFrame(prevIndex).GetMethod().ReflectedType;
        }

        public static string GetPrevClassName(int prevIndex = 1)
        {
            return GetPrevClassType(prevIndex).Name;
        }

        public static string GetPrevClassFullName(int prevIndex = 1)
        {
            return GetPrevClassType(prevIndex).FullName;
        }

        public static MethodInfo ConvertToMethodInfo(this UnityEngine.Events.UnityEventBase unityEvent, int index)
        {
            if (unityEvent == null) return null;
            Type type = unityEvent.GetPersistentTarget(index)?.GetType();

            return type?.GetMethod(unityEvent.GetPersistentMethodName(index));

            //아래와 같은 방법도있지만 이 경우 static이면 못불러옴
            //MethodInfo methodInfo = UnityEventBase.GetValidMethodInfo(VersionManager.Instance.Static_EventAfterBuild.GetPersistentTarget(0), VersionManager.Instance.Static_EventAfterBuild.GetPersistentMethodName(0),Type.EmptyTypes);
        }

        public static string GetClassAndMethodName(this UnityEngine.Events.UnityEvent unityEvent, int index)
        {
            if (unityEvent == null || unityEvent.GetPersistentEventCount() == 0 || unityEvent.GetPersistentEventCount() <= index)
            {
                return string.Empty;
            }

            return unityEvent.GetPersistentTarget(index)?.GetType()?.FullName + "." + unityEvent.GetPersistentMethodName(index);
        }

#if UNITY_EDITOR
        public static Type GetUnityEditorClassType(string typeName)
        {
            Type type = Type.GetType("UnityEditor." + typeName + ",UnityEditor");
            if (type != null)
            {
                return type;
            }

            return Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor." + typeName);
        }

#endif

        public static Type GetClassTypeForciblyInRuntime(string typeName)
        {
            Type type = Type.GetType(typeName);
            if (type != null)
            {
                return type;
            }

            AssemblyName[] referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();

            for (int i = 0; i < referencedAssemblies.Length; i++)
            {
                Assembly assembly = Assembly.Load(referencedAssemblies[i]);
                if (assembly != null)
                {
                    type = assembly.GetType(typeName);
                    if (type != null)
                        return type;
                }
            }

            return null;
        }

        public static Type GetTypeForcibly(string typeName)
        {
            Type type = Type.GetType(typeName);
            if (type != null)
            {
                return type;
            }

            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < assemblies.Length; ++i)
            {
                if (assemblies[i] == null) continue;

                type = Array.Find(assemblies[i].GetTypes(), (t) => (t != null && t.IsClass && t.FullName.Equals(typeName)));
                if (type != null)
                {
                    return type;
                }
            }

            if (type == null)
            {
                Debug.LogWarning("Couldn't found type");
            }
            return type;
        }

        /// <summary>
        /// <para>숨겨져있는 클래스(dll이나 Editor폴더 포함)의 메소드까지 클래스이름, 메소드이름 만으로 실행시킴</para>
        /// <para>non Static: instanceObj 필요/ Static: instanceObj -> null</para>
        /// <para>아직 parameters에 배열은 못넣음</para>
        /// isOverloading true를 하면 오버로딩된 메소드까지 고려한 코드가 실행되므로 진짜 필요할때만 쓰기
        /// </summary>
        /// <param name="classFullName"></param>
        /// <param name="methodName"></param>
        /// <param name="isOverloading"></param>
        /// <param name="instanceObj"></param>
        /// <param name="parameters"></param>
        public static object InvokeMethodForcibly(object instanceObj, bool isPublic, bool isOverloading, Type classType, string methodName, params object[] parameters)
        {
            if (classType == null) return null;

            if (isOverloading)
            {
                MethodInfo[] methodInfos = null;
                if (isPublic)
                {
                    methodInfos = classType.GetMethods().FindAll((method) => method != null && method.Name.Equals(methodName));
                }
                else
                {
                    methodInfos = classType.GetTypeInfo().GetDeclaredMethods(methodName).ToArray();
                }

                int paramLength = parameters?.Length ?? 0;

                for (int i = 0; i < methodInfos.Length; ++i)
                {
                    if (methodInfos[i] == null) continue;
                    if (methodInfos[i].GetParameters().Length == paramLength)
                    {
                        return methodInfos[i].Invoke(obj: instanceObj, parameters: parameters);
                    }
                }
                return null;
            }
            else
            {
                if (isPublic)
                {
                    return classType.GetMethod(methodName)?.Invoke(obj: instanceObj, parameters: parameters);
                }
                else
                {
                    return classType.GetTypeInfo().GetDeclaredMethod(methodName)?.Invoke(obj: instanceObj, parameters: parameters);
                }
            }
        }

        /// <summary>
        /// <para>className에 namespace까지 포함시켜서 적어야함</para>
        /// <para>숨겨져있는 클래스(dll이나 Editor폴더 포함)의 메소드까지 클래스이름, 메소드이름 만으로 실행시킴</para>
        /// <para>non Static: instanceObj 필요/ Static: instanceObj -> null</para>
        /// <para>아직 parameters에 배열은 못넣음</para>
        /// isOverloading true를 하면 오버로딩된 메소드까지 고려한 코드가 실행되므로 진짜 필요할때만 쓰기
        /// </summary>
        /// <param name="instanceObj"></param>
        /// <param name="isPublic"></param>
        /// <param name="isOverloading"></param>
        /// <param name="classFullName"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        public static void InvokeMethodForcibly(object instanceObj, bool isPublic, bool isOverloading, string classFullName, string methodName, params object[] parameters)
        {
            InvokeMethodForcibly(instanceObj, isPublic, isOverloading, GetTypeForcibly(classFullName), methodName, parameters: parameters);
        }

        /// <summary>
        /// <para>변수값 강제로 가져옴</para>
        /// non Static의 경우 instanceObj 필요
        /// </summary>
        /// <param name="className"></param>
        /// <param name="fieldName"></param>
        /// <param name="instanceObj"></param>
        /// <returns></returns>
        public static object GetValueForcibly(bool isPublic, string className, string fieldName, object instanceObj = null)
        {
            Type classType = GetTypeForcibly(className);

            if (isPublic)
            {
                return classType.GetField(fieldName).GetValue(instanceObj);
            }
            else //protected, private
            {
                return classType.GetTypeInfo().GetDeclaredField(fieldName).GetValue(instanceObj);
            }
        }

        public static void SetValueForcibly<T>(Component component, string fieldName, T value, bool isContainsCheck = false, bool onPrivate = true)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            if (onPrivate)
            {
                bindingFlags |= BindingFlags.NonPublic;
            }
            Type compType = component?.GetType();
            if (compType == null)
            {
                return;
            }
            if (isContainsCheck)
            {
                FieldInfo[] fieldInfos = compType.GetFields(bindingFlags).FindAll((f) => f.Name.Contains(fieldName));

                for (int i = 0; i < fieldInfos.Length; ++i)
                {
                    fieldInfos[i].SetValue(component, value);
                }
            }
            else
            {
                FieldInfo fieldInfo = compType.GetField(fieldName, bindingFlags);
                fieldInfo.SetValue(component, value);
            }
        }

        public static IEnumerable<FieldInfo> GetAllFields(Type t)
        {
            if (t == null)
            {
                return Enumerable.Empty<FieldInfo>();
            }

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                                 BindingFlags.Static | BindingFlags.Instance;
            return t.GetFields(flags).Concat(GetAllFields(t.BaseType));
        }

        //dll땡겨오기 테스트필요
        public static Type[] ConvertTypeForDll(this string typeName, bool referenced, bool gac)
        {
            //안될거는 거름
            if (string.IsNullOrEmpty(typeName) || (!referenced && !gac))
                return new Type[] { };

            List<string> assemblyFullnameList = new List<string>();
            List<Type> typeList = new List<Type>();

            if (referenced)
            {
                AssemblyName[] assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();

                for (int i = 0; i < assemblyNames.Length; ++i)
                {
                    Assembly assembly = Assembly.Load(assemblyNames[i].FullName);

                    Type type = assembly?.GetType(typeName, false, true);

                    if (type != null && !assemblyFullnameList.Contains(assembly.FullName))
                    {
                        typeList.Add(type);
                        assemblyFullnameList.Add(assembly.FullName);
                    }
                }
            }

            if (gac)
            {
                //GAC files
                string gacPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Windows) + "\\assembly";
                foreach (string file in GetAssemblyCacheFiles(gacPath))
                {
                    try
                    {
                        Assembly assembly = Assembly.ReflectionOnlyLoadFrom(file);
                        Type type = assembly?.GetType(typeName, false, true);

                        if (type != null && !assemblyFullnameList.Contains(assembly.FullName))
                        {
                            typeList.Add(type);
                            assemblyFullnameList.Add(assembly.FullName);
                        }
                    }
                    catch { }
                }
            }

            return typeList.ToArray();
        }

        //dll땡겨오기 테스트필요 ㅠ
        public static string[] GetAssemblyCacheFiles(string path)
        {
            List<string> files = new List<string>();

            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo fi in di.GetFiles("*.dll"))
            {
                files.Add(fi.FullName);
            }

            foreach (DirectoryInfo diChild in di.GetDirectories())
            {
                var files2 = GetAssemblyCacheFiles(diChild.FullName);
                files.AddRange(files2);
            }

            return files.ToArray();
        }


        #endregion Util

        #region Core

        public static Type[] GetAllBaseClassTypes(this Type targetType, Type finalBaseType = null)
        {
            if (targetType == null) return new Type[] { };

            if (finalBaseType == null) finalBaseType = typeof(UnityEngine.MonoBehaviour);

            List<Type> types = new List<Type>() { targetType };

            while (types.Last().BaseType != null)
            {
                Type baseType = types.Last().BaseType;

                if (baseType == finalBaseType || (baseType.IsGenericType && baseType.GetGenericTypeDefinition().Equals(typeof(SingletonCoreAbstract<>))))
                {
                    break;
                }
                types.Add(baseType);
            }
            return types.ToArray();
        }

        public static PropertyInfo FindProperty(this Type targetType, string fieldName, bool inAllBaseType = false
            , BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
            , Func<PropertyInfo, bool> predicate = null)
        {
            Type[] types = inAllBaseType ? GetAllBaseClassTypes(targetType: targetType) : new Type[] { targetType };

            if (inAllBaseType)
                bindingFlags |= BindingFlags.DeclaredOnly;

            int typeLength = types.Length;

            bool Find(PropertyInfo field)
            {
                return field.Name == fieldName;
            }

            bool FindWithPredicate(PropertyInfo field)
            {
                return field.Name == fieldName && predicate(field);
            }


            Func<PropertyInfo, bool> p;
            if (predicate != null)
                p = FindWithPredicate;
            else
                p = Find;

            for (int i = typeLength - 1; i >= 0; i--)
            {
                var fieldInfo = types[i].GetProperties(bindingFlags).FirstOrDefault(p);
                if (fieldInfo != null)
                    return fieldInfo;
            }

            return null;
        }

        public static FieldInfo FindField(this Type targetType, string fieldName, bool inAllBaseType = false
            , BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
            , Func<FieldInfo, bool> predicate = null)
        {
            Type[] types = inAllBaseType ? GetAllBaseClassTypes(targetType: targetType) : new Type[] { targetType };

            if (inAllBaseType)
                bindingFlags |= BindingFlags.DeclaredOnly;

            int typeLength = types.Length;

            bool Find(FieldInfo field)
            {
                return field.Name == fieldName;
            }

            bool FindWithPredicate(FieldInfo field)
            {
                return field.Name == fieldName && predicate(field);
            }

            Func<FieldInfo, bool> p;
            if (predicate != null)
                p = FindWithPredicate;
            else
                p = Find;

            for (int i = typeLength - 1; i >= 0; i--)
            {
                var fieldInfo = types[i].GetFields(bindingFlags).FirstOrDefault(p);
                if (fieldInfo != null)
                    return fieldInfo;
            }

            return null;
        }

        public static MethodInfo FindMethodInfo<TResult>(this Type targetType, string methodName, int paramCnt = 0, bool inAllBaseType = true
            , BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
            , Func<MethodInfo, bool> predicate = null)
        {
            if (string.IsNullOrEmpty(methodName)) return null;
            Type returnType = typeof(TResult);

            Type[] types = inAllBaseType ? GetAllBaseClassTypes(targetType: targetType) : new Type[] { targetType };

            if (inAllBaseType)
                bindingFlags |= BindingFlags.DeclaredOnly;

            int typeLength = types.Length;

            bool Find(MethodInfo m)
            {
                if (m == null || !m.Name.Equals(methodName) || m.ReturnType == null || m.ReturnType != returnType)
                {
                    return false;
                }
                var paramInfos = m.GetParameters();
                int pCnt = paramInfos == null ? 0 : paramInfos.Length;
                if (pCnt != paramCnt)
                {
                    return false;
                }
                return true;
            }

            bool FindWithPredicate(MethodInfo method)
            {
                return Find(method) && predicate(method);
            }

            Func<MethodInfo, bool> p;
            if (predicate != null)
                p = FindWithPredicate;
            else
                p = Find;

            for (int i = typeLength - 1; i >= 0; i--)
            {
                var methodInfo = types[i].GetMethods(bindingFlags).FirstOrDefault(p);
                if (methodInfo != null)
                    return methodInfo;
            }

            return null;
        }

        public static FieldInfo[] GetAllClassFields(this Type targetType, bool inAllBaseType = false, BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly,
                                                    Func<FieldInfo, bool> predicate = null)
        {
            Type[] types = inAllBaseType ? GetAllBaseClassTypes(targetType: targetType) : new Type[] { targetType };

            int typeLength = types.Length;
            List<FieldInfo> fieldInfoList = new List<FieldInfo>();

            if (predicate != null)
            {
                for (int i = typeLength - 1; i >= 0; i--)
                {
                    var fieldInfos = types[i].GetFields(bindingFlags).Where(predicate);

                    if (fieldInfos.Count() > 0)
                    {
                        fieldInfoList.AddRange(fieldInfos);
                    }
                }
            }
            else
            {
                for (int i = typeLength - 1; i >= 0; i--)
                {
                    FieldInfo[] fieldInfos = types[i].GetFields(bindingFlags);

                    if (fieldInfos.Length > 0)
                    {
                        fieldInfoList.AddRange(fieldInfos);
                    }
                }
            }

            return fieldInfoList.ToArray();
        }

        public static PropertyInfo[] GetAllClassProperties(this Type targetType, bool inAllBaseType = false, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly,
                                                            Func<PropertyInfo, bool> predicate = null)
        {
            Type[] types = inAllBaseType ? GetAllBaseClassTypes(targetType: targetType) : new Type[] { targetType };

            int typeLength = types.Length;

            List<PropertyInfo> propertyList = new List<PropertyInfo>();
            if (predicate != null)
            {
                for (int i = typeLength - 1; i >= 0; i--)
                {
                    var propertyInfos = types[i].GetProperties(bindingFlags).Where(predicate);

                    if (propertyInfos.Count() > 0)
                    {
                        propertyList.AddRange(propertyInfos);
                    }
                }
            }
            else
            {
                for (int i = typeLength - 1; i >= 0; i--)
                {
                    PropertyInfo[] propertyInfos = types[i].GetProperties(bindingFlags);

                    if (propertyInfos.Length > 0)
                    {
                        propertyList.AddRange(propertyInfos);
                    }
                }
            }

            return propertyList.ToArray();
        }

        public static MethodInfo[] GetAllClassMethods(this Type targetType, bool inAllBaseType = false, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public,
                                                            Func<MethodInfo, bool> predicate = null)
        {
            Type[] types = inAllBaseType ? GetAllBaseClassTypes(targetType: targetType) : new Type[] { targetType };

            if (inAllBaseType)
                bindingFlags |= BindingFlags.DeclaredOnly;

            int typeLength = types.Length;

            List<MethodInfo> methodList = new List<MethodInfo>();

            if (predicate != null)
            {
                for (int i = typeLength - 1; i >= 0; i--)
                {
                    var methodInfos = types[i].GetMethods(bindingFlags).Where(predicate);

                    if (methodInfos.Count() > 0)
                    {
                        methodList.AddRange(methodInfos);
                    }
                }
            }
            else
            {
                for (int i = typeLength - 1; i >= 0; i--)
                {
                    MethodInfo[] methodInfos = types[i].GetMethods(bindingFlags);

                    if (methodInfos.Length > 0)
                    {
                        methodList.AddRange(methodInfos);
                    }
                }
            }

            return methodList.ToArray();
        }

        public static Type[] GetAllBasePropertyTypes(object target)
        {
            Type targetType = target?.GetType();

            if (targetType == null) return new Type[] { };

            List<Type> types = new List<Type>() { targetType };

            while (types.Last().BaseType != null)
            {
                types.Add(types.Last().BaseType);
            }
            return types.ToArray();
        }


        /// <summary>
        /// Gets all fields from an object and its hierarchy inheritance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>All fields of the type.</returns>
        private static List<FieldInfo> GetAllFields(this Type type, BindingFlags flags)
        {
            // Early exit if Object type
            if (type == typeof(System.Object))
            {
                return new List<FieldInfo>();
            }

            // Recursive call
            var fields = type.BaseType.GetAllFields(flags);
            fields.AddRange(type.GetFields(flags | BindingFlags.DeclaredOnly));
            return fields;
        }

        /// <summary>
        /// Perform a deep copy of the class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>A deep copy of obj.</returns>
        /// <exception cref="System.ArgumentNullException">Object cannot be null</exception>
        public static T DeepCopy<T>(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }
            return (T)DoCopy(obj);
        }


        /// <summary>
        /// Does the copy.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Unknown type</exception>
        private static object DoCopy(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            // Value type
            var type = obj.GetType();
            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }

            // Array
            else if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                var array = obj as Array;
                Array copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(DoCopy(array.GetValue(i)), i);
                }
                return Convert.ChangeType(copied, obj.GetType());
            }

            // Unity Object
            else if (_UnityObjType.IsAssignableFrom(type))
            {
                return obj;
            }

            // Class -> Recursion
            else if (type.IsClass)
            {
                var copy = Activator.CreateInstance(obj.GetType());

                var fields = type.GetAllFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    var fieldValue = field.GetValue(obj);
                    if (fieldValue != null)
                    {
                        field.SetValue(copy, DoCopy(fieldValue));
                    }
                }

                return copy;
            }

            // Fallback
            else
            {
                throw new ArgumentException("Unknown type");
            }
        }
        #endregion Core

        #region TypeExtension


        public static bool IsComplex(this Type type)
        {
            return type != null && !type.IsValueType && type != typeof(string);
        }

        public static bool IsCustomComplex(this Type type)
        {
            return type != null && (type.GetCustomElementType()?.IsComplex() ?? false);
        }

        public static Type GetCustomElementType(this Type type, object value)
        {
            return value != null
                ? value.GetType().GetCustomElementType()
                : type?.GetCustomElementType();
        }

        public static Type GetCustomElementType(this Type type)
        {
            if (type.IsCollection())
            {
                return type.IsArray
                    ? type.GetElementType()
                    : type.GetGenericArguments()[0];
            }
            else
            {
                return type;
            }
        }

        public static bool IsCustomComplex(this Type type, object value)
        {
            return value != null
                ? value.GetType().IsCustomComplex()
                : type?.IsCustomComplex() ?? false;
        }

        public static bool IsCollection(this Type type)
        {
            var collectionTypeName = typeof(IEnumerable<>).Name;
            return (type.Name == collectionTypeName || type.GetInterface(typeof(IEnumerable<>).Name) != null ||
                    type.IsArray) && type != typeof(string);
        }

        public static bool HasDefaultConstructor(this Type type)
        {
            return type.IsValueType || type.GetConstructor(Type.EmptyTypes) != null;
        }
        #endregion TypeExtension

    }
}
