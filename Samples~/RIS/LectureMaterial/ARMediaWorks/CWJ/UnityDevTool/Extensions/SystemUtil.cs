using System;
using System.Reflection;
using System.IO;
using UnityEngine;
using System.Text;

namespace CWJ
{
    public static class SystemUtil
    {
        public static long GetTimeStamp()
        {
            return GetTimeStamp(DateTime.Now);
        }
        public static long GetTimeStamp(this DateTime value)
        {
            return ((DateTimeOffset)value).ToUnixTimeMilliseconds();
        }

        public static DateTime ConvertTimestampToDateTime(this long value)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddMilliseconds(value).ToLocalTime();
            return dt;
        }

        public static string ConvertToOrdinal(this int num)
        {
            if (num <= 0) return num.ToString();
            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }
            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
            }
            return num + "th";
        }

        public static string ConvertToWords(this int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + ConvertToWords(Math.Abs(number));

            string words = string.Empty;

            if ((number / 1000000) > 0)
            {
                words += ConvertToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += ConvertToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += ConvertToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != string.Empty)
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
        public static string SecondToDigitalMinute(float value)
        {
            int hour = 0;
            int minute = 0;
            int second = 0;

            if (value > 60 * 60)
            {
                hour = UnityEngine.Mathf.FloorToInt(value / 60.0f / 60.0f);
            }

            if (value > 60)
            {
                minute = UnityEngine.Mathf.FloorToInt(value / 60.0f);
            }

            second = UnityEngine.Mathf.FloorToInt(value % 60.0f);

            return (hour == 0 ? "" : (hour < 10 ? "0" : "") + hour.ToString() + ":") + (minute < 10 ? "0" : "") + minute.ToString() + ":" + (second < 10 ? "0" : "") + second.ToString();
        }
        public static string TimeSpanToString(this TimeSpan timeSpan, bool isVisibleMilli = false)
        {
            return $"{(int)timeSpan.TotalHours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}" + (isVisibleMilli ? $":{timeSpan.Milliseconds:000}" : "");
        }

        public static string HoursToTime(this double hours, bool isVisibleMilli = false)
        {
            return TimeSpanToString(TimeSpan.FromHours(hours), isVisibleMilli);
        }

        public static string MinutesToTime(this double minutes, bool isVisibleMilli = false)
        {
            return TimeSpanToString(TimeSpan.FromMinutes(minutes), isVisibleMilli);
        }

        public static string SecondsToTime(this double seconds, bool isVisibleMilli = false)
        {
            return TimeSpanToString(TimeSpan.FromSeconds(seconds), isVisibleMilli);
        }

        public static string MilliSecondsToTime(this double milliSeconds, bool isVisibleMilli = false)
        {
            return TimeSpanToString(TimeSpan.FromMilliseconds(milliSeconds), isVisibleMilli);
        }

        //public static void SendMessageWithDelay(this Action action, float delay)
        //{
        //    Timer timer = new Timer(delay);

        //    timer.Elapsed +=(a,b)=>
        //    {
        //        action.Invoke();
        //        timer.Stop();
        //        timer.Dispose();
        //    };

        //    timer.Start();
        //}

        public static string ClipboardValue { get => GUIUtility.systemCopyBuffer; set => GUIUtility.systemCopyBuffer = value; }
        public static void CopyToClipboard(this string str)
        {
            //GUIUtility.systemCopyBuffer = str;
            var textEditor = new UnityEngine.TextEditor();
            textEditor.text = str;
            textEditor.SelectAll();
            textEditor.Copy();
            UnityEngine.Debug.Log($"Copied!\n(\"{str}\")");
        }
    }
}