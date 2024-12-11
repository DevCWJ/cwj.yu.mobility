using System.Linq;
using System.Net.Mail;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace CWJ
{
    public static class RegexUtil
    {
        // 출처 : http://www.csharpstudy.com/Practical/Prac-validemail.aspx
        // 정규식 관련기술. 구글링 결과를 참고함
        //Regular Expression

        /// <summary>
        /// <para>문자의 모든 공백을 제거</para>
        /// <para>Regex.Replace보다는 string.Replace가 빠름</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveAllSpacesWithRegex(this string str)
        {
            return Regex.Replace(str, @"\s", "");
        }

        public static string[] RemoveAllSpacesWithRegex(this string[] strArray)
        {
            if (strArray != null)
            {
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (string.IsNullOrEmpty(strArray[i]))
                    {
                        continue;
                    }
                    strArray[i] = strArray[i].RemoveAllSpacesWithRegex();
                }
            }
            return strArray;
        }

        //public static string ExtractNumber(this string str)
        //{
        //    return Regex.Replace(str, @"\D", "");
        //}

        const string WithDigit = @"[0-9]";
        public static string GetOnlyLetter(this string str)
        {
            //return Regex.Replace(str, @"\d", string.Empty);
            return Regex.Replace(str, WithDigit, string.Empty);

        }
        const string WithoutDigit = @"[^0-9]";
        public static string GetOnlyNumber(string str)
        {
            if (str.LengthSafe() == 0)
            {
                return string.Empty;
            }
            return Regex.Replace(str, WithoutDigit, string.Empty);
        }

        //public const string EmailPattern = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
        public const string EmailPattern = @"^([0-9a-zA-Z]" + //Start with a digit or alphabetical
                                            @"([\+\-_\.][0-9a-zA-Z]+)*" + // No continuous or ending +-_. chars in email
                                            @")+" +
                                            @"@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$";

        public const string SpecialPattern = @"[^a-zA-Z\s\d\u3131-\u318E\uAC00-\uD7A3]";

        /// <summary>
        /// 이메일의 형식이 맞는지 확인
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string email)
        {
            return Regex.IsMatch(email, EmailPattern);
        }

        /// <summary>
        /// <para>문자열 길이 검사 기능</para>
        /// 최소값(min) 이상 최대값(max) 이하 길이인지 검사
        /// </summary>
        /// <param name="str"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool LengthIsAround(this string str, int min, int max= -1)
        {
            if (str == null && min > 0)
            {
                return false;
            }
            int length = str.Length;
            if (max < min)
                return min <= length;
            else
                return (min <= length) && (length <= max);
        }
        public static bool IsAround(this int num, int min, int max = -1)
        {
            if (max < min)
                return min <= num;
            else
                return (min <= num) && (num <= max);
        }
        public static bool IsAround(this float num, float min, float max = -1)
        {
            if (max < min)
                return min <= num;
            else
                return (min <= num) && (num <= max);
        }
        /// <summary>
        /// 특수문자를 포함하고있는지
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool HasSpecialCharacter(this string str)
        {
            return !str.Equals(Regex.Replace(str, SpecialPattern, string.Empty, RegexOptions.Singleline));
        }

        public static string SplitCamelCase(this string camelCaseString)
        {
            if (string.IsNullOrEmpty(camelCaseString)) return camelCaseString;

            string camelCase = Regex.Replace(Regex.Replace(camelCaseString, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
            string firstLetter = camelCase.Substring(0, 1).ToUpper();

            if (camelCaseString.Length > 1)
            {
                string rest = camelCase.Substring(1);

                return firstLetter + rest;
            }
            return firstLetter;
        }

        #region For InputField Validator
        public static bool CheckValidation_Email(string emailaddress, out string validateValue)
        {
            validateValue = emailaddress;
            if (string.IsNullOrEmpty(emailaddress))
            {
                return false;
            }
            try
            {
                var m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        //public static string GetValid_Password(string str, out string value)
        //{
        //    value = str.Trim();
        //    return string.Join(string.Empty, Enumerable.Repeat("*", value.Length - 1)) + value.Last();
        //}
        public static bool CheckValidation_Password(string inputStr, out string validateValue)
        {
            validateValue = inputStr;
            return validateValue != null && validateValue.Any(char.IsLetter) && validateValue.Any(char.IsDigit);
        }
        public static string GetValidateStr_Name(string inputStr)
        {
            return Regex.Replace(inputStr, @"[^a-zA-Zㄱ-ㅎ가-힣]", string.Empty);
        }
        
        //@"^(19|20)\d{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[0-1])$"
        public static string GetValidateStr_YYYYMMDD(string inputStr)
        {
            return Regex.Replace(inputStr, @"^(19|20)\d{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[0-1])$", string.Empty);
        }

        public static string GetValidateStr_AccountId(string inputStr)
        {
            return Regex.Replace(inputStr, @"[^0-9a-zA-Z]", string.Empty);
        }
        public static string GetValidateStr_Password(string inputStr)
        {
            return Regex.Replace(inputStr, @"[ㄱ-ㅎ가-힣]", string.Empty);
        }

        public static bool CheckValidation_AccountId(string inputStr, out string validateValue)
        {
            validateValue = inputStr;
            return validateValue != null && validateValue.Any(char.IsLetterOrDigit);
        }

        public static string GetValidateStr_Year(string inputStr)
        {
            return GetOnlyNumber(inputStr);
        }
        public static bool CheckValidation_Year(string inputStr, out string validateValue)
        {
            validateValue = GetOnlyNumber(inputStr);
            if (validateValue.Length == 2 && int.TryParse(validateValue, out int num))
                validateValue = (num.IsAround(0, 23) ? "20" : "19") + validateValue;

            return int.TryParse(validateValue, out int n) && n.IsAround(1900, 2023);
        }
        public static string GetValidateStr_MonthOrDay(string inputStr)
        {
            return GetOnlyNumber(inputStr);
        }
        public static bool CheckValidation_Month(string inputStr, out string validateValue)
        {
            validateValue = GetOnlyNumber(inputStr).RemoveStart("0");
            return int.TryParse(validateValue, out int n) && n.IsAround(1, 12);
        }
        public static bool CheckValidation_Day(string inputStr, out string validateValue)
        {
            validateValue = GetOnlyNumber(inputStr).RemoveStart("0");
            return int.TryParse(validateValue, out int n) && n.IsAround(1, 31);
        }


        public const string Replacement = "$1-$2-$3";

        /// <summary>
        /// Get Hyphen With phone number
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static string GetValidateStr_PhoneNum(string inputStr)
        {
            if (Rgx_Replace_Phone_Incomplete == null)
            {
                Rgx_Replace_Phone_Incomplete = new Regex(@"(^01[016789])([0-9]{4})?([0-9]+)$");
                Rgx_Replace_Phone_Complete = new Regex(@"(^01[016789])([0-9]{3,4})([0-9]{4})$");
            }
            if (inputStr == null || inputStr.Length == 0)
            {
                return string.Empty;
            }
            string value = GetOnlyNumber(inputStr);
            int length = value.Length;

            if (length > 2)
            {
                if (value[0].Equals('0') && value[1].Equals('1'))
                {
                    value = ((length < 10 ? Rgx_Replace_Phone_Incomplete : Rgx_Replace_Phone_Complete)
                        .Replace(value, Replacement))
                        .Replace("--", "-");
                }
                else
                {
                    //value = Regex.Replace(value, Replace_TelNumComplete, Replacement).Replace("--", "-");
                    //핸드폰입력창에 전화번호도 허용할것인지
                }
            }
            return value;
        }
        static Regex Rgx_Replace_Phone_Incomplete, Rgx_Replace_Phone_Complete;

        public static bool CheckValidation_PhoneNum(string inputStr, out string validateValue)
        {
            if (Rgx_Match_Phone == null) Rgx_Match_Phone = new Regex(@"^01[016789]-[0-9]{3,4}-[0-9]{4}$");
            validateValue = inputStr;
            return validateValue.LengthSafe().IsAround(12, 13) && Rgx_Match_Phone.Match(validateValue).Success;
        }
        static Regex Rgx_Match_Phone;

        public static string GetValidateStr_CompRegNum(string inputStr)
        {
            if (Rgx_Replace_CompRegNum_Incomplete == null)
            {
                Rgx_Replace_CompRegNum_Incomplete = new Regex(@"(^[0-9]{3})([0-9]{2})?([0-9]+)$");
                Rgx_Replace_CompRegNum_Complete = new Regex(@"(^[0-9]{3})([0-9]{2})([0-9]{5})$");            
            }
            //Regex.Replace(digitStr, Replace_CogRegNumComplete, "$1-$2-*****")
            string num = GetOnlyNumber(inputStr);
            return ((num.Length < 10 ? Rgx_Replace_CompRegNum_Incomplete : Rgx_Replace_CompRegNum_Complete)
                .Replace(num, Replacement))
                .Replace("--", "-");
        }
        static Regex Rgx_Replace_CompRegNum_Incomplete, Rgx_Replace_CompRegNum_Complete;

        public static bool CheckValidation_CompRegNum(string inputStr, out string validateValue)
        {
            if (Rgx_Match_CompRegNum == null) Rgx_Match_CompRegNum = new Regex(@"^[0-9]{3}-[0-9]{2}-[0-9]{5}$");
            validateValue = inputStr;
            return validateValue.Length == 12 && Rgx_Match_CompRegNum.Match(validateValue).Success;
        }
        static Regex Rgx_Match_CompRegNum;

        /// <summary>
        /// Get Hyphen With telephone number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetValidateStr_TeleNum(string inputStr)
        {
            if (Rgx_Replace_ServiceNum == null)
            {
                Rgx_Replace_ServiceNum = new Regex(@"(^1[0-9]{3})([0-9]{4})$");
                Rgx_Replace_TeleNum = new Regex(@"(^02|^050[2-8]|^0[0-9]{2})([0-9]{3,4})([0-9]{4})$");
            }
            string value = GetOnlyNumber(inputStr);
            if (value.Length == 0)
            {
                return string.Empty;
            }
            return (value[0].Equals('1') ? Rgx_Replace_ServiceNum : Rgx_Replace_TeleNum)
                            .Replace(value, Replacement);
        }
        static Regex Rgx_Replace_TeleNum, Rgx_Replace_ServiceNum;
        public static bool CheckValidation_TeleNum(string inputStr, out string validateValue)
        {
            if (Rgx_Match_TeleNum == null)
            {
                Rgx_Match_TeleNum = new Regex(@"(^02|^050[2-8]|^0[0-9]{2}|)-([0-9]{3,4})-([0-9]{4})$");
                Rgx_Match_ServiceNum = new Regex(@"(^1[0-9]{3})-([0-9]{4})$");
            }
            validateValue = inputStr;
            if(validateValue.LengthSafe() == 0)
            {
                return false;
            }
            return (validateValue[0].Equals('1') ? Rgx_Match_ServiceNum : Rgx_Match_TeleNum).Match(validateValue).Success;
        }
        static Regex Rgx_Match_TeleNum, Rgx_Match_ServiceNum;


        #endregion

    }
}