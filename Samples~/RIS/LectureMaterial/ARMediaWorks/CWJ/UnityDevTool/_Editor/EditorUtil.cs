#if UNITY_EDITOR
using System;

using Microsoft.CSharp;
using System.CodeDom;
using UnityEditor;
using System.Text.RegularExpressions;

namespace CWJ.EditorOnly
{
    public static class EditorUtil
    {
      public const bool isCWJDebuggingMode =
#if CWJ_EDITOR_DEBUG_ENABLED
                true;
#else
                false;
#endif

        public static void OpenScriptFromStackTrace(string stackTrace)
        {
            var regex = Regex.Match(stackTrace, @"\(at .*\.cs:[0-9]+\)$", RegexOptions.Multiline);
            if (regex.Success)
            {
                string line = stackTrace.Substring(regex.Index + 4, regex.Length - 5);
                int lineSeparator = line.IndexOf(':');
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(line.Substring(0, lineSeparator));
                if (script != null)
                    AssetDatabase.OpenAsset(script, int.Parse(line.Substring(lineSeparator + 1)));
            }
        }

        public static string[] ToCsFriendlyTypeNames(this Type[] systemTypes)
        {
            string[] typeNames = new string[systemTypes.Length];

            //Cannot use C# Dotnet 2.0 without Editor Folder
            using (var provider = new CSharpCodeProvider())
            {
                for (int i = 0; i < systemTypes.Length; i++)
                {
                    if (string.Equals(systemTypes[i].Namespace, ReflectionUtil.SystemNameSpace))
                    {
                        string csFriendlyName = provider.GetTypeOutput(new CodeTypeReference(systemTypes[i]));
                        if (csFriendlyName.IndexOf(ReflectionUtil.Dot) == -1)
                        {
                            typeNames[i] = csFriendlyName;
                            continue;
                        }
                    }

                    typeNames[i] = systemTypes[i].Name;
                }
            }

            return typeNames;
        }

    }
} 
#endif