using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Michsky.MUIP;
using UnityEngine.UI;
using UnityEngine.Events;

namespace CWJ
{
    public class UIHelper : CWJ.Singleton.SingletonBehaviour<UIHelper>
    {
        [SerializeField] GameObject loadingObj;
        [SerializeField] NotificationManager notiMngr;
        [SerializeField] Sprite notifIconInfo;
        [SerializeField] Sprite notifIconWarning;
        public Sprite wifiIcon, bluetoothIcon, gpsIcon;

        public bool LoadingEnabled() => loadingObj.activeSelf;

        Coroutine CO_TimeoutTurnOff = null;

        public bool TurnOnLoadingUI(Func<bool> loadingSuccessPredicate = null, float timeout = 4, Action timeoutAction = null)
        {
            if (loadingObj.activeSelf)
            {
                return false;
            }
            if (CO_TimeoutTurnOff != null)
                TurnOffLoadingUI();
            loadingObj.SetActive(true);
            timeoutAction += TurnOffLoadingUI;
            if (loadingSuccessPredicate == null)
                loadingSuccessPredicate = () => !loadingObj.activeSelf;
            CO_TimeoutTurnOff = StartCoroutine(CWJ.CoroutineUtil.GetCoroutine_WaitUntilWithTimeout(timeout, loadingSuccessPredicate
                , () => !loadingObj.activeSelf,
                TurnOffLoadingUI, timeoutAction, startDelay: 0.5f));
            return true;
        }

        public void TurnOffLoadingUI()
        {
            if (loadingObj.activeSelf)
                loadingObj.SetActive(false);
            if (CO_TimeoutTurnOff != null)
            {
                StopCoroutine(CO_TimeoutTurnOff);
                CO_TimeoutTurnOff = null;
            }
        }


        public bool NotiEnabled()
        {
            return notiMngr.isOn;
        }
        public void ShowNotification(string title, string message, Sprite customIcon = null, bool isError = false, float timer = 3)
        {
            if (notiMngr.isOn)
            {
                notiMngr.onCloseVolatile += () => ShowNotification(title, message, customIcon, isError, timer);
                return;
            }
            notiMngr.useCustomContent = false;
            notiMngr.title = title;
            notiMngr.description = message;
            if (customIcon == null)
            {
                string t_upper = title.ToUpper();
                if (t_upper.Equals("BLE"))
                    notiMngr.icon = bluetoothIcon;
                else if (t_upper.Equals("GPS"))
                    notiMngr.icon = gpsIcon;
                else if (t_upper.Equals("WIFI"))
                    notiMngr.icon = wifiIcon;
                else
                    notiMngr.icon = isError ? notifIconWarning : notifIconInfo;
            }
            else
            {
                notiMngr.icon = customIcon;
            }
            notiMngr.timer = timer;
            notiMngr.UpdateUI();
            notiMngr.Open();
        }

        public void HideNotification()
        {
            notiMngr.Close();
        }

        public void PopupMsg(string title, string content, UnityAction okAction = null, UnityAction cancelAction = null, UnityAction closeAction = null, bool isOnlyOkBtn = false, bool isCenterOrUp = true)
        {
            PopupMsgUI.Instance.PopMsg(title, content, okAction, cancelAction, closeAction, isOnlyOkBtn, isCenterOrUp);
        }
    }
}
