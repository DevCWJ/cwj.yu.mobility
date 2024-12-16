using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

using UnityEngine;
using System.Globalization;
using System.CodeDom;
//using Microsoft.CSharp;
//using System.CodeDom;

namespace CWJ
{
    public static class StringUtil
    {
        public static string ConvertToStrLength(string strData, int length, char fillChr)
        {
            return strData.PadLeft(length, fillChr);
        }

        public static string ConverToNumberStr(string numData, int length, char fillChr = '0')
        {
            return int.TryParse(numData, out int result) ? result.ToString("D" + length) : ConvertToStrLength(numData, length, fillChr);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="decimal"></param>
        /// <param name="leftLength"> -1 일땐 소수점앞자리 그대로 유지, 0일땐 앞숫자를 0으로 바꿈</param>
        /// <param name="rightLength"> -1 일땐 소수점뒷자리 그대로 유지, 0일땐 소수점없앰</param>
        /// <returns></returns>
        public static string ConvertToDecimalLength(float @decimal, int leftLength, int rightLength)
        {
            string numStr = @decimal.ToString();
            string[] splits = numStr.Contains(".") ? numStr.Split(".") : new string[2] { numStr, null };

            if (leftLength == 0)
                splits[0] = "0";
            else if (leftLength > 0)
                splits[0] = string.Format("{0:D" + leftLength + "}", int.Parse(splits[0]));

            if (rightLength == 0 || (rightLength == -1 && splits[1] == null))
            {
                return splits[0];
            }

            if (rightLength > 0)
                splits[1] = string.Format("{0:F" + rightLength + "}", @decimal).Split(".")[1];

            return string.Join(".", splits);
            //035.859600
            //***.******
        }

        public static byte[] HexStrToBytes(this string hexString)
        {
            if (hexString.Contains("-"))
                hexString = hexString.Replace("-", string.Empty);
            int length = hexString.Length / 2;
            byte[] bytes = new byte[length];
            char cTmp;
            for (int i = 0; i < length; i++)
            {
                cTmp = hexString[i * 2];
                bytes[i] = (byte)((cTmp < 0x40 ? cTmp - 0x30 : (cTmp < 0x47 ? cTmp - 0x37 : cTmp - 0x57)) << 4);
                cTmp = hexString[i * 2 + 1];
                bytes[i] += (byte)(cTmp < 0x40 ? cTmp - 0x30 : (cTmp < 0x47 ? cTmp - 0x37 : cTmp - 0x57));
            }

            return bytes;
        }

        /// <summary>
        /// faster than BitConverter
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="byteLength"></param>
        /// <param name="toLowerCase"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToHexStrWithLine(this IList<byte> bytes, int byteLength = -1, bool toLowerCase = false, char separator = '-')
        {
            byte addByte = 0x37;
            if (toLowerCase) addByte = 0x57;
            if (byteLength == -1)
                byteLength = bytes.Count;
            char[] cArr = new char[byteLength * 2 + (byteLength - 1)];
            byte bTmp;

            void update(int _i, byte _b)
            {
                bTmp = ((byte)(_b >> 4));
                cArr[_i * 3] = (char)(bTmp > 9 ? bTmp + addByte : bTmp + 0x30);
                bTmp = ((byte)(_b & 0xF));
                cArr[_i * 3 + 1] = (char)(bTmp > 9 ? bTmp + addByte : bTmp + 0x30);
            }
            update(0, bytes[0]);
            for (int i = 1; i < byteLength; ++i)
            {
                cArr[i * 3 - 1] = separator;
                update(i, bytes[i]);
            }

            return new string(cArr);
        }

        public static string ToHexStr(this IList<byte> bytes, int byteLength = -1, bool toLowerCase = false)
        {
            byte addByte = 0x37;
            if (toLowerCase) addByte = 0x57;
            if (byteLength == -1)
                byteLength = bytes.Count;
            char[] cArr = new char[byteLength * 2];
            byte bTmp;

            void update(int _i, byte _b)
            {
                bTmp = ((byte)(_b >> 4));
                cArr[_i * 2] = (char)(bTmp > 9 ? bTmp + addByte : bTmp + 0x30);
                bTmp = ((byte)(_b & 0xF));
                cArr[_i * 2 + 1] = (char)(bTmp > 9 ? bTmp + addByte : bTmp + 0x30);
            }

            for (int i = 0; i < byteLength; ++i)
            {
                update(i, bytes[i]);
            }

            return new string(cArr);
        }

        public static string GetRight(this string str, int n)
        {
            return str.Substring(str.Length - n);
        }

        public static string GetExtractNumber(this string str)
        {
            return new string(str.Where(Char.IsDigit).ToArray());
        }
        //public static string[] GetFriendlyTypeNames(this Type[] systemTypes)
        //{
        //    string[] typeNames = new string[systemTypes.Length];

        //    using (var provider = new CSharpCodeProvider())
        //    {
        //        for (int i = 0; i < systemTypes.Length; i++)
        //        {
        //            if (string.Equals(systemTypes[i].Namespace, "System"))
        //            {
        //                string csFriendlyName = provider.GetTypeOutput(new CodeTypeReference(systemTypes[i]));
        //                if (csFriendlyName.IndexOf('.') == -1)
        //                {
        //                    typeNames[i] = csFriendlyName;
        //                    continue;
        //                }
        //            }

        //            typeNames[i] = systemTypes[i].Name;
        //        }
        //    }

        //    return typeNames;
        //}

        //public static string GetFriendlyTypeName(this Type systemType)
        //{
        //    return GetFriendlyTypeNames(new Type[] { systemType })[0];
        //}

        public static string GetCodeFriendlyName(string name)
        {
            name = name.Replace('/', '_').Replace(' ', '_');

            if (char.IsLetter(name[0]) == false)
                name = "_" + name;

            for (int charIndex = 0; charIndex < name.Length; charIndex++)
            {
                if (char.IsLetterOrDigit(name[charIndex]) == false && name[charIndex] != '_')
                {
                    name = name.Remove(charIndex, 1);
                    name = name.Insert(charIndex, "_");
                }
            }

            return name;
        }

        /// <summary>
        /// 카멜에서 파스칼케이스로 변경할때
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CamelToPascalCase(this string str)
        {
            return char.ToUpperInvariant(str[0]) + str.Substring(1);
        }

        /// <summary>
        /// 띄어쓰기나 '_' 가 있는 문자를 파스칼표기법으로 변경할때 사용
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string str)
        {
            string pascalStr = str.ToLower().Replace("_", " ");
            var info = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
            pascalStr = info.ToTitleCase(pascalStr).Replace(" ", string.Empty);
            return pascalStr;
        }

        /// <summary>
        /// 띄어쓰기나 '_' 가 있는 문자를 카멜표기법으로 변경할때 사용
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string str)
        {
            string camelStr = str.ToPascalCase();
            camelStr = char.ToLowerInvariant(camelStr[0]) + camelStr.Substring(1);
            return camelStr;
        }

        public static bool Contains(this string str, char chr) => str.IndexOf(chr) >= 0;

        public static string RemoveCWJBehaviourName(this string name)
        {
            if (name.Contains("_Inspector") || name.Contains("_Window") || name.Contains("_ScriptableObject") || name.Contains("_Function"))
            {
                return name.Substring(0, name.LastIndexOf('_')).Replace('_', ' ');
            }
            else
            {
                return name.Replace('_', ' ');
            }
        }

        public static string BytesToString(this byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }

        public static byte[] StringToBytes(this string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        public static string ConvertStringToBytesForLog(this string text, out byte[] bytes)
        {
            bytes = text.StringToBytes();
            if (bytes.Length == 0) return "";
            string arrayLine = "\n\t{\n\t\t" + bytes[0];
            for (int i = 1; i < bytes.Length; i++)
            {
                arrayLine += ", " + bytes[i];
            }
            arrayLine += "\n\t};";

            return arrayLine;
        }

        public static string ReplaceStart(this string str, string oldStr, string newStr, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            return (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(oldStr) && str.StartsWith(oldStr, stringComparison)) ? (newStr + str.Substring(oldStr.Length, str.Length - oldStr.Length)) : str;
        }

        public static string RemoveStart(this string str, string removeStr, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            return ReplaceStart(str, removeStr, string.Empty, stringComparison);
        }

        public static string TrimStart(this string str, string removeStr, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            string lastStr;
            do
            {
                lastStr = str;
                str = lastStr.RemoveStart(removeStr, stringComparison);
            } while (!str.Equals(lastStr));

            return str;
        }

        public static string ReplaceEnd(this string str, string oldStr, string newStr, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            return (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(oldStr) && str.EndsWith(oldStr, stringComparison)) ? (str.Substring(0, str.Length - oldStr.Length) + newStr) : str;
        }

        public static string RemoveEnd(this string str, string removeStr, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            return ReplaceEnd(str, removeStr, string.Empty, stringComparison);
        }

        public static string TrimEnd(this string str, string removeStr, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            string lastStr;
            do
            {
                lastStr = str;
                str = lastStr.RemoveEnd(removeStr, stringComparison);
            } while (!str.Equals(lastStr));

            return str;
        }

        public static string AddStrEndIfNotExists(this string str, string addStr)
        {
            return !str.EndsWith(addStr) ? (str + addStr) : str;
        }

        public static string RemoveGenericMark(this string name)
        {
            return name.RemoveEnd("`1");
        }

        public static string GetNicifyVariableName(this string name)
        {
            name = name.RemoveCWJBehaviourName();
            name = name.RemoveGenericMark();
#if UNITY_EDITOR
            name = UnityEditor.ObjectNames.NicifyVariableName(name);
#endif
            return name;
        }

        public static bool ConvertToBool(this string str, bool defaultReturn)
        {
            bool value = false;
            try
            {
                value = bool.Parse(str);
            }
            catch (System.Exception e)
            {
                value = defaultReturn;
                Debug.LogError(str + " <string> -> <bool> 변환 오류\n" + e.ToString());
            }

            return value;
        }

        public static Vector3 ConvertToVector3(string sVector)
        {
            if (sVector.StartsWith("("))
            {
                if (sVector.EndsWith(")"))
                {
                    sVector = sVector.Substring(1, sVector.Length - 2);
                }
                else if (sVector.Contains(")"))
                {
                    sVector = sVector.RemoveStart("(");
                    var spl = sVector.Split(")");
                    sVector = spl[0];
                }
            }

            if (!sVector.Contains(","))
            {
                throw new InvalidCastException();
            }
            string[] sArray = sVector.Split(',');
            Vector3 resultVec = Vector3.zero;
            if (sArray.Length == 2)
            {
                resultVec = new Vector3(
    float.Parse(sArray[0]),
    float.Parse(sArray[1]), 0);
            }
            else if (sArray.Length == 3)
            {
                resultVec = new Vector3(
    float.Parse(sArray[0]),
    float.Parse(sArray[1]),
    float.Parse(sArray[2]));
            }
            return resultVec;
        }

        public static string ToStringByDetailed(this Vector2Int v)
        {
            return $"({v.x}, {v.y})";
        }

        public static string ToStringByDetailed(this Vector3Int v)
        {
            return $"({v.x}, {v.y}, {v.z})";
        }

        public static string ToStringByDetailed(this Vector2 v)
        {
            return $"({v.x}, {v.y})";
        }

        public static string ToStringByDetailed(this Vector3 v)
        {
            return $"({v.x}, {v.y}, {v.z})";
        }

        public static string ToStringByDetailed(this Vector4 v)
        {
            return $"({v.x}, {v.y}, {v.z}, {v.w})";
        }

        public static string ToStringByDetailed(this Quaternion q)
        {
            return $"({q.x}, {q.y}, {q.z}, {q.w})";
        }

        public static string ToStringByDetailed(this Color c)
        {
            return $"Color [R={c.r}, G={c.g}, B={c.b}, A={c.a}]";
        }

        public static string ToStringByDetailed(this Bounds b)
        {
            return $"Bounds [Center={b.center.ToStringByDetailed()}, Size={b.size.ToStringByDetailed()}";
        }
        public static string ToStringByDetailed(this BoundsInt b)
        {
            return $"BoundsInt [Center={b.center.ToStringByDetailed()}, Size={b.size.ToStringByDetailed()}";
        }

        public static string ToStringByDetailed(this Rect r)
        {
            return $"Rect [X/Y=({r.x}, {r.y}), W/H=({r.width}, {r.height})]";
        }
        public static string ToStringByDetailed(this RectInt r)
        {
            return $"RectInt [X/Y=({r.x}, {r.y}), W/H=({r.width}, {r.height})]";
        }

        public static string ToStringByDetailed(this Array array)
        {
            if (array == null)
                return "null";
            else
                return "Array(" + array.Length + ")\n{\n" + string.Join(",\n", array.Cast<object>().Select(o => "  " + ToReadableString(o)).ToArray()) + "\n}";
        }

        public static string ToStringByDetailed<T>(this T[,] array)
        {
            StringBuilder sb = new StringBuilder();

            int rows = array.GetLength(0); // 행의 개수
            int cols = array.GetLength(1); // 열의 개수

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sb.Append(array[i, j]);

                    // 각 요소 구분
                    if (j < cols - 1)
                    {
                        sb.Append(", "); // 열 사이의 구분자
                    }
                }
                sb.AppendLine(); // 행 구분자 (줄바꿈)
            }

            return sb.ToString();
        }

        public static string ToStringByDetailed<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            if (dict == null)
                return "null";
            else
                return "Dictionary\n{\n" + string.Join(",\n", dict.Select(kvp => "[" + kvp.Key.ToString() + "] " + ToReadableString(kvp.Value)).ToArray()) + "\n}";
        }

        /// <summary>
        /// 특정 문자 뒤 문자들을 제거해줌 (닉네임 #태그 제거에 적합)
        /// </summary>
        /// <param name="content"></param>
        /// <param name="deleteChar">제거할 문자(ex:#)</param>
        /// <returns></returns>
        public static string DeleteCharacterBack(this string content, Char deleteChar)
        {
            int index = content.LastIndexOf(deleteChar);
            return index == -1 ? content : content.Substring(0, index);
        }

        /// <summary>
        /// <para>문자의 모든 공백을 제거</para>
        /// </summary>
        public static string RemoveAllSpaces(this string str)
        {
            return str.Replace(" ", "");
        }

        public static string[] RemoveAllSpaces(this string[] strArray)
        {
            if (strArray != null)
            {
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (string.IsNullOrEmpty(strArray[i]))
                    {
                        continue;
                    }
                    strArray[i] = strArray[i].RemoveAllSpaces();
                }
            }
            return strArray;
        }

        public static string WithCalculateSpace(this string value, int maxLength)
        {
            if (maxLength < value.Length)
            {
                return value;
            }
            else
            {
                string returnValue = "";

                int length = maxLength - value.Length;
                for (int i = 0; i < length; i++)
                {
                    returnValue += " ";
                }
                return value + returnValue;
            }
        }

        public static string FirstLetterToUpperCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            var a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static int[] AllIndexesOf(this string str, string value)
        {
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes.ToArray();

                indexes.Add(index);
            }
        }

        public static int[] AllLastIndexesOf(this string str, string value)
        {
            List<int> indexes = new List<int>();
            for (int index = str.Length; ; index -= value.Length)
            {
                index = str.LastIndexOf(value, index);
                if (index == -1)
                    return indexes.ToArray();

                indexes.Add(index);
            }
        }

        public static string GetIndent(int indentLength)
        {
            return indentLength == 0 ? "" : new string('\t', indentLength);
        }

        public static string GetIndent(int indentLength, int blankSize)
        {
            return indentLength == 0 ? "" : new string(' ', blankSize * indentLength);
        }

        public static string SetIndent(this string str, int indentLength)
        {
            string indet = GetIndent(indentLength);
            return indet + str.Replace("\n", "\n" + indet);
        }

        public static string SetIndent(this string[] strs, int indentLength)
        {
            return string.Join("\n", strs).SetIndent(indentLength);
        }

        public static string SetAutoInsertIndent(this string str, int startIndentLength, string indentBlankChar = "\t")
        {
            string[] lines = str.Replace("\t", "").Split('\n');

            int openCnt = 0, closeCnt = 0;

            Func<int, string> getIndent = null;
            if (indentBlankChar == "\t")
            {
                getIndent = GetIndent;
            }
            else
            {
                getIndent = (size) => GetIndent(size, indentBlankChar.Length);
            }

            Action<int> setLine = (index) => { int offset = openCnt - closeCnt; if (offset > 0) lines[index] = lines[index].Insert(0, getIndent(offset)); };

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].TrimStart();
                if (string.IsNullOrEmpty(line)) continue;

                if (line[0].Equals('{'))
                {
                    setLine(i);
                    openCnt++;
                    line = "\n" + line;
                }
                else
                {
                    if (line[0].Equals('}'))
                    {
                        closeCnt++;
                        lines[i] += "\n";
                    }

                    setLine(i);
                }
            }

            return string.Join($"\n{getIndent(startIndentLength)}", lines);
        }

        public static string ToReadableString(object obj)
        {
            if (obj == null) return "null";

            Type type = obj.GetType();
            if (type == typeof(bool) || type.IsNumericType() || type == typeof(string))
            {
                return obj.ToString();
            }
            else if (type == typeof(TimeSpan))
            {
                return obj.ToString();
            }
            else if (type == typeof(DateTime))
            {
                return obj.ToString();
            }
            else if (type == typeof(Vector2))
            {
                return ((Vector2)obj).ToStringByDetailed();
            }
            else if (type == typeof(Vector2Int))
            {
                return ((Vector2Int)obj).ToStringByDetailed();
            }
            else if (type == typeof(Vector3))
            {
                return ((Vector3)obj).ToStringByDetailed();
            }
            else if (type == typeof(Vector3Int))
            {
                return ((Vector3Int)obj).ToStringByDetailed();
            }
            else if (type == typeof(Vector4))
            {
                return ((Vector4)obj).ToStringByDetailed();
            }
            else if (type == typeof(Quaternion))
            {
                return ((Quaternion)obj).ToStringByDetailed();
            }
            else if (type == typeof(Color))
            {
                return ((Color)obj).ToStringByDetailed();
            }
            else if (type == typeof(Bounds))
            {
                return ((Bounds)obj).ToStringByDetailed();
            }
            else if (type == typeof(BoundsInt))
            {
                return ((BoundsInt)obj).ToStringByDetailed();
            }
            else if (type == typeof(Rect))
            {
                return ((Rect)obj).ToStringByDetailed();
            }
            else if (type == typeof(RectInt))
            {
                return ((RectInt)obj).ToStringByDetailed();
            }
            else if (type.IsEnum)
            {
                return (obj as System.Enum).ToString();
            }
            else if (typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                return (obj as UnityEngine.Object).name;
            }
            else if (type == typeof(Array))
            {
                return (obj as Array).ToStringByDetailed();
            }
            else if (type.IsArray)
            {
                return ArrayOrListToDetailedString(type, obj, true);
            }
            else if (type.IsGenericList())
            {
                return ArrayOrListToDetailedString(type, obj, false);
            }
            else if (type.IsClassOrStructOrTuple())
            {
                return ReflectionUtil.GetAllDataToText("", obj, ReflectionUtil.EConvertType.Log, isColorText: false);
            }
            else
            {
                IFormattable formattable = obj as IFormattable;
                if (formattable != null)
                {
                    return formattable.ToString(null, CultureInfo.InvariantCulture);
                }

                Debug.LogError(type.Name + " is not supported type");
                return obj.ToString();
            }
        }

        public static string ToStringByDetailed<T>(this List<T> list)
        {
            return ArrayOrListToDetailedString(list.GetType(), list, false);
        }

        public static string ToStringByDetailed<T>(this T[] array)
        {
            return ArrayOrListToDetailedString(array.GetType(), array, true);
        }

        public static string ArrayOrListToDetailedString(Type type, object obj, bool isArray)
        {
            Type elemType = null;
            int length = 0;
            Func<int, object> getValue = null;

            if (isArray)
            {
                elemType = type.GetElementType();
                Array array = (obj as Array);
                if (array == null)
                {
                    array = Array.CreateInstance(elemType, 0);
                }
                length = array.Length;
                getValue = (i) => array.GetValue(i);
            }
            else
            {
                if (obj == null || obj.Equals(null))
                {
                    obj = Activator.CreateInstance(type);
                }

                elemType = type.GetGenericArguments()[0];
                IList list = (IList)obj;
                length = list.Count;
                getValue = (i) => list[i];
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{(isArray ? "Array" : "List")}<{elemType.Name}>({length})");
            stringBuilder.AppendLine("{");
            for (int i = 0; i < length; ++i)
            {
                stringBuilder.AppendLine("  [" + i + "] " + ToReadableString(getValue(i)) + $"{(i + 1 < length ? "," : string.Empty)}");
            }
            stringBuilder.Append("}");
            getValue = null;
            return stringBuilder.ToString();
        }

        #region Addon

        public enum Alignment
        {
            Left = 0,
            Right = 1,
            Center = 2
        }

        #region Constants

        public const string RX_OPEN_TO_CLOSE_PARENS = @"\(" +
                                                      @"[^\(\)]*" +
                                                      @"(" +
                                                      @"(" +
                                                      @"(?<Open>\()" +
                                                      @"[^\(\)]*" +
                                                      @")+" +
                                                      @"(" +
                                                      @"(?<Close-Open>\))" +
                                                      @"[^\(\)]*" +
                                                      @")+" +
                                                      @")*" +
                                                      @"(?(Open)(?!))" +
                                                      @"\)";

        public const string RX_OPEN_TO_CLOSE_ANGLES = @"<" +
                                                      @"[^<>]*" +
                                                      @"(" +
                                                      @"(" +
                                                      @"(?<Open><)" +
                                                      @"[^<>]*" +
                                                      @")+" +
                                                      @"(" +
                                                      @"(?<Close-Open>>)" +
                                                      @"[^<>]*" +
                                                      @")+" +
                                                      @")*" +
                                                      @"(?(Open)(?!))" +
                                                      @">";

        public const string RX_OPEN_TO_CLOSE_BRACKETS = @"\[" +
                                                        @"[^\[\]]*" +
                                                        @"(" +
                                                        @"(" +
                                                        @"(?<Open>\[)" +
                                                        @"[^\[\]]*" +
                                                        @")+" +
                                                        @"(" +
                                                        @"(?<Close-Open>\])" +
                                                        @"[^\[\]]*" +
                                                        @")+" +
                                                        @")*" +
                                                        @"(?(Open)(?!))" +
                                                        @"\]";

        public const string RX_UNESCAPED_COMMA = @"(?<!\\),";
        public const string RX_UNESCAPED_COMMA_NOTINPARENS = @"(?<!\\),(?![^()]*\))";

        #endregion Constants

        #region Matching

        public static bool IsNullOrEmpty(string value)
        {
            return System.String.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhitespace(string value)
        {
            if (System.String.IsNullOrEmpty(value)) return true;
            return value.Trim() == "";
        }

        public static bool Equals(string valueA, string valueB, bool isIgnoreCase)
        {
            return (isIgnoreCase) ? String.Equals(valueA, valueB) : String.Equals(valueA, valueB, StringComparison.OrdinalIgnoreCase);
        }

        public static bool Equals(string valueA, string valueB)
        {
            return Equals(valueA, valueB, false);
        }

        public static bool Equals(string value, params string[] others)
        {
            if ((others == null || others.Length == 0))
            {
                return String.IsNullOrEmpty(value);
            }

            foreach (var str in others)
            {
                if (value == str) return true;
            }

            return false;
        }

        public static bool Equals(string value, string[] others, bool isIgnoreCase)
        {
            if ((others == null || others.Length == 0))
            {
                return String.IsNullOrEmpty(value);
            }

            foreach (var sval in others)
            {
                if (StringUtil.Equals(value, sval, isIgnoreCase)) return true;
            }

            return false;
        }

        public static bool StartsWith(string value, string start)
        {
            return StartsWith(value, start);
        }

        public static bool StartsWith(string value, string start, bool isIgnoreCase)
        {
            return value.StartsWith(start, (isIgnoreCase) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        public static bool EndsWith(string value, string end)
        {
            return EndsWith(value, end, false);
        }

        public static bool EndsWith(string value, string end, bool isIgnoreCase)
        {
            return value.EndsWith(end, (isIgnoreCase) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        public static bool Contains(string str, params string[] values)
        {
            if (str == null || values == null || values.Length == 0) return false;

            foreach (var sother in values)
            {
                if (str.Contains(sother)) return true;
            }

            return false;
        }

        public static bool Contains(string str, bool ignorCase, string sother)
        {
            if (string.IsNullOrEmpty(str)) return string.IsNullOrEmpty(sother);
            if (sother == null) return false;

            if (ignorCase)
            {
                str = str.ToLower();
                if (str.Contains(sother.ToLower())) return true;
            }
            else
            {
                if (str.Contains(sother)) return true;
            }

            return false;
        }

        public static bool Contains(string str, bool ignorCase, params string[] values)
        {
            if (str == null || values == null || values.Length == 0) return false;

            if (ignorCase)
            {
                str = str.ToLower();
                foreach (var sother in values)
                {
                    if (str.Contains(sother.ToLower())) return true;
                }
            }
            else
            {
                foreach (var sother in values)
                {
                    if (str.Contains(sother)) return true;
                }
            }

            return false;
        }

        #endregion Matching

        #region Morphing

        public static string ToLower(string value)
        {
            return (value + "").ToLower();
        }

        public static string ToUpper(string value)
        {
            return (value + "").ToUpper();
        }

        public static string Trim(string value)
        {
            return value?.Trim();
        }

        public static string[] Split(string value, string delim)
        {
            return value?.Split(new string[] { delim }, StringSplitOptions.None);
        }

        public static string[] Split(string value, params char[] delim)
        {
            return value?.Split(delim);
        }

        public static string[] SplitFixedLength(string value, string delim, int len)
        {
            if (value == null) return new string[len];

            string[] arr = value.Split(new string[] { delim }, StringSplitOptions.None);
            if (arr.Length != len) Array.Resize(ref arr, len);
            return arr;
        }

        public static string[] SplitFixedLength(string value, char delim, int len)
        {
            if (value == null) return new string[len];

            string[] arr = value.Split(delim);
            if (arr.Length != len) Array.Resize(ref arr, len);
            return arr;
        }

        public static string[] SplitFixedLength(string value, char[] delims, int len)
        {
            if (value == null) return new string[len];

            string[] arr = value.Split(delims);
            if (arr.Length != len) Array.Resize(ref arr, len);
            return arr;
        }

        public static string EnsureLength(string str, int len, bool isPadWhiteSpace = false, Alignment align = Alignment.Left)
        {
            if (str.Length > len) str = str.Substring(0, len);

            if (isPadWhiteSpace) str = PadWithChar(str, len, align, ' ');

            return str;
        }

        public static string EnsureLength(string str, int len, char padChar, Alignment align = Alignment.Left)
        {
            if (str.Length > len) str = str.Substring(0, len);

            str = PadWithChar(str, len, align, padChar);

            return str;
        }

        public static string PadWithChar(string str, int length, Alignment align = 0, char chr = ' ')
        {
            if (chr == '\0') return null;

            switch (align)
            {
                case Alignment.Right:
                    return new String(chr, (int)Math.Max(0, length - str.Length)) + str;

                case Alignment.Center:
                    length = Math.Max(0, length - str.Length);
                    var sr = new String(chr, (int)(Math.Ceiling(length / 2.0f))); // if odd, pad more on the right
                    var sl = new String(chr, (int)(Math.Floor(length / 2.0f)));
                    return sl + str + sr;

                case Alignment.Left:
                    return str + new String(chr, (int)Math.Max(0, length - str.Length));
            }

            return str;
        }

        public static string PadWithChar(string str, int length, char alignChr, char chr = ' ')
        {
            switch (Char.ToUpper(alignChr))
            {
                case 'L':
                    return PadWithChar(str, length, Alignment.Left, chr);

                case 'C':
                    return PadWithChar(str, length, Alignment.Center, chr);

                case 'R':
                    return PadWithChar(str, length, Alignment.Right, chr);
            }

            return null;
        }

        #endregion Morphing

        #region Special Formatting

        private class LoDExtendedFormatter : ICustomFormatter, IFormatProvider
        {
            #region ICustomFormatter Interface

            public string Format(string formatString, object arg, IFormatProvider formatProvider)
            {
                const string REGX = @"^((p-?\d+)|(str)|(string)|(num\?)|(float)|(single)|(dbl)|(double)|(num)|(dec)|(cash)|(int)|(bool)|(ascii)|(char)|(date)|(time)):";

                try
                {
                    if (System.String.IsNullOrEmpty(formatString)) return Convert.ToString(arg);

                    var m = Regex.Match(formatString, REGX, RegexOptions.IgnoreCase);
                    if (m.Success)
                    {
                        string sType = m.Value.Substring(0, m.Value.Length - 1);
                        formatString = formatString.Substring(m.Length);
                        if (formatString.EndsWith("??"))
                        {
                            if (arg == null || (arg is string && StringUtil.IsNullOrWhitespace(arg as string)))
                                return "";
                            else
                                formatString = formatString.Substring(0, formatString.Length - 2);
                        }

                        arg = ConvertUtil.LoDConvertTo(arg, sType);
                    }
                    else
                    {
                        if (formatString.EndsWith("??"))
                        {
                            if (arg == null || (arg is string && StringUtil.IsNullOrWhitespace(arg as string)))
                                return "";
                            else
                                formatString = formatString.Substring(0, formatString.Length - 2);
                        }
                    }

                    if (arg is bool && formatString.Contains(";"))
                    {
                        //bool
                        string[] arr = StringUtil.Split(formatString, ";");
                        return (ConvertUtil.ToBool(arg)) ? arr[0] : arr[1];
                    }
                    else if (arg is string)
                    {
                        //string
                        string str = arg as String;
                        if (Regex.Match(formatString, @"^\<\d+$").Success)
                        {
                            int len = int.Parse(formatString.Substring(1));
                            return StringUtil.EnsureLength(str, len, true, Alignment.Left);
                        }
                        else if (Regex.Match(formatString, @"^\>\d+$").Success)
                        {
                            int len = int.Parse(formatString.Substring(1));
                            return StringUtil.EnsureLength(str, len, true, Alignment.Right);
                        }
                        else if (Regex.Match(formatString, @"^\^\d+$").Success)
                        {
                            int len = int.Parse(formatString.Substring(1));
                            return StringUtil.EnsureLength(str, len, true, Alignment.Center);
                        }
                        else
                        {
                            return System.String.Format("{0:" + formatString + "}", str);
                        }
                    }
                    else if (arg is TimeSpan && formatString.StartsWith("-?"))
                    {
                        //timespan with neg
                        formatString = formatString.Substring(2);
                        if ((TimeSpan)arg < TimeSpan.Zero)
                            return "-" + System.String.Format("{0:" + formatString + "}", arg);
                        else
                            return System.String.Format("{0:" + formatString + "}", arg);
                    }
                    else if (arg != null)
                    {
                        //all else
                        return System.String.Format("{0:" + formatString + "}", arg);
                    }
                    else
                    {
                        return "";
                    }
                }
                catch
                {
                }

                return "";
            }

            #endregion ICustomFormatter Interface

            #region FormatPrivider Interface

            public object GetFormat(Type formatType)
            {
                return this;
            }

            #endregion FormatPrivider Interface
        }

        private static LoDExtendedFormatter cachedFormatter = new LoDExtendedFormatter();

        public static string Format(string formatString, params object[] objects)
        {
            try
            {
                return String.Format(cachedFormatter, formatString, objects);
            }
            catch
            {
                throw new ArgumentException("format string syntax error");
            }
        }

        public static string FormatValue(object obj, string format)
        {
            return String.Format(cachedFormatter, "{0:" + format + "}", obj);
        }

        public static string[] FormatValues(object[] objs, string format)
        {
            if (objs == null) return null;
            if (objs.Length == 0) return new String[] { };

            string[] arr = new string[objs.Length];
            for (int i = 0; i < objs.Length; i++)
            {
                arr[i] = String.Format(cachedFormatter, "{0:" + format + "}", objs[i]);
            }
            return arr;
        }

        #endregion Special Formatting

        #region Replace Chars

        public static string EnsureNotStartWith(this string value, string start)
        {
            if (value.StartsWith(start)) return value.Substring(start.Length);
            else return value;
        }

        public static string EnsureNotStartWith(this string value, string start, bool ignoreCase)
        {
            if (value.StartsWith(start, StringComparison.OrdinalIgnoreCase)) return value.Substring(start.Length);
            else return value;
        }

        public static string EnsureNotStartWith(this string value, string start, bool ignoreCase, System.Globalization.CultureInfo culture)
        {
            if (value.StartsWith(start, ignoreCase, culture)) return value.Substring(start.Length);
            else return value;
        }

        public static string EnsureNotStartWith(this string value, string start, StringComparison comparison)
        {
            if (value.StartsWith(start, comparison)) return value.Substring(start.Length);
            else return value;
        }

        public static string EnsureNotEndsWith(this string value, string end)
        {
            if (value.EndsWith(end)) return value.Substring(0, value.Length - end.Length);
            else return value;
        }

        public static string EnsureNotEndsWith(this string value, string end, bool ignoreCase)
        {
            if (value.EndsWith(end, StringComparison.OrdinalIgnoreCase)) return value.Substring(0, value.Length - end.Length);
            else return value;
        }

        public static string EnsureNotEndsWith(this string value, string end, bool ignoreCase, System.Globalization.CultureInfo culture)
        {
            if (value.EndsWith(end, ignoreCase, culture)) return value.Substring(0, value.Length - end.Length);
            else return value;
        }

        public static string EnsureNotEndsWith(this string value, string end, StringComparison comparison)
        {
            if (value.EndsWith(end, comparison)) return value.Substring(0, value.Length - end.Length);
            else return value;
        }

        #endregion Replace Chars

        #endregion Addon
    }
}
