#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;

using UnityEditor;

using UnityEngine;

using UnityObject = UnityEngine.Object;
namespace CWJ.AccessibleEditor
{
    public static partial class EditorGUI_CWJ
    {
        public delegate object DrawVariousTypeHandler(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0);

        public static DrawVariousTypeHandler GetArrayElemDrawVariousTypeDelegate(bool isArray, Type type, string name, object value)
        {
            bool tmp = false;
            Type elemType = (isArray ? OnArrayConstructor(type, new object[] { value }, ref tmp).elemType : OnListConstructor(type, new object[] { value }, ref tmp).elemType);
            return GetDrawVariousTypeDelegate(elemType);
        }

        private static Dictionary<Type, DrawVariousTypeHandler> VariousTypeDrawerDic = new Dictionary<Type, DrawVariousTypeHandler>()
        {
            { typeof(System.Object), NULL__DrawLabel_Exception },
            { typeof(bool), Delegate_GetBoolType },
            { typeof(byte), Delegate_GetByteType },
            { typeof(sbyte), Delegate_GetSByteType },
            { typeof(short), Delegate_GetShortType },
            { typeof(ushort), Delegate_GetUShortType },
            { typeof(int), Delegate_GetIntType },
            { typeof(uint), Delegate_GetUIntType },
            { typeof(long), Delegate_GetLongType },
            { typeof(ulong), Delegate_GetULongType },
            { typeof(float), Delegate_GetFloatType },
            { typeof(double), Delegate_GetDoubleType },
            { typeof(char), Delegate_GetCharType },
            { typeof(string), Delegate_GetStringType },
            { typeof(TimeSpan), Delegate_GetTimeSpanType },
            { typeof(DateTime), Delegate_GetDateTimeType },
            { typeof(Vector2), Delegate_GetVector2Type },
            { typeof(Vector2Int), Delegate_GetVector2IntType },
            { typeof(Vector3), Delegate_GetVector3Type },
            { typeof(Vector3Int),Delegate_GetVector3IntType },
            { typeof(Vector4),Delegate_GetVector4Type },
            { typeof(Quaternion),Delegate_GetQuaternionType },
            { typeof(Color),Delegate_GetColorType },
            { typeof(Bounds),Delegate_GetBoundsType },
            { typeof(BoundsInt),Delegate_GetBoundsIntType },
            { typeof(Rect),Delegate_GetRectType },
            { typeof(RectInt),Delegate_GetRectIntType },
            { typeof(LayerMask),Delegate_GetLayerMaskType },
            { TypeOfCoroutine, NULL__DrawLabel_Exception },
            { typeof(IEnumerator), NULL__DrawLabel_Exception }
        };

        public static DrawVariousTypeHandler GetDrawVariousTypeDelegate(Type type)
        {
            if (VariousTypeDrawerDic.TryGetValue(type, out var drawVariousTypeHandler))
            {
                return drawVariousTypeHandler;
            }

            drawVariousTypeHandler = null;

            // TODO : UnityEvent
            if (TypeOfUnityEventBase.IsAssignableFrom(type) || typeof(Delegate).IsAssignableFrom(type)) 
            {
                //
            }
            else
            {
                if (type.IsEnum)
                {
                    if (type.IsDefined(typeof(FlagsAttribute), true))
                        drawVariousTypeHandler = Delegate_GetFlagEnumType;
                    else
                        drawVariousTypeHandler = Delegate_GetEnumType;
                }
                else if (type.IsInterface)
                    drawVariousTypeHandler = Delegate_GetInterfaceType;
                else if (typeof(UnityObject).IsAssignableFrom(type))
                    drawVariousTypeHandler = Delegate_GetUnityObjectType;
                else if (type.IsArray)
                    drawVariousTypeHandler = Delegate_GetArrayType;
                else if (type.IsGenericList())
                    drawVariousTypeHandler = Delegate_GetListType;
                else if (type.IsClassOrStructOrTuple())
                    drawVariousTypeHandler = Delegate_GetClassOrStructType;
            }

            if (drawVariousTypeHandler == null)
                drawVariousTypeHandler = NULL__DrawLabel_Exception;

            VariousTypeDrawerDic.Add(type, drawVariousTypeHandler);

            return drawVariousTypeHandler;
        }


        #region DrawVariousTypeElement
        private static object Delegate_GetBoolType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.Toggle(name, (bool)lastValue);
        }
        private static object Delegate_GetByteType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.IntField(name, (byte)lastValue).ToByte();
        }
        private static object Delegate_GetSByteType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.IntField(name, (sbyte)lastValue).ToSByte();
        }
        private static object Delegate_GetShortType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.IntField(name, (short)lastValue).ToShort();
        }
        private static object Delegate_GetUShortType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.IntField(name, (ushort)lastValue).ToUShort();
        }
        private static object Delegate_GetIntType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.IntField(name, (int)lastValue);
        }
        private static object Delegate_GetUIntType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.LongField(name, (uint)lastValue).ToUInt();
        }
        private static object Delegate_GetLongType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.LongField(name, (long)lastValue);
        }
        private static object Delegate_GetULongType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.FloatField(name, (ulong)lastValue).ToULong();
        }
        private static object Delegate_GetFloatType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.FloatField(name, (float)lastValue);
        }
        private static object Delegate_GetDoubleType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.DoubleField(name, (double)lastValue);
        }
        private static object Delegate_GetCharType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.TextField(name, string.Empty + (char)lastValue);
        }
        private static object Delegate_GetStringType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.TextField(name, (string)lastValue);
        }
        private static object Delegate_GetTimeSpanType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            var prevValue = (TimeSpan)lastValue;
            return TimeSpan.TryParse(EditorGUILayout.TextField(name, prevValue.ToString()), out var newValue) ? newValue : prevValue;
        }
        private static object Delegate_GetDateTimeType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            var prevValue = (DateTime)lastValue;
            return DateTime.TryParse(EditorGUILayout.TextField(name, prevValue.ToString()), out var newValue) ? newValue : prevValue;
        }
        private static object Delegate_GetVector2Type(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.Vector2Field(name, (Vector2)lastValue);
        }
        private static object Delegate_GetVector2IntType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.Vector2IntField(name, (Vector2Int)lastValue);
        }
        private static object Delegate_GetVector3Type(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.Vector3Field(name, (Vector3)lastValue);
        }
        private static object Delegate_GetVector3IntType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.Vector3IntField(name, (Vector3Int)lastValue);
        }
        private static object Delegate_GetVector4Type(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.Vector4Field(name, (Vector4)lastValue);
        }
        private static object Delegate_GetQuaternionType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            Quaternion prevValue = (Quaternion)lastValue;
            Vector4 tmpValue = EditorGUILayout.Vector4Field(name, new Vector4(prevValue.x, prevValue.y, prevValue.z, prevValue.w));
            return new Quaternion(tmpValue.x, tmpValue.y, tmpValue.z, tmpValue.w);
        }
        private static object Delegate_GetColorType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.ColorField(name, (Color)lastValue);
        }
        private static object Delegate_GetBoundsType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.BoundsField(name, (Bounds)lastValue);
        }
        private static object Delegate_GetBoundsIntType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.BoundsIntField(name, (BoundsInt)lastValue);
        }
        private static object Delegate_GetRectType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.RectField(name, (Rect)lastValue);
        }
        private static object Delegate_GetRectIntType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.RectIntField(name, (RectInt)lastValue);
        }

        private static object Delegate_GetLayerMaskType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return DrawLayerMask(type, name, lastValue, ref isValueChangedViaCode);
        }

        private static object Delegate_GetEnumType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.EnumPopup(name, (Enum)lastValue);
        }

        private static object Delegate_GetFlagEnumType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return EditorGUILayout.EnumFlagsField(name, (Enum)lastValue);
        }

        private static object Delegate_GetInterfaceType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return DrawInterface(type, name, lastValue, ref isValueChangedViaCode);
        }

        private static object Delegate_GetUnityObjectType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return DrawUnityObject(type, name, lastValue, ref isValueChangedViaCode);
        }

        private static object Delegate_GetArrayType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return DrawArrayType(type, name, lastValue, ref isValueChangedViaCode, reflectObjInstanceID);
        }

        private static object Delegate_GetListType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return DrawListType(type, name, lastValue, ref isValueChangedViaCode, reflectObjInstanceID);
        }

        private static object Delegate_GetClassOrStructType(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            return DrawClassOrStructType(type, name, lastValue, ref isValueChangedViaCode, reflectObjInstanceID);
        }

        private static void DrawLabel_Exception(Type type, string name)
        {
            if (type == null) return;
            EditorGUILayout.LabelField($"{name} ({type.FullName} is not supported type)");
        }

        public static object NULL__DrawLabel_Exception(Type type, string name, object lastValue, ref bool isValueChangedViaCode, int reflectObjInstanceID = 0)
        {
            if (type == null) return lastValue;
            DrawLabel_Exception(type, name);
            return lastValue;
        }
#endregion
    }
}
#endif