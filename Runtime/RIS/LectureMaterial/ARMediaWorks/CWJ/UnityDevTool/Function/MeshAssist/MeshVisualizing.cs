using UnityEngine;

namespace CWJ.EditorOnly
{

    public class MeshVisualizing : MonoBehaviour
    {
#if UNITY_EDITOR
        public enum VisualizingType
        {
            Bounds,
            Mesh
        }

        [GetComponent(isFindOnlyWhenNull: true), SerializeField, OnValueChanged(nameof(OnChangedTarget))] Transform target = null;
        void OnChangedTarget()=> SetTarget(target);
        public void SetTarget(Transform _target)
        {
            if (!_target)
            {
                return;
            }
            target = _target;
            meshFilter = target.GetComponentInChildren<MeshFilter>();
            if (meshFilter)
                OnChangedMesh();
        }
        private void OnChangedMesh()
        {
            bounds = meshFilter.sharedMesh.bounds;
            meshToDraw = (visualizingMode == VisualizingType.Bounds) ? MeshUtil.CreateCubeMesh(size: bounds.size, center: bounds.center, name: gameObject.name) : meshFilter.sharedMesh;
        }

        [GetComponent(isFindOnlyWhenNull: true), SerializeField, Readonly] MeshFilter meshFilter = null;

        [SerializeField] VisualizingType visualizingMode = VisualizingType.Bounds;

        [Readonly, SerializeField] Bounds bounds;

        [Readonly, VisualizeProperty] Mesh meshToDraw { get; set; } = null;

        [SerializeField] Color fillColors = new Color().GetSkyBlue(0.27f);
        [SerializeField] Color lineColor = new Color().GetSkyBlue();

        bool isChanged = false;

        private void OnValidate()
        {
            isChanged = true;
        }

        private void OnDrawGizmos()
        {
            if (!this.enabled || !target || !meshFilter)
            {
                return;
            }

            if (isChanged || meshToDraw == null)
            {
                OnChangedMesh();
                isChanged = false;
            }

            if (meshToDraw == null)
            {
                return;
            }

            Vector3 pos = (visualizingMode == VisualizingType.Bounds) ? target.TransformPoint(bounds.center) : target.position; //renderer.bounds.center는 world position값을 반환하지만 sharedMesh.bounds.center는 local position값을 반환함
            Quaternion rot = target.rotation;
            Vector3 scale = target.lossyScale;

            Gizmos.color = lineColor;
            Gizmos.DrawWireMesh(meshToDraw, pos, rot, scale);

            Gizmos.color = fillColors;
            Gizmos.DrawMesh(meshToDraw, pos, rot, scale);
        }

        [InvokeButton]
        private void SaveBoundsToCubeMesh()
        {
            if (visualizingMode == VisualizingType.Mesh)
            {
                Debug.LogError($"{nameof(visualizingMode)}를 {nameof(VisualizingType.Bounds)}로 설정해주세요");
                return;
            }

            string fileName = meshToDraw.name;
            string extension = "asset";
            string path = AccessibleEditor.AccessibleEditorUtil.SelectFilePath(fileName, extension);

            if (string.IsNullOrEmpty(path)) return;

            UnityEngine.Object asset = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(object));
            if (asset == null)
            {
                Debug.Log(path);
                UnityEditor.AssetDatabase.CreateAsset(meshToDraw, path);
            }
            else
            {
                ((Mesh)asset).Clear();
                UnityEditor.EditorUtility.CopySerialized(meshToDraw, asset);
                UnityEditor.AssetDatabase.SaveAssets();
            }
        }
        private void OnDisable() { }
#endif
    }
}
