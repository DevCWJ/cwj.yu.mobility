using System;
using System.Collections.Generic;

using UnityEngine;
using CWJ.AccessibleEditor;
using System.Linq;
using CWJ.Serializable;
using System.Reflection;
using UnityEditor;

namespace CWJ.EditorOnly
{
    public class SerializationCache_ScriptableObject : CWJScriptableObject
    {
        [System.Serializable] public class ScriptNameToPathDictionary : SerializedDictionary<string, string> { }
        [SerializableDictionary(isReadonly: true), SerializeField] private ScriptNameToPathDictionary assetPathCache = new ScriptNameToPathDictionary();

        [System.Serializable] public class StrCodeContainerDictionary : SerializedDictionary<string, CodeContainer> { }
        [SerializableDictionary(isReadonly: true)] public StrCodeContainerDictionary codeContainerCache = new StrCodeContainerDictionary();

        public override void OnConstruct()
        {
            base.OnConstruct();
            assetPathCache = new ScriptNameToPathDictionary();
            codeContainerCache = new StrCodeContainerDictionary();
        }

        public string GetScriptPath(Type targetType)
        {
            return GetScriptPath(targetType.Name);
        }

        public string GetScriptPath(string scriptName)
        {
            if(!assetPathCache.TryGetValue(scriptName, out string path) || string.IsNullOrEmpty( path))
            {
                if (!AccessibleEditorUtil.TryGetScriptPath(scriptName, out path))
                {
                    return String.Empty;
                }

                assetPathCache.Add(scriptName, path);
            }

            return path;
        }

        public void WriteCodeContainer()
        {
            Action<CodeContainer> writeCode = (codeData) =>
            {
                if (string.IsNullOrEmpty(codeData.name)) { return; }

                string scriptPath = codeData.scriptPath;

                if (string.IsNullOrEmpty(scriptPath)) { return; }

                string[] lines = System.IO.File.ReadAllLines(scriptPath);

                if (lines.Length == 0) { return; }

                int index = lines.FindIndex((l) => l.Contains(codeData.declareCode));
                if (index == -1) { return; }

                lines[index] = lines[index].Split(new string[] { codeData.declareCode }, StringSplitOptions.None)[0] +
                                codeData.declareCode + " = " + codeData.assignCode + ";";

                System.IO.File.WriteAllLines(scriptPath, lines);
            };

            codeContainerCache.Values.ForEach(writeCode);

            assetPathCache.Clear();
            codeContainerCache.Clear();

            SaveScriptableObj();

            AssetDatabase.Refresh();
        }


    }

    [Serializable]
    public struct CodeContainer
    {
        [SerializeField] private string _name;
        public string name => _name;

        public string scriptPath;

        [SerializeField] public string declareCode;
        [SerializeField] public string assignCode;

        //수정된 스크립트Type과 fieldInfo, value값을 SerializeDataContainer로 변환
        public CodeContainer(FieldInfo fieldInfo, object value, bool isIncludeBaseClass)
        {
            this._name = null;
            this.scriptPath = null;
            this.declareCode = null;
            this.assignCode = null;
            //Get
            string realTypeNameCode = null;
            string assignCode = ConvertToAssignCode(fieldInfo.FieldType, ref realTypeNameCode, value);

            if (assignCode == null)
            {
                return;
            }

            Type scriptType = fieldInfo.DeclaringType;
            string declareCode = GetDeclareCode(scriptType, out scriptPath, ref realTypeNameCode, fieldInfo, isIncludeBaseClass);

            if (declareCode == null)
            {
                return;
            }

            assignCode = ConvertToAssignCode(fieldInfo.FieldType, ref realTypeNameCode, value); //한번더 거쳐야함

            //Assign
            this._name = System.IO.Path.GetFileNameWithoutExtension(scriptPath) + "/" + fieldInfo.Name;
            this.declareCode = declareCode;

            this.assignCode = assignCode;
        }

        //선언문 code 가져오는 함수, 추가로 assetPath와 fieldTypeName을 정정
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptType"></param>
        /// <param name="assetPath"></param>
        /// <param name="fieldTypeName"></param>
        /// <param name="fieldInfo"></param>
        /// <param name="isIncludeBaseClass"></param>
        /// <returns></returns>
        private string GetDeclareCode(Type scriptType, out string assetPath, ref string fieldTypeName, FieldInfo fieldInfo, bool isIncludeBaseClass)
        {
            assetPath = ForcedFieldSerialization.ScriptableObj.GetScriptPath(scriptType.Name);

            if (string.IsNullOrEmpty(assetPath))
            {
                return null;
            }

            var lines = System.IO.File.ReadAllLines(assetPath).Select(l =>
            {
                string tl = l.Trim();
                if (tl.Length == 0 || tl.StartsWith("//") || !tl.Contains(fieldInfo.Name))
                {
                    return null;
                }
                return tl;
            }).Where(l => l != null);

            int lineLength = lines.Count();

            string accessLevel;
            if (fieldInfo.IsPrivate) accessLevel = "private";
            else if (fieldInfo.IsPublic) accessLevel = "public";
            else accessLevel = "protected";
            accessLevel += " ";

            //namespace때문에 loop 돌며 체크할 함수 (사실 걍 namespace Full로 입력하게 놔둘거면 loop시킬필요도 없어서 이 Func도 필요없어짐)
            Func<string, string> getCorrectDeclareCode = (originDeclareCode) =>
            {
                string a = accessLevel + originDeclareCode;
                bool isPrivate = fieldInfo.IsPrivate;
                var line = lines.FirstOrDefault((l) =>
                            {
                                if (l == null)
                                {
                                    return false;
                                }
                                if (l.EndsWith(a + ";") || l.Contains(a + " ") || l.Contains(a + "=") || l.Contains(a))
                                {
                                    return true;
                                }
                                if (isPrivate && l.Contains(originDeclareCode))
                                {
                                    accessLevel = string.Empty;
                                    return true;
                                }
                                return false;
                            });

                if (string.IsNullOrEmpty(line))
                {
                    return null;
                }
                return accessLevel + originDeclareCode;
            };

            string declareCode = string.Empty;
            string[] typeNameSpaceSplits = fieldTypeName.Split('.');
            string typeNameSpace = fieldTypeName;

            if (lineLength > 0)
            {
                for (int i = 0; i < typeNameSpaceSplits.Length; ++i)
                {
                    string code = getCorrectDeclareCode(typeNameSpace + " " + fieldInfo.Name);
                    if (code != null)
                    {
                        declareCode = code;
                        break;
                    }
                    if (i + 1 < typeNameSpaceSplits.Length)
                    {
                        typeNameSpace = typeNameSpace.Replace(typeNameSpaceSplits[i] + ".", string.Empty);
                    }
                }
            }


            if (string.IsNullOrEmpty(declareCode))
            {
                if (isIncludeBaseClass)
                {
                    Type[] baseTypes = ReflectionUtil.GetAllBaseClassTypes(targetType: scriptType);
                    if (baseTypes.Length >= 2 && baseTypes[1] != null)
                    {
                        return GetDeclareCode(baseTypes[1], out assetPath, ref fieldTypeName, fieldInfo, isIncludeBaseClass);
                    }
                }
                else
                {
                    Debug.LogError("ERROR hh");
                }
                return null;
            }

            fieldTypeName = typeNameSpace; //불필요한 namespace를 제거한 상태의 깔끔한 typeName을 가져옴

            return declareCode;
        }

        //object를 code로 변환. 배열이 아직 안되어있음
        public static string ConvertToAssignCode(Type fieldType, ref string typeName, object value)
        {
            bool isDecidedTypeName = !string.IsNullOrEmpty(typeName);
            if (!isDecidedTypeName)
            {
                typeName = fieldType.FullName;
                if (typeName.Contains('+')) typeName = typeName.Replace('+', '.');
            }
            if (value == null)
            {
                return "null";
            }
            if (typeof(bool) == fieldType)
            {
                typeName = "bool";
                return (bool)value ? "true" : "false";
            }
            else if (typeof(byte) == fieldType)
            {
                typeName = "byte";
            }
            else if (typeof(short) == fieldType)
            {
                typeName = "short";
            }
            else if (typeof(int) == fieldType)
            {
                typeName = "int";
            }
            else if (typeof(long) == fieldType)
            {
                typeName = "long";
            }
            else if (typeof(float) == fieldType)
            {
                typeName = "float";
                return $"{value}f";
            }
            else if (typeof(double) == fieldType)
            {
                typeName = "double";
                return $"{value}f";
            }
            else if (typeof(string) == fieldType)
            {
                typeName = "string";
                return $"\"{value}\"";
            }
            else if (typeof(Vector2Int) == fieldType || typeof(Vector3Int) == fieldType)
            {
                return $"new {typeName}{value.ToString()}";
            }
            else if (typeof(Vector2) == fieldType)
            {
                Vector2 vector = (Vector2)value;
                return $"new {typeName}({vector.x}f, {vector.y}f)";
            }
            else if (typeof(Vector3) == fieldType)
            {
                Vector3 vector = (Vector3)value;
                return $"new {typeName}({vector.x}f, {vector.y}f, {vector.z}f)";
            }
            else if (typeof(Vector4) == fieldType)
            {
                Vector4 vector = (Vector4)value;
                return $"new {typeName}({vector.x}f, {vector.y}f, {vector.z}f, {vector.w}f)";
            }
            else if (typeof(Quaternion) == fieldType)
            {
                Quaternion vector = (Quaternion)value;
                return $"new {typeName}({vector.x}f, {vector.y}f, {vector.z}f, {vector.w}f)";
            }
            else if (fieldType == typeof(Color))
            {
                if (!isDecidedTypeName)
                {
                    typeName = nameof(UnityEngine) + "." + nameof(Color);
                }
                Color color = (Color)value;
                return $"new {typeName}({color.r}f, {color.g}f, {color.b}f, {color.a}f)";
            }
            else if (fieldType == typeof(Bounds))
            {
                Bounds bounds = (Bounds)value;
                return $"new {typeName}(new {nameof(Vector3)}({bounds.center.x}f, {bounds.center.y}f, {bounds.center.z}f), new {nameof(Vector3)}({bounds.size.x}f, {bounds.size.y}f, {bounds.size.z}f))";
            }
            else if (fieldType == typeof(Rect))
            {
                Rect rect = (Rect)value;
                return $"new {typeName}(new {nameof(Vector2)}({rect.position.x}f, {rect.position.y}f), new {nameof(Vector2)}({rect.size.x}f, {rect.size.y}f))";
            }
            else if (fieldType.IsEnum)
            {
                return $"{typeName}.{value.ToString()}";
            }
            else //배열 추가해야함...근데 추가안할듯
            {
                return null;
            }

            return value.ToString();
        }

        public static bool IsPossibleConvertToCode(Type fieldType)
        {
            string s = null;
            return !string.IsNullOrEmpty(ConvertToAssignCode(fieldType, ref s, fieldType.GetDefaultValue()));
        }

    }
}