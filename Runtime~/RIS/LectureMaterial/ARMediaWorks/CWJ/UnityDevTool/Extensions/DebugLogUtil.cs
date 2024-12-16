using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CWJ
{
    /// <summary>
    /// 주의. CWJ.AccessibleEditor.DebugSetting에서 isLogEnabled가 비활성화되어있어도 실행되는 함수들임.
    /// </summary>
    public static class DebugLogUtil
    {
        public static bool IsLogEnabled =
#if CWJ_LOG_DISABLED
            false;
#else
            true;
#endif

        public static void WriteLogForcibly(string log)
        {
            CWJ.AccessibleEditor.DebugSetting.DebugLogWriter.WriteLogBeforeSystemLoaded(log);
        }

        public static void AssertWithClassName(this Type classType, bool condition, string message, bool isComment = false, bool isBigFont = true, UnityEngine.Object obj = null, bool isPreventStackTrace = false)
        {
            if (condition) return;

            PrintLogWithClassName(classType, message, logType: LogType.Assert, isComment: isComment, isBigFont: isBigFont, obj: obj, isPreventStackTrace: isPreventStackTrace);
        }

        public static void PrintLogWithClassName(this Type classType, Exception exception, UnityEngine.Object obj = null, bool isPreventOverlapMsg = false, bool isPreventStackTrace = false)
        {
            classType.PrintLogWithClassName(exception.Message, LogType.Exception, isComment: false, isBigFont: false, obj: obj, isPreventOverlapMsg: isPreventOverlapMsg, isPreventStackTrace: isPreventStackTrace);
        }

        public static Action<bool> SetUnityLoggerEnabled = null;

#if UNITY_EDITOR
        public static readonly HashSet<string> LogPool = new HashSet<string>();
#endif
        public static void PrintLogWithClassName(this Type classType, string message, LogType logType = LogType.Warning, bool isComment = true, bool isBigFont = true, UnityEngine.Object obj = null, bool isPreventOverlapMsg = false, bool isPreventStackTrace = true)
        {
#if UNITY_EDITOR
            if (isPreventOverlapMsg)
            {
                if (!LogPool.Add(message))
                {
                    return;
                }
            }
#endif
            if (!Application.isEditor)
            {
                if (isPreventStackTrace)
                    isPreventStackTrace = false;
            }


            isComment = isComment && (logType != LogType.Error && logType != LogType.Exception && logType != LogType.Assert);
            message = message.TrimEnd("\n");
            message = isComment ? ("//" + message).SetColor(new Color().GetCommentsColor()) : message;
            message = isBigFont ? message.SetSize(18, isViewOnlyOneLine: true) : message;
            message = message.Replace("\n\n", "\n");

            message = message + "\n" + $"<{classType.GetCWJClassNameMark()}>".SetBold();
            if (logType == LogType.Log)
                PrintLog(message: message, context: obj, isPreventStackTrace: isPreventStackTrace);
            else if (logType == LogType.Warning)
                PrintLogWarning(message: message, context: obj, isPreventStackTrace: isPreventStackTrace);
            else if (logType == LogType.Assert)
                PrintLogAssert(false, message: message, context: obj, isPreventStackTrace: isPreventStackTrace);
            else
                PrintLogError(message: message, context: obj, isPreventStackTrace: isPreventStackTrace);
        }

        public static string GetCWJClassNameMark(this Type classType)
        {
            return $"{nameof(CWJ)}.{classType.Name.GetNicifyVariableName().RemoveAllSpaces()}";
        }

        const string StackTraceEndComment = "--- End of inner exception stack trace ---\r\n";
        static readonly string[] StackTraceEndSplitSep = new string[1] { StackTraceEndComment };
        public static string GetRealErrorTrace(string errorLog)
        {
            if (!errorLog.Contains(StackTraceEndComment))
            {
                return errorLog;
            }
            string originException = errorLog.Split(StackTraceEndSplitSep, StringSplitOptions.None)[0];
            return originException;
        }

        const string LineBreak = "\r\n";
        static readonly string[] SplitSep_LineBreak = new string[1] { LineBreak };

        const string AtKeyword = "at ";
        const string InKeyword = "] in ";
        static readonly string[] SplitSep_InKeyword = new string[1] { InKeyword };
        const string CsFile = ".cs:";
        static readonly string[] SplitSep_CsFile = new string[1] { CsFile };

        const string MetaFile = ">:";
        static readonly string[] SplitSep_MetaFile = new string[1] { MetaFile };

        public static void ConvertToHyperLinkStr(ref string msg)
        {
            string[] lines = msg.Split(SplitSep_LineBreak, StringSplitOptions.None);
            int length = lines.Length;
            int cnt = 0;
            for (int i = 0; i < length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line) || !line.StartsWith(AtKeyword))
                {
                    continue;
                }

                bool isCsFile = line.Contains(CsFile);

                if (line.Contains(InKeyword) && (isCsFile || line.Contains(MetaFile)))
                {
                    string[] sp = line.Split(isCsFile ? SplitSep_CsFile : SplitSep_MetaFile, StringSplitOptions.None);
                    if (int.TryParse(sp[1], out int csLine))
                    {
                        sp = sp[0].Split(SplitSep_InKeyword, StringSplitOptions.None);
                        string path = isCsFile ? (sp[1] + ".cs") : (sp[1].Substring(1, sp[1].Length - 1));
                        line = $"{sp[0]}]{LineBreak}  ⇒ in <a href=\"{path}\" line=\"{csLine}\">{path}:{csLine}</a>";
                    }
                }

                lines[i] = $"[{cnt++}] {line}{LineBreak}";
            }
            msg = string.Join(LineBreak, lines);
        }

        private static void _PrintLog(LogType logType, Action<string, UnityEngine.Object> PrintLogAction, string msg, UnityEngine.Object obj, bool isPreventStackTrace, bool isAutoHyperLink = false)
        {
            if (!isPreventStackTrace)
            {
                var lines = GetStackTrace().Split('\n');
                lines = lines.SubArray(5, lines.Length - 6);
                msg += "\n" + string.Join("\n", lines);
            }

            UnityEngine.Application.SetStackTraceLogType(logType, UnityEngine.StackTraceLogType.None);

            try
            {
                if (isAutoHyperLink)
                    ConvertToHyperLinkStr(ref msg);

                if (!IsLogEnabled)
                    SetUnityLoggerEnabled?.Invoke(true);
                PrintLogAction.Invoke(msg, obj);
            }
            finally
            {
                UnityEngine.Application.SetStackTraceLogType(logType, UnityEngine.StackTraceLogType.ScriptOnly);
                if (!IsLogEnabled)
                    SetUnityLoggerEnabled?.Invoke(false);
            }

        }

        const string UnityEditorUIElements = "UnityEditor.UIElements.";
        const string CreateIMGUIInspectorFromEditor = "<CreateIMGUIInspectorFromEditor>b__0";
        const string PathAt = "(at ";
        static readonly string DebugLogUtilCsFile = "\\" + nameof(DebugLogUtil) + ".cs:";

        public static string GetStackTrace()
        {
            string callStack = StackTraceUtility.ExtractStackTrace();

            bool isNeedSkip = callStack.Contains(DebugLogUtilCsFile);
            bool isNeedTake = callStack.Contains(CreateIMGUIInspectorFromEditor);

            if (!isNeedSkip && !isNeedTake) return callStack;

            IEnumerable<string> callStacks = StackTraceUtility.ExtractStackTrace().Split('\n');
            int skipCnt = 0;
            int takeCnt = 0;
            
            foreach (string trace in callStacks)
            {
                if (isNeedSkip)
                {
                    int index = trace.LastIndexOf(PathAt);
                    if (index >= 0 && trace.Substring(index).Contains(DebugLogUtilCsFile))
                    {
                        ++skipCnt;
                        continue;
                    }
                }

                if (isNeedTake && (!isNeedSkip || skipCnt > 0))
                {
                    if (trace.StartsWith(UnityEditorUIElements) && trace.Contains(CreateIMGUIInspectorFromEditor))
                    {
                        break;
                    }
                    ++takeCnt;
                }
            }

            if (isNeedSkip)
            {
                callStacks = callStacks.Skip(skipCnt);
            }
            if (isNeedTake)
            {
                callStacks = callStacks.Take(takeCnt);
            }

            return string.Join("\n", callStacks);
        }

        public static void PrintLog(string message, UnityEngine.Object context = null, bool isPreventStackTrace = true)
        {
            _PrintLog(LogType.Log, UnityEngine.Debug.Log, message, context, isPreventStackTrace);
        }

        public static void PrintLogWarning(string message, UnityEngine.Object context = null, bool isPreventStackTrace = true)
        {
            _PrintLog(LogType.Warning, UnityEngine.Debug.LogWarning, message, context, isPreventStackTrace);
        }

        public static void PrintLogError(string message, UnityEngine.Object context = null, bool isPreventStackTrace = true)
        {
            message = "[ERROR] ".SetStyle(new Color().GetDarkRed(), isBold: true, size: 18, isViewOneLine: true) + message;
            _PrintLog(LogType.Error, UnityEngine.Debug.LogError, message, context, isPreventStackTrace, isAutoHyperLink: true);
        }

        public static void PrintLogAssert(bool condition, string message, UnityEngine.Object context = null, bool isPreventStackTrace = true)
        {
            message = "[ASSERT] ".SetStyle(new Color().GetDarkRed(), isBold: true, size: 18, isViewOneLine: true) + message;
            _PrintLog(LogType.Assert, (msg, obj) => UnityEngine.Debug.Assert(condition: condition, message: msg, context: obj), message, context, isPreventStackTrace, isAutoHyperLink: true);
        }

        public static void PrintLogException<T>(string message, UnityEngine.Object context = null, bool isPreventStackTrace = true) where T: Exception
        {
            message = ("[EXCEPTION] ".SetStyle(new Color().GetDarkRed(), isBold: true) + message).SetSize(18, true);

            _PrintLog(LogType.Exception, (msg, obj) => UnityEngine.Debug.LogException((T)Activator.CreateInstance(typeof(T), msg), context),
                message, context, isPreventStackTrace, isAutoHyperLink: true);
        }

        public static void LogClear()
        {
#if UNITY_EDITOR
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.Editor));
            assembly.GetType("UnityEditor.LogEntries")?.GetMethod("Clear")?.Invoke(new object(), null);
#endif
        }
    }
}