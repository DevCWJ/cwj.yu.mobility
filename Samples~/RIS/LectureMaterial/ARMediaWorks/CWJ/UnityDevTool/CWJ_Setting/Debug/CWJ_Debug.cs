using System.Collections.Generic;
using System;
using CWJ;
using UnityEngine;
/// <summary>
/// 인스펙터에서 Component의 최상단에 있는 [Debug Setting]의 isIgnoreDebug를 끄고싶을때만(해당 Component에서 실행되는 CWJ_Debug를 차단) CWJ_Debug를 쓰면됨 
/// </summary>
public class CWJ_Debug
{
    #region Type (HashCode)
    public static bool IsAllowSpecifiedType_prev;
    public static bool IsAllowSpecifiedType;
    public static bool IsIgnoreSpecifiedType_prev;
    public static bool IsIgnoreSpecifiedType;

    private static HashSet<int> AllowTypes = new HashSet<int>();
    public static void ClearAllowTypes() => AllowTypes.Clear();
    public static bool IsExistsInAllowTypes(int id) => AllowTypes.Count > 0 && AllowTypes.Contains(id);
    public static void RegistAllowType(int typeID, bool isAddAllow)
    {
        if (isAddAllow)
        {
            AllowTypes.Add(typeID);
        }
        else
        {
            AllowTypes.Remove(typeID);
        }
    }

    public static void RegistAllowType(int[] typeIDs, bool isAddAllow)
    {
        if (isAddAllow)
        {
            AllowTypes.AddRange(typeIDs);
        }
        else
        {
            AllowTypes.RemoveRange(typeIDs);
        }
    }


    private static HashSet<int> IgnoreTypes = new HashSet<int>();
    public static void ClearIgnoreTypes() => IgnoreTypes.Clear();
    public static bool IsExistsInIgnoreTypes(int id) => IgnoreTypes.Count > 0 && IgnoreTypes.Contains(id);
    public static void RegistIgnoreType(int id, bool isAddIgnore)
    {
        if (isAddIgnore)
        {
            IgnoreTypes.Add(id);
        }
        else
        {
            IgnoreTypes.Remove(id);
        }
    }

    public static bool IsAbleToUseCWJDebug()
    {
#if UNITY_EDITOR
        if (IsIgnoreSpecifiedType && IgnoreTypes.Count > 0 && IgnoreTypes.Contains(new System.Diagnostics.StackTrace().GetFrame(2).GetMethod().ReflectedType.GetHashCode()))
        {
            return false;
        }
        if (IsAllowSpecifiedType && (AllowTypes.Count == 0 || !AllowTypes.Contains(new System.Diagnostics.StackTrace().GetFrame(2).GetMethod().ReflectedType.GetHashCode())))
        {
            return false;
        }
#endif
        return true;
    }
    #endregion

    #region Obj (InstanceID)

    public static bool IsAllowSpecifiedObj_prev;
    public static bool IsAllowSpecifiedObj;
    public static bool IsIgnoreSpecifiedObj_prev;
    public static bool IsIgnoreSpecifiedObj;

    private static HashSet<int> AllowObjs = new HashSet<int>();
    public static bool IsExistsInAllowObjs(int id) => AllowObjs.Count > 0 && AllowObjs.Contains(id);
    public static void ClearAllowObjs() => AllowObjs.Clear();
    public static void RegistAllowObj(int id, bool isAddAllow)
    {
        if (isAddAllow)
        {
            AllowObjs.Add(id);
        }
        else
        {
            AllowObjs.Remove(id);
        }
    }


    private static HashSet<int> IgnoreObjs = new HashSet<int>();
    public static bool IsExistsInIgnoreObjs(int id) => IgnoreObjs.Count > 0 && IgnoreObjs.Contains(id);
    public static void ClearIgnoreObjs() => IgnoreObjs.Clear();
    public static void RegistIgnoreObj(int id, bool isAddIgnore)
    {
        if (isAddIgnore)
        {
            IgnoreObjs.Add(id);
        }
        else
        {
            IgnoreObjs.Remove(id);
        }
    }

    public static bool IsAbleToUseCWJDebug(UnityEngine.Object obj)
    {
#if UNITY_EDITOR
        if (IsIgnoreSpecifiedType && IgnoreTypes.Count > 0 && IgnoreTypes.Contains(new System.Diagnostics.StackTrace().GetFrame(2).GetMethod().ReflectedType.GetHashCode()))
        {
            return false;
        }
        if (IsAllowSpecifiedType && (AllowTypes.Count == 0 || !AllowTypes.Contains(new System.Diagnostics.StackTrace().GetFrame(2).GetMethod().ReflectedType.GetHashCode())))
        {
            return false;
        }

        if (IsIgnoreSpecifiedObj && AllowObjs.Count > 0 && AllowObjs.Contains(obj.GetInstanceID()))
        {
            return false;
        }
        if (IsAllowSpecifiedObj && (AllowObjs.Count == 0 || !AllowObjs.Contains(obj.GetInstanceID())))
        {
            return false;
        }
#endif
        return true;
    }

    #endregion

    #region Print Log Methods
    public static void Log(object message) { if (IsAbleToUseCWJDebug()) Debug.Log(message); }

    public static void Log(object message, UnityEngine.Object context) { if (IsAbleToUseCWJDebug(context)) Debug.Log(message, context); }

    public static void LogFormat(string format, params object[] args) { if (IsAbleToUseCWJDebug()) Debug.LogFormat(format, args); }

    public static void LogFormat(UnityEngine.Object context, string format, params object[] args) { if (IsAbleToUseCWJDebug(context)) Debug.LogFormat(context, format, args); }

    public static void LogWarning(object message) { if (IsAbleToUseCWJDebug()) Debug.LogWarning(message); }

    public static void LogWarning(object message, UnityEngine.Object context) { if (IsAbleToUseCWJDebug(context)) Debug.LogWarning(message, context); }

    public static void LogWarningFormat(string format, params object[] args) { if (IsAbleToUseCWJDebug()) Debug.LogWarningFormat(format, args); }

    public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args) { if (IsAbleToUseCWJDebug(context)) Debug.LogWarningFormat(context, format, args); }

    public static void LogError(object message) { if (IsAbleToUseCWJDebug()) Debug.LogError(message); }

    public static void LogError(object message, UnityEngine.Object context) { if (IsAbleToUseCWJDebug(context)) Debug.LogError(message, context); }

    public static void LogErrorFormat(string format, params object[] args) { if (IsAbleToUseCWJDebug()) Debug.LogErrorFormat(format, args); }

    public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args) { if (IsAbleToUseCWJDebug(context)) Debug.LogErrorFormat(context, format, args); }

    public static void Assert(bool condition, string message, UnityEngine.Object context) { if (IsAbleToUseCWJDebug(context)) Debug.Assert(condition, message, context); }

    public static void Assert(bool condition, object message, UnityEngine.Object context) { if (IsAbleToUseCWJDebug(context)) Debug.Assert(condition, message, context); }

    public static void Assert(bool condition, string message) { if (IsAbleToUseCWJDebug()) Debug.Assert(condition, message); }

    public static void Assert(bool condition, object message) { if (IsAbleToUseCWJDebug()) Debug.Assert(condition, message); }

    public static void Assert(bool condition, UnityEngine.Object context) { if (IsAbleToUseCWJDebug(context)) Debug.Assert(condition, context); }

    public static void Assert(bool condition) { if (IsAbleToUseCWJDebug()) Debug.Assert(condition); }

    public static void AssertFormat(bool condition, string format, params object[] args) { if (IsAbleToUseCWJDebug()) Debug.AssertFormat(condition, format, args); }

    public static void AssertFormat(bool condition, UnityEngine.Object context, string format, params object[] args) { if (IsAbleToUseCWJDebug(context)) Debug.AssertFormat(condition, context, format, args); }

    //[EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Assert(bool, string, params object[]) is obsolete. Use AssertFormat(bool, string, params object[]) (UnityUpgradable) -> AssertFormat(*)", true)]
    public static void Assert(bool condition, string format, params object[] args)
    { if (IsAbleToUseCWJDebug()) Debug.Assert(condition, format, args); }

    public static void LogAssertion(object message, UnityEngine.Object context) { if (IsAbleToUseCWJDebug(context)) Debug.LogAssertion(message, context); }

    public static void LogAssertion(object message) { if (IsAbleToUseCWJDebug()) Debug.LogAssertion(message); }

    public static void LogAssertionFormat(UnityEngine.Object context, string format, params object[] args) { if (IsAbleToUseCWJDebug(context)) Debug.LogAssertionFormat(context, format, args); }

    public static void LogAssertionFormat(string format, params object[] args) { if (IsAbleToUseCWJDebug()) Debug.LogAssertionFormat(format, args); }

    #endregion Print Log Methods

    #region 유니티 Debug.cs의 Log출력관련 외의 나머지 함수들 (verified: 18.3.9f1, 19.1.14f1) 

    public static void LogException(Exception exception, UnityEngine.Object context) { if (IsAbleToUseCWJDebug(context)) Debug.LogException(exception, context); }

    public static void LogException(Exception exception) { if (IsAbleToUseCWJDebug()) Debug.LogException(exception); }

    public static void Break() { if (IsAbleToUseCWJDebug()) Debug.Break(); }

    public static void ClearDeveloperConsole() { if (IsAbleToUseCWJDebug()) Debug.ClearDeveloperConsole(); }

    public static void DebugBreak() { if (IsAbleToUseCWJDebug()) Debug.DebugBreak(); }

    public static void DrawLine(UnityEngine.Vector3 start, UnityEngine.Vector3 end, UnityEngine.Color color, float duration) { if (IsAbleToUseCWJDebug()) Debug.DrawLine(start, end, color, duration); }

    public static void DrawLine(UnityEngine.Vector3 start, UnityEngine.Vector3 end, UnityEngine.Color color) { if (IsAbleToUseCWJDebug()) Debug.DrawLine(start, end, color); }

    public static void DrawLine(UnityEngine.Vector3 start, UnityEngine.Vector3 end, [UnityEngine.Internal.DefaultValue("Color.white")] UnityEngine.Color color, [UnityEngine.Internal.DefaultValue("0.0f")] float duration, [UnityEngine.Internal.DefaultValue("true")] bool depthTest) { if (IsAbleToUseCWJDebug()) Debug.DrawLine(start, end, color, duration, depthTest); }

    public static void DrawLine(UnityEngine.Vector3 start, UnityEngine.Vector3 end) { if (IsAbleToUseCWJDebug()) Debug.DrawLine(start, end); }

    public static void DrawRay(UnityEngine.Vector3 start, UnityEngine.Vector3 dir, UnityEngine.Color color, float duration) { if (IsAbleToUseCWJDebug()) Debug.DrawRay(start, dir, color, duration); }

    public static void DrawRay(UnityEngine.Vector3 start, UnityEngine.Vector3 dir, [UnityEngine.Internal.DefaultValue("Color.white")] UnityEngine.Color color, [UnityEngine.Internal.DefaultValue("0.0f")] float duration, [UnityEngine.Internal.DefaultValue("true")] bool depthTest) { if (IsAbleToUseCWJDebug()) Debug.DrawRay(start, dir, color, duration, depthTest); }

    public static void DrawRay(UnityEngine.Vector3 start, UnityEngine.Vector3 dir) { if (IsAbleToUseCWJDebug()) Debug.DrawRay(start, dir); }

    public static void DrawRay(UnityEngine.Vector3 start, UnityEngine.Vector3 dir, UnityEngine.Color color) { if (IsAbleToUseCWJDebug()) Debug.DrawRay(start, dir, color); }

    #endregion 유니티 Debug.cs의 Log출력관련 외의 나머지 함수들 (verified: 18.3.9f1, 19.1.14f1) 
}