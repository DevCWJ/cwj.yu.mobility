using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Reflection;

namespace CWJ.UI
{
    /// <summary>
    /// UI가 UGUI.InputField로 작업되어 있을경우 InputField 오브젝트에 추가해주기
    /// <br/>엥간하면 TMPro.InputField 쓰기. 
    /// </summary>
    [AddComponentMenu("CWJ" +"/InputField_BugFixer")]
    [RequireComponent(typeof(InputField))]
    public class FixedInputFieldBug : MonoBehaviour, IPointerClickHandler
#if UNITY_STANDALONE_WIN
        , IDeselectHandler
#endif
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("GameObject/" + nameof(CWJ) + "/UI/InputField_BugFixer", false, 10)]
        public static void CreateCWJInputField()
        {
            UnityEditor.EditorApplication.ExecuteMenuItem("GameObject/UI/Input Field");

            System.Action editorCallback = () =>
            {
                GameObject newThisObj = UnityEditor.Selection.activeGameObject;
                newThisObj.AddComponent<FixedInputFieldBug>();
                newThisObj.name = "InputField_BugFixed";
                UnityEditor.Selection.activeObject = newThisObj;

                CWJ.AccessibleEditor.DisplayDialogUtil.DisplayDialog<FixedInputFieldBug>($"InputField+{nameof(FixedInputFieldBug)} 생성 완료");
            };

            System.Func<bool> IsSelectionGetInputField = () =>
            {
                return UnityEditor.Selection.activeGameObject != null && UnityEditor.Selection.activeGameObject.GetComponent<InputField>();
            };
            CWJ.AccessibleEditor.EditorCallback.AddWaitForPredicateCallback(editorCallback, IsSelectionGetInputField, 3);
        }
#endif

        [GetComponent] public InputField inputField;
        [Readonly] [SerializeField] private bool isInit = false;
        static FixedInputFieldBug CurFocusedScript;
        static FieldInfo mTextFieldInfo = null;

        private void Reset()
        {
            Initialize();
        }

        [ContextMenu(nameof(Initialize))]
        private void Initialize()
        {
            if (isInit && inputField == GetComponent<InputField>())
                return;

            inputField = GetComponent<InputField>();
            if (inputField == null)
            {
                Debug.LogError("[ERROR] InputFied Target Null!");
                isInit = false;
                this.enabled = false;
            }
            else
            {
                inputField.onEndEdit.AddListener_New(Input_OnEndEdited);
                isInit = true;
            }
        }

        private void Awake()
        {
            if (!isInit || inputField == null)
                Initialize();

            DisabledFocus();

            if (mTextFieldInfo == null) 
                mTextFieldInfo = inputField.GetType().GetField("m_Text", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        private void OnDisable()
        {
            DisabledFocus();
        }

        private void Input_OnEndEdited(string tmp)
        {
            DisabledFocus();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EnabledFocus();
        }

        private void EnabledFocus()
        {
            CurFocusedScript?.DisabledFocus(this);
            CurFocusedScript = this;

            if (CO_SetFocusInputField != null)
            {
                StopCoroutine(CO_SetFocusInputField);
            }
            CO_SetFocusInputField = StartCoroutine(DO_SetFocusInputField());
        }

        protected void DisabledFocus(FixedInputFieldBug newFocusedScript = null)
        {
            if (CurFocusedScript == this)
            {
                CurFocusedScript = null;
            }

            if (CO_SetFocusInputField != null)
            {
                StopCoroutine(CO_SetFocusInputField);
                CO_SetFocusInputField = null;
            }

            if (newFocusedScript != this)
            {
                inputField.enabled = false;
            }
        }

        private Coroutine CO_SetFocusInputField = null;

        private IEnumerator DO_SetFocusInputField()
        {
            //yield return null;
            if (!inputField.enabled)
                inputField.enabled = true;
            inputField.Select();
            inputField.ActivateInputField();
            yield return null;
            inputField.MoveTextEnd(false);

            CO_SetFocusInputField = null;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (inputField.contentType == InputField.ContentType.Password) return;
            mTextFieldInfo.SetValue(inputField, inputField.textComponent.text);
            //윈도우os+터치Input issue 때문에 추가
        }
    }
}