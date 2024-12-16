#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

using UnityEditor;
using UnityEditor.Events;
using UnityEngine.Events;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace CWJ.EditorOnly
{
    public static class UnityEventCopyPaste
    {
        /// <summary>
        /// reflection 테스트용
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TC"></typeparam>
        /// <param name="unityEvent"></param>
        /// <param name="comps"></param>
        /// <param name="method"></param>
        public static void AddListenerViaSelection<T, TC>(this T unityEvent, in TC[] comps, MethodInfo method = null) where T : UnityEvent where TC : UnityObject
        {
            if (method == null)
                method = typeof(UnityObject).GetMethod("EnsureRunningOnMainThread", BindingFlags.NonPublic | BindingFlags.Instance);

            comps.ForEach(c => UnityEventTools.AddPersistentListener(unityEvent, Delegate.CreateDelegate(typeof(UnityAction), c, method) as UnityAction));
        }


        private struct UnityEventData
        {
            public SerializedObject sCompObject;
            public string unityEventName { get; private set; }

            public SerializedProperty sCalls;
            public int originalCallLength { get; private set; }

            public UnityEventData(SerializedObject @null)
            {
                this.sCompObject = null;
                sCalls = null;
                unityEventName = null;
                originalCallLength = 0;
            }

            public UnityEventData(in UnityObject comp, in string unityEventName)
            {
                sCompObject = new SerializedObject(comp);
                this.unityEventName = unityEventName;
                sCalls = sCompObject?.FindProperty(string.Format(CallsPropertyPathFormat, unityEventName));
                originalCallLength = (sCalls != null) ? sCalls.arraySize : 0;
            }

            public UnityEventData(UnityEventData unityEventSerialized)
            {
                this.sCompObject = unityEventSerialized.sCompObject;
                this.sCalls = unityEventSerialized.sCalls;
                this.unityEventName = unityEventSerialized.unityEventName;
                originalCallLength = unityEventSerialized.originalCallLength;
            }
        }

        const string CallsPropertyPathFormat = "{0}.m_PersistentCalls.m_Calls";
        const string LogLineStruct = "({0}) {1}";

        static bool IsOnceCopy = false;
        static List<PersistentCallData> Cache_copiedCalls = null;
        static StringBuilder CopyPasteLog;

        static void PrintCopyPasteLog()
        {
            if (CopyPasteLog == null || CopyPasteLog.Length == 0)
            {
                return;
            }
            DebugLogUtil.PrintLog(CopyPasteLog.ToString(), isPreventStackTrace: true);
            CopyPasteLog.Clear();
            CopyPasteLog = null;
        }
        static void InitCache()
        {
            IsOnceCopy = false;
            if (Cache_copiedCalls != null)
                Cache_copiedCalls.Clear();
            Cache_copiedCalls = null;
            if (CopyPasteLog != null)
                CopyPasteLog.Clear();
            CopyPasteLog = null;
        }

        private static void ClearPersistentCalls(in UnityEventData unityEventSerialized)
        {
            if (unityEventSerialized.originalCallLength == 0)
            {
                return;
            }
            unityEventSerialized.sCalls.ClearArray();
            if (unityEventSerialized.originalCallLength != unityEventSerialized.sCalls.arraySize)
                unityEventSerialized.sCompObject.ApplyModifiedProperties();
        }

        public static void CopyPersistentCalls(in UnityObject sourceComp, in string sourceEventName, in bool isOnceCopy, in int copyIndex = -1)
            => CopyPersistentCalls(new UnityEventData(sourceComp, sourceEventName), isOnceCopy, copyIndex);
        private static void CopyPersistentCalls(UnityEventData copySrcData, in bool isOnceCopy, in int copyIndex = -1)
        {
            if (copySrcData.originalCallLength == 0)
            {
                InitCache();
                return;
            }
            IsOnceCopy = isOnceCopy;
            CopyPasteLog = new StringBuilder();

            void AddCache_NonCheck(int targetIndex, SerializedProperty callSerializedProp)
            {
                var call = new PersistentCallData(callSerializedProp, copySrcData.unityEventName);
                Cache_copiedCalls.Add(call);
                CopyPasteLog.AppendLine(string.Format(LogLineStruct, targetIndex, call));
            }

            void AddCache_CheckOverlap(int targetIndex, SerializedProperty callSerializedProp)
            {
                if (SerializedProperty.DataEquals(callSerializedProp, Cache_copiedCalls[targetIndex].propCache.GetArrayElementAtIndex(targetIndex)))
                    CopyPasteLog.AppendLine(string.Format(LogLineStruct, targetIndex, "<SKIP>"));
                else
                    AddCache_NonCheck(targetIndex, callSerializedProp);
            }

            if (Cache_copiedCalls == null)
                Cache_copiedCalls = new List<PersistentCallData>(capacity: (copyIndex == -1 ? copySrcData.originalCallLength : 1));

            int targetCnt = copyIndex == -1 ? copySrcData.originalCallLength : 1;

            if (Cache_copiedCalls.CountSafe() > targetCnt)
                Cache_copiedCalls.RemoveRange(targetCnt, Cache_copiedCalls.Count - targetCnt);

            Action<int, SerializedProperty> addCache;
            if (Cache_copiedCalls.CountSafe() == targetCnt) // NeedOverlapCheck
                addCache = AddCache_CheckOverlap;
            else
                addCache = AddCache_NonCheck;

            CopyPasteLog.AppendLine("[Copy Log]");

            if (copyIndex == -1)
            {
                for (int i = 0; i < targetCnt; i++)
                    addCache(i, copySrcData.sCalls.GetArrayElementAtIndex(i));
            }
            else
                addCache(copyIndex, copySrcData.sCalls.GetArrayElementAtIndex(copyIndex));

            PrintCopyPasteLog();
        }

        public static void PastePersistentCalls(in UnityObject targetComp, in string targetUnityEventName)
            => PastePersistentCalls(new UnityEventData(targetComp, targetUnityEventName));
        private static void PastePersistentCalls(UnityEventData destData)
        {
            int pasteLength = Cache_copiedCalls.CountSafe();
            if (pasteLength == 0)
            {
                InitCache();
                return;
            }

            CopyPasteLog = new StringBuilder();

            void PasteCall(int targetIndex)
            {
                int dstIndex = destData.originalCallLength + targetIndex;
                destData.sCalls.InsertArrayElementAtIndex(dstIndex);
                var call = new PersistentCallData(destData.sCalls.GetArrayElementAtIndex(dstIndex), destData.unityEventName);
                PersistentCallData.SimpleClone(Cache_copiedCalls[targetIndex], call);
                CopyPasteLog.AppendLine(string.Format(LogLineStruct, targetIndex, call));
            }

            CopyPasteLog.AppendLine("[Paste Log]");

            for (int i = 0; i < pasteLength; ++i)
                PasteCall(i);

            destData.sCompObject.ApplyModifiedProperties();

            PrintCopyPasteLog();

            if (IsOnceCopy)
                InitCache();
        }

        private struct PersistentCallData
        {
            public SerializedProperty propCache { get; private set; }
            private UnityObject propObject;
            private string propPath;
            private SerializedProperty target;
            private SerializedProperty methodName;
            private SerializedProperty mode;

            private SerializedProperty arg_object;
            private SerializedProperty arg_objectTypeName;
            private SerializedProperty arg_int;
            private SerializedProperty arg_float;
            private SerializedProperty arg_string;
            private SerializedProperty arg_bool;

            private SerializedProperty callState;

            internal PersistentCallData(in SerializedProperty serializedProp, in string propPath)
            {
                propCache = serializedProp;
                propObject = serializedProp.serializedObject.targetObject;
                this.propPath = propPath;

                target = serializedProp?.FindPropertyRelative("m_Target");
                methodName = serializedProp?.FindPropertyRelative("m_MethodName");
                mode = serializedProp?.FindPropertyRelative("m_Mode");

                var arguments = serializedProp?.FindPropertyRelative("m_Arguments");
                arg_object = arguments?.FindPropertyRelative("m_ObjectArgument");
                arg_objectTypeName = arguments?.FindPropertyRelative("m_ObjectArgumentAssemblyTypeName");
                arg_int = arguments?.FindPropertyRelative("m_IntArgument");
                arg_float = arguments?.FindPropertyRelative("m_FloatArgument");
                arg_string = arguments?.FindPropertyRelative("m_StringArgument");
                arg_bool = arguments?.FindPropertyRelative("m_BoolArgument");

                callState = serializedProp?.FindPropertyRelative("m_CallState");
            }

            internal static void SimpleClone(in PersistentCallData source, in PersistentCallData dest)
            {
                dest.target.objectReferenceValue = source.target.objectReferenceValue;
                dest.methodName.stringValue = source.methodName.stringValue;
                dest.mode.enumValueIndex = source.mode.enumValueIndex;

                dest.arg_object.objectReferenceValue = source.arg_object.objectReferenceValue;
                dest.arg_objectTypeName.stringValue = source.arg_objectTypeName.stringValue;
                dest.arg_int.intValue = source.arg_int.intValue;
                dest.arg_float.floatValue = source.arg_float.floatValue;
                dest.arg_string.stringValue = source.arg_string.stringValue;
                dest.arg_bool.boolValue = source.arg_bool.boolValue;

                dest.callState.enumValueIndex = source.callState.enumValueIndex;
            }

            public override string ToString()
                => $"[{(UnityEventCallState)callState.enumValueIndex}] {target.objectReferenceValue}.{methodName.stringValue}({GetParamSignature(mode.enumValueIndex)})";

            private string GetParamSignature(in int enumIndex)
            {
                Type GetEventType(in PersistentCallData self)
                {
                    var bindingFlags = BindingFlags.NonPublic
                                                    | BindingFlags.Public
                                                    | BindingFlags.Instance;
                    string[] names = self.propPath.Split('.');
                    object result = self.propObject.GetType()
                                        .GetField(names.FirstOrDefault(), bindingFlags)?.GetValue(self.propObject);
                    while (!(result is UnityEventBase))
                    {
                        result = result.GetType()
                                        .GetField(names.LastOrDefault(), bindingFlags)?.GetValue(result);
                    }

                    return result?.GetType();
                }

                switch (enumIndex)
                {
                    case 0:
                        return $"{GetEventType(this)} (dynamic call)";
                    case 1:
                        return $"{typeof(void)}";
                    case 2:
                        return $"{arg_object.objectReferenceValue.GetType()} = {arg_object.objectReferenceValue}";
                    case 3:
                        return $"{typeof(int)} = {arg_int.intValue}";
                    case 4:
                        return $"{typeof(float)} = {arg_float.floatValue}";
                    case 5:
                        return $"{typeof(string)} = {arg_string.stringValue}";
                    case 6:
                        return $"{typeof(bool)} = {arg_bool.boolValue}";
                    default:
                        return string.Empty;
                }
            }
        }
    }

} 
#endif