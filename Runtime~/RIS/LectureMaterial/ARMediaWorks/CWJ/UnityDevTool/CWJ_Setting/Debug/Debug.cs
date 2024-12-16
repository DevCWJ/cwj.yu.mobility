
#if CWJ_LOG_DISABLED || (!UNITY_EDITOR && CWJ_LOG_DISABLED_IN_BUILD)
//^ #if CWJ_LOG_DISABLED || (!UNITY_EDITOR && CWJ_LOG_DISABLED_IN_BUILD) 가 정상

using CWJ;

using UnityEngine;

/// <summary>
/// first commit [19.09.11]
/// <br/>Updated 230221
/// <para>CWJ</para>
/// </summary>
public class Debug
{

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    private static void EditorInitOnLoad()
    {
        InitEmptyLogger();
    }
#endif
    [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void RuntimeInitializeOnLoadMethod()
    {
        CWJ.DebugLogUtil.PrintLogWithClassName(typeof(Debug), "All logging is Disabled. " 
#if UNITY_EDITOR
           + $"[Check the '{CWJ.AccessibleEditor.DebugSetting.DebugSetting_Window.WindowMenuItemPath}']"
#endif
            , UnityEngine.LogType.Log, isComment: false);

        InitEmptyLogger();
    }

    static ILogger _EmptyLogger = null;
    static ILogger EmptyLogger
    {
        get
        {
            if (_EmptyLogger == null)
                _EmptyLogger = new EmptyLogger();
            return _EmptyLogger;
        }
    }
    static ILogger _DefaultLogger = null;
    public static ILogger DefaultLogger
    {
        get
        {
            if (_DefaultLogger == null)
                _DefaultLogger = (ILogger)typeof(UnityEngine.Debug)
                    .GetField(UnityDebugDefaultLoggerFieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).GetValue(null);
            return _DefaultLogger;
        }
    }

    static System.Reflection.FieldInfo _CurLoggerField = null;
    static System.Reflection.FieldInfo CurLoggerField
    {
        get
        {
            if(_CurLoggerField == null)
                _CurLoggerField = typeof(UnityEngine.Debug)
                    .GetField(UnityDebugLoggerFieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            return _CurLoggerField;
        }
    }
    private static void InitEmptyLogger()
    {
        if (CWJ.DebugLogUtil.SetUnityLoggerEnabled == null)
            CWJ.DebugLogUtil.SetUnityLoggerEnabled += SetLoggerEnabled;
        SetLoggerEnabled(false);
    }

    private static void SetLoggerEnabled(bool isEnabled)
    {
        DefaultLogger.logEnabled = isEnabled;
        CurLoggerField?.SetValue(null, isEnabled ? DefaultLogger : EmptyLogger);
    }

    private const string UnityDebugLoggerFieldName = "s_Logger";
    private const string UnityDebugDefaultLoggerFieldName = "s_DefaultLogger";
    public const string DEVELOPED_BY_CWJ = nameof(Debug) + ".cs is checked";
    //[System.Diagnostics.Conditional(CWJ_LOG_ENABLED)] 
    public const string CWJ_LOG_ENABLED = CWJ.AccessibleEditor.DebugSetting.CWJDefineSymbols.CWJ_LOG_ENABLED; //Scripting Define Symbols에 등록 될 일 없어야함

    public static ILogger unityLogger => UnityEngine.Debug.unityLogger;

    public static bool developerConsoleVisible
    {
        get => UnityEngine.Debug.developerConsoleVisible;
        set => UnityEngine.Debug.developerConsoleVisible = value;
    }

    public static bool isDebugBuild => UnityEngine.Debug.isDebugBuild;

    [System.Obsolete("Debug.logger is obsolete. Please use Debug.unityLogger instead (UnityUpgradable) -> unityLogger")]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static ILogger logger => unityLogger;

    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void DrawLine(Vector3 start, Vector3 end, Color color) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void DrawLine(Vector3 start, Vector3 end) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void DrawLine(Vector3 start, Vector3 end, [UnityEngine.Internal.DefaultValue("Color.white")] Color color, [UnityEngine.Internal.DefaultValue("0.0f")] float duration, [UnityEngine.Internal.DefaultValue("true")] bool depthTest) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void DrawRay(Vector3 start, Vector3 dir) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void DrawRay(Vector3 start, Vector3 dir, [UnityEngine.Internal.DefaultValue("Color.white")] Color color, [UnityEngine.Internal.DefaultValue("0.0f")] float duration, [UnityEngine.Internal.DefaultValue("true")] bool depthTest) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void Break() { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void DebugBreak() { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void Log(object message) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void Log(object message, Object context) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogFormat(string format, params object[] args) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogFormat(Object context, string format, params object[] args) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogError(object message) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogError(object message, Object context) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogErrorFormat(string format, params object[] args) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogErrorFormat(Object context, string format, params object[] args) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void ClearDeveloperConsole() { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogException(System.Exception exception) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogException(System.Exception exception, Object context) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogWarning(object message) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogWarning(object message, Object context) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogWarningFormat(string format, params object[] args) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogWarningFormat(Object context, string format, params object[] args) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void Assert(bool condition) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void Assert(bool condition, Object context) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void Assert(bool condition, object message) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void Assert(bool condition, string message) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void Assert(bool condition, object message, Object context) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void Assert(bool condition, string message, Object context) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void AssertFormat(bool condition, string format, params object[] args) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void AssertFormat(bool condition, Object context, string format, params object[] args) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogAssertion(object message) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogAssertion(object message, Object context) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogAssertionFormat(string format, params object[] args) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void LogAssertionFormat(Object context, string format, params object[] args) { }
    [System.Diagnostics.Conditional(CWJ_LOG_ENABLED)]
    public static void Assert(bool condition, string format, params object[] args) { }
}

#endif