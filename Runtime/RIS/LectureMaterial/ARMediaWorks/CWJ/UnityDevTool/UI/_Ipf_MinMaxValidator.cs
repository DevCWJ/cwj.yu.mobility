
using UnityEngine;

namespace CWJ
{
    public abstract class _Ipf_MinMaxValidator : TMPro.TMP_InputValidator
    {

        public int minValue = -10;
        public int maxValue = 10;

        private bool _hasMinus;
        public bool hasMinus { get => _hasMinus; }

        private int _inputMaxLength = -1;
        public int inputMaxLength
        {
            get
            {
                if (_inputMaxLength <= 0)
                {
                    _inputMaxLength = CalculateMaxLength(minValue, maxValue, ref _hasMinus);
                }
                return _inputMaxLength;
            }
        }
        public abstract bool UseDecimalPoint { get; }
        protected abstract int CalculateMaxLength(int minValue, int maxValue, ref bool hasMinus);

        protected abstract bool ValidateNumberStr(string prevText, char newCh, string appendedTmp);

        public override sealed char Validate(ref string text, ref int pos, char ch)
        {
            //if (!Regex.IsMatch(ch.ToString(), @"^[0-9]+$", RegexOptions.IgnoreCase)
            //    || text.LengthSafe() >= maxLength)
            //    return (char)0;
            if (text.LengthSafe() >= inputMaxLength)
            {
                return (char)0;
            }

            string appendedTmp = string.IsNullOrEmpty(text) ? ch.ToString() : text + ch;

            bool isMinusOnly = hasMinus && appendedTmp.Equals("-");

            if (isMinusOnly == false)
            {
                if (!ValidateNumberStr(text, ch, appendedTmp))
                {
                    return (char)0;
                }
            }

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WebGLPlayer:
                    text = appendedTmp;
                    pos += 1;
                    break;

                default:
                    return ch;
            }
            return (char)0;
        }
    }
}
