using UnityEngine;

namespace CWJ
{

    [CreateAssetMenu(fileName = "CWJ_FloatValidator", menuName = "CWJ/TMP/FloatValidator", order = int.MaxValue)]
    [System.Serializable]
    public class Ipf_FloatValidator : _Ipf_MinMaxValidator
    {
        public override bool UseDecimalPoint => true;
        public int afterDecimalLength = 1;
        protected override int CalculateMaxLength(int minValue, int maxValue, ref bool hasMinus)
        {
            hasMinus = minValue < 0;
            if (afterDecimalLength <= 0)
                afterDecimalLength = 1;
            string minStr = StringUtil.ConvertToDecimalLength(minValue, -1, afterDecimalLength);
            string maxStr = StringUtil.ConvertToDecimalLength(maxValue, -1, afterDecimalLength);
            string maxLengthStr = (minStr.Length > maxStr.Length) ? minStr : maxStr;
            return maxLengthStr.Length;
        }

        protected override bool ValidateNumberStr(string prevText, char newCh, string appendedTmp)
        {
            bool isDotWithBeforeDecimalIsInt = newCh.Equals('.') && int.TryParse(prevText, out int frontInt);
            return isDotWithBeforeDecimalIsInt || (float.TryParse(appendedTmp, out float val) && (minValue <= val && val <= maxValue));
        }


        //e : [+-]?(\d+([.]\d*)?([eE][+-]?\d+)?|[.]\d+([eE][+-]?\d+)?)
        //non e: [+-]?(\d+([.]\d*)?(e[+-]?\d+)?|[.]\d+(e[+-]?\d+)?)
    }

}