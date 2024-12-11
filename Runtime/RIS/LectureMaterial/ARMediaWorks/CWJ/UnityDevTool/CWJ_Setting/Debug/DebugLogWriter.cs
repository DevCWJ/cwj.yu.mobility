using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace CWJ.AccessibleEditor.DebugSetting
{
    public class DebugLogWriter
    {
        public static readonly string GetLogFolderPath = AccessibleEditorUtil.GetPersistentDataPath + "/CWJ_LOG/";

        public static readonly string GetEditorLogFolderPath = Application.persistentDataPath + "/CWJ_LOG_editor/";

        private static string LogFolderPath;
        private static string LogFilePath;

        /// <summary>
        /// 로그가 찍히지않는경우 사용할것.
        /// <br/>보통 씬 불러오기전에 사용한 Debug.Log의 경우 Logs.txt에 저장되지 않을수 있는데, 그럴때 사용하면 됨.
        /// </summary>
        /// <param name="log"></param>
        public static void WriteLogBeforeSystemLoaded(string log)
        {
#if CWJ_LOG_SAVE
            if (!isInit)
            {
                if (LogStrBuilder == null)
                    LogStrBuilder = new StringBuilder();
                LogStrBuilder.AppendLine(log + "\r\n");
            }
            else
            {
                WriteLogOnTxt(log);
            }
#else
            Debug.Log(log);
#endif
        }


        /// <summary>
        /// SubsystemRegistration->AfterAssembliesLoaded->BeforeSplashScreen->BeforeSceneLoad 순서임
        /// </summary>
#if CWJ_LOG_SAVE
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        private static void StartLogMsgWrite()
        {
            LogFolderPath = Application.isEditor ? GetEditorLogFolderPath : GetLogFolderPath;
            if (!LogFolderPath.IsFolderExists(true, false))
            {
                UnityEngine.Debug.LogError("[CWJ] DebugLogWriter Error - 폴더를 찾거나 생성할수 없음" + LogFolderPath);
                return;
            }
            try
            {
                string fileName = DateTime.Now.ToString("yy-MM-dd_HH-mm-ss") + " Logs.txt";
                var fStream = File.CreateText(LogFilePath = PathUtil.GetFilePathPreventOverlap(LogFolderPath, fileName));
                fStream.Close();
                //using (new FileStream(LogFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite, 4096))
                //{ }

                if (LogStrBuilder == null)
                    LogStrBuilder = new StringBuilder();
                else
                {
                    WriteLogOnTxt(LogStrBuilder.AppendLine("------------------- Above logs are write before in [SubsystemRegistration] -------------------").ToString());
                    LogStrBuilder.Length = 0;
                }
                Application.logMessageReceivedThreaded += OnLogMessageReceived;
                isInit = true;
                Debug.Log("[CWJ] DebugLogwWriter Success " + LogFilePath);

            }
            catch
            {
                UnityEngine.Debug.LogError("[CWJ] DebugLogWriter Error - 폴더를 찾거나 생성할수 없음2" + LogFolderPath);
            }
        }



        private const string NonFilledArrow = "▷";
        private const string FilledArrow = "▶";
        private const string DebugTitle = " - Debug.";
        private const string MessageTitle = "\r\n(Ⅰ)Message :\r\n\"";
        private const string StackTraceTitle = "\r\n(Ⅱ)StackTrace :\r\n";
        private static StringBuilder LogStrBuilder = null;
        private static bool isInit = false;

        /// <summary>
        /// TODO 왜 PC에선 되고 AOS에선 한번만 되는걸까 테스트필요
        /// </summary>
        /// <param name="message"></param>
        /// <param name="stackTrace"></param>
        /// <param name="logType"></param>
        private static void OnLogMessageReceived(string message, string stackTrace, LogType logType)
        {
            if (logType != LogType.Log && logType != LogType.Warning)
            {
                UnityEngine.Debug.developerConsoleVisible = false; //exception이 발생되면 자동으로 true가 되어 개발용 Console GUI가 표시됨.
            }
#if CWJ_LOG_SAVE
            LogStrBuilder.Append(logType.Equals(LogType.Exception) ? FilledArrow : NonFilledArrow).Append(DateTime.Now.ToString("HH:mm:ss")).Append(DebugTitle).Append(logType.ToString());
            LogStrBuilder.Append(MessageTitle).Append(message).Append("\"");
            if (stackTrace != null)
                LogStrBuilder.Append(StackTraceTitle).Append(stackTrace.TrimEnd()).Append("\r\n");
            WriteLogOnTxt(LogStrBuilder.ToString());
            LogStrBuilder.Length = 0;
#endif
        }

       static void WriteLogOnTxt(string log)
        {
            try
            {
                using (var fs = new FileStream(LogFilePath, FileMode.Append, FileAccess.ReadWrite, FileShare.Read, 4096))
                {
                    using (TextWriter textWriter = new StreamWriter(fs, Encoding.UTF8, 4096, false))
                    {
                        textWriter.WriteLine(log);
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.LogError("[CWJ] writeLog Error " + ex.ToString());
            }
        }

        private const string LogWriterSettingFileName = "_LogWriterSetting.ini";

#if CWJ_LOG_SAVE
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
#endif
        private static void OnAfterSceneLoad()
        {
            string settingIniPath = string.Concat(LogFolderPath, LogWriterSettingFileName);

            bool isSaveLogEnabled = true;

            var ini = new IniFile();
            if (!File.Exists(settingIniPath))
            {
                ini[nameof(DebugSetting)][nameof(isSaveLogEnabled)] = isSaveLogEnabled = true;
                ini.Save(settingIniPath);
            }
            else
            {
                ini.Load(settingIniPath);
                isSaveLogEnabled = ini[nameof(DebugSetting)][nameof(isSaveLogEnabled)].ToBool();
                Debug.LogError("[CWJ] ??" + isSaveLogEnabled);
            }
            ini = null;

            if (isSaveLogEnabled)
            {
                MonoBehaviourEventHelper.LastQuitEvent += StopLogMsgWrite;
            }
            else
            {
                UnityEngine.Debug.LogError("[CWJ] Disable DebugLogWriter");
                StopLogMsgWrite();
                File.Delete(LogFilePath);
            }

            if (!MonoBehaviourEventHelper.IS_EDITOR)
            {
                UnityDevConsoleVisible.UpdateInstance();
            }

            UnityEngine.Debug.developerConsoleVisible = false; //exception이 발생되면 자동으로 true가 되어 개발용 Console GUI가 표시됨.
        }
        private static void StopLogMsgWrite()
        {
            Application.logMessageReceivedThreaded -= OnLogMessageReceived;
            if (LogStrBuilder != null)
            {
                if (!isInit)
                    WriteLogOnTxt(LogStrBuilder.ToString());
                LogStrBuilder.Length = 0;
                LogStrBuilder = null;
            }
        }
#region old code

        /// <summary>
        /// 이전 클래스,메소드,매개변수 이름을 간략히 가져옴
        /// 간략한 실행위치만 반환해주기 때문에 개발자가 직접 작성한 Log,Warning,Error에서만 실행되어야함
        /// ExceptionMessageReceived 에서는 자동으로 줄바꿈이 추가되어있어서 자체적 Debug.log/warning/error를 위해 여기서 줄바꿈
        /// </summary>
        /// <param name="_skipFrames">GetStackTrace()-Log()이전의 위치가 필요하기때문에 2가 Default</param>
        /// <returns></returns>
        //private static string GetSimpleStackTrace(int _skipFrames = 2)
        //{
        //    StackFrame stackFrame = new StackFrame(_skipFrames, true);
        //    System.Reflection.MethodBase method = stackFrame.GetMethod();
        //    System.Reflection.ParameterInfo[] parameterArray = method.GetParameters();

        //    StringBuilder builder = new StringBuilder();
        //    builder.Append(string.Concat("\r\n[", method.ReflectedType.Name, " : ", method.Name));
        //    builder.Append("(");
        //    int paramsLength = parameterArray.Length;
        //    if (paramsLength > 0)
        //    {
        //        builder.Append(parameterArray[0].ParameterType.Name).Append(" ").Append(parameterArray[0].Name);
        //        for (int i = 1; i < paramsLength; i++)
        //        {
        //            builder.Append(", ").Append(parameterArray[i].ParameterType.Name).Append(" ").Append(parameterArray[i].Name);
        //        }
        //    }

        //    string fileNameLine = stackFrame.GetFileName();
        //    if (fileNameLine.Contains(@"\"))
        //    {
        //        fileNameLine = fileNameLine.Substring(fileNameLine.LastIndexOf(@"\"));
        //    }

        //    builder.Append(string.Concat(") (at ", fileNameLine, " : ", stackFrame.GetFileLineNumber())).Append(")]\r\n");

        //    return builder.ToString();
        //}
        //public class Debug
        //{
        //private static bool IsBreakedSave = false;
        //    private const string Type_Log = "Log";
        //    private const string Type_Warning = "Warning";
        //    private const string Type_Error = "Error";
        //    private const string Type_Assert = "Assert";
        //    private const string Type_Exception = "Exception";

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void Log(object message)
        //    {
        //        UnityEngine.Debug.Log(message);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived((string)message, GetSimpleStackTrace(), LogType.Log);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void Log(object message, UnityEngine.Object context)
        //    {
        //        UnityEngine.Debug.Log(message, context);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived((string)message, GetSimpleStackTrace(), LogType.Log);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void LogFormat(string format, params object[] args)
        //    {
        //        UnityEngine.Debug.LogFormat(format, args);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived(string.Format(format, args), GetSimpleStackTrace(), LogType.Log);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
        //    {
        //        UnityEngine.Debug.LogFormat(context, format, args);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived(string.Format(format, args), GetSimpleStackTrace(), LogType.Log);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void LogWarning(object message)
        //    {
        //        UnityEngine.Debug.LogWarning(message);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived((string)message, GetSimpleStackTrace(), LogType.Warning);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void LogWarning(object message, UnityEngine.Object context)
        //    {
        //        UnityEngine.Debug.LogWarning(message, context);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived((string)message, GetSimpleStackTrace(), LogType.Warning);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void LogWarningFormat(string format, params object[] args)
        //    {
        //        UnityEngine.Debug.LogWarningFormat(format, args);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived(string.Format(format, args), GetSimpleStackTrace(), LogType.Warning);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
        //    {
        //        UnityEngine.Debug.LogWarningFormat(context, format, args);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived(string.Format(format, args), GetSimpleStackTrace(), LogType.Warning);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void LogError(object message)
        //    {
        //        UnityEngine.Debug.LogError(message);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived((string)message, GetSimpleStackTrace(), LogType.Error);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void LogError(object message, UnityEngine.Object context)
        //    {
        //        UnityEngine.Debug.LogError(message, context);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived((string)message, GetSimpleStackTrace(), LogType.Error);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void LogErrorFormat(string format, params object[] args)
        //    {
        //        UnityEngine.Debug.LogErrorFormat(format, args);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived(string.Format(format, args), GetSimpleStackTrace(), LogType.Error);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
        //    {
        //        UnityEngine.Debug.LogErrorFormat(context, format, args);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived(string.Format(format, args), GetSimpleStackTrace(), LogType.Error);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void Assert(bool condition, string message, UnityEngine.Object context)
        //    {
        //        UnityEngine.Debug.Assert(condition, message, context);

        //        if (!condition && !IsBreakedSave)
        //        {
        //            OnLogMessageReceived(message, GetSimpleStackTrace(), LogType.Assert);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void Assert(bool condition, object message, UnityEngine.Object context)
        //    {
        //        UnityEngine.Debug.Assert(condition, message, context);

        //        if (!condition && !IsBreakedSave)
        //        {
        //            OnLogMessageReceived((string)message, GetSimpleStackTrace(), LogType.Assert);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void Assert(bool condition, string message)
        //    {
        //        UnityEngine.Debug.Assert(condition, message);

        //        if (!condition && !IsBreakedSave)
        //        {
        //            OnLogMessageReceived(message, GetSimpleStackTrace(), LogType.Assert);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void Assert(bool condition, object message)
        //    {
        //        UnityEngine.Debug.Assert(condition, message);

        //        if (!condition && !IsBreakedSave)
        //        {
        //            OnLogMessageReceived((string)message, GetSimpleStackTrace(), LogType.Assert);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void Assert(bool condition, UnityEngine.Object context)
        //    {
        //        UnityEngine.Debug.Assert(condition, context);

        //        if (!condition && !IsBreakedSave)
        //        {
        //            OnLogMessageReceived("Assertion failed", GetSimpleStackTrace(), LogType.Assert);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void Assert(bool condition)
        //    {
        //        UnityEngine.Debug.Assert(condition);

        //        if (!condition && !IsBreakedSave)
        //        {
        //            OnLogMessageReceived("Assertion failed", GetSimpleStackTrace(), LogType.Assert);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void AssertFormat(bool condition, string format, params object[] args)
        //    {
        //        UnityEngine.Debug.AssertFormat(condition, format, args);

        //        if (!condition && !IsBreakedSave)
        //        {
        //            OnLogMessageReceived(string.Format(format, args), GetSimpleStackTrace(), LogType.Assert);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void AssertFormat(bool condition, UnityEngine.Object context, string format, params object[] args)
        //    {
        //        UnityEngine.Debug.AssertFormat(condition, context, format, args);

        //        if (!condition && !IsBreakedSave)
        //        {
        //            OnLogMessageReceived(string.Format(format, args), GetSimpleStackTrace(), LogType.Assert);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    [Obsolete("Assert(bool, string, params object[]) is obsolete. Use AssertFormat(bool, string, params object[]) (UnityUpgradable) -> AssertFormat(*)", true)]
        //    public static void Assert(bool condition, string format, params object[] args)
        //    {
        //        UnityEngine.Debug.AssertFormat(condition, format, args);

        //        if (!condition && !IsBreakedSave)
        //        {
        //            OnLogMessageReceived(string.Format(format, args), GetSimpleStackTrace(), LogType.Assert);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void LogAssertion(object message, UnityEngine.Object context)
        //    {
        //        UnityEngine.Debug.LogAssertion(message, context);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived((string)message, GetSimpleStackTrace(), LogType.Assert);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void LogAssertion(object message)
        //    {
        //        UnityEngine.Debug.LogAssertion(message);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived((string)message, GetSimpleStackTrace(), LogType.Assert);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void LogAssertionFormat(UnityEngine.Object context, string format, params object[] args)
        //    {
        //        UnityEngine.Debug.LogAssertionFormat(context, format, args);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived(string.Format(format, args), GetSimpleStackTrace(), LogType.Assert);
        //        }

        //    }

        //    [Conditional(CWJ_LOG_SAVE)]
        //    public static void LogAssertionFormat(string format, params object[] args)
        //    {
        //        UnityEngine.Debug.LogAssertionFormat(format, args);

        //        if (!IsBreakedSave)
        //        {
        //            OnLogMessageReceived(string.Format(format, args), GetSimpleStackTrace(), LogType.Assert);
        //        }

        //    }
        //}

        //#region 18.3.9f1 버전 유니티 Debug 스크립트의 Log출력 빼고 나머지 함수들
        //public static bool developerConsoleVisible
        //{
        //    get => UnityEngine.Debug.developerConsoleVisible;
        //    set => UnityEngine.Debug.developerConsoleVisible = value;
        //}

        //public static ILogger unityLogger => UnityEngine.Debug.unityLogger;

        //public static bool isDebugBuild => UnityEngine.Debug.isDebugBuild;

        //[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        //[Obsolete("Debug.logger is obsolete. Please use Debug.unityLogger instead (UnityUpgradable) -> unityLogger")]
        //public static ILogger Logger => UnityEngine.Debug.unityLogger;

        //[Conditional(CWJ_LOG_ENABLED)] public static void LogException(Exception exception, UnityEngine.Object context) { UnityEngine.Debug.LogException(exception, context); }

        //[Conditional(CWJ_LOG_ENABLED)] public static void LogException(Exception exception) { UnityEngine.Debug.LogException(exception); }

        //[Conditional(CWJ_LOG_ENABLED)] public static void Break() { UnityEngine.Debug.Break(); }

        //[Conditional(CWJ_LOG_ENABLED)] public static void ClearDeveloperConsole() { UnityEngine.Debug.ClearDeveloperConsole(); }

        //[Conditional(CWJ_LOG_ENABLED)] public static void DebugBreak() { UnityEngine.Debug.DebugBreak(); }

        //[Conditional(CWJ_LOG_ENABLED)] public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration) { UnityEngine.Debug.DrawLine(start, end, color, duration); }

        //[Conditional(CWJ_LOG_ENABLED)] public static void DrawLine(Vector3 start, Vector3 end, Color color) { UnityEngine.Debug.DrawLine(start, end, color); }

        //[Conditional(CWJ_LOG_ENABLED)]
        //public static void DrawLine(Vector3 start, Vector3 end, [UnityEngine.Internal.DefaultValue("Color.white")] Color color, [UnityEngine.Internal.DefaultValue("0.0f")] float duration, [UnityEngine.Internal.DefaultValue("true")] bool depthTest)
        //{
        //    UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
        //}

        //[Conditional(CWJ_LOG_ENABLED)] public static void DrawLine(Vector3 start, Vector3 end) { UnityEngine.Debug.DrawLine(start, end); }

        //[Conditional(CWJ_LOG_ENABLED)] public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration) { UnityEngine.Debug.DrawRay(start, dir, color, duration); }

        //[Conditional(CWJ_LOG_ENABLED)]
        //public static void DrawRay(Vector3 start, Vector3 dir, [UnityEngine.Internal.DefaultValue("Color.white")] Color color, [UnityEngine.Internal.DefaultValue("0.0f")] float duration, [UnityEngine.Internal.DefaultValue("true")] bool depthTest)
        //{
        //    UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
        //}

        //[Conditional(CWJ_LOG_ENABLED)] public static void DrawRay(Vector3 start, Vector3 dir) { UnityEngine.Debug.DrawRay(start, dir); }

        //[Conditional(CWJ_LOG_ENABLED)] public static void DrawRay(Vector3 start, Vector3 dir, Color color) { UnityEngine.Debug.DrawRay(start, dir, color); }
        //#endregion 18.3.9f1 버전 유니티 Debug 스크립트의 Log출력 빼고 나머지 함수들

#endregion old code
    }
}