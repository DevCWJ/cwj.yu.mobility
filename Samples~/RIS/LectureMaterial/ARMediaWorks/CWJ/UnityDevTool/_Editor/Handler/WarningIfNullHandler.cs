using System;
using System.Reflection;
using System.Collections;
using UnityEditor;

using UnityEngine;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    public class ErrorIfNullHandler
    {
         [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            CustomAttributeHandler.EditorSceneOpenedEvent += RequiredFieldLooper; //씬 열 때 + 프로젝트 열 때
            CustomAttributeHandler.ExitingEditModeEvent += RequiredFieldLooper; // 런타임 첫프레임
            CustomAttributeHandler.EditorWillSaveAfterModifiedEvent+= RequiredFieldLooper; //유니티 저장 시도를 할 때
            CustomAttributeHandler.BeforeBuildEvent+= RequiredFieldLooper; //빌드직전.
        }

        private static void RequiredFieldLooper(MonoBehaviour comp, Type classType)
        {
            FieldInfo[] requiredFields = classType.GetFieldsInAttribute<ErrorIfNullAttribute>();
            bool objIsInAcitve = comp.gameObject.activeInHierarchy;
            foreach (var field in requiredFields)
            {
                string nullLog;
                if(!objIsInAcitve && field.GetCustomAttribute<ErrorIfNullAttribute>().isWarningOnlyActive)
                {
                    continue;
                }
                if (field.IsNullWithErrorMsg(comp, out nullLog, AttributeUtil.NullCheckType.All))
                {
                    PrintErrorLog(classType, field, nullLog, comp);
                    continue;
                }
            }
        }

        private static void PrintErrorLog(Type type, FieldInfo field, string nullLog, MonoBehaviour comp)
        {
            EditorApplication.isPlaying = false;

            string sceneName = comp?.gameObject?.scene.name;
            string objName = comp?.gameObject?.name;
            string scriptName = type?.Name;
            string fieldName = field?.Name;

            typeof(ErrorIfNullAttribute).PrintLogWithClassName($"{"[Null]".SetColor(Color.magenta)} {scriptName}.{fieldName} is {nullLog} \n[Info]\nSceneName: {sceneName}\nObjectName: {objName}\nComponentName: {scriptName}\nFieldName: {fieldName}", LogType.Error, isComment: false, obj: comp, isPreventOverlapMsg: false);
            BuildEventSystem.BuildCancel(byTheUser: false, comments: "Plz Check : " + nameof(ErrorIfNullAttribute));
        }
    }
}