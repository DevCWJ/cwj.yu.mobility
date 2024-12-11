using CWJ;
using CWJ.Singleton;

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupMsgUI : SingletonBehaviourDontDestroy<PopupMsgUI>, CWJ.Singleton.IDontAutoCreatedWhenNull
{
    [SerializeField, ErrorIfNull] GameObject rootCanvasObj;
    [SerializeField] TextMeshProUGUI titleTxt;
    [SerializeField] TextMeshProUGUI contentTxt;
    [SerializeField] Button okBtn;
    [SerializeField] TextMeshProUGUI blueBtnTxt;
    [SerializeField] Button cancelBtn; 
    [SerializeField] TextMeshProUGUI redBtnTxt;
    [SerializeField] UnityEvent okBtnEvent = new UnityEvent();
    [SerializeField] UnityEvent cancelBtnEvent = new UnityEvent();
    [SerializeField] UnityEvent closeEvent = new UnityEvent();

    [Serializable]
    public struct PopMsgData
    {
        public bool isValidData;
        public string title, content;
        public UnityAction okAction, cancelAction, closeAction;
        public bool isOnlyOkBtn, isCenterOrUp;

        public PopMsgData(string title, string content, UnityAction okAction, UnityAction cancelAction, UnityAction closeAction, bool isOnlyOkBtn, bool isCenterOrUp)
        {
            this.isValidData = true;
            this.title = title;
            this.content = content;
            this.okAction = okAction;
            this.cancelAction = cancelAction;
            this.closeAction = closeAction;
            this.isOnlyOkBtn = isOnlyOkBtn;
            this.isCenterOrUp = isCenterOrUp;
        }

        string GetName(UnityAction a) => a != null ? a.Method.Name : string.Empty;

        public override string ToString()
        {
            return title + "|" + content + "|" + GetName(okAction) + "|" + GetName(cancelAction) + "|" + GetName(closeAction);
        }
    }

    [SerializeField] PopMsgData lastPopMsgData;
    Queue<PopMsgData> popMsgDataStack = new Queue<PopMsgData>();

    string blueBtnNameBackup, redBtnNameBackup;

    protected override void _OnEnable()
    {
        rootCanvasObj.transform.SetParent(null);
        rootCanvasObj.transform.SetAsLastSibling();
    }
    protected override void _Awake()
    {
        rootCanvasObj.GetComponent<Canvas>().sortingOrder = 1;

        if (blueBtnNameBackup == null)
        {
            blueBtnNameBackup = blueBtnTxt.text;
            redBtnNameBackup = redBtnTxt.text;
        }

        okBtn.onClick.AddListener(() =>
        {
            okBtnEvent?.Invoke();
            Close();
        });

        if (cancelBtn != null)
        {
            cancelBtn.onClick.AddListener(() =>
            {
                cancelBtnEvent?.Invoke();
                Close();
            });
        }
        if (!lastPopMsgData.isValidData)
            Close();
    }

    RectTransform rectTrf = null;

    [InvokeButton]
    void _SetPivot(bool isCenterOrUpper)
    {
        if (rectTrf == null)
            rectTrf = transform.GetComponent<RectTransform>();
        Vector2 pivot = isCenterOrUpper ? new Vector2(0.5f, 0.5f) : new Vector2(0.5f, 1);
        rectTrf.anchorMin = pivot;
        rectTrf.anchorMax = pivot;
        rectTrf.pivot = pivot;
        if (!isCenterOrUpper)
            rectTrf.anchoredPosition = new Vector2(0, -250);
        else
            rectTrf.anchoredPosition = Vector2.zero;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (cancelBtn != null && cancelBtn.gameObject.activeSelf)
            {
                cancelBtn.onClick?.Invoke();
            }
        }
    }
    
    public bool IsContainsCurMsg(PopMsgData popData)
    {
        return popData.ToString().Equals(lastPopMsgData.ToString());
    }

    public bool IsContainsCurMsg(string title, string content, out PopMsgData popMsgData, UnityAction okAction , UnityAction cancelAction , UnityAction closeAction , bool isOnlyOkBtn , bool isCenterOrUp )
    {
        return this.enabled & (popMsgData = new PopMsgData(title, content, okAction, cancelAction, closeAction, isOnlyOkBtn, isCenterOrUp)).ToString().Equals(lastPopMsgData.ToString());
    }

    public bool IsAlreadyQueMsg(PopMsgData popMsgData)
    {
        if (!popMsgDataStack.TryPeek(out var lastStack))
        {
            return false;
        }

        return popMsgData.ToString().Equals(lastStack.ToString());
    }

    public PopMsgData PopMsgChangeBtnName(string title, string content, string blueBtnName, string redBtnName, UnityAction okAction = null, UnityAction cancelAction = null, UnityAction closeAction = null, bool isOnlyOkBtn = false, bool isCenterOrUp = true)
    {
        if (blueBtnNameBackup == null)
        {
            blueBtnNameBackup = blueBtnTxt.text;
            redBtnNameBackup = redBtnTxt.text;
        }

        if (!rootCanvasObj.activeSelf)
        {
            blueBtnTxt.SetText(blueBtnName);
            redBtnTxt.SetText(redBtnName);
        }
        else
        {
            this.closeEvent.AddListener(() =>
            {
                blueBtnTxt.SetText(blueBtnName);
                redBtnTxt.SetText(redBtnName);
            });
        }

        return PopMsg(title, content, okAction, cancelAction, closeAction, isOnlyOkBtn, isCenterOrUp);
    }
    void ResetUI()
    {
        blueBtnTxt.SetText(blueBtnNameBackup);
        redBtnTxt.SetText(redBtnNameBackup);
        _SetPivot(true);
    }

    public PopMsgData PopMsg(string title, string content, UnityAction okAction = null, UnityAction cancelAction = null, UnityAction closeAction = null, bool isOnlyOkBtn = false, bool isCenterOrUp= true)
    {
        if (IsContainsCurMsg(title, content, out var newMsgData, okAction, cancelAction, closeAction, isOnlyOkBtn, isCenterOrUp)
            && rootCanvasObj.activeSelf)
        {
            return default(PopMsgData);
        }

        if (rootCanvasObj.activeSelf)
        {
            if (!IsAlreadyQueMsg(newMsgData))
                popMsgDataStack.Enqueue(newMsgData);
            return default(PopMsgData);
        }

        lastPopMsgData = newMsgData;
        titleTxt.SetText(newMsgData.title);
        contentTxt.SetText(newMsgData.content);
        
        okBtnEvent.RemoveAllListeners_New();
        closeEvent.RemoveAllListeners_New();
#if UNITY_EDITOR
        closeEvent = null;
        closeEvent = new UnityEvent();
#endif
        cancelBtnEvent.RemoveAllListeners_New();

        if (newMsgData.closeAction != null)
            this.closeEvent.AddListener(newMsgData.closeAction);

        if (newMsgData.okAction != null)
            this.okBtnEvent.AddListener(newMsgData.okAction);

        if (cancelBtn != null)
        {
            if (newMsgData.cancelAction != null)
                this.cancelBtnEvent.AddListener(newMsgData.cancelAction);
            cancelBtn.gameObject.SetActive(!isOnlyOkBtn && (newMsgData.cancelAction != null || newMsgData.okAction != null));
        }

        _SetPivot(isCenterOrUp);
        rootCanvasObj.SetActive(true);
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        enabled = true;
        return newMsgData;
    }

    public void Close()
    {
        enabled = false;
        ResetUI();
        closeEvent?.Invoke();
        closeEvent.RemoveAllListeners_New();
        rootCanvasObj.SetActive(false);
        if (popMsgDataStack != null && popMsgDataStack.Count > 0)
        {
            var data = popMsgDataStack.Dequeue();
            PopMsg(data.title, data.content, data.okAction, data.cancelAction, data.closeAction, data.isOnlyOkBtn, data.isCenterOrUp);
        }
    }


}
