using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using CWJ.Serializable;
using CWJ.Singleton.SwapSingleton;

using UnityEngine;
using UnityEngine.Events;

namespace CWJ
{
    [AddComponentMenu("Scripts/" + nameof(CWJ) + "/CWJ_" + nameof(_KeyEventManager))]
    public class _KeyEventManager : SingletonBehaviour_Swap<_KeyEventManager>
    {
        [DrawHeaderAndLine("Variable For Check")]
        [Tooltip("등록된 Listener가 있어야 체크가능")]
        [Readonly, SerializeField] private bool _isHoldDown;//클릭중인지

        public bool isHoldDown { get => _isHoldDown; protected set => _isHoldDown = value; }

        [Tooltip("등록된 Listener가 있어야 체크가능")]
        [Readonly, SerializeField] private bool _isCursorMoving; //클릭후 포인터를 이동중인지

        public bool isCursorMoving { get => _isCursorMoving; protected set => _isCursorMoving = value; }

#if UNITY_EDITOR

        [UnityEditor.MenuItem("GameObject/" + nameof(CWJ) + "/" + nameof(_KeyEventManager), false, 10)]
        public static void CreateTouchManager()
        {
            Transform backupParent = UnityEditor.Selection.activeTransform;

            if (IsExists)
            {
                UnityEditor.Selection.activeTransform = FindUtil.FindObjectOfType_New<_KeyEventManager>(false, false).transform;
                return;
            }

            GameObject newObj = new GameObject(nameof(_KeyEventManager), typeof(_KeyEventManager));
            newObj.transform.SetParent(backupParent);
            newObj.transform.Reset();
            UnityEditor.Selection.activeObject = newObj;
        }

#endif
        [DrawHeaderAndLine("")]
        [Space]
        /// <summary>
        /// Editor 디버깅용
        /// </summary>
        [Readonly] [SerializeField] protected bool isMobile;

        public DictionaryVisualized<KeyCode, List<KeyListener>> keyListenerDic = new();
        public int listenerSubscribingCnt = 0;
        public int listenerTotalCnt = 0;

        protected UnityEvent[] touchEvents = new UnityEvent[5]
        {
            new UnityEvent(), //Began
            new UnityEvent(), //Moved
            new UnityEvent(), //Stationary
            new UnityEvent(), //Ended
            new UnityEvent() //Canceled
        };

        protected UnityEvent onUpdateEnded = new UnityEvent();


        /// <summary>
        /// 최초 추가거나 Subscribe disable상태였으면 true
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public virtual bool AddKeyListener(KeyListener listener)
        {
            if (!listener)
            {
                return false;
            }
            var targetKeyCode = listener.detectTargetKey;
            if (targetKeyCode == KeyCode.None)
            {
                return false;
            }
            if (listener.IsSubscribed)
            {
#if UNITY_EDITOR
                if (this.isSwapped)
                    Debug.LogError($"{nameof(_KeyEventManager)} 의 <{nameof(KeyListener)}.{listener.detectTargetKey}> '{listener.gameObject.name}' 이미 활성화 되어있음", listener.gameObject);
#endif
                return false;
            }

            bool existsKeyCode = keyListenerDic.TryGetValue(targetKeyCode, out var listenerList);
            if (!existsKeyCode)
            {
                keyListenerDic.Add(targetKeyCode, listenerList = new List<KeyListener>());
            }
            bool isNew = !existsKeyCode || !listenerList.Contains(listener);
            if (isNew)
            {
                listenerList.Add(listener);
                ++listenerTotalCnt;
            }
            ++listenerSubscribingCnt;
            listener._SetSubscribedForcibly(true);
#if UNITY_EDITOR
            if (this.isSwapped)
                Debug.Log($"{nameof(_KeyEventManager)} 의 <{nameof(KeyListener)}.{listener.detectTargetKey}> '{listener.gameObject.name}' {(isNew ? "추가" : "활성화").SetColor(new Color().GetOrientalBlue())} 완료", listener.gameObject);
#endif
            return true;
        }

        /// <summary>
        ///  오브젝트를 지우진않음 RemoveListener만 함
        /// </summary>
        /// <param name="listener"></param>
        /// <returns>can really remove</returns>
        public virtual bool RemoveKeyListener(KeyListener listener)
        {
            if (!listener)
            {
                return false;
            }
            var targetKeyCode = listener.detectTargetKey;
            if (targetKeyCode == KeyCode.None)
            {
                return false;
            }
            if (!listener.IsSubscribed)
            {
#if UNITY_EDITOR
            if (this.isSwapped)
                Debug.LogError($"{nameof(_KeyEventManager)} 의 <{nameof(KeyListener)}.{listener.detectTargetKey}> '{listener.gameObject.name}' 이미 비활성화 되어있음", listener.gameObject);
#endif
                return false;
            }

            bool isExists = keyListenerDic.TryGetValue(targetKeyCode, out var list) && list.Contains(listener);

            if (isExists)
            {
#if UNITY_EDITOR
            if (this.isSwapped)
                Debug.Log($"{nameof(_KeyEventManager)} 의 <{nameof(KeyListener)}.{listener.detectTargetKey}> '{listener.gameObject.name}' {("비활성화 완료.").SetColor(new Color().GetLightRed())}", listener.gameObject);
#endif
                --listenerSubscribingCnt;
            }
            listener._SetSubscribedForcibly(false);
            return isExists;
        }

        protected override void OnBeforeInstanceAssigned(_KeyEventManager prev, _KeyEventManager newInstance)
        {
            if (keyListenerDic == null)
                keyListenerDic = new();
        }

        public virtual bool OnDestroyKeyListener(KeyListener listener)
        {
            if (!listener)
            {
                return false;
            }
            var targetKeyCode = listener.detectTargetKey;
            if (targetKeyCode == KeyCode.None)
            {
                return false;
            }
            bool isRemoved = false;
            if (keyListenerDic.TryGetValue(targetKeyCode, out var list))
            {
                if (list != null)
                {
                    isRemoved = list.Remove(listener);
                    if (isRemoved)
                    {
#if UNITY_EDITOR
                        if (this.isSwapped)
                            Debug.Log($"{nameof(_KeyEventManager)} 의 <{nameof(KeyListener)}.{listener.detectTargetKey}> '{listener.gameObject.name}' {("제거됨.").SetColor(new Color().GetMagenta())}", listener.gameObject);
#endif

                        --listenerTotalCnt;
                        if (listener.IsSubscribed)
                        {
                            --listenerSubscribingCnt;
                            listener._SetSubscribedForcibly(false);
                        }
                    }
                }
                if (list == null || list.Count == 0)
                    keyListenerDic.Remove(targetKeyCode);
            }

            return isRemoved;
        }

        public virtual void UpdateInputSystem() { }

        protected void Update()
        {
            if (listenerSubscribingCnt > 0)
                UpdateInputSystem();
        }

        protected override Type GetSwapType()
        {
#if !UNITY_EDITOR
isMobile = 
#if !UNITY_ANDROID
            false;
#else
            true;
#endif
#endif
            return isMobile ? typeof(TouchManager_Mobile) : typeof(KeyEventManager_PC);
        }

        public bool isSwapped = false;

        protected override void SwapSetting(_KeyEventManager newComp)
        {
            var copiedKv = keyListenerDic?.ToArrayOnlyValues() ?? null;
            keyListenerDic?.Clear();
            listenerSubscribingCnt = 0;
            listenerTotalCnt = 0;

            int length = copiedKv != null ? copiedKv.Length : 0;

            if (!newComp.isSwapped)
            {
                newComp.isMobile = isMobile;
                newComp.listenerSubscribingCnt = 0;
                newComp.listenerTotalCnt = 0;

                newComp.isSwapped = true;

                if (newComp.keyListenerDic == null)
                    newComp.keyListenerDic = new DictionaryVisualized<KeyCode, List<KeyListener>>(capacity: length);
            }

            if (length > 0)
            {
                foreach (var item in copiedKv)
                {
                    int cnt = item.Count;
                    for (int i = 0; i < cnt; i++)
                    {
                        var listener = item[i];
                        listener._SetSubscribedForcibly(false);
                        newComp.AddKeyListener(listener);
                    }
                }
            }

            try
            {
                if (keyListenerDic != null && keyListenerDic.Count > 0)
                    SwapSetting(newComp);
            }
            catch(Exception e) { Debug.LogError(e.ToString()); }
        }
    }
}
