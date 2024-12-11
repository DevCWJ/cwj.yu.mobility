
#if CWJ_LOG_DISABLED || (!UNITY_EDITOR && CWJ_LOG_DISABLED_IN_BUILD)

using System;
using UnityEngine;

namespace CWJ
{
    public class EmptyLogger : ILogger, ILogHandler
    {
        public EmptyLogger() { }

        public ILogHandler logHandler { get => Debug.DefaultLogger.logHandler; set { } }

        public bool logEnabled { get => false; set { } }

        public LogType filterLogType { get; set; }


        public bool IsLogTypeAllowed(LogType logType)
        {
            return false;
        }

        private static string GetString(object message)
        {
            return string.Empty;
        }

        public void Log(LogType logType, object message)
        { }

        public void Log(LogType logType, object message, UnityEngine.Object context)
        { }

        public void Log(LogType logType, string tag, object message)
        { }

        public void Log(LogType logType, string tag, object message, UnityEngine.Object context)
        { }

        public void Log(object message)
        { }

        public void Log(string tag, object message)
        { }

        public void Log(string tag, object message, UnityEngine.Object context)
        { }

        public void LogWarning(string tag, object message)
        { }

        public void LogWarning(string tag, object message, UnityEngine.Object context)
        { }

        public void LogError(string tag, object message)
        { }

        public void LogError(string tag, object message, UnityEngine.Object context)
        { }

        public void LogException(Exception exception)
        { }

        public void LogException(Exception exception, UnityEngine.Object context)
        { }

        public void LogFormat(LogType logType, string format, params object[] args)
        { }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        { }
    }
}
#endif