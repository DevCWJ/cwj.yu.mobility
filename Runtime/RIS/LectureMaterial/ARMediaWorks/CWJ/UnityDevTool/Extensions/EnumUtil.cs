using System;
using System.Collections.Generic;
//#if UNITY_EDITOR
//using System.Linq.Expressions;
//#endif
using UnityEngine;

#if !UNITY_WEBGL
using CWJ.EnumHelper;
#endif

namespace CWJ
{
    public static partial class EnumUtil
    {
        public static string ToString_Fast<TEnum>(this TEnum value)
            where TEnum : struct, Enum
        {
#if UNITY_WEBGL
            return value.ToString();
#else
            return FastEnum.GetName(value);
#endif
        }
        public static bool TryToEnum<TEnum>(this string enumName, out TEnum result, bool ignoreCase = false)
            where TEnum : struct, Enum
        {
#if UNITY_WEBGL
            return Enum.TryParse<TEnum>(enumName, ignoreCase, out result);
#else

            return FastEnum.TryParse(enumName, out result, ignoreCase);
#endif
        }
        /// <summary>
        /// TryToEnum 쓰는걸 추천
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumName"></param>
        /// <param name="isNoticeError"></param>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(this string enumName, bool isNoticeError = true) where TEnum : struct, Enum
        {
            if (isNoticeError && (string.IsNullOrEmpty(enumName) || !Enum.IsDefined(typeof(TEnum), enumName)))
            {
                return default(TEnum);
            }
#if UNITY_WEBGL
            return Enum.Parse<TEnum>(enumName);
#else
            return FastEnum.Parse<TEnum>(enumName);
#endif
        }

        public static TEnum ToEnum<TEnum>(this int enumIndex)
            where TEnum : struct, Enum
        {
#if UNITY_WEBGL
            return (TEnum)Enum.ToObject(typeof(TEnum), enumIndex);
#else
            return FastEnum.IntToEnums32NonCache<TEnum>(enumIndex);
#endif
        }

        public static bool EnumValueIsDefined(object value, Type enumType)
        {
            if (enumType == null) throw new ArgumentNullException(nameof(enumType));
            if (!enumType.IsEnum) throw new ArgumentException("Must be enum type.", nameof(enumType));
            if (value == null) return false;

            try
            {
                if (value is string)
                    return Enum.IsDefined(enumType, value);
                else if (ConvertUtil.IsNumeric(value))
                {
                    value = ConvertUtil.ToPrim(value, Type.GetTypeCode(enumType));
                    return Enum.IsDefined(enumType, value);
                }
            }
            catch
            {
            }

            return false;
        }
        public static bool CanConvertToEnum<TEnum>(this string enumName)
            where TEnum : struct, Enum
        {
            if (string.IsNullOrEmpty(enumName))
            {
                return false;
            }
#if UNITY_WEBGL
            return Enum.IsDefined(typeof(TEnum), enumName);
#else
            return FastEnum.IsDefined<TEnum>(enumName);
#endif
        }

        public static bool TryToEnum<TEnum>(this int enumIndex, out TEnum result)
            where TEnum : struct, Enum
        {
#if UNITY_WEBGL
            var enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));

            foreach (var value in enumValues)
            {
                if (Convert.ToInt32(value).Equals(enumIndex))
                {
                    result = value;
                    return true;
                }
            }
            result = default(TEnum);
            return false;
#else
            if (FastEnum.IsDefined<TEnum>(enumIndex))
            {
                result = FastEnum.IntToEnums32NonCache<TEnum>(enumIndex);
                return true;
            }
            result = default(TEnum);
            return false;
#endif
        }



        /// <summary>
        /// WebGL의 경우 캐싱해둘것
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static IReadOnlyList<string> GetNames<TEnum>() where TEnum : struct, Enum
        {
#if UNITY_WEBGL
            return Array.AsReadOnly(Enum.GetNames(typeof(TEnum)));
#else
            return FastEnum.GetNames<TEnum>();
#endif
        }

        public static IReadOnlyList<TEnum> GetEnumArray<TEnum>() where TEnum : struct, Enum
        {
#if UNITY_WEBGL
            return Array.AsReadOnly((TEnum[])Enum.GetValues(typeof(TEnum)));
#else
            return FastEnum.GetValues<TEnum>();
#endif
        }

        /// <summary>
        /// Enum 갯수 가져오기
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static int GetEnumLength<TEnum>() where TEnum : struct, Enum
        {
            return GetNames<TEnum>().Count;
        }


        public static int ToInt<T>(this T value)
            where T : struct, Enum
        {
#if UNITY_WEBGL
            // WEBGL을 위해 Expression.Compile()을 사용하지 않고, 단순히 byte로 캐스팅
            return Convert.ToInt32(value);
#else
            return FastEnum.Enum32ToIntNonCache(value);
#endif
        }
        public static byte ToByte<T>(this T value)
            where T : struct, Enum
        {
#if UNITY_WEBGL
            return Convert.ToByte(value);
#else
            return FastEnumExtensions.ToByte(value);
#endif
        }

        public static TEnum ToEnum<TEnum>(this object @object) where TEnum : struct, Enum
        {
            if (@object == null)
            {
                return default(TEnum);
            }

            if (@object is string)
            {
                if (TryToEnum((string)@object, out TEnum value))
                {
                    return value;
                }
            }
            else if (ConvertUtil.TryParseInt(@object, out int intObj))
            {
                if (TryToEnum(intObj, out TEnum val))
                {
                    return val;
                }
            }
            else
            {
                var o = ConvertUtil.ToPrim(@object, typeof(TEnum));
                if (o != null)
                {
                    return (TEnum)o;
                }
            }
            return default(TEnum);
        }

        public static int GetLength<TEnum>() where TEnum : struct, Enum
        {
            return GetNames<TEnum>().Count;
        }

        public static TEnum GetValidEnum<TEnum>(int index) where TEnum : struct, Enum
        {
            int length = GetLength<TEnum>();
            return index >= 0 ? (index % length).ToEnum<TEnum>() : (length - (Math.Abs(index) % length)).ToEnum<TEnum>();
        }

        public static TEnum NextEnum<TEnum>(this TEnum curEnum, int nextInterval = 1) where TEnum : struct, Enum
        {
            return GetValidEnum<TEnum>(curEnum.ToInt() + nextInterval);
        }

        public static TEnum PreviousEnum<TEnum>(this TEnum curEnum, int prevInterval = 1) where TEnum : struct, Enum
        {
            return GetValidEnum<TEnum>(curEnum.ToInt() - prevInterval);
        }


        //#if !UNITY_WEBGL
        //        public static short ToInt16<T>(this T value)
        //                    where T : struct, Enum
        //                    => FastEnumExtensions.ToInt16(value);
        //        public static sbyte ToSByte<T>(this T value)
        //            where T : struct, Enum
        //            => FastEnumExtensions.ToSByte(value);

        //        public static ushort ToUInt16<T>(this T value)
        //            where T : struct, Enum
        //            => FastEnumExtensions.ToUInt16(value);
        //        public static int ToInt32<T>(this T value)
        //                    where T : struct, Enum
        //                    => FastEnumExtensions.ToInt32(value);
        //        public static uint ToUInt32<T>(this T value)
        //                    where T : struct, Enum
        //                    => FastEnumExtensions.ToUInt32(value);
        //        public static long ToInt64<T>(this T value)
        //                    where T : struct, Enum
        //                    => FastEnumExtensions.ToInt64(value);
        //        public static ulong ToUInt64<T>(this T value)
        //                    where T : struct, Enum
        //                    => FastEnumExtensions.ToUInt64(value);
        //#endif

        //        public static class Cache<TEnum> where TEnum : struct, Enum
        //        {
        //            public static readonly Func<TEnum, int> ConvertInt = ConvertIntFunc();
        //            public static readonly Func<TEnum, byte> ConvertByte = ConvertByteFunc();

        //            private static Func<TEnum, int> ConvertIntFunc()
        //            {
        //                return enumValue => Convert.ToInt32(enumValue);
        //            }

        //            private static Func<TEnum, byte> ConvertByteFunc()
        //            {
        //                return enumValue => Convert.ToByte(enumValue);
        //            }

        //#if UNITY_EDITOR
        //            private static Func<TEnum, int> ConvertIntFunc()
        //            {
        //                ParameterExpression fromEnum = Expression.Parameter(typeof(TEnum), nameof(fromEnum));
        //                UnaryExpression convertChecked = Expression.ConvertChecked(fromEnum, typeof(int));
        //                return Expression.Lambda<Func<TEnum, int>>(convertChecked, fromEnum).Compile();
        //            }

        //            private static Func<TEnum, byte> ConvertByteFunc()
        //            {
        //                ParameterExpression fromEnum = Expression.Parameter(typeof(TEnum), nameof(fromEnum));
        //                UnaryExpression convertChecked = Expression.ConvertChecked(fromEnum, typeof(byte));
        //                return Expression.Lambda<Func<TEnum, byte>>(convertChecked, fromEnum).Compile();
        //            }
        //#endif
        //        }
    }
}