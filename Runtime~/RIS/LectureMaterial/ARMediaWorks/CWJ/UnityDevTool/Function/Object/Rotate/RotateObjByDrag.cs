using UnityEngine;
using UnityEngine.EventSystems;
using CWJ.SceneHelper;
using System;

namespace CWJ
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class RotateObjByDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, CWJ.SceneHelper.INeedSceneObj
    {
        [SerializeField] SceneObjContainer _sceneObjs;
        public SceneObjContainer sceneObjs
        {
            get => _sceneObjs;
            set
            {
                _sceneObjs = value;
                //if (value.hasCanvasOf3D)
                //    mousePosToWorldPos = value.canvasOf3D.renderMode == RenderMode.WorldSpace ? GetMousePos_WorldSpace : GetMousePos_ScreenSpace;
                //else
                //    mousePosToWorldPos = null;
            }
        }

        public Transform AxesPivot;
        [Range(1, 20f)]
        public float rotateSensivity = 7f;
        bool hasRigidbody;

        Quaternion rotBackup;

        private void Reset()
        {
            if (!TryGetComponent<Rigidbody>(out var rig))
                rig = gameObject.AddComponent<Rigidbody>();
            rig.isKinematic = true;
            rig.useGravity = false;
        }

        private void Awake()
        {
            hasRigidbody = GetComponentInParent<Rigidbody>(true);
            rotBackup = AxesPivot.localRotation;
        }

        public void ResetRotation()
        {
            isDragging = false;
            AxesPivot.localRotation = Quaternion.identity;
            AxesPivot.localRotation = rotBackup;
        }

        private void OnDisable()
        {
            isDragging = false;
        }


        //Func<Vector3, Vector3> mousePosToWorldPos = null;

        //Vector3 GetMousePos_WorldSpace(Vector3 mousePos)
        //{
        //    return mousePos.CanvasToWorldPos_WorldSpaceRenderMode(sceneObjs.playerCamera, sceneObjs.canvasOf3D);
        //}

        //Vector3 GetMousePos_ScreenSpace(Vector3 mousePos)
        //{
        //    return mousePos.CanvasToWorldPos_ScreenSpaceRenderMode(sceneObjs.playerCamera, sceneObjs.canvasOf2D);
        //}

        Vector3 prevMousePos = Vector3.zero;

        private void Update()
        {
            if (isDragging)
            {
                Ray ray = sceneObjs.playerCamera.ScreenPointToRay(Input.mousePosition);
                Plane plane = new Plane(sceneObjs.playerCamTrf.forward, AxesPivot.position);
                if (plane.Raycast(ray, out float distance))
                {
                    Vector3 mousePos = ray.GetPoint(distance);
                    Vector3 posDelta = mousePos - prevMousePos;

                    float rotSpeed = rotateSensivity * Time.deltaTime * 200f;

                    AxesPivot.Rotate(Vector3.up, -Vector3.Dot(posDelta, sceneObjs.playerCamTrf.right) * rotSpeed, Space.World);
                    AxesPivot.Rotate(sceneObjs.playerCamTrf.right, Vector3.Dot(posDelta, sceneObjs.playerCamTrf.up) * rotSpeed, Space.World);

                    prevMousePos = mousePos;
                }
            }
        }

        bool isDragging = false;

        void _OnMouseDown(Vector3 mousePos)
        {
            if (!hasRigidbody) return;
            if (isDragging) return;
            isDragging = true;

            Ray ray = sceneObjs.playerCamera.ScreenPointToRay(mousePos);
            Plane plane = new Plane(sceneObjs.playerCamTrf.forward, AxesPivot.position);
            if (plane.Raycast(ray, out float distance))
            {
                prevMousePos = ray.GetPoint(distance);
            }
        }

        void _OnMouseUp()
        {
            isDragging = false;
        }

        private void OnMouseDown()
        {
            _OnMouseDown(Input.mousePosition);
        }

        private void OnMouseUp()
        {
            _OnMouseUp();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _OnMouseDown(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _OnMouseUp();
        }

        public void OnDrag(PointerEventData eventData) { }
    }
}
