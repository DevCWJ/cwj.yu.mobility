#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

namespace CWJ.AccessibleEditor
{
    /// <summary>
    /// <para/>[21.01.21]
    /// </summary>
    public class MeshCombinerWindow : EditorWindow
    {
        private static GUIContent _WindowContent = null;
        private static GUIContent WindowContent
        {
            get
            {
                if (_WindowContent == null)
                    _WindowContent = new GUIContent("Mesh Combiner", AccessibleEditorUtil.EditorHelperObj.IconTexture);
                return _WindowContent;
            }
        }

        private const string WindowMenuPath = nameof(CWJ) + "/" + "Combine Meshes";

        [MenuItem("GameObject/" + WindowMenuPath, false, 50)]
        [MenuItem(WindowMenuPath, priority = 50)]
        public static void Open()
        {
            MeshCombinerWindow window = GetWindow<MeshCombinerWindow>(true);
            window.titleContent = WindowContent;
            window.minSize = new Vector2(377, 350);
            window.Show();
            Init();
            UpdateSelectedObjs();
        }

        private const string CombinedObjectName = "[CWJ_CombinedMesh]";

        private static bool IsAutoNaming = true;
        private static string NewCombinedObjName = null;

        private static bool HasMeshCollider = false, NeedMergedMeshCol = false;
        private static bool IsStatic = true, IsLightmapped = true;
        private static bool IsDestroyAfterCombined = false;

        private static string ExportPath = null;

        private static GameObject[] SelectedObjs = null;
        private static int SelectedObjCnt = 0;
        private static int AllMatCnt = 0;
        private static int WillCombinedMatCnt = 0;

        private static void UpdateSelectedObjs()
        {
            SelectedObjs = Selection.gameObjects;

            if (SelectedObjs.LengthSafe() == 0)
            {
                Init();
                return;
            }

            List<Material> allMats = new List<Material>();

            //Get All Materials
            foreach (GameObject obj in SelectedObjs)
            {
                if (obj.IsNullOrMissing() || !obj.activeInHierarchy) continue;

                foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
                {
                    Material[] mats = r.sharedMaterials;
                    if (mats != null && mats.Length > 0)
                    {
                        allMats.AddRange(mats);
                    }
                }
            }

            AllMatCnt = allMats.Count;
            WillCombinedMatCnt = allMats.Distinct().Count();
            SelectedObjCnt = SelectedObjs.Length;
        }

        Vector2 scrollPos;
        bool isSelectedObjsFoldout = true;

        private void OnGUI()
        {
            EditorGUILayout.HelpBox(new GUIContent(typeof(MeshCombinerWindow).GetMyInfo(), WindowContent.image), true);
            EditorGUILayout.Space();

            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorGUILayout.LabelField("Application is Playing or will change Playmode");
                return;
            }

            if (EditorApplication.isCompiling)
            {
                EditorGUILayout.LabelField("Please wait until the compilation of the script has finished.");
                return;
            }

            EditorGUILayout.BeginVertical(EditorGUICustomStyle.OuterBox);

            EditorGUI_CWJ.DrawObjectsField("Selected Objs", ref SelectedObjs, ref isSelectedObjsFoldout, ref scrollPos,
                                            isReadonlyLength: true, 
                                            updateCallback: UpdateSelectedObjs);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Draw Calls in Selection :", AllMatCnt.ToString());
            EditorGUILayout.LabelField("Draw Calls Combined :", WillCombinedMatCnt.ToString());

            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            EditorGUILayout.Space();

            if (AllMatCnt == 0)
            {
                return;
            }

            EditorGUILayout.BeginVertical(EditorGUICustomStyle.OuterBox);

            if (IsAutoNaming = EditorGUILayout.Toggle("Is Auto Naming", IsAutoNaming))
            {
                string autoName = $"{CombinedObjectName}_{SelectedObjs[0].name}";
                int length = FindUtil.GetRootGameObjects_New(false).FindAll(g => g.name.StartsWith(autoName)).Length;
                NewCombinedObjName = $"{autoName}{(length > 0 ? $" ({length})" : "")}";
                EditorGUILayout.LabelField("Combined Mesh Name :", NewCombinedObjName);
            }
            else
            {
                NewCombinedObjName = EditorGUILayout.TextField("Combined Mesh Name :", NewCombinedObjName);
            }

            EditorGUILayout.BeginHorizontal();
            GUI.enabled = (IsStatic = EditorGUILayout.Toggle("Editor Static Object", IsStatic));
            IsLightmapped = IsStatic & EditorGUILayout.Toggle("Generate Lightmap UVs", IsLightmapped);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUI.enabled = (HasMeshCollider = EditorGUILayout.Toggle("Has Mesh Collider", HasMeshCollider));
            NeedMergedMeshCol = HasMeshCollider & EditorGUILayout.Toggle("Need Merged Mesh Cols", NeedMergedMeshCol);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            IsDestroyAfterCombined = EditorGUILayout.Toggle("Remove Originals", IsDestroyAfterCombined);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Set Export Path", GUILayout.ExpandWidth(false)))
            {
                ExportPath = UpdateExportPath();
            }
            EditorGUILayout.LabelField(": " + (string.IsNullOrEmpty(ExportPath) ? "NULL" : ExportPath));
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Combine"))
            {
                if (!typeof(MeshCombinerWindow).DisplayDialog("Are you sure to combine the \nmeshes of selected objects?", ok: "Yes", cancel: "No"))
                {
                    return;
                }

                if (string.IsNullOrEmpty(ExportPath))
                {
                    ExportPath = UpdateExportPath();
                }
                if (!string.IsNullOrEmpty(ExportPath))
                    CombineProcess();
                //EditorUtility.ClearProgressBar();
                //CreatePrefabFromSelected();
            }

            EditorGUILayout.EndVertical();
        }

        private string UpdateExportPath()
        {
            string newPath = AccessibleEditorUtil.SelectFolderPath(title: "Choose Folder in which to Export Combined Meshes");
            return string.IsNullOrEmpty(newPath) ? ExportPath : newPath;
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void CombineProcess()
        {
            GameObject selectedRootObj = SelectedObjs[0];
            GameObject[] backupSelectedObjs = SelectedObjs;

            Vector3 backupRootPosition = new Vector3(selectedRootObj.transform.position.x, selectedRootObj.transform.position.y, selectedRootObj.transform.position.z);

            //		backupRootPosition = StoreOriginalPositions(oldGameObjects);
            //		backupRootRotation = StoreOriginalQuaternions(oldGameObjects);

            Matrix4x4 rootMatrix = selectedRootObj.transform.worldToLocalMatrix;

            Dictionary<Material, List<MeshCache>> matByMeshCacheDic = new Dictionary<Material, List<MeshCache>>();

            EditorUtility.DisplayProgressBar("Collect Mesh Data", "Loading...", 0);

            for (int i = 0; i < SelectedObjCnt; i++)
            {
                EditorUtility.DisplayProgressBar("Collect Mesh Data", $"{i} / {SelectedObjCnt}", ((float)i / SelectedObjCnt));

                if (SelectedObjs[i].IsNullOrMissing() || !SelectedObjs[i].activeInHierarchy) continue;

                foreach (MeshFilter filter in SelectedObjs[i].GetComponentsInChildren<MeshFilter>())
                {
                    if (filter == null || filter.sharedMesh.IsNullOrMissing()) continue;
                    Renderer renderer = filter.GetComponent<Renderer>();
                    if (renderer == null || !renderer.enabled) continue;

                    MeshCache meshCache = new MeshCache(mesh: filter.sharedMesh,
                                                trfMatrix: rootMatrix * filter.transform.localToWorldMatrix);
                    Material[] sharedMaterials = renderer.sharedMaterials;
                    for (int j = 0; j < sharedMaterials.Length; j++)
                    {
                        meshCache.subMeshIndex = System.Math.Min(j, meshCache.mesh.subMeshCount - 1);

                        if (!matByMeshCacheDic.TryGetValue(sharedMaterials[j], out var meshCacheList))
                            matByMeshCacheDic.Add(sharedMaterials[j], new List<MeshCache>() { meshCache });
                        else
                            meshCacheList.Add(meshCache);
                    }
                }
            }

            string exportFolderPath = System.IO.Path.Combine(ExportPath, NewCombinedObjName) + System.IO.Path.DirectorySeparatorChar;
            if (System.IO.Directory.Exists(exportFolderPath))
            {
                bool isExists = true;
                int i = 0;
                string p = exportFolderPath;
                while (isExists)
                {
                    p = $"{exportFolderPath} ({i})";
                    isExists = System.IO.Directory.Exists(p);
                }
                exportFolderPath = p;
            }
            try
            {
                var folderInfo = new System.IO.DirectoryInfo(exportFolderPath);
                folderInfo.Create();
            }
            catch (System.Exception e)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError(exportFolderPath + "\n" + e);
                return;
            }

            int cacheLength = matByMeshCacheDic.Count;
            GameObject[] combinedObjects = new GameObject[cacheLength];
            GameObject rootObj = new GameObject(NewCombinedObjName);
            Transform rootTrf = rootObj.transform;
            rootTrf.Reset();
            rootTrf.position = backupRootPosition;

            int index = 0;

            foreach (var keyValue in matByMeshCacheDic)
            {
                EditorUtility.DisplayProgressBar("Combine Mesh", $"{index} / {cacheLength}", ((float)index / cacheLength));

                GameObject combinedObj = new GameObject($"{NewCombinedObjName}_{index}");
                combinedObj.layer = selectedRootObj.layer;
                combinedObj.transform.SetParent(rootTrf, true);
                combinedObj.transform.localScale = Vector3.one;
                combinedObj.transform.localRotation = selectedRootObj.transform.localRotation;
                combinedObj.transform.position = backupRootPosition;
                MeshFilter filter = combinedObj.AddComponent<MeshFilter>();
                MeshRenderer renderer = combinedObj.AddComponent<MeshRenderer>();

                Mesh combinedMesh = GetCombinedMesh(keyValue.Value.ToArray(), combinedObj.name);
                filter.sharedMesh = combinedMesh;
                Material sharedMaterial = keyValue.Key;
                renderer.material = sharedMaterial;

                AssetDatabase.CreateAsset(combinedMesh, exportFolderPath + combinedMesh.name + ".asset");

                if (combinedObj.isStatic = IsStatic)
                {
                    if (IsLightmapped)
                        Unwrapping.GenerateSecondaryUVSet(filter.sharedMesh);
                }

                if (HasMeshCollider)
                {
                    combinedObj.AddComponent<MeshCollider>();
                }

                combinedObjects[index++] = combinedObj;
            }

            if (combinedObjects.FindIndex(g => g == null) >= 0)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError("Error : MeshCacheList has NULL");
                return;
            }

            if (IsDestroyAfterCombined)
            {
                for (int i = 0; i < backupSelectedObjs.Length; i++)
                {
                    DestroyImmediate(backupSelectedObjs[i]);
                    backupSelectedObjs[i] = null;
                }
            }
            else
            {
                for (int i = 0; i < backupSelectedObjs.Length; i++)
                {
                    if (backupSelectedObjs[i].activeInHierarchy)
                        backupSelectedObjs[i].SetActive(false);
                }
            }

            if (NeedMergedMeshCol)
            {
                rootObj.AddComponent<MeshCollider>();
                rootObj.AddComponent<MeshColliderMerge>();
            }

            string path = exportFolderPath + rootObj.name + ".prefab";
            //PrefabUtility.CreatePrefab(path, newParent); <-for old unity version
            PrefabUtility.SaveAsPrefabAsset(rootObj, path);
            GameObject prefabObj = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path)) as GameObject;
            prefabObj.transform.position = rootTrf.position;
            DestroyImmediate(rootObj);
            prefabObj.transform.SetAsLastSibling();

            EditorUtility.ClearProgressBar();

            typeof(MeshColliderMerge).DisplayDialog("Completed");

            EditorCallback.AddWaitForSecondsCallback(() => AccessibleEditorUtil.PingObj(prefabObj), 0.1f);
            //AccessibleEditorUtil.PingObj(prefabObj);
        }

        private static void Init()
        {
            SelectedObjs = new GameObject[0];
            AllMatCnt = 0;
            WillCombinedMatCnt = 0;
            SelectedObjCnt = 0;

            IsAutoNaming = true;
            NewCombinedObjName = null;
            HasMeshCollider = false; NeedMergedMeshCol = false;
            IsStatic = true; IsLightmapped = true;
            IsDestroyAfterCombined = false;
            ExportPath = null;
        }

        private void Init(UnityEngine.SceneManagement.Scene scene) { Init(); }
        private void Init(PlayModeStateChange obj) { Init(); }

        protected void OnEnable()
        {
            CWJ_EditorEventHelper.ReloadedScriptEvent += Init;
            CWJ_EditorEventHelper.EditorSceneOpenedEvent += Init;
            CWJ_EditorEventHelper.PlayModeStateChangedEvent += Init;
        }

        protected void OnDisable()
        {
            CWJ_EditorEventHelper.ReloadedScriptEvent -= Init;
            CWJ_EditorEventHelper.EditorSceneOpenedEvent -= Init;
            CWJ_EditorEventHelper.PlayModeStateChangedEvent -= Init;
            Init();
        }

        //private List<Quaternion> StoreOriginalQuaternions(GameObject[] GO)
        //{
        //    List<Quaternion> quats = new List<Quaternion>();
        //    for (int x = 0; x < GO.Length; x++)
        //    {
        //        Quaternion q = new Quaternion(GO[x].transform.localRotation.x, GO[x].transform.localRotation.y, GO[x].transform.localRotation.z, GO[x].transform.localRotation.w);
        //        quats.Add(q);
        //    }
        //    return quats;
        //}

        //private List<Vector3> StoreOriginalPositions(GameObject[] GO)
        //{
        //    List<Vector3> pos = new List<Vector3>();
        //    for (int x = 0; x < GO.Length; x++)
        //    {
        //        Vector3 p = new Vector3(GO[x].transform.position.x, GO[x].transform.position.y, GO[x].transform.position.z);
        //        pos.Add(p);

        //    }
        //    return pos;
        //}

        //private void CreatePrefabFromSelected()
        //{
        //    //get selections and return errors
        //    Transform[] selection = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
        //    if (selection.Length == 0)
        //    {
        //        EditorUtility.DisplayDialog("No GameObjects have been selected!", "Please select one or more GameObjects with Combined Meshes.", "");
        //        return;
        //    }

        //    List<GameObject> selectionmeshes = new List<GameObject>();
        //    for (int i = 0; i < selection.Length; i++)
        //    {
        //        //get meshes filters in seletion
        //        MeshFilter[] temps = selection[i].GetComponentsInChildren<MeshFilter>();
        //        for (int n = 0; n < temps.Length; n++)
        //        {
        //            selectionmeshes.Add(temps[n].gameObject);
        //        }
        //    }
        //    //return error if not meshfilters
        //    if (selectionmeshes.Count == 0)
        //    {
        //        EditorUtility.DisplayDialog("No GameObjects with Combined Meshes selected!", "Please select one or more GameObjects with Combined Meshes.", "");
        //        return;
        //    }

        //    for (int i = 0; i < selectionmeshes.Count; i++)
        //    {
        //        if (selectionmeshes[i] == null)//repeat objects
        //            continue;
        //        string name = selectionmeshes[i].name.Replace("(Clone)", "");
        //        // Debug.Log("Trying to Convert " + name + " into Prefab");
        //        //Need to check for exisitence of sharedmesh and then if it is saved already 
        //        MeshFilter lemesh = selectionmeshes[i].GetComponent<MeshFilter>();
        //        if (lemesh != null)
        //        {
        //            if (lemesh.sharedMesh.name != "Combined Mesh")//ensuring combined mesh
        //            {
        //                Debug.LogError(name + " does not contain a Combined Mesh " + lemesh.sharedMesh.name + " " + lemesh.name);
        //                continue;
        //            }
        //            else if (!AssetDatabase.Contains(lemesh.sharedMesh))
        //                AssetDatabase.CreateAsset(lemesh.sharedMesh, targetFolder + "/" + name + ".asset");
        //        }
        //        else
        //        {
        //            continue;
        //        }
        //        //save prefab or replace it 

        //        string path = targetFolder + "/" + name + ".prefab";
        //        //PrefabUtility.CreatePrefab(path, selectionmeshes[i].gameObject); <-for old unity version
        //        PrefabUtility.SaveAsPrefabAsset(selectionmeshes[i].gameObject, path);
        //        Transform parentTrf = selectionmeshes[i].transform.parent;
        //        Vector3 childPos = selectionmeshes[i].transform.position;

        //        DestroyImmediate(selectionmeshes[i]);

        //        GameObject prefabObj = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path)) as GameObject;
        //        //ensure prefab replacement in scene
        //        prefabObj.transform.parent = parentTrf;
        //        prefabObj.transform.position = childPos;
        //    }
        //    AssetDatabase.Refresh();
        //    Debug.Log("Combined Meshes located in " + targetFolder);
        //}

        #region Mesh Combine

        private struct MeshCache
        {
            public Mesh mesh;
            public Matrix4x4 trfMatrix;
            public int subMeshIndex;

            public MeshCache(Mesh mesh, Matrix4x4 trfMatrix)
            {
                this.mesh = mesh;
                this.trfMatrix = trfMatrix;
                this.subMeshIndex = 0;
            }
        }

        private Mesh GetCombinedMesh(MeshCache[] meshCaches, string meshName, bool isGenerateStrips = false)
        {
            int vertexCount = 0;
            int triangleCount = 0;
            int stripCount = 0;
            foreach (MeshCache combine in meshCaches)
            {
                if (combine.mesh)
                {
                    vertexCount += combine.mesh.vertexCount;

                    if (isGenerateStrips)
                    {
                        // SUBOPTIMAL FOR PERFORMANCE
                        int curStripCount = combine.mesh.GetTriangles(combine.subMeshIndex).Length;
                        if (curStripCount != 0)
                        {
                            if (stripCount != 0)
                            {
                                if ((stripCount & 1) == 1)
                                    stripCount += 3;
                                else
                                    stripCount += 2;
                            }
                            stripCount += curStripCount;
                        }
                        else
                        {
                            isGenerateStrips = false;
                        }
                    }
                }
            }

            // Precomputed how many triangles we need instead
            if (!isGenerateStrips)
            {
                foreach (MeshCache combine in meshCaches)
                {
                    if (combine.mesh)
                    {
                        triangleCount += combine.mesh.GetTriangles(combine.subMeshIndex).Length;
                    }
                }
            }

            Vector3[] vertices = new Vector3[vertexCount];
            Vector3[] normals = new Vector3[vertexCount];
            Vector4[] tangents = new Vector4[vertexCount];
            Vector2[] uv = new Vector2[vertexCount];
            Vector2[] uv1 = new Vector2[vertexCount];
            Color[] colors = new Color[vertexCount];

            int[] triangles = new int[triangleCount];
            int[] strip = new int[stripCount];

            int offset;

            offset = 0;
            foreach (MeshCache combine in meshCaches)
            {
                if (combine.mesh)
                    Copy(combine.mesh.vertexCount, combine.mesh.vertices, vertices, ref offset, combine.trfMatrix);
            }

            offset = 0;
            foreach (MeshCache combine in meshCaches)
            {
                if (combine.mesh)
                {
                    Matrix4x4 invTranspose = combine.trfMatrix;
                    invTranspose = invTranspose.inverse.transpose;
                    CopyNormal(combine.mesh.vertexCount, combine.mesh.normals, normals, ref offset, invTranspose);
                }

            }
            offset = 0;
            foreach (MeshCache combine in meshCaches)
            {
                if (combine.mesh)
                {
                    Matrix4x4 invTranspose = combine.trfMatrix;
                    invTranspose = invTranspose.inverse.transpose;
                    CopyTangents(combine.mesh.vertexCount, combine.mesh.tangents, tangents, ref offset, invTranspose);
                }

            }
            offset = 0;
            foreach (MeshCache combine in meshCaches)
            {
                if (combine.mesh)
                    Copy(combine.mesh.vertexCount, combine.mesh.uv, uv, ref offset);
            }

            offset = 0;
            foreach (MeshCache combine in meshCaches)
            {
                if (combine.mesh)
                    Copy(combine.mesh.vertexCount, combine.mesh.uv2, uv1, ref offset);
            }

            offset = 0;
            foreach (MeshCache combine in meshCaches)
            {
                if (combine.mesh)
                    CopyColors(combine.mesh.vertexCount, combine.mesh.colors, colors, ref offset);
            }

            int triangleOffset = 0;
            int stripOffset = 0;
            int vertexOffset = 0;
            foreach (MeshCache combine in meshCaches)
            {
                if (combine.mesh)
                {
                    if (isGenerateStrips)
                    {
                        int[] inputstrip = combine.mesh.GetTriangles(combine.subMeshIndex);
                        if (stripOffset != 0)
                        {
                            if ((stripOffset & 1) == 1)
                            {
                                strip[stripOffset + 0] = strip[stripOffset - 1];
                                strip[stripOffset + 1] = inputstrip[0] + vertexOffset;
                                strip[stripOffset + 2] = inputstrip[0] + vertexOffset;
                                stripOffset += 3;
                            }
                            else
                            {
                                strip[stripOffset + 0] = strip[stripOffset - 1];
                                strip[stripOffset + 1] = inputstrip[0] + vertexOffset;
                                stripOffset += 2;
                            }
                        }

                        for (int i = 0; i < inputstrip.Length; i++)
                        {
                            strip[i + stripOffset] = inputstrip[i] + vertexOffset;
                        }
                        stripOffset += inputstrip.Length;
                    }
                    else
                    {
                        int[] inputtriangles = combine.mesh.GetTriangles(combine.subMeshIndex);
                        for (int i = 0; i < inputtriangles.Length; i++)
                        {
                            triangles[i + triangleOffset] = inputtriangles[i] + vertexOffset;
                        }
                        triangleOffset += inputtriangles.Length;
                    }

                    vertexOffset += combine.mesh.vertexCount;
                }
            }

            Mesh mesh = new Mesh();
            mesh.name = meshName;
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.colors = colors;
            mesh.uv = uv;
            mesh.uv2 = uv1;
            mesh.tangents = tangents;
            if (isGenerateStrips)
                mesh.SetTriangles(strip, 0);
            else
                mesh.triangles = triangles;

            return mesh;
        }

        static void Copy(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
        {
            for (int i = 0; i < src.Length; i++)
                dst[i + offset] = transform.MultiplyPoint(src[i]);
            offset += vertexcount;
        }

        static void CopyNormal(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
        {
            for (int i = 0; i < src.Length; i++)
                dst[i + offset] = transform.MultiplyVector(src[i]).normalized;
            offset += vertexcount;
        }

        static void Copy(int vertexcount, Vector2[] src, Vector2[] dst, ref int offset)
        {
            for (int i = 0; i < src.Length; i++)
                dst[i + offset] = src[i];
            offset += vertexcount;
        }

        static void CopyColors(int vertexcount, Color[] src, Color[] dst, ref int offset)
        {
            for (int i = 0; i < src.Length; i++)
                dst[i + offset] = src[i];
            offset += vertexcount;
        }

        static void CopyTangents(int vertexcount, Vector4[] src, Vector4[] dst, ref int offset, Matrix4x4 transform)
        {
            for (int i = 0; i < src.Length; i++)
            {
                Vector4 p4 = src[i];
                Vector3 p = new Vector3(p4.x, p4.y, p4.z);
                p = transform.MultiplyVector(p).normalized;
                dst[i + offset] = new Vector4(p.x, p.y, p.z, p4.w);
            }

            offset += vertexcount;
        }

        #endregion
    }
}
#endif