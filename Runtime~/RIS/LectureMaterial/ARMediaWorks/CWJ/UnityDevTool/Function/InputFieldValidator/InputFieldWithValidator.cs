using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CWJ
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputStr"></param>
    /// <returns>validate value</returns>
    public delegate string GetValidateStr(string inputStr);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputStr"></param>
    /// <param name="value"></param>
    /// <returns>isValid</returns>
    public delegate bool CheckValidation(string inputStr, out string value);
    [System.Serializable]
    public abstract class InputFieldWithValidator
    {
        [VisualizeField]
        protected static Dictionary<string, InputFieldWithValidator> ValidatorByKeyDict
              = new Dictionary<string, InputFieldWithValidator>();

        protected static readonly Color validColor = new Color().GetSkyBlue();
        protected static readonly Color invalidColor = new Color().GetLightRed();
        protected const string FigmaIpfKeyword = "Field-";
        protected static int LengthOfFigmaIpfKeyword => FigmaIpfKeyword.Length;

        /// <summary>
        /// 필수적으로 세팅해줘야함
        /// </summary>
        /// <param name="keyValuePairs"></param>
        public static void InitKeyAndValidators(params (string key, InputFieldWithValidator validator, string displayName)[] keyValuePairs)
        {
            foreach (var kv in keyValuePairs)
            {
                AddKeyAndValidators(kv.key, kv.validator, kv.displayName);
            }
        }

        public static void AddKeyAndValidators(string key, InputFieldWithValidator validator, string displayName)
        {
            if (!string.IsNullOrEmpty(key) && !ValidatorByKeyDict.ContainsKey(key))
            {
                validator.SetName(key, displayName);
                ValidatorByKeyDict.Add(key, validator);
            }
        }

        public static InputFieldWithValidator[] GetValidatorsInTrf(Transform ipfRoot)
        {
            List<InputFieldWithValidator> list = new List<InputFieldWithValidator>();
            foreach (var item in ipfRoot.GetComponentsInChildren<TMP_InputField>())
            {
                var validator = GetValidator(item);
                if (validator != null)
                    list.Add(validator);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 이걸로 받는 <see cref="InputFieldWithValidator"/>를 통해 value를 체크, 
        /// <see cref="CheckInvalidAndSetFocus"/>를 통해 통과되지 못한 Ipf도 체크 가능함
        /// </summary>
        /// <param name="inputField"></param>
        /// <returns></returns>
        public static InputFieldWithValidator GetValidator(TMP_InputField inputField)
        {
            if (inputField == null)
            {
                return null;
            }
            string objName = inputField.gameObject.name;
            string key = objName.StartsWith(FigmaIpfKeyword) ? objName.Substring(LengthOfFigmaIpfKeyword) : null;
            var validator = GetValidator(key);
            if (validator != null)
            {
                validator.Init(inputField);
                return validator;
            }
            return null;
        }

        public static InputFieldWithValidator GetValidator(string keyName)
        {
            if (ValidatorByKeyDict.TryGetValue(keyName, out var validator))
            {
                return validator;
            }
            Debug.LogError(keyName + "는 존재하지 않음!!");
            return null;
        }

        /// <summary>
        /// isValid가 false면 포커싱 켜줌
        /// </summary>
        /// <returns></returns>
        public bool CheckInvalidAndSetFocus()
        {
            if (ipf == null)
            {
                return false;
            }
            if (isEssential && (!isValid || isForciblyInvalid))
            {
                if(!isValid && !isForciblyInvalid)
                {
                    if (!isEdited)
                    {
                        isEdited = true;
                    }
                    OnEndEdit(null);
                    if (isValid)
                    {
                        return false;
                    }
                }

                ipf.Select();
                ipf.MoveTextEnd(true);
                ipf.ActivateInputField();

                AndroidToast.Instance.ShowToastMessage(errorLog, AndroidToast.ToastTime.Long);
                return true;
            }
            

            return false;
        }

        [VisualizeProperty] public string keyName { get; private set; } = null;
        [VisualizeProperty] public string displayName { get; private set; } = null;
        [VisualizeProperty] public TMP_InputField ipf { get; private set; } = null;
        [VisualizeProperty] public Image outlineImg { get; private set; } = null;
        [VisualizeProperty] public string value { get; protected set; }
        [VisualizeProperty] public string ipfText => ipf != null ? ipf.text : string.Empty;
        [VisualizeProperty] public bool isValid { get; protected set; }
        [VisualizeProperty] public bool isEdited { get; protected set; }

        [VisualizeProperty] public string errorLog { get; protected set; }
        protected virtual string invalidReason => " 양식에 맞게 입력하세요.";

        [VisualizeProperty] public bool isOutlineChecked { get; protected set; }
        /// <summary>
        /// default : true
        /// </summary>
        [VisualizeProperty] public virtual bool isEssential => true;
        /// <summary>
        /// default : false
        /// </summary>
        [VisualizeProperty] protected virtual bool isAbleWhiteSpace => false;
        [VisualizeProperty] public virtual int minLimit => isEssential ? 1 : 0;
        [VisualizeProperty] public virtual int maxLimit => -1;
        protected abstract GetValidateStr callback_GetValidateStr { get; }
        protected abstract CheckValidation callback_CheckValidation { get; }

        private GetValidateStr getValidateStr;
        private CheckValidation checkValidation;

        private void SetName(string keyName, string displayName)
        {
            this.keyName = keyName;
            this.displayName = displayName;
        }

        private InputFieldWithValidator Init(TMP_InputField inputField)
        {
            if (this.ipf != null && inputField == ipf)
            {
                return this;
            }

            getValidateStr = callback_GetValidateStr ?? GetValidStr_Default;
            checkValidation = callback_CheckValidation ?? CheckValidate_Default;

            ipf = inputField;
            if (ipf.textViewport == null)
                ipf.textViewport = ipf.textComponent.GetComponent<RectTransform>();

            if (maxLimit > 0)
            {
                ipf.characterLimit = maxLimit;
            }
            ipf.onValueChanged.RemoveAllListeners();

            ipf.onValueChanged.AddListener(OnValueChanged);

            ipf.onEndEdit.RemoveAllListeners();
            ipf.onEndEdit.AddListener(OnEndEdit);
            outlineImg = ipf.GetComponentInChildren<Image>();
            ClearCache();

            return this;
        }

        public void ClearCache()
        {
            if (ipf != null)
                ipf.SetTextWithoutNotify(string.Empty);
            errorLog = value = string.Empty;
            isForciblyInvalid = isEdited = isValid = isOutlineChecked = false;
            if (outlineImg != null)
                outlineImg.color = Color.white;
        }


        /// <summary>
        /// 버튼 입력없이 value를 가져오려면 이걸 써야함
        /// </summary>
        public void Submit()
        {
            OnEndEdit(ipf.text);
            EventSystem.current.SetSelectedGameObject(null);
        }

        private string GetValidStr_Default(string inputStr) => inputStr;
        private bool CheckValidate_Default(string input, out string validateValue) => (validateValue = input) != null && input.Trim().Length > 0;

        public void OnValueChanged(string inputStr)
        {
            if (!isEdited && inputStr.LengthSafe() > 0)
            {
                isEdited = true;
            }
            if (!isAbleWhiteSpace)
            {
                inputStr = inputStr.Replace(" ", string.Empty);
            }

            string validateStr = getValidateStr.Invoke(inputStr);

            if (validateStr != inputStr)
            {
                ipf.SetTextWithoutNotify(validateStr);
                ipf.MoveTextEnd(false);
            }
            if (isOutlineChecked)
            {
                OnEndEdit(validateStr);
            }
        }

        bool isForciblyInvalid = false;
        /// <summary>
        /// 이거로 Valid건드리면 이거로 isValid true 해주기전까진 안바뀜
        /// </summary>
        /// <param name="_isValid"></param>
        /// <param name="invalidReason"></param>
        public void SetValidForcibly(bool _isValid, string invalidReason = null)
        {
            isValid = _isValid;
            if (outlineImg != null)
                outlineImg.color = isValid ? validColor : invalidColor;

            if (!isValid && invalidReason != null)
                AndroidToast.Instance.ShowToastMessage(errorLog = invalidReason);
            isForciblyInvalid = !isValid;
        }

        public void OnEndEdit(string endEditStr)
        {
            UnityEngine.Debug.LogError("EndEdit: " + endEditStr);
            if (!isEdited)
            {
                return;
            }

            string lastValue = value;

            if (endEditStr == null)
                endEditStr = ipf.text ?? string.Empty;

            bool isLengthSuccess = endEditStr.Length.IsAround(minLimit, maxLimit);
            string validateStr = null;
            isValid = isLengthSuccess && checkValidation.Invoke(endEditStr, out validateStr);
            if (validateStr != null)
            {
                if (!validateStr.Equals(endEditStr))
                    ipf.SetTextWithoutNotify(validateStr);
                value = validateStr;
            }
            else
            {
                value = endEditStr;
            }


            if (isEssential)
            {
                if (isForciblyInvalid)
                {
                    isOutlineChecked = false;
                    if (!isEdited || endEditStr == lastValue)
                        return;
                }
                if (outlineImg != null)
                    outlineImg.color = isValid ? validColor : invalidColor;


                if (isValid)
                {
                    errorLog = string.Empty;
                }
                else
                {
                    errorLog = displayName;
                    if (isLengthSuccess)
                        errorLog += invalidReason;
                    else
                    {
                        if (minLimit == maxLimit)
                        {
                            errorLog += $" : 글자수 {minLimit}개 만 허용됩니다.";
                        }
                        else
                        {
                            if (maxLimit < 0)
                                errorLog += $" : {minLimit}글자 이상이어야 합니다.";
                            else
                                errorLog += $" : {minLimit}에서 {maxLimit}자 사이여야 합니다.";
                        }

                    }
#if UNITY_ANDROID || UNITY_EDITOR
                    if (!isOutlineChecked)
                        AndroidToast.Instance.ShowToastMessage(errorLog, AndroidToast.ToastTime.Long);
#endif
                }
                isOutlineChecked = !isValid;
            }
        }
    }

    public class IpfValidator_Default : InputFieldWithValidator
    {
        protected override GetValidateStr callback_GetValidateStr => null;
        protected override CheckValidation callback_CheckValidation => null;
    }

    public class IpfValidator_NotEssential : InputFieldWithValidator
    {
        protected override GetValidateStr callback_GetValidateStr => null;
        protected override CheckValidation callback_CheckValidation => null;
        public override bool isEssential => false;
    }
    public class IpfValidator_DefaultWhiteSpace : InputFieldWithValidator
    {
        protected override GetValidateStr callback_GetValidateStr => null;
        protected override CheckValidation callback_CheckValidation => null;
        protected override bool isAbleWhiteSpace => true;
    }
    public class IpfValidator_AccountId : InputFieldWithValidator
    {
        public const int MinLimit = 3;
        protected override GetValidateStr callback_GetValidateStr => RegexUtil.GetValidateStr_AccountId;
        protected override CheckValidation callback_CheckValidation => RegexUtil.CheckValidation_AccountId;
        public override int minLimit => MinLimit;
        public override int maxLimit => 30;
        protected override string invalidReason => "는 글자나 문자로만 이루어져야 합니다";
    }

    public class IpfValidator_Name : InputFieldWithValidator
    {
        protected override GetValidateStr callback_GetValidateStr => RegexUtil.GetValidateStr_Name;
        protected override CheckValidation callback_CheckValidation => null;
        public override int minLimit => 2;
    }
    public class IpfValidator_Password : InputFieldWithValidator
    {
        public const int MinLimit = 4;
        protected override GetValidateStr callback_GetValidateStr => RegexUtil.GetValidateStr_Password;
        protected override CheckValidation callback_CheckValidation => RegexUtil.CheckValidation_Password;
        public override int minLimit => MinLimit;
        protected override string invalidReason => "는 문자, 숫자를 조합하여 입력해주세요";
    }

    public class IpfValidator_PasswordCheck : InputFieldWithValidator
    {
        TMP_InputField passwordIpf;

        public IpfValidator_PasswordCheck(TMP_InputField passwordIpf)
        {
            this.passwordIpf = passwordIpf;
        }

        bool CheckValidation_PasswordCheck(string inputStr, out string validateValue)
        {
            validateValue = inputStr;
            return passwordIpf.text.Equals(validateValue);
        }
        protected override GetValidateStr callback_GetValidateStr => RegexUtil.GetValidateStr_Password;
        protected override CheckValidation callback_CheckValidation => CheckValidation_PasswordCheck;
        public override int minLimit => IpfValidator_Password.MinLimit;
        protected override string invalidReason => " : '비밀번호' 와 동일한 값을 입력해주세요.";
    }

    public class IpfValidator_Email : InputFieldWithValidator
    {
        protected override GetValidateStr callback_GetValidateStr => null;
        protected override CheckValidation callback_CheckValidation => RegexUtil.CheckValidation_Email;
        public override int maxLimit => 40; //30+@gmail.com
        protected override string invalidReason => " : 이메일 양식에 맞춰 입력해주세요";
    }

    public class IpfValidator_PhoneNum : InputFieldWithValidator
    {
        protected override GetValidateStr callback_GetValidateStr => RegexUtil.GetValidateStr_PhoneNum;
        protected override CheckValidation callback_CheckValidation => RegexUtil.CheckValidation_PhoneNum;
        public override int minLimit => 12; //(3-3-4)
        public override int maxLimit => 13; //(3-4-4)
        protected override string invalidReason => " : 국내 핸드폰 번호(010) 양식에 맞춰 입력해주세요";
    }

    public class IpfValidator_CompRegNum : InputFieldWithValidator
    {
        protected override GetValidateStr callback_GetValidateStr => RegexUtil.GetValidateStr_CompRegNum;
        protected override CheckValidation callback_CheckValidation => RegexUtil.CheckValidation_CompRegNum;
        public override int minLimit => 12;
        public override int maxLimit => 12; //(2-3-5)
    }

    public class IpfValidator_Year : InputFieldWithValidator
    {
        protected override GetValidateStr callback_GetValidateStr => RegexUtil.GetValidateStr_Year;
        protected override CheckValidation callback_CheckValidation => RegexUtil.CheckValidation_Year;
        public override int minLimit => 2;
        public override int maxLimit => 4;
    }

    public class IpfValidator_Month : InputFieldWithValidator
    {
        protected override GetValidateStr callback_GetValidateStr => RegexUtil.GetValidateStr_MonthOrDay;
        protected override CheckValidation callback_CheckValidation => RegexUtil.CheckValidation_Month;
        public override int minLimit => 1;
        public override int maxLimit => 2;
    }
    public class IpfValidator_Day : InputFieldWithValidator
    {
        protected override GetValidateStr callback_GetValidateStr => RegexUtil.GetValidateStr_MonthOrDay;
        protected override CheckValidation callback_CheckValidation => RegexUtil.CheckValidation_Day;
        public override int minLimit => 1;
        public override int maxLimit => 2;
    }
    public class IpfValidator_Telephone : InputFieldWithValidator
    {
        protected override GetValidateStr callback_GetValidateStr => RegexUtil.GetValidateStr_TeleNum;
        protected override CheckValidation callback_CheckValidation => RegexUtil.CheckValidation_TeleNum;
        public override int minLimit => 9;
        public override int maxLimit => 14; //(4-4-4)
        public override bool isEssential => false;
    }
}