using System;
using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;

namespace CWJ
{
    [CreateAssetMenu(fileName = "ByteSizeValidator", menuName = "CWJ/TMP/ByteSizeValidator", order = int.MaxValue)]
    [Serializable]
    public class Ipf_ByteSizeValidator : TMPro.TMP_InputValidator
    {
        public int maxByteSize = 6;
        [Tooltip("UTF8 : 3byte / EUCKR : 2byte")]
        public bool isDataHasKr = true; 

        public bool IsEqualsMaxByteSize(string text)
        {
            return GetByteSize(text) == maxByteSize;
        }

        public int GetByteSize(string text)
        {
            // UTF8말고 EUCKR같은거로 하고싶으면 - Nuget Package -> System.Text.Encoding.CodePages 설치 필요
            // CodePages : https://www.nuget.org/packages/System.Text.Encoding.CodePages/
            // codepage 인덱스 참고 : https://learn.microsoft.com/ko-kr/dotnet/api/system.text.encodinginfo.getencoding?view=net-8.0
            // ex : Encoding _Encoding_EucKr = CodePagesEncodingProvider.Instance.GetEncoding(51949);
            return (isDataHasKr ? Encoding.UTF8 : Encoding.ASCII).GetByteCount(text);
        }

        public bool IsValidate(string text)
        {
            int byteSize = GetByteSize(text);
            return byteSize <= maxByteSize;
        }

        public override char Validate(ref string text, ref int pos, char ch)
        {
            if (Regex.IsMatch(ch.ToString(), @"[^a-zA-Z0-9가-힣ㄱ-ㅎㅏ-ㅣ]"))
                return (char)0;

            string tmp = text == null ? ch.ToString() : text + ch;
            if (!IsValidate(tmp))
            {
                return (char)0;
            }

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WebGLPlayer:
                    text = tmp;
                    pos += 1;
                    break;

                default:
                    return ch;
            }
            return (char)0;
        }
    }

}