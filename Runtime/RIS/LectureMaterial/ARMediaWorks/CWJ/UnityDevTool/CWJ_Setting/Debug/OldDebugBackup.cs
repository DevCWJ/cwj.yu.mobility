
#region Old Copy Code

//#region Print Log Methods

//    [Conditional(CWJ_LOG_ENABLED)] public static void Log(object message) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void Log(object message, UnityEngine.Object context) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogFormat(string format, params object[] args) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogFormat(UnityEngine.Object context, string format, params object[] args) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogWarning(object message) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogWarning(object message, UnityEngine.Object context) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogWarningFormat(string format, params object[] args) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogError(object message) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogError(object message, UnityEngine.Object context) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogErrorFormat(string format, params object[] args) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void Assert(bool condition, string message, UnityEngine.Object context) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void Assert(bool condition, object message, UnityEngine.Object context) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void Assert(bool condition, string message) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void Assert(bool condition, object message) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void Assert(bool condition, UnityEngine.Object context) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void Assert(bool condition) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void AssertFormat(bool condition, string format, params object[] args) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void AssertFormat(bool condition, UnityEngine.Object context, string format, params object[] args) { }

//    //[EditorBrowsable(EditorBrowsableState.Never)]
//    [Obsolete("Assert(bool, string, params object[]) is obsolete. Use AssertFormat(bool, string, params object[]) (UnityUpgradable) -> AssertFormat(*)", true)]
//    [Conditional(CWJ_LOG_ENABLED)] public static void Assert(bool condition, string format, params object[] args) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogAssertion(object message, UnityEngine.Object context) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogAssertion(object message) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogAssertionFormat(UnityEngine.Object context, string format, params object[] args) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogAssertionFormat(string format, params object[] args) { }

//#endregion Print Log Methods

//#region 유니티 Debug.cs의 Log출력관련 외의 나머지 함수들 (verified: 18.3.9f1, 19.1.14f1) 

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogException(Exception exception, UnityEngine.Object context) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void LogException(Exception exception) { }

//    [UnityEngine.Bindings.FreeFunction("PauseEditor")]
//    [Conditional(CWJ_LOG_ENABLED)] public static void Break() { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void ClearDeveloperConsole() { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void DebugBreak() { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void DrawLine(UnityEngine.Vector3 start, UnityEngine.Vector3 end, UnityEngine.Color color, float duration) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void DrawLine(UnityEngine.Vector3 start, UnityEngine.Vector3 end, UnityEngine.Color color) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void DrawLine(UnityEngine.Vector3 start, UnityEngine.Vector3 end, [UnityEngine.Internal.DefaultValue("Color.white")] UnityEngine.Color color, [UnityEngine.Internal.DefaultValue("0.0f")] float duration, [UnityEngine.Internal.DefaultValue("true")] bool depthTest) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void DrawLine(UnityEngine.Vector3 start, UnityEngine.Vector3 end) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void DrawRay(UnityEngine.Vector3 start, UnityEngine.Vector3 dir, UnityEngine.Color color, float duration) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void DrawRay(UnityEngine.Vector3 start, UnityEngine.Vector3 dir, [UnityEngine.Internal.DefaultValue("Color.white")] UnityEngine.Color color, [UnityEngine.Internal.DefaultValue("0.0f")] float duration, [UnityEngine.Internal.DefaultValue("true")] bool depthTest) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void DrawRay(UnityEngine.Vector3 start, UnityEngine.Vector3 dir) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void DrawRay(UnityEngine.Vector3 start, UnityEngine.Vector3 dir, UnityEngine.Color color) { }

//    [Conditional(CWJ_LOG_ENABLED)] public static void Assert(bool condition, string format, params object[] args) {}
//#endregion 유니티 Debug.cs의 Log출력관련 외의 나머지 함수들 (verified: 18.3.9f1, 19.1.14f1) 
#endregion