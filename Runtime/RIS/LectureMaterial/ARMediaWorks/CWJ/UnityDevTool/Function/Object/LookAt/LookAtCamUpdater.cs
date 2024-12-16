
using UnityEngine;
using UnityEngine.Events;
using CWJ.SceneHelper;
using System;

namespace CWJ
{

    public class LookAtCamUpdater : CWJ.Singleton.SingletonBehaviour<LookAtCamUpdater>, CWJ.SceneHelper.INeedSceneObj
    {
        [VisualizeProperty] public SceneObjContainer sceneObjs { get; set; }
#if UNITY_EDITOR
        [VisualizeField]
        private static UnityEvent<Vector3, bool>
#else
        private static Action<Vector3, bool> 
#endif
        _CamPosUpdateEvent;

        [VisualizeField] private static Vector3 LastCamPos;

        public static void AddUpdateListener(
#if UNITY_EDITOR
            UnityAction
#else
            Action
#endif
           <Vector3, bool>  action)
        {
#if UNITY_EDITOR
            if (_CamPosUpdateEvent == null)
                _CamPosUpdateEvent = new UnityEvent<Vector3, bool>();
            _CamPosUpdateEvent.AddListener(action);
#else
            _CamPosUpdateEvent += action;
#endif
        }

        public static void RemoveUpdateListener(
#if UNITY_EDITOR
            UnityAction
#else
            Action
#endif
            <Vector3, bool> action)
        {
#if UNITY_EDITOR
            if (_CamPosUpdateEvent != null)
                _CamPosUpdateEvent.RemoveListener(action);
#else
            _CamPosUpdateEvent -= action;
#endif
        }
        
        protected override void _Awake()
        {
            LastCamPos = Vector3.zero;
        }


        void LateUpdate()
        {
            if (!sceneObjs.hasPlayerCam)
            {
                return;
            }

            Vector3 curCamPos = sceneObjs.playerCamTrf.position;
            _CamPosUpdateEvent?.Invoke(curCamPos, !curCamPos.Equals(LastCamPos));
            LastCamPos = curCamPos;
        }
    }

}