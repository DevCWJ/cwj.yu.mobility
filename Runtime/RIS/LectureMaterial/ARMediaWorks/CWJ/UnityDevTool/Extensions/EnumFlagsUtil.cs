
using System.Collections.Generic;
using System;
using System.Linq;

namespace CWJ
{
    public static partial class EnumUtil
    {
        public static IEnumerable<TEnum> GetHasFlags<TEnum>(TEnum input) where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
               .Where(f => input.HasFlag(f));
        }

        /// <summary>
        /// Enum을 다른 Enum으로
        /// </summary>
        //Enum 확정이면 그냥 (Enum)otherEnum 하기
        //public static TEnum ToOtherEnum<TEnum>(this Enum enumSource)
        //{
        //    return (TEnum)Enum.Parse(typeof(TEnum), enumSource.ToString(), true);
        //}
        public static bool IsObsolete<TEnum>(TEnum value) where TEnum : Enum
        {
            var attributes = (ObsoleteAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(ObsoleteAttribute), false);
            return (attributes != null && attributes.Length > 0);
        }

        public static object ToEnumsNumericType(Enum e)
        {
            if (e == null) return null;

            switch (e.GetTypeCode())
            {
                case TypeCode.SByte:
                    return Convert.ToSByte(e);

                case TypeCode.Byte:
                    return Convert.ToByte(e);

                case TypeCode.Int16:
                    return Convert.ToInt16(e);

                case TypeCode.UInt16:
                    return Convert.ToUInt16(e);

                case TypeCode.Int32:
                    return Convert.ToInt32(e);

                case TypeCode.UInt32:
                    return Convert.ToUInt32(e);

                case TypeCode.Int64:
                    return Convert.ToInt64(e);

                case TypeCode.UInt64:
                    return Convert.ToUInt64(e);

                default:
                    return null;
            }
        }

        private static object ToEnumsNumericType(ulong v, TypeCode code)
        {
            switch (code)
            {
                case TypeCode.Byte:
                    return (byte)v;

                case TypeCode.SByte:
                    return (sbyte)v;

                case TypeCode.Int16:
                    return (short)v;

                case TypeCode.UInt16:
                    return (ushort)v;

                case TypeCode.Int32:
                    return (int)v;

                case TypeCode.UInt32:
                    return (uint)v;

                case TypeCode.Int64:
                    return (long)v;

                case TypeCode.UInt64:
                    return v;

                default:
                    return null;
            }
        }



        public static T AddFlag<T>(this T e, T value) where T : struct, IConvertible
        {
            return (T)Enum.ToObject(typeof(T), Convert.ToInt64(e) | Convert.ToInt64(value));
        }

        public static T RemoveFlag<T>(this T e, T value) where T : struct, IConvertible
        {
            var x = Convert.ToInt64(e);
            var y = Convert.ToInt64(value);
            return (T)Enum.ToObject(typeof(T), x & ~(x & y));
        }

        public static T SetFlag<T>(this T e, T flag, bool isAdd) where T : struct, IConvertible
        {
            return isAdd ? e.AddFlag(flag) : e.RemoveFlag(flag);
        }

        public static T ReversalFlag<T>(this T e, T value) where T : struct, IConvertible
        {
            return (T)Enum.ToObject(typeof(T), Convert.ToInt64(e) ^ Convert.ToInt64(value));
        }

        public static bool IsNull<T>(this T e) where T : Enum
        {
            return Convert.ToInt64(e) == 0;
        }

        public static bool IsAll<T>(this T e) where T : Enum
        {
            return Convert.ToInt64(e) == ~0;
        }

        public static int Flags_ToIndex<T>(this T target) where T : Enum
        {
            if (target.IsNull())
            {
                return -1;
            }
            if (target.IsAll())
            {
                return -2;
            }
            return Array.IndexOf(Enum.GetValues(typeof(T)), target);
        }
        public static T Flags_GetByIndex<T>(int index) where T : Enum
        {
            return (T)(Enum.GetValues(typeof(T))).GetValue(index);
        }


        public static T[] Flags_ConvertToArray<T>(this T target) where T : Enum
        {
            if (target.IsNull())
            {
                return new T[0];
            }
            var values = Enum.GetValues(typeof(T));


            int skip = (Convert.ToInt64(values.GetValue(0)) == 0) ? 1 : 0;

            if (target.IsAll())
            {
                return values.Cast<T>().Skip(skip).ToArray();
            }
            else
            {
                return values.Cast<T>().Skip(skip)
                    .Where(e => target.HasFlag(e)).ToArray();
            }
        }

        /// <summary>
        /// Enum(Flags)에게 string.Contains와 같음
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Flags_Contains(this Enum target, Enum value)
        {
            return target == value || target.HasFlag(value);
        }
        /// <summary>
        /// Enum(Flags)에게 Linq의 Any와 같음
        /// <para/>
        /// 다중값(EMode.A | EMode.B)이 value로 들어갈시, 그중 하나라도 포함되는게 있는지 체크할때 사용할것
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Flags_Any(this Enum target, Enum value)
        {
            return (Convert.ToInt64(target) & Convert.ToInt64(value)) != 0;
        }
        //public static bool HasFlag(this Enum e, Enum value)
        //{
        //    long v = Convert.ToInt64(value);
        //    return (Convert.ToInt64(e) & v) == v;
        //}

        //public static bool HasFlag(this Enum e, ulong value)
        //{
        //    return (Convert.ToUInt64(e) & value) == value;
        //}

        //public static bool HasFlag(this Enum e, long value)
        //{
        //    return (Convert.ToInt64(e) & value) == value;
        //}



        public static bool HasAnyFlag(this Enum e, ulong value)
        {
            return (Convert.ToUInt64(e) & value) != 0;
        }

        public static bool HasAnyFlag(this Enum e, long value)
        {
            return (Convert.ToInt64(e) & value) != 0;
        }

        public static IEnumerable<Enum> EnumerateFlags(Enum e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var tp = e.GetType();
            ulong max = 0;
            foreach (var en in Enum.GetValues(tp))
            {
                ulong v = Convert.ToUInt64(en);
                if (v > max) max = v;
            }
            int loops = (int)Math.Log(max, 2) + 1;

            ulong ie = Convert.ToUInt64(e);
            for (int i = 0; i < loops; i++)
            {
                ulong j = (ulong)Math.Pow(2, i);
                if ((ie & j) != 0)
                {
                    var js = ToEnumsNumericType(j, e.GetTypeCode());
                    if (Enum.IsDefined(tp, js)) yield return (Enum)Enum.Parse(tp, js.ToString());
                }
            }
        }

        public static IEnumerable<T> EnumerateFlags<T>(T e) where T : struct, IConvertible
        {
            var tp = e.GetType();
            if (!tp.IsEnum) throw new ArgumentException("Type must be an enum.", "T");

            ulong max = 0;
            foreach (var en in Enum.GetValues(tp))
            {
                ulong v = Convert.ToUInt64(en);
                if (v > max) max = v;
            }
            int loops = (int)Math.Log(max, 2) + 1;

            ulong ie = Convert.ToUInt64(e);
            for (int i = 0; i < loops; i++)
            {
                ulong j = (ulong)Math.Pow(2, i);
                if ((ie & j) != 0)
                {
                    var js = ToEnumsNumericType(j, e.GetTypeCode());
                    if (Enum.IsDefined(tp, js))
                    {
                        yield return (T)js;
                    }
                }
            }
        }

        public static IEnumerable<Enum> GetUniqueEnumFlags(Type enumType)
        {
            if (enumType == null) throw new ArgumentNullException(nameof(enumType));
            if (!enumType.IsEnum) throw new ArgumentException("Type must be an enum.", nameof(enumType));

            foreach (Enum e in Enum.GetValues(enumType))
            {
                //var d = Convert.ToDecimal(e);
                //if (d > 0 && MathUtil.IsPowerOfTwo(Convert.ToUInt64(d))) yield return e as Enum;

                switch (e.GetTypeCode())
                {
                    case TypeCode.Byte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        if (MathUtil.IsPowerOfTwo(Convert.ToUInt64(e))) yield return e;
                        break;

                    case TypeCode.SByte:
                        {
                            sbyte i = Convert.ToSByte(e);
                            if (i == sbyte.MinValue || (i > 0 && MathUtil.IsPowerOfTwo((ulong)i))) yield return e;
                        }
                        break;

                    case TypeCode.Int16:
                        {
                            short i = Convert.ToInt16(e);
                            if (i == short.MinValue || (i > 0 && MathUtil.IsPowerOfTwo((ulong)i))) yield return e;
                        }
                        break;

                    case TypeCode.Int32:
                        {
                            int i = Convert.ToInt32(e);
                            if (i == int.MinValue || (i > 0 && MathUtil.IsPowerOfTwo((ulong)i))) yield return e;
                        }
                        break;

                    case TypeCode.Int64:
                        {
                            long i = Convert.ToInt64(e);
                            if (i == long.MinValue || (i > 0 && MathUtil.IsPowerOfTwo((ulong)i))) yield return e;
                        }
                        break;
                }
            }
        }
    }
}
