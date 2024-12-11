using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using UnityEngine.SceneManagement;
using UnityEngine.Events;

#if CWJ_EXISTS_NEWINPUTSYSTEM
using UnityEngine.InputSystem;
#endif

namespace CWJ
{
    [System.Serializable] public class UnitySelectableClickEvent : UnityEvent<Selectable> { }

    /// <summary>
    /// <para/>Detect Selectable Object Click
    /// <br/>1.Button, Toggle, Dropdown, 등을 클릭했을때를 감지해줌
    /// <br/>2.클릭이 감지되었을때 클릭소리를 나도록 설정가능
    /// <br/>3.<see cref="OnSelectableClickEvent"/>를 통해 이벤트 등록가능
    /// <para/>Invisible Cursor
    /// <br/>1.Curosr를 보이지않게 설정가능
    /// <para/>TODO : InputField는 소리가 나지않음 테스트필요
    /// <para/>[21.01.13]
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class InputClickManager : CWJ.Singleton.SingletonBehaviour<InputClickManager>
    {
        #region Unity Method

        private void Update()
        {
            curPointerPos = _GetPointerPos();
            _GetBtnState(out isPressed, out isReleased);

            DetectSelectableClick();

            UpdateCursorVisibleSetting();

#if UNITY_EDITOR
            Editor_VisualizePointerStatus();
#endif
        }

        protected override void _Reset()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
                audioSource.playOnAwake = false;
#if UNITY_EDITOR
                UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
                isVisualizeInHierarchy = CWJ.AccessibleEditor.DisplayDialogUtil.DisplayDialog<InputClickManager>
                    ($"{nameof(InputClickManager)}의 Editor Debug 모드를 활성화 하시겠습니까?\n실행중에 Selectable 클릭시 Hierarchy에서 feedback이 옵니다", ok: "yes", cancel: "no");
#endif
            }
        }

        protected override void _Awake()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        protected override void _OnDestroy()
        {
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SetCurSceneClickSound(null);
            if (CurSelectable.IsNullOrMissing() || !CurSelectable.gameObject.IsDontDestroyOnLoad())
            { //DontDestroyOnLoad오브젝트가 아닐때 Init
                CurSelectable = null;
                return;
            }
        }

        protected override void _OnDisable()
        {
            CurSelectable = null;
        }
        #endregion

        #region Common

        private Vector2 curPointerPos;
        private bool isPressed;
        private bool isReleased;

        private Vector2 _GetPointerPos()
        {
#if !CWJ_EXISTS_NEWINPUTSYSTEM
            Vector3 mousePos = Input.mousePosition;
#endif
            return
#if CWJ_EXISTS_NEWINPUTSYSTEM
            Mouse.current.position.ReadValue(); //Android 코드 추가해야함
#else
#if UNITY_ANDROID
            Input.touchCount > 0 ? Input.GetTouch(0).position:
#endif
            new Vector2(mousePos.x, mousePos.y);
#endif
        }

#if UNITY_ANDROID
        private bool isTouchPressedPrevFrame = false;
#endif
        private void _GetBtnState(out bool isPressed, out bool isReleased)
        {
#if !CWJ_EXISTS_NEWINPUTSYSTEM && UNITY_ANDROID
            bool isTouchPressed = Input.touchCount > 0 && Input.GetTouch(0).phase < TouchPhase.Ended;
            bool isTouchReleased = !isTouchPressed && isTouchPressedPrevFrame;
            isTouchPressedPrevFrame = isTouchPressed;
#endif
            isPressed = (
#if CWJ_EXISTS_NEWINPUTSYSTEM
            Mouse.current != null && Mouse.current.leftButton.isPressed //AOS 코드 추가해야함
#else
#if UNITY_ANDROID
            isTouchPressed ||
#endif
            Input.GetKey(keyToDetectClick)
#endif
            );

            isReleased = (
#if CWJ_EXISTS_NEWINPUTSYSTEM
            Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame //AOS 코드 추가해야함
#else
#if UNITY_ANDROID
            isTouchReleased ||
#endif
            Input.GetKeyUp(keyToDetectClick)
#endif
            );
        }

        private void CheckRayResults(ref List<RaycastResult> raycastResults)
        {
            if (EventSystem.current == null)
            {
                raycastResults?.Clear();
                return;
            }

            var ped = new PointerEventData(EventSystem.current);
            ped.position = curPointerPos;
            EventSystem.current.RaycastAll(ped, raycastResults);
        }

        private bool CheckIsPointerOverSelectable(List<RaycastResult> raycastResults)
        {
            bool result = false;
            if (raycastResults.Count > 0)
            {
                Selectable nearRootSelectable = raycastResults[0].gameObject.GetComponentInParent<Selectable>();
                if (nearRootSelectable != null && nearRootSelectable.IsInteractable())
                {
                    if (raycastResults.Exists(r => r.gameObject.GetComponent<Selectable>() == nearRootSelectable))
                    {
                        result = true;
                    }
                    else
                    {
                        var rootCanvas = nearRootSelectable.GetComponentInParent<Canvas>()?.rootCanvas;
                        var pointerPos = _GetPointerPos();
                        if (CheckIsRectTrfContainsPointer(nearRootSelectable.GetComponent<RectTransform>(), pointerPos, rootCanvas) ||
                            CheckIsRectTrfContainsPointer(nearRootSelectable.targetGraphic?.GetComponent<RectTransform>(), pointerPos, rootCanvas))
                        {
                            result = true;
                        }
                    }
                }

                raycastResults.Clear();
            }
            return result;
        }

        //public

        public Vector2 GetPointerPos()
        {
            if (isActiveAndEnabled)
                return curPointerPos;
            else
                return _GetPointerPos();
        }

        public void GetInputState(out bool isPressed, out bool isReleased)
        {
            if (isActiveAndEnabled)
            {
                isPressed = this.isPressed;
                isReleased = this.isReleased;
            }
            else
                _GetBtnState(out isPressed, out isReleased);
        }

        public bool GetIsPointerOverUI()
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>(5);
            CheckRayResults(ref raycastResults);
            return raycastResults.Count > 0;
        }

        public bool GetIsPointerOverSelectable()
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>(5);
            CheckRayResults(ref raycastResults);
            return CheckIsPointerOverSelectable(raycastResults);
        }
#endregion

#region Detect Selectable UI Click - Variables
        public class SelectInfo
        {
            [SerializeField, Readonly] Selectable curSelectable;
            [SerializeField, Readonly] Canvas targetCanvas;
            RectTransform curSelectedTrf;
            RectTransform curSelectedTargetGraphicTrf;
        }

        [Header("Detect Selectable UI Click")]
#if !CWJ_EXISTS_NEWINPUTSYSTEM
        [SerializeField]
        KeyCode keyToDetectClick = KeyCode.Mouse0;
#endif


        HashSet<int> selectedIDs = new HashSet<int>();
        [SerializeField, Readonly] Selectable curSelectable;
        public Selectable CurSelectable
        {
            get => curSelectable;
            protected set
            {
                if (curSelectable = value)
                {
                    isPointerContainsSelectedRect = isSelected = true;
                    targetCanvas = CurSelectable.GetComponentInParent<Canvas>().rootCanvas;
                    curSelectedTrf = CurSelectable.GetComponent<RectTransform>();
                    curSelectedTargetGraphicTrf = CurSelectable.targetGraphic?.GetComponent<RectTransform>();
                }
                else
                {
                    isPointerContainsSelectedRect = isSelected = false;
                    targetCanvas = null;
                    curSelectedTrf = null;
                    curSelectedTargetGraphicTrf = null;
                }
            }
        }

        [SerializeField, Readonly] bool isSelected;
        [SerializeField, Readonly] bool isPointerContainsSelectedRect;
        [SerializeField, Readonly] Canvas targetCanvas;
        RectTransform curSelectedTrf;
        RectTransform curSelectedTargetGraphicTrf;

        public UnitySelectableClickEvent OnSelectableClickEvent = new UnitySelectableClickEvent();

        [FoldoutGroup("Click Sound", true)]
        public bool isClickSoundEnabled;
        [SerializeField] private AudioSource audioSource;

        [Tooltip("If all clickSound is null, no click sound is triggered")]
        [SerializeField] private AudioClip defaulClickSound = null;
        public void SetDefaultClickSound(AudioClip audioClip)
        {
            defaulClickSound = audioClip;
        }

        [FoldoutGroup("Click Sound", false)]
        [SerializeField] private AudioClip curSceneClickSound = null;
        public void SetCurSceneClickSound(AudioClip audioClip)
        {
            curSceneClickSound = audioClip;
        }

        private bool select;
        public bool Select { get => select; set => select = value; }

#endregion

#region Detect Selectable UI Click - Method
        private void DetectSelectableClick()
        {
            EventSystem curEventSystem = EventSystem.current;

            if (!isSelected)
            {
                if (isPressed && !curEventSystem.IsNullOrMissing() && !curEventSystem.currentSelectedGameObject.IsNullOrMissing())
                {
                    CurSelectable = curEventSystem.currentSelectedGameObject.GetComponent<Selectable>();
                    Debug.LogError("?");
                }
            }
            else
            {
                if (!isReleased)
                {
                    if (curEventSystem.IsNullOrMissing() || curEventSystem.currentSelectedGameObject.IsNullOrMissing()
                        || CurSelectable.IsNullOrMissing()
                        || CurSelectable.gameObject != curEventSystem.currentSelectedGameObject
                        || !CurSelectable.gameObject.activeInHierarchy || !CurSelectable.IsInteractable())
                    { //Destroyed or Disabled or Uninteractable
                        CurSelectable = null;
                        return;
                    }
                    else
                    {
                        isPointerContainsSelectedRect = CheckIsRectTrfContainsPointer(curSelectedTargetGraphicTrf, curPointerPos, targetCanvas)
                                                        || CheckIsRectTrfContainsPointer(curSelectedTrf, curPointerPos, targetCanvas);
                    }
                }
                else
                {
                    if (isPointerContainsSelectedRect) // 이전 프레임의 isPointerContainsSelectedRect로 판단함
                    {
                        OnSelectableClick();
                    }
                }
            }

            if (isReleased)
            {
                CurSelectable = null;
            }
        }

        private bool CheckIsRectTrfContainsPointer(RectTransform rectTransform, Vector2 pointerPos, Canvas rootCanvas)
        {
            if (rectTransform.IsNullOrMissing()) return false;
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pointerPos, rootCanvas?.worldCamera);
        }

        private void OnSelectableClick()
        {
            if (isClickSoundEnabled)
            { //play click sound
                var sound = curSceneClickSound ?? defaulClickSound;
                if (sound) audioSource.PlayOneShot(sound);
            }

            OnSelectableClickEvent?.Invoke(CurSelectable);
        }
#endregion

#region Cursor Visible - Variables

        [Foldout("Cursor Visible", true)]
        [SerializeField] private bool isCursorVisibleModifiable = false;

        public bool IsCursorVisibleModifiable
        {
            get => isCursorVisibleModifiable;
            set
            {
                if (!value)
                {
                    IsCursorVisible = true;
                }
                isCursorVisibleModifiable = value;
            }
        }

        [SerializeField] private bool isCursorVisible = true;

        public bool IsCursorVisible
        {
            get => isCursorVisible;
            set
            {
                if (!isCursorVisibleModifiable) return;

                Cursor.visible = isCursorVisible = value;
                Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }


        [Foldout("Cursor Visible", false)]
        [SerializeField]
#if CWJ_EXISTS_NEWINPUTSYSTEM
        Key cursorVisibleModifyKey = Key.Escape;
#else
        KeyCode cursorVisibleModifyKey = KeyCode.Escape;
#endif

#endregion

#region Cursor Visible - Method
        private void UpdateCursorVisibleSetting()
        {
            if (isCursorVisibleModifiable)
            {
                bool isCursorVisibleKeyPressed =
#if CWJ_EXISTS_NEWINPUTSYSTEM
                    Keyboard.current != null && Keyboard.current[cursorVisibleModifyKey].wasPressedThisFrame;
#else
                    Input.GetKeyDown(cursorVisibleModifyKey);
#endif
                if (isCursorVisibleKeyPressed)
                {
                    IsCursorVisible = !IsCursorVisible;
                }
            }
        }
#endregion

#region Editor Debug
#if UNITY_EDITOR
        //인스펙터 확인용
        [Header("Editor Debug")]
        [SerializeField] bool isVisualizeInHierarchy = false;
        [SerializeField, Readonly] bool isPointerOverUI;
        [SerializeField, Readonly] bool isPointerOverSelectableUI;
        List<RaycastResult> raycastResults;
        bool isEditorInit = false;
        private void Editor_VisualizePointerStatus()
        {
            if (!isVisualizeInHierarchy)
            {
                if (isEditorInit)
                {
                    OnSelectableClickEvent.RemoveListener(Editor_OnSelectedSelectable);
                    raycastResults = null;
                    isEditorInit = false;
                }
                return;
            }

            if (!isEditorInit)
            {
                OnSelectableClickEvent.AddListener(Editor_OnSelectedSelectable);
                raycastResults = new List<RaycastResult>(5);
                isEditorInit = true;
            }

            CheckRayResults(ref raycastResults);
            isPointerOverUI = raycastResults.Count > 0;
            isPointerOverSelectableUI = CheckIsPointerOverSelectable(raycastResults);
            raycastResults.Clear();
        }

        private void Editor_OnSelectedSelectable(Selectable select)
        {
            if (isVisualizeInHierarchy && select != null)
                UnityEditor.EditorGUIUtility.PingObject(select.gameObject);
        }
#endif
#endregion
    }
}
