using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CWJ
{
    /// <summary>
    /// 카메라 or target만 쳐다보게하는거. (Non Update() method)
    /// <para/>옛날에 만든거 못찾겠어서 새로 제작
    /// </summary>
    [DisallowMultipleComponent]
    public class LookAtCam : MonoBehaviour
    {
        public enum EUseAxis
        {
            All,
            UseOnlyX,
            UseOnlyY,
            UseOnlyZ
        }

        [SerializeField] private EUseAxis useAxisTypes = EUseAxis.All;
        public bool isInverse = true;
        [Tooltip("Vector specifying the upward direction. (Default = Vector3.up)")]
        public Vector3 worldUp = Vector3.up;
        [Foldout("Worldspace Z축고정")]
        [DrawHeader("이거켜면 다 작동안하고 월드축기준 고정만해줌")]
        [SerializeField] private bool isJustHoldZ = false;
        [Foldout("Worldspace Z축고정")]
        [SerializeField] private float holdWorldRotZ = 0;


        public void SetAxis(EUseAxis axis)
        {
            if (myTrf == null)
                Awake();
            OnDisable();
            useAxisTypes = axis;
            OnEnable();
        }

        Transform myTrf;

        private void Awake()
        {
            myTrf = GetComponent<Transform>();
        }

        private void OnEnable()
        {
            if (isJustHoldZ)
            {
                LookAtCamUpdater.AddUpdateListener(JustHoldZ_OnReceiveCamPos);
            }
            else
            {
                if (useAxisTypes == EUseAxis.All)
                    LookAtCamUpdater.AddUpdateListener(All_OnReceiveCamPos);
                else if (useAxisTypes == EUseAxis.UseOnlyX)
                    LookAtCamUpdater.AddUpdateListener(XAxis_OnReceiveCamPos);
                else if (useAxisTypes == EUseAxis.UseOnlyY)
                    LookAtCamUpdater.AddUpdateListener(YAxis_OnReceiveCamPos);
                else if (useAxisTypes == EUseAxis.UseOnlyZ)
                    LookAtCamUpdater.AddUpdateListener(ZAxis_OnReceiveCamPos);
            }

            _lastMyPos = myTrf.position;
            _lastMyPos.y = myTrf.position.y + 0.1f;
        }

        private void OnDisable()
        {
            if (MonoBehaviourEventHelper.IS_QUIT)
            {
                return;
            }
            if (isJustHoldZ)
            {
                LookAtCamUpdater.RemoveUpdateListener(JustHoldZ_OnReceiveCamPos);
            }
            else
            {
                if (useAxisTypes == EUseAxis.All)
                    LookAtCamUpdater.RemoveUpdateListener(All_OnReceiveCamPos);
                else if (useAxisTypes == EUseAxis.UseOnlyX)
                    LookAtCamUpdater.RemoveUpdateListener(XAxis_OnReceiveCamPos);
                else if (useAxisTypes == EUseAxis.UseOnlyY)
                    LookAtCamUpdater.RemoveUpdateListener(YAxis_OnReceiveCamPos);
                else if (useAxisTypes == EUseAxis.UseOnlyZ)
                    LookAtCamUpdater.RemoveUpdateListener(ZAxis_OnReceiveCamPos);
            }
        }

        void All_OnReceiveCamPos(Vector3 camPos, bool isChanged)
        {
            HandleLookAt(camPos, isChanged, null);
        }

        void XAxis_OnReceiveCamPos(Vector3 camPos, bool isChanged)
        {
            HandleLookAt(camPos, isChanged, (my, cam) => new Vector3(my.x, cam.y, cam.z));
        }

        void YAxis_OnReceiveCamPos(Vector3 camPos, bool isChanged)
        {
            HandleLookAt(camPos, isChanged, (my, cam) => new Vector3(cam.x, my.y, cam.z));
        }

        void ZAxis_OnReceiveCamPos(Vector3 camPos, bool isChanged)
        {
            HandleLookAt(camPos, isChanged, null, isZAxis: true);
        }

        Vector3 _lastMyPos;

        void HandleLookAt(Vector3 camPos, bool isChanged, System.Func<Vector3, Vector3, Vector3> axisFix, bool isZAxis = false)
        {
            if (IsNeedUpdateLookAt(isChanged, out var myPos))
            {
                if (axisFix != null)
                    camPos = axisFix(myPos, camPos);

                if (isZAxis)
                    UpdateRotZAxis(myPos, camPos);
                else
                    UpdateLookAt(myPos, camPos);

                _lastMyPos = myPos;
            }
        }

        void JustHoldZ_OnReceiveCamPos(Vector3 camPos, bool isChanged)
        {
            if (IsNeedUpdateLookAt(isChanged, out var myPos))
            {
                myTrf.eulerAngles = new Vector3(myTrf.eulerAngles.x, myTrf.eulerAngles.y, holdWorldRotZ);
                _lastMyPos = myPos;
            }
        }




        bool IsNeedUpdateLookAt(bool isChangedCamPos, out Vector3 curMyPos)
        {
            curMyPos = myTrf.position;
            return isChangedCamPos || curMyPos != _lastMyPos;
        }

        void UpdateLookAt(Vector3 myPos, Vector3 camPos)
        {
            myTrf.LookAt(myPos + GetDir(myPos, camPos), worldUp);
        }

        void UpdateRotZAxis(Vector3 myPos, Vector3 camPos)
        {
            Vector3 direction = GetDir(myPos, camPos);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            myTrf.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        Vector3 GetDir(Vector3 myPos, Vector3 camPos)
        {
            return isInverse ? (myPos - camPos) : (camPos - myPos);
        }

        //Vector3 _lastMyPos;
        //void All_OnReceiveCamPos(Vector3 camPos, bool isChanged)
        //{
        //    if (IsNeedUpdateLookAt(isChanged, out var myPos))
        //        _OnLookAt(myPos, camPos);
        //}

        //void X_OnReceiveCamPosOneAxis(Vector3 camPos, bool isChanged)
        //{
        //    if (IsNeedUpdateLookAt(isChanged, out var myPos))
        //    {
        //        camPos.x = myPos.x;
        //        _OnLookAt(myPos, camPos);
        //    }
        //}
        //void Y_OnReceiveCamPosOneAxis(Vector3 camPos, bool isChanged)
        //{
        //    if (IsNeedUpdateLookAt(isChanged, out var myPos))
        //    {
        //        camPos.y = myPos.y;
        //        _OnLookAt(myPos, camPos);
        //    }
        //}
        //void Z_OnReceiveCamPosOneAxis(Vector3 camPos, bool isChanged)
        //{
        //    if (IsNeedUpdateLookAt(isChanged, out var myPos))
        //    {
        //        camPos.z = myPos.z;
        //        Vector3 direction = GetDir(myPos, camPos);

        //        // Z축만 회전할 수 있도록, 방향 벡터의 x와 y만 사용하여 각도를 계산
        //        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //        myTrf.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        //        _lastMyPos = myPos;
        //    }
        //}

        //bool IsNeedUpdateLookAt(bool isChangedCamPos, out Vector3 curMyPos)
        //{
        //    curMyPos = myTrf.position;
        //    return isChangedCamPos || (curMyPos.x != _lastMyPos.x || curMyPos.y != _lastMyPos.y || curMyPos.z != _lastMyPos.z);
        //}

        ///// <summary>
        ///// All,X,Y 에서만 LookAt 됨
        ///// </summary>
        ///// <param name="myPos"></param>
        ///// <param name="camPos"></param>
        //void _OnLookAt(Vector3 myPos , Vector3 camPos)
        //{
        //    myTrf.LookAt(myPos + GetDir(myPos, camPos), worldUp);
        //    _lastMyPos = myPos;
        //}

        //Vector3 GetDir(Vector3 myPos, Vector3 camPos)
        //{
        //    return isInverse ? (myPos - camPos) : (camPos - myPos);
        //}
    }
}