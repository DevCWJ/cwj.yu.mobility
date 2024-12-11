
using UnityEngine;
namespace CWJ
{
    [CreateAssetMenu(fileName = "CWJ_IntValidator", menuName = "CWJ/TMP/IntValidator", order = int.MaxValue)]
    [System.Serializable]
    public class Ipf_IntValidator : _Ipf_MinMaxValidator
    {
        public override bool UseDecimalPoint => false;
        protected override int CalculateMaxLength(int minValue, int maxValue, ref bool hasMinus)
        {
            hasMinus = minValue < 0;
            return Mathf.Max(minValue.ToString().Length, maxValue.ToString().Length);
        }

        protected override bool ValidateNumberStr(string prevText, char newCh, string appendedTmp)
        {
            return int.TryParse(appendedTmp, out int val) && (minValue <= val && val <= maxValue);
        }
    }

}