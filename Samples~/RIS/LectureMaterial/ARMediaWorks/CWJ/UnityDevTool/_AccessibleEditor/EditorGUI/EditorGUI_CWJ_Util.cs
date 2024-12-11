#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEditor;

using UnityObject = UnityEngine.Object;
using UnityEngine.Events;


namespace CWJ.AccessibleEditor
{
    public static partial class EditorGUI_CWJ
    {
        public static MethodInfo FindMethod<T>(this T unityEventBase, string name, object listener, PersistentListenerMode mode, System.Type argumentType) where T: UnityEventBase
        {
            Debug.LogError(unityEventBase.GetType().GetGenericArguments().Length+" "+unityEventBase.GetType().GenericTypeArguments.Length);
            switch (mode)
            {
                case PersistentListenerMode.EventDefined:
                    return UnityEventBase.GetValidMethodInfo(listener, name, unityEventBase.GetType().GenericTypeArguments);
                case PersistentListenerMode.Void:
                    return UnityEventBase.GetValidMethodInfo(listener, name, new System.Type[0]);
                case PersistentListenerMode.Object:
                    return UnityEventBase.GetValidMethodInfo(listener, name, new System.Type[1] { argumentType ?? typeof(UnityEngine.Object) });
                case PersistentListenerMode.Int:
                    return UnityEventBase.GetValidMethodInfo(listener, name, new System.Type[1] { typeof(int) });
                case PersistentListenerMode.Float:
                    return UnityEventBase.GetValidMethodInfo(listener, name, new System.Type[1] { typeof(float) });
                case PersistentListenerMode.String:
                    return UnityEventBase.GetValidMethodInfo(listener, name, new System.Type[1] { typeof(string) });
                case PersistentListenerMode.Bool:
                    return UnityEventBase.GetValidMethodInfo(listener, name, new System.Type[1] { typeof(bool) });
                default:
                    return (MethodInfo)null;
            }
        }

        public static UnityObject GetTargetObject(this SerializedProperty property)
        {
            return property.serializedObject.targetObject;
        }

        public static Type GetEnumerableType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments()[0];

            var iface = (from i in type.GetInterfaces()
                         where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                         select i).FirstOrDefault();

            if (iface == null)
                throw new ArgumentException("Does not represent an enumerable type.", "type");

            return GetEnumerableType(iface);
        }

        public static FieldInfo GetFieldInfo(this SerializedProperty property)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            FieldInfo _GetField(Type type, string path)
            {
                return type.GetField(path, bindingFlags);
            }

            var parentType = property.GetTargetObject().GetType();
            var splits = property.propertyPath.Split('.');
            var fieldInfo = _GetField(parentType, splits[0]);

            if (splits.Length <= 1)
            {
                return fieldInfo;
            }

            for (var i = 1; i < splits.Length; i++)
            {
                if (splits[i] == "Array")
                {
                    i += 2;
                    if (i >= splits.Length)
                        continue;

                    var type = fieldInfo.FieldType.IsArray
                        ? fieldInfo.FieldType.GetElementType()
                        : fieldInfo.FieldType.GetGenericArguments()[0];

                    fieldInfo = _GetField(type, splits[i]);
                }
                else
                {
                    fieldInfo = i + 1 < splits.Length && splits[i + 1] == "Array"
                        ? _GetField(parentType, splits[i])
                        : _GetField(fieldInfo.FieldType, splits[i]);
                }

                if (fieldInfo == null)
                    throw new Exception("Invalid FieldInfo. " + property.propertyPath);

                parentType = fieldInfo.FieldType;
            }

            return fieldInfo;
        }

        public static Type GetFieldType(this SerializedProperty property, bool isArrayListType = true)
        {
            var fieldInfo = property.GetFieldInfo();

            if (isArrayListType && property.isArray && property.propertyType != SerializedPropertyType.String)
            {
                if (fieldInfo.FieldType.IsArray) return fieldInfo.FieldType.GetElementType();
                else if (fieldInfo.FieldType.IsGenericList()) return fieldInfo.FieldType.GetGenericArguments()[0];
            }

            return fieldInfo.FieldType;
        }

        public static Type GetArrayOrListElementType(this Type listType)
        {
            if (listType.IsArray)
            {
                return listType.GetElementType();
            }
            else if (listType.IsGenericType && listType.GetGenericTypeDefinition() == typeof(List<>))
            {
                return listType.GetGenericArguments()[0];
            }
            return null;
        }


        //public static byte Clamp(this byte n, byte min, byte max)
        //{
        //    return n < min ? min : (n > max ? max : n);
        //}

        //public static sbyte Clamp(this sbyte n, sbyte min, sbyte max)
        //{
        //    return n < min ? min : (n > max ? max : n);
        //}

        //public static short Clamp(this short n, short min, short max)
        //{
        //    return n < min ? min : (n > max ? max : n);
        //}

        //public static ushort Clamp(this ushort n, ushort min, ushort max)
        //{
        //    return n < min ? min : (n > max ? max : n);
        //}

        public static int Clamp(this int n, int min, int max)
        {
            return n < min ? min : (n > max ? max : n);
        }

        public static byte ToByte(this int n) => (byte)n.Clamp(byte.MinValue, byte.MaxValue);
        public static sbyte ToSByte(this int n) => (sbyte)n.Clamp(sbyte.MinValue, sbyte.MaxValue);
        public static short ToShort(this int n) => (short)n.Clamp(short.MinValue, short.MaxValue);
        public static ushort ToUShort(this int n) => (ushort)n.Clamp(ushort.MinValue, ushort.MaxValue);

        //public static uint Clamp(this uint n, uint min, uint max)
        //{
        //    return n < min ? min : (n > max ? max : n);
        //}

        public static long Clamp(this long n, long min, long max)
        {
            return n < min ? min : (n > max ? max : n);
        }
        public static uint ToUInt(this long n) => (uint)n.Clamp(uint.MinValue, uint.MaxValue);

        //public static ulong Clamp(this ulong n, ulong min, ulong max)
        //{
        //    return n < min ? min : (n > max ? max : n);
        //}

        public static float Clamp(this float n, float min, float max)
        {
            return n < min ? min : (n > max ? max : n);
        }
        public static ulong ToULong(this float n) => (ulong)n.Clamp(ulong.MinValue, ulong.MaxValue);

        public static double Clamp(this double n, double min, double max)
        {
            return n < min ? min : (n > max ? max : n);
        }

        public static decimal Clamp(this decimal n, decimal min, decimal max)
        {
            return n < min ? min : (n > max ? max : n);
        }
    }
}
#endif