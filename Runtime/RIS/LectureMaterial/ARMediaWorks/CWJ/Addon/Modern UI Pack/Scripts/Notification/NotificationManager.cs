using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;

namespace Michsky.MUIP
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public class NotificationManager : MonoBehaviour
    {
        // Content
        public Sprite icon;
        public string title = "Notification Title";
        [TextArea] public string description = "Notification description";

        // Resources
        public Animator notificationAnimator;
        public Image iconObj;
        public TextMeshProUGUI titleObj;
        public TextMeshProUGUI descriptionObj;
        public Button clickBtn;

        // Settings
        public bool enableTimer = true;
        public float timer = 3f;
        public bool useCustomContent = false;
        public bool useStacking = false;
        public bool isClickToClose = true;
        [HideInInspector] public bool isOn = false;
        public StartBehaviour startBehaviour = StartBehaviour.Disable;
        public CloseBehaviour closeBehaviour = CloseBehaviour.Disable;

        // Events
        public UnityEvent onOpen;
        public UnityEvent onClose;
        public Action onCloseVolatile;
        public UnityEvent onBtnClick;

        public enum StartBehaviour { None, Disable }
        public enum CloseBehaviour { None, Disable, Destroy }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }

#endif

        void Awake()
        {
            isOn = false;

            if (useCustomContent == false) { UpdateUI(); }
            if (notificationAnimator == null) { notificationAnimator = gameObject.GetComponent<Animator>(); }
            if (startBehaviour == StartBehaviour.Disable) { gameObject.SetActive(false); }
            if (useStacking == true)
            {
                try
                {
                    NotificationStacking stacking = transform.GetComponentInParent<NotificationStacking>();
                    stacking.notifications.Add(this);
                    stacking.enableUpdating = true;
                }

                catch { Debug.LogError("<b>[Notification]</b> 'Stacking' is enabled but 'Notification Stacking' cannot be found in parent.", this); }
            }
            if (clickBtn = GetComponentInChildren<Button>(true))
            {
                onBtnClick = new UnityEvent();
                if (isClickToClose)
                    onBtnClick.AddListener(Close);
                clickBtn.onClick.AddListener(onBtnClick.Invoke);
            }
        }

        public void Open()
        {
            if (isOn == true)
                return;

            gameObject.SetActive(true);
            isOn = true;

            if (CO_StartTimer != null)
            {
                StopCoroutine(CO_StartTimer);
                CO_StartTimer = null;
            }
            if (CO_DisableNotification != null)
            {
                StopCoroutine(CO_DisableNotification);
                CO_DisableNotification = null;
            }

            notificationAnimator.Play("In");
            onOpen.Invoke();

            if (enableTimer == true) { CO_StartTimer = StartCoroutine(DO_StartTimer()); }
        }

        public void Close()
        {
            if (isOn == false)
                return;

            isOn = false;
            notificationAnimator.Play("Out");
            onClose.Invoke();
            if (onCloseVolatile != null)
            {
                onCloseVolatile.Invoke();
                onCloseVolatile = null;
            }
            if (CO_DisableNotification == null)
                CO_DisableNotification = StartCoroutine(DO_DisableNotification());
        }

        // Obsolete
        public void OpenNotification() { Open(); }
        public void CloseNotification() { Close(); }

        public void UpdateUI()
        {
            if (iconObj != null) { iconObj.sprite = icon; }
            if (titleObj != null) { titleObj.SetText(title); }
            if (descriptionObj != null) { descriptionObj.SetText(description); }
        }

        Coroutine CO_StartTimer = null;
        IEnumerator DO_StartTimer()
        {
            yield return null;
            yield return new WaitForSeconds(timer);

            CloseNotification();
            CO_StartTimer = null;
        }

        Coroutine CO_DisableNotification = null;
        IEnumerator DO_DisableNotification()
        {
            yield return null;

            yield return new WaitForSeconds(1f);

            if (closeBehaviour == CloseBehaviour.Disable) { gameObject.SetActive(false); isOn = false; }
            else if (closeBehaviour == CloseBehaviour.Destroy) { Destroy(gameObject); }

            CO_DisableNotification = null;
        }
    }
}