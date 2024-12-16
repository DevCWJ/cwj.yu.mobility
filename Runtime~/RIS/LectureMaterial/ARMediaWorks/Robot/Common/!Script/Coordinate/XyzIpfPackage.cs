
using System;
using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CWJ.YU.Mobility
{
    public class XyzIpfPackage : MonoBehaviour
    {
        public TextMeshProUGUI nameLabel;
        public TMP_InputField xIpf, yIpf, zIpf;
        public _Ipf_MinMaxValidator validator_minMax;
        public Vector3 curVector3 = Vector3.zero;
        public float checkChangeDelay = 0.5f;
        [SerializeField] UnityEvent<Vector3> onChanged = new();
        float min, max;
        public void Init(string nameStr, _Ipf_MinMaxValidator validator = null, Color color = default)
        {
            if (!nameLabel.gameObject.activeSelf)
                nameLabel.gameObject.SetActive(true);
            nameLabel.SetText(nameStr);
            if (color != default)
                nameLabel.color = color;
            if (validator != null)
                validator_minMax = validator;

            void InitIpf(TMP_InputField ipf)
            {
                ipf.contentType = TMP_InputField.ContentType.Custom;
                ipf.lineType = TMP_InputField.LineType.SingleLine;
                ipf.inputType = TMP_InputField.InputType.Standard;
                if (validator_minMax != null)
                {
                    ipf.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
                    ipf.inputValidator = validator_minMax;
                    ipf.characterLimit = validator_minMax.inputMaxLength + 1;
                    ipf.keyboardType = validator_minMax.UseDecimalPoint ? TouchScreenKeyboardType.DecimalPad : TouchScreenKeyboardType.NumberPad;
                    ipf.placeholder.GetComponent<TextMeshProUGUI>().SetText($"{validator_minMax.minValue} ~ {validator_minMax.maxValue}");
                }
                else
                {
                    ipf.characterValidation = TMP_InputField.CharacterValidation.Decimal;
                    ipf.keyboardType = TouchScreenKeyboardType.DecimalPad;
                    ipf.characterLimit = 10;
                }
            }
            InitIpf(xIpf);
            InitIpf(yIpf);
            InitIpf(zIpf);
            gameObject.name = nameStr;
        }
        public void SubscribeChangeVector3(UnityAction<Vector3> onChangedCallback)
        {
            onChanged.AddListener(onChangedCallback);
        }

        private void Start()
        {
            Debug.Assert(validator_minMax);
            min = validator_minMax.minValue;
            max = validator_minMax.maxValue;

            xIpf.onEndEdit.AddListener(StartUpdateVector);
            yIpf.onEndEdit.AddListener(StartUpdateVector);
            zIpf.onEndEdit.AddListener(StartUpdateVector);
            if (xIpf.inputValidator == null) xIpf.inputValidator = validator_minMax;
            if (yIpf.inputValidator == null) yIpf.inputValidator = validator_minMax;
            if (zIpf.inputValidator == null) zIpf.inputValidator = validator_minMax;
        }
        private void OnDestroy()
        {
            onChanged.RemoveAllListeners();
        }

        Coroutine CO_ChangedValue = null;

        private void StartUpdateVector(string _)
        {
            if (CO_ChangedValue != null)
            {
                StopCoroutine(CO_ChangedValue);
                CO_ChangedValue = null;
            }
            CO_ChangedValue = StartCoroutine(IE_UpdateVectorDelay());
        }

        IEnumerator IE_UpdateVectorDelay()
        {
            yield return null;

            yield return new WaitForSeconds(checkChangeDelay);

            if (GetFloatValue(xIpf, out float x) | GetFloatValue(yIpf, out float y) | GetFloatValue(zIpf, out float z))
            {
                curVector3 = new Vector3(x, y, z);
                onChanged.Invoke(curVector3);
                // Debug.LogError("changed"+curVector3.ToStringByDetailed());
            }
            CO_ChangedValue = null;
        }


        private bool GetFloatValue(TMP_InputField ipf, out float value)
        {
            string input = ipf.text;
            string output = input;
            value = 0;
            if (!string.IsNullOrEmpty(input) && float.TryParse(input, out value))
            {
                if (value > max) value = max;
                else if (value < min) value = min;
                output = value.ToString("F1");
                value = float.Parse(output);
                if (input != output)
                    ipf.SetTextWithoutNotify(output);
                return true;
            }
            return false;
        }
    }
}
