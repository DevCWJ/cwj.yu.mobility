using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace CWJ
{
    /// <summary>
    /// <para/>[21.01.21]
    /// </summary>
    [RequireComponent(typeof(MeshCollider))]
    public class MeshColliderMerge : MonoBehaviour
    {
        void Reset()
        {
            meshCollider = gameObject.GetOrAddComponent<MeshCollider>();
        }

        [SerializeField, Readonly] MeshCollider meshCollider;
        [HideInInspector] public GameObject[] combinedObjs;
        [HideInInspector] public GameObject boxMeshHolder;

        [Header("Combine Settings")]
        [Tooltip("GameObjects in this list will not be merged.")]
        public MeshCollider[] ignoreMeshColliders = null;

        [Header("Optimize Settings")]
        [Tooltip("Distance between vertices to merge.")]
        [Range(0.01f, 2f)]
        public float mergeVerticesThreshold = 0.1f;

        bool _HasCombinedGameOjects() => combinedObjs.LengthSafe() > 0;

        bool Able_ConvertBoxColliders() => (!_HasCombinedGameOjects() && boxMeshHolder == null);
        [InvokeButton, ReadonlyConditional(nameof(Able_ConvertBoxColliders), forPredicateComparison:false)]
        public void ConvertBoxColliders()
        {
            if (!Able_ConvertBoxColliders()) return;
            if (transform.childCount == 0) return;
            if (Application.isPlaying) return;

            BoxCollider[] boxColliders = transform.GetComponentsInChildren<BoxCollider>();
            if (boxColliders.Length < 1)
            {
                Debug.LogError("Found no box colliders");
                return;
            }
            boxMeshHolder = new GameObject("Converted Box Colliders");
            boxMeshHolder.transform.SetParent(transform);
            boxMeshHolder.transform.SetAsFirstSibling();
            Mesh boxMesh = MeshUtil.CreateSimpleCubeMesh();
            for (int i = 0; i < boxColliders.Length; i++)
            {
                BoxCollider b = boxColliders[i];
                b.enabled = false;
                Transform t = boxColliders[i].transform;
                GameObject o = new GameObject();
                o.name = t.name + " (Box Collider)";
                o.transform.position = t.position;
                o.transform.rotation = t.rotation;
                o.transform.localScale = t.localScale;
                o.transform.SetParent(t);
                o.transform.localScale = b.size;
                o.transform.localPosition = b.center;
                MeshCollider cm = o.AddComponent<MeshCollider>();
                cm.sharedMesh = boxMesh;
                o.transform.SetParent(boxMeshHolder.transform);
            }
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(boxMeshHolder);
#endif
        }

        bool Able_UndoConvertBoxColliders() => (!_HasCombinedGameOjects() && boxMeshHolder != null);
        [InvokeButton, ReadonlyConditional(nameof(Able_UndoConvertBoxColliders), forPredicateComparison: false)]
        public void UndoConvertBoxColliders()
        {
            if (!Able_UndoConvertBoxColliders()) return;
            if (transform.childCount == 0) return;
            if (Application.isPlaying) return;

            foreach (BoxCollider boxCol in transform.GetComponentsInChildren<BoxCollider>())
            {
                boxCol.enabled = true;
            }

#if UNITY_EDITOR
            DestroyImmediate(boxMeshHolder);
#else
		    Destroy(boxMeshHolder);
#endif
            boxMeshHolder = null;
        }


        bool Able_CombineMeshes() => !_HasCombinedGameOjects();

        [InvokeButton, ReadonlyConditional(nameof(Able_CombineMeshes), forPredicateComparison: false)]
        public void CombineMeshes()
        {
            if (!Able_CombineMeshes()) return;
            if (transform.childCount == 0) return;

            if (meshCollider.IsNullOrMissing()) meshCollider = gameObject.GetOrAddComponent<MeshCollider>();
            MeshCollider[] meshColiders = null;
            meshColiders = transform.GetComponentsInChildren<MeshCollider>();
            combinedObjs = new GameObject[meshColiders.Length];
            Mesh meshSprites = new Mesh();
            CombineInstance[] combineInstaces = new CombineInstance[meshColiders.Length];
            for (int i = 0; i < combineInstaces.Length; i++)
            {
#if UNITY_EDITOR
                EditorUtility.DisplayProgressBar("Merging", "" + i + " / " + combineInstaces.Length, (float)i / combineInstaces.Length);
#endif
                if (i != 0 && (ignoreMeshColliders == null || !ignoreMeshColliders.IsExists(meshColiders[i])))
                {
                    combineInstaces[i] = new CombineInstance()
                    {
                        mesh = meshColiders[i].sharedMesh,
                        transform = transform.worldToLocalMatrix * meshColiders[i].transform.localToWorldMatrix
                    };
                    combinedObjs[i] = meshColiders[i].gameObject;
                }
                else
                {
                    combineInstaces[i] = new CombineInstance()
                    {
                        mesh = new Mesh(),
                        transform = transform.worldToLocalMatrix
                    };
                }
            }

            foreach (var o in combinedObjs)
            {
                var meshCol = o.GetComponent<MeshCollider>();
                if (meshCol != null) meshCol.enabled = false;
            }

#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
#endif
            meshSprites.name = "MeshCollider Merge Instance";
            meshSprites.Clear();
            meshSprites.CombineMeshes(combineInstaces);
            int originalVerts = meshSprites.vertexCount;
            meshSprites.MergeVertices(mergeVerticesThreshold);
            int removed = originalVerts - meshSprites.vertexCount;
            int percentage = (int)(((float)removed / originalVerts) * 100);
            meshCollider.sharedMesh = meshSprites;
            Debug.Log("Verts reduced from " + originalVerts + " to " + meshCollider.sharedMesh.vertexCount + "\nRemoved " + removed + " Verts" + " (" + percentage + "%)");
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(gameObject);
#endif
        }

        [InvokeButton, ReadonlyConditional(nameof(Able_CombineMeshes), forPredicateComparison: true)]
        void ReleaseCombinedMesh()
        {
            if (Able_CombineMeshes()) return;

            foreach (var o in combinedObjs)
            {
                var meshCol = o.GetComponent<MeshCollider>();
                if (meshCol != null) meshCol.enabled = true;
            }

            if (meshCollider.sharedMesh != null) meshCollider.sharedMesh = null; // DestroyImmediate(meshCollider.sharedMesh);
            combinedObjs = null;
        }

#if UNITY_EDITOR
        bool Able_Editor_SaveMesh() => meshCollider.sharedMesh != null;

        [InvokeButton, ReadonlyConditional(nameof(Able_Editor_SaveMesh), forPredicateComparison: false)]
        void Editor_SaveMesh()
        {
            string path = AccessibleEditor.AccessibleEditorUtil.SelectFilePath("[CWJ_MergedMeshCol]_" + transform.name, "asset");
            if (path != null && path != "")
            {
                UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                if (asset == null)
                {
                    AssetDatabase.CreateAsset(meshCollider.sharedMesh, path);
                }
                else
                {
                    ((Mesh)asset)?.Clear();
                    EditorUtility.CopySerialized(meshCollider.sharedMesh, asset);
                    AssetDatabase.SaveAssets();
                }
                meshCollider.sharedMesh = (Mesh)AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

                Debug.Log("Saved mesh asset: " + path);
            }
        }
#endif

    }
}
