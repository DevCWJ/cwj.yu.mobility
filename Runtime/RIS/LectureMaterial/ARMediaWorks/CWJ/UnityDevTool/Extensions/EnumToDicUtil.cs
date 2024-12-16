using CWJ.Serializable;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CWJ
{
    public static partial class EnumUtil
    {
        public static DictionaryVisualized<TE, TUE> AddCallbackInDictionaryByEnum<TE, TUE>(this DictionaryVisualized<TE, TUE> callbackDic
                            , UnityEngine.Object target, string exampleMethod, TE enumStartValue = default(TE), char separatorChr = '_')
                where TE : struct, Enum
                where TUE : UnityEngine.Events.UnityEvent, new()
        {
            if (target == null)
            {
                return null;
            }
            if (!exampleMethod.Contains(separatorChr))
            {
                return null;
            }
            Type t = target.GetType();

            string methodNameBase = exampleMethod.Split(separatorChr)[0] + separatorChr;
            var enumArray = GetEnumArray<TE>();

            //string testMethodName = methodNameBase + enumArray[enumStartIndex].ToString();
            //var methodInfo = t.GetMethod(testMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static);

            if (callbackDic == null)
                callbackDic = new DictionaryVisualized<TE, TUE>();

            int enumStartIndex = enumStartValue.Equals(default(TE)) ? 0 : enumStartValue.ToInt();
            int cnt = enumArray.Count;
            for (int i = enumStartIndex; i < cnt; i++) // ignore [0]NULL
            {
                TE enumElem = enumArray[i];
                string methodName = methodNameBase + enumElem.ToString();
                var ua = ReflectionUtil.ConvertToUnityAction(methodName, target);
                if (ua != null)
                {
                    if (!callbackDic.TryGetValue(enumElem, out var ue))
                    {
                        callbackDic.Add(enumElem, ue = new TUE());
                    }
                    ue.AddListener_New(ua);
                }
                else
                {
                    Debug.LogError($"{t.Name} 에 '{methodName}' 함수 없음", target);
                }
            }
            return callbackDic;
        }

        public static DictionaryVisualized<TE, TUE> GetCallbackDictionaryByEnum<TE, TUE, TP0>(this DictionaryVisualized<TE, TUE> callbackDic
                                    , UnityEngine.Object target, string exampleMethod, int enumStartIndex = 0, char separatorChr = '_')
                where TE : struct, Enum
                where TUE : UnityEngine.Events.UnityEvent<TP0>, new()
        {
            if (target == null)
            {
                return null;
            }
            if (!exampleMethod.Contains(separatorChr))
            {
                return null;
            }
            Type t = target.GetType();

            string methodNameBase = exampleMethod.Split(separatorChr)[0] + separatorChr;
            var enumArray = GetEnumArray<TE>();

            //string testMethodName = methodNameBase + enumArray[enumStartIndex].ToString();
            //var methodInfo = t.GetMethod(testMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static);

            if (callbackDic == null)
                callbackDic = new DictionaryVisualized<TE, TUE>();
            int cnt = enumArray.Count;
            for (int i = enumStartIndex; i < cnt; i++) // ignore [0]NULL
            {
                TE enumElem = enumArray[i];
                string methodName = methodNameBase + enumElem.ToString();
                var ua = ReflectionUtil.ConvertToUnityAction<TP0>(methodName, target);
                if (ua != null)
                {
                    if (!callbackDic.TryGetValue(enumElem, out var ue))
                    {
                        callbackDic.Add(enumElem, ue = new TUE());
                    }
                    ue.AddListener_New(ua);
                }
                else
                {
                    Debug.LogError($"{t.Name} 에 '{methodName}' 함수 없음", target);
                }
            }
            return callbackDic;
        }
    }
}
