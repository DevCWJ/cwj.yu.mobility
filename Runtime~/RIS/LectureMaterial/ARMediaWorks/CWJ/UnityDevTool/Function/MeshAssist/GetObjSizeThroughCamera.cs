using UnityEngine;

namespace CWJ
{
    [ExecuteAlways]
    public class GetObjSizeThroughCamera : MonoBehaviour
    {
        [OnValueChanged(nameof(OnTargetObjChanged))] public GameObject targetObj;
        void OnTargetObjChanged()
        {
            Init();
        }

        public new Camera camera = null;


        [FoldoutGroup("Renderer (red)", true)]
        [GetComponentInChildren, Readonly, SerializeField] Renderer[] renderers;
        [Readonly, SerializeField] Bounds rendererBounds;
        [FoldoutGroup("Renderer (red)", false)]
        [Readonly, SerializeField] Rect rendererRect;

        [FoldoutGroup("MeshFilter (blue)", true)]
        [GetComponentInChildren, Readonly, SerializeField] MeshFilter[] meshFilters;
        [Readonly, SerializeField] Bounds meshBounds;
        [FoldoutGroup("MeshFilter (blue)", false)]
        [Readonly, SerializeField] Rect meshRect;

        private void Reset()
        {
            Init();
        }

        private void Init()
        {
            renderers = null; meshFilters = null;
            rendererBounds = new Bounds(); meshBounds = new Bounds();
            rendererRect = new Rect(); meshRect = new Rect();
        }

        void OnDrawGizmos()
        {
            if (targetObj == null) return;

            Gizmos.color = Color.yellow;
            foreach (var r in renderers)
            {
                Gizmos.DrawWireCube(r.bounds.center, r.bounds.size);
            }

            Gizmos.color = Color.red;
            rendererBounds = MeshUtil.GetWorldSpaceBounds(renderers);
            Gizmos.DrawWireCube(rendererBounds.center, rendererBounds.size);

            Gizmos.color = Color.blue;
            meshBounds = MeshUtil.GetWorldSpaceBounds(meshFilters);
            Gizmos.DrawWireCube(meshBounds.center, meshBounds.size);
        }

        void OnGUI()
        {
            if (targetObj == null) return;

            GUI.color = Color.red;
            rendererRect = MeshUtil.GetSizeOfBoundsThroughCamera(camera: camera, renderers: renderers);
            GUI.Box(rendererRect, nameof(rendererRect));

            GUI.color = Color.blue;
            meshRect = MeshUtil.GetSizeOfBoundsThroughCamera(camera: camera, meshFilters: meshFilters);
            GUI.Box(meshRect, nameof(meshRect));
        }
    } 
}