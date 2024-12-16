using System;
using System.Linq;

using UnityEngine;

using Color = UnityEngine.Color;

namespace CWJ
{
    public static class RichTextUtil
    {
        /// <summary>
        /// Log 색 설정
        /// <para>오렌지색은 new Color(255, 128, 0) 이렇게 매개변수로 던지면됨</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string SetColor(this string message, Color color)
        {
            if (!IsByteColorValue(color))
            {
                color = ToByteColor(color);
            }
            return CheckValidMsg(string.Concat($"<color=#{((byte)(color.r)):X2}{((byte)(color.g)):X2}{((byte)(color.b)):X2}>", message, "</color>"), message);
        }

        private static bool IsByteColorValue(Color color)
        {
            return color.r > 1f || color.g > 1f || color.b > 1f;
        }

        private static Color ToByteColor(Color color)
        {
            color.r *= 255;
            color.g *= 255;
            color.b *= 255;
            return color;
        }

        /// <summary>
        /// Log 크기 설정
        /// <para>23이 한줄 가득 차는 크기임</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string SetSize(this string message, int size, bool isViewOnlyOneLine = false) => (isViewOnlyOneLine && !message.StartsWith(GetViewOneLineTag()) ? GetViewOneLineTag() : "") + CheckValidMsg(string.Concat($"<size={size}>", message, "</size>"), message);

        public static string GetViewOneLineTag() => "<size=23> </size>";

        public static string CheckValidMsg(string fullMessage, string message)
        {
            if (fullMessage.Contains("\n"))
            {
                string[] fullMsgs = fullMessage.Split(new string[] { message }, StringSplitOptions.None);
                string startTag = fullMsgs[0];
                string endTag = fullMsgs[1];

                string[] lines = fullMessage.Split(new string[] { "\n" }, StringSplitOptions.None);

                if (!string.IsNullOrEmpty(lines[0].Trim()))
                {
                    lines[0] = lines[0] + endTag;
                }

                int length = lines.Length;
                for (int i = 1; i < length - 1; i++)
                {
                    if (!string.IsNullOrEmpty(lines[i].Trim()))
                    {
                        lines[i] = startTag + lines[i] + endTag;
                    }
                }

                if (!string.IsNullOrEmpty(lines[length - 1].Trim()))
                {
                    lines[length - 1] = startTag + lines[length - 1];
                }
                return string.Join("\n", lines);
            }
            else
            {
                return fullMessage;
            }
        }

        /// <summary>
        /// Log 굵기 설정
        /// </summary>
        /// <param name="message"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string SetBold(this string message) => CheckValidMsg(string.Concat($"<b>", message, "</b>"), message);

        /// <summary>
        /// Log Italic 기울임체 설정
        /// </summary>
        /// <param name="message"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string SetItalic(this string message) => CheckValidMsg(string.Concat($"<i>", message, "</i>"), message);

        /// <summary>
        /// <para>Log 색과 크기 설정</para>
        /// <para>23은 한줄 가득 차는 크기임</para>
        /// 오렌지색은 new Color(255, 128, 0) 이렇게 매개변수로 던지면됨
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string SetStyle(this string message, Color color, int size = 0, bool isBold = false, bool isItalic = false, bool isViewOneLine = false)
        {
            string returnMessage = message.SetColor(color);
            if (isBold)
            {
                returnMessage = returnMessage.SetBold();
            }
            if (isItalic)
            {
                returnMessage = returnMessage.SetItalic();
            }
            if (size > 0)
            {
                returnMessage = returnMessage.SetSize(size, isViewOnlyOneLine: false);
            }
            if (isViewOneLine && !returnMessage.StartsWith(GetViewOneLineTag()))
            {
                returnMessage = GetViewOneLineTag() + returnMessage;
            }
            return returnMessage;
        }


        //public static Color GetCommentsColor(this Color color, byte alpha = 255) => SetColor(color, 87, 165, 71, alpha);
        public static Color GetCommentsColor(this Color color, float alpha = 1f) => InitColor(color, 0.3137255f, 0.5882353f, 0.254902f, alpha);

        //public static Color GetClassNameColor(this Color color, byte alpha = 255) => SetColor(color, 50, 165, 139, alpha);
        public static Color GetClassNameColor(this Color color, float alpha = 1f) => InitColor(color, 0.1372549f, 0.4470588f, 0.3843137f, alpha);

        public static Color GetLightRed(this Color color, float alpha = 1f) => InitColor(color, 1f, 0.3254901f, 0.2901960f, alpha);

        public static Color GetDarkRed(this Color color, float alpha = 1f) => InitColor(color, 0.9294118f, 0, 0, alpha);

        public static Color GetBrown(this Color color, float alpha = 1f) => InitColor(color, 0.3960784f, 0.2627451f, 0.1294118f, alpha);

        public static Color GetPink(this Color color, float alpha = 1f) => InitColor(color, 1, 0.5960785f, 0.7960784f, alpha);

        public static Color GetMagenta(this Color color, float alpha = 1f) => InitColor(color, 1f, 0, 1f, alpha);

        public static Color GetOrange(this Color color, float alpha = 1f) => InitColor(color, 1, 0.5019608f, 0, alpha);

        public static Color GetYellow(this Color color, float alpha = 1f) => InitColor(color, 1, 0.827451f, 0, alpha);

        public static Color GetGreen(this Color color, float alpha = 1f) => InitColor(color, 0.4f, 1, 0, alpha);

        public static Color GetLightGreen(this Color color, float alpha = 1f) => InitColor(color, 0.6274511f, 0.8313726f, 0.4078431f, alpha);


        public static Color GetBlue(this Color color, float alpha = 1f) => InitColor(color, 0, 0.5294118f, 0.7411765f, alpha);

        public static Color GetSkyBlue(this Color color, float alpha = 1f) => InitColor(color, 0.3137255f, 0.5882353f, 1f, alpha);

        public static Color GetOrientalBlue(this Color color, float alpha = 1f) => InitColor(color, 0.09411765f, 0.4627451f, 0.9843137f, alpha);

        public static Color GetIndigo(this Color color, float alpha = 1f) => InitColor(color, 0.2941177f, 0, 0.509804f, alpha);

        public static Color GetViolet(this Color color, float alpha = 1f) => InitColor(color, 0.4980392f, 0, 1, alpha);

        private static Color InitColor(Color color, float r, float g, float b, float a)
        {
            color.r = r; color.g = g; color.b = b; color.a = a;
            return color;
        }

        public static Color SetAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }


        /// <summary>
        /// <para><color></color>,<size></size>,<b></b>,<i></i></para>
        /// 들을 빼주는 함수(DisplayDialog에서는 RichText 지원안해주기때문)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ExcludeRichTextFormat(this string message, params string[] ignoreTags) //나중에 정규식으로 수정하기 (-> 정정: 굳이 바꿀필요 없음 성능은 반복문방식이 낫다고함)
        {
            if (string.IsNullOrEmpty(message)) return message;

            string[] tags = (ignoreTags?.Length == 0 ? new string[] { "color", "size", "b", "i" } : ignoreTags);

            Func<string, bool> isContinsTags = (s) => message.Contains("<" + s) && message.Contains("</" + s + ">");
            while (tags.Any((s) => isContinsTags(s)))
            {
                for (int i = 0; i < tags.Length; i++)
                {
                    if (!isContinsTags(tags[i]))
                    {
                        continue;
                    }
                    int startOpenIndex = message.IndexOf("<" + tags[i]);
                    int startCloseIndex = message.Substring(startOpenIndex, message.Length - startOpenIndex).IndexOf(">") + startOpenIndex;
                    if (startOpenIndex >= 0 && startCloseIndex >= 0)
                    {
                        message = string.Concat(message.Substring(0, startOpenIndex), message.Substring(startCloseIndex + 1, message.Length - (startCloseIndex + 1)));
                    }
                    string endTag = "</" + tags[i] + ">";
                    int endIndex = message.IndexOf(endTag);
                    if (endIndex >= 0)
                    {
                        message = string.Concat(message.Substring(0, endIndex), message.Substring(endIndex + endTag.Length, message.Length - (endIndex + endTag.Length)));
                    }
                }
            }
            return message;
        }
    }
}