using UnityEngine;
using UnityEngine.Events;

namespace CWJ
{
    [AddComponentMenu("Scripts/" + nameof(CWJ) + "/CWJ_" + nameof(KeyListener))]
    public class KeyListener : MonoBehaviour
    {
        [SearchableEnum] public KeyCode detectTargetKey = KeyCode.None;

        [Tooltip("mobile only\n멀티 터치 전용 이벤트일 시 체크")]
        [ReadonlyConditional(EPlayMode.PlayMode)] public bool isMultiTouchOnly;

        public UnityEvent[] touchEvents { get; private set; } = null;

        [Space]
        public UnityEvent onTouchBegan = new UnityEvent();//터치 시작 이벤트
        public UnityEvent onTouchMoving = new UnityEvent(); //홀드 터치 움직임 이벤트
        public UnityEvent onTouchStationary = new UnityEvent(); //홀드 터치 고정 이벤트
        public UnityEvent onTouchEnded = new UnityEvent(); //터치 종료 이벤트

        [Header("터치인식 수가 너무 많을때 실행됨")]
        public UnityEvent onTouchCanceled = new UnityEvent(); //터치 취소됨 이벤트, 모바일전용

        [Space]
        public UnityEvent onUpdateEnded = new UnityEvent(); //업데이트 마지막 실행 이벤트

        private void Awake()
        {
            touchEvents = new UnityEvent[5];
            touchEvents[0] = onTouchBegan;
            touchEvents[1] = onTouchMoving;
            touchEvents[2] = onTouchStationary;
            touchEvents[3] = onTouchEnded;
            touchEvents[4] = onTouchCanceled;
        }
        public bool IsSubscribed { get; private set; } = false;
        public bool _SetSubscribedForcibly(bool s) => IsSubscribed = s;

        protected _KeyEventManager GetKeyEventManager()=>
#if UNITY_ANDROID
                TouchManager_Mobile.Instance;
#else
                KeyEventManager_PC.Instance;
#endif

        private void OnEnable()
        {
            if (detectTargetKey == KeyCode.None) return;
            var keyManager = GetKeyEventManager();
            IsSubscribed = keyManager && keyManager.AddKeyListener(this);
        }

        private void OnDisable()
        {
            if (MonoBehaviourEventHelper.IS_QUIT || detectTargetKey == KeyCode.None) return;

            var keyManager = GetKeyEventManager();
            if (keyManager && keyManager.RemoveKeyListener(this))
                IsSubscribed = false;
        }

        private void OnDestroy()
        {
            if (MonoBehaviourEventHelper.IS_QUIT || detectTargetKey == KeyCode.None) return;

            var keyManager = GetKeyEventManager();
            if (keyManager && keyManager.OnDestroyKeyListener(this))
                IsSubscribed = false;
        }
    }
}