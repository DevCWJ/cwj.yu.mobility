#if UNITY_EDITOR
using UnityEngine;
using CWJ.Serializable;
using UnityEditor;
using System;
using System.IO;

namespace CWJ.AccessibleEditor
{
	[System.Serializable] public class StrScriptableObjDictionary : SerializedDictionary<string, CWJScriptableObject> { }

	[CreateAssetMenu(fileName = "ScriptableObjectStore", menuName = "CWJ/Editor/Cache/InitCacheThisFolder(Dangerous)")]
	public sealed class ScriptableObjectStore : CWJScriptableObject
	{
		private static ScriptableObjectStore _Instance = null;

		public static ScriptableObjectStore Instanced
		{
			get
			{
				if (!_Instance)
				{
					_ThisStoreFilePath = null;
					//string path = MyPath;
					string path = GetCacheFilePath<ScriptableObjectStore>();
					if (path == null)
					{
						return null;
					}

					var obj = AssetDatabase.LoadAssetAtPath<ScriptableObjectStore>(path);
					if (!obj)
					{
						//if (!MyPath.IsFolderExists(false, isPrintLog: false))
						//{
						//    CWJ_EditorEventHelper.OnUnityDevToolDelete();
						//    typeof(ScriptableObjectStore).PrintLogWithClassName($"CWJ.UnityDevTool is Deleted.\nor {nameof(ScriptableObjectStore)}'s PATH is Wrong", LogType.Error);
						//}
						////obj = CreateScriptableObj<ScriptableObjectStore>(MyPath);
						if (!_ReserveCreate)
						{
							_ReserveCreate = true;
							EditorApplication.delayCall += CreateMySelf;
						}

						return null;
					}

					_Instance = obj;
				}

				return _Instance;
			}
		}


		private static bool _ReserveCreate = false;

		static void CreateMySelf()
		{
			string path = GetCacheFilePath<ScriptableObjectStore>();
			if (path == null)
				return;
			_Instance = AssetDatabase.LoadAssetAtPath<ScriptableObjectStore>(path);
			if (!_Instance)
			{
				_ThisStoreFilePath = null;
				_Instance = ScriptableObject.CreateInstance<ScriptableObjectStore>();
				AssetDatabase.CreateAsset(_Instance, /*AssetDatabase.GenerateUniqueAssetPath(*/path /*)*/);
				_Instance.OnConstruct();
				EditorUtility.SetDirty(_Instance);
			}
		}

		private const string ThisStoreName = nameof(ScriptableObjectStore);
		private const string FilterOfThisStore = "t:" + nameof(ScriptableObjectStore);
		private static string _ThisStoreFilePath = null;

		public static bool TryGetStoreFilePath(out string path)
		{
			if (_ThisStoreFilePath == null)
			{
				var asstes = AssetDatabase.FindAssets(FilterOfThisStore);
				if (asstes.LengthSafe() > 0)
				{
					path = (_ThisStoreFilePath = AssetDatabase.GUIDToAssetPath(asstes[0]));
					return true;
				}
				else
				{
					path = null;
					return false;
				}
			}

			path = _ThisStoreFilePath;
			return true;
		}


	    private static void EnsureEditorCacheFolderExists(string fullPath)
	    {
		    if (string.IsNullOrEmpty(fullPath)) return;

		    string[] subFolders = fullPath.Replace('\\', '/').Split('/'); // "CWJ", "UnityDevTool", "_EditorCache"

		    string currentPath = subFolders[0];
		    if (!currentPath.StartsWith("Assets"))
		    {
			    Debug.LogError("EnsureEditorCacheFolderExists: Path must start with 'Assets/'");
			    return;
		    }

		    for (int i = 1; i < subFolders.Length; i++)
		    {
			    string folderName = subFolders[i];
			    currentPath = currentPath + "/" + folderName;

			    if (!AssetDatabase.IsValidFolder(currentPath))
			    {
				    string parentFolder = System.IO.Path.GetDirectoryName(currentPath)?.Replace('\\', '/');
				    if (!string.IsNullOrEmpty(parentFolder))
					    AssetDatabase.CreateFolder(parentFolder, folderName);
			    }
		    }
	    }

	    private static string GetScriptDirectory<T>(string scriptName = null)
	    {
		    scriptName ??= typeof(T).Name;
		    var scriptPaths = AssetDatabase.FindAssets($"{scriptName} t:script");
		    if (scriptPaths.LengthSafe() == 0) return null;

		    string scriptPath = AssetDatabase.GUIDToAssetPath(scriptPaths[0]);
		    return Path.GetDirectoryName(scriptPath)!.Replace('\\', '/');
	    }

	    public static string GetCacheFilePath<T>(string typeName = null) where T : CWJScriptableObject
	    {
		    typeName ??= typeof(T).Name;

		    // 대상 자산 검색
		    var findAssets = AssetDatabase.FindAssets($"t:{typeName}");
		    if (findAssets.LengthSafe() > 0)
			    return AssetDatabase.GUIDToAssetPath(findAssets[0]);

		    // ScriptableObjectStore.asset 검색
		    var storeAssets = AssetDatabase.FindAssets($"t:{nameof(ScriptableObjectStore)}");
		    var assetPath = storeAssets.LengthSafe() > 0 ? AssetDatabase.GUIDToAssetPath(storeAssets[0]) : null;
		    if (assetPath != null)
		    {
			    return $"{Path.GetDirectoryName(assetPath).Replace('\\', '/')}/{typeName}.asset";
		    }

		    // ScriptableObjectStore.cs가 존재하는 경우 _Cache 폴더 생성
		    string storeScriptDirectory = GetScriptDirectory<ScriptableObjectStore>();
		    if (string.IsNullOrEmpty(storeScriptDirectory))
		    {
			    CWJ.DebugLogUtil.PrintLogError($"Error: {nameof(ScriptableObjectStore)}.cs 파일이 존재하지 않습니다. 작업을 중단합니다.");
			    return null;
		    }

		    string unityDevToolDir = PathUtil.GetParentDirectory(storeScriptDirectory, 2).Replace('\\', '/');
		    string cacheDirectory = unityDevToolDir + "/_EditorCache";
		    EnsureEditorCacheFolderExists(cacheDirectory);

		    return $"{cacheDirectory}/{typeName}.asset";
	    }

        //const string CachePathFormat = "Assets/CWJ/UnityDevTool/_Cache/{0}.asset";

        //public readonly static string MyPath = string.Format(CachePathFormat, nameof(ScriptableObjectStore));

        private static T CreateScriptableObj<T>(string path) where T : CWJScriptableObject
        {
            if (string.IsNullOrEmpty(path))
            {
                if (!AccessibleEditorUtil.TryGetScriptPath(typeof(T).Name, out path))
                    return null;
                else
	                path = path + ".asset";
            }
            T ins = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(ins, /*AssetDatabase.GenerateUniqueAssetPath(*/path/*)*/);
            ins.OnConstruct();
	        EditorUtility.SetDirty(ins);

            return ins;
        }

        [SerializeField, SerializableDictionary(isReadonly: true)] StrScriptableObjDictionary scriptableObjDic = new StrScriptableObjDictionary();

        const string SearchFileTypeFormat = "t:{0}";

        // readonly static string[] CWJFolderPath = new[] { "Assets/CWJ" };
        public T GetScriptableObj<T>() where T : CWJScriptableObject
        {
            string key = typeof(T).FullName;
	        bool hasKey = scriptableObjDic.TryGetValue(key, out var value);
	        if (hasKey && !value.IsNullOrMissing())
            {
                try
                {
                    T returnVal = (T)value;
                    return returnVal;
                }
                catch (System.InvalidCastException e)
                {
                    Debug.LogError("CWJ폴더를 삭제후 다시 import해주세요\n" + e.ToString());
                }
            }
            string typeName = typeof(T).Name;
	        string path = GetCacheFilePath<T>(typeName);
            T obj;
            if (string.IsNullOrEmpty(path))
            {
                obj = CreateScriptableObj<T>(path);
            }
            else
            {
                obj = DelegateUtil.ManyConditions(
                    checkNotNull: (o) => o,
                () => AssetDatabase.LoadAssetAtPath<T>(path),
                () =>
                {
	                try
	                {
		                var guids = AssetDatabase.FindAssets(string.Format(SearchFileTypeFormat, typeName));
		                if (guids.LengthSafe() == 0)
		                {
			                return CreateScriptableObj<T>(path);
		                }
		                else
		                {
			                string[] paths = guids.ConvertAll(AssetDatabase.GUIDToAssetPath);
			                return paths[0] != null ? AssetDatabase.LoadAssetAtPath<T>(paths[0]) : CreateScriptableObj<T>(path);
		                }
	                }
	                catch (Exception ex)
	                {
		                Debug.LogError($"Error while finding or creating ScriptableObject: {ex.Message}");
		                return null;
	                }
                }
            );
            }

            if (!hasKey)
                scriptableObjDic.Add(key, obj);
            else
                scriptableObjDic[key] = obj;

	        EditorUtility.SetDirty(this);
	        AssetDatabase.SaveAssets();
	        // AssetDatabase.Refresh();
            return obj;
        }
	}
}
#endif
