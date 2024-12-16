#if UNITY_EDITOR

using System.Collections.Generic;

namespace CWJ.AccessibleEditor
{
    public class CachedSymbol_ScriptableObject : Initializable_ScriptableObject
    {
        public List<string> cachedSymbolNames = new List<string>();

        public void AddSymbolCache(IList<string> symbolNames)
        {
            cachedSymbolNames.AddRangeNotOverlap(symbolNames);
        }

        public void RemoveSymbolCache(IList<string> symbolNames)
        {
            cachedSymbolNames.RemoveRange(symbolNames);
        }

        public bool CheckIsDefineSymbolDiff()
        {
            if (!isInitialized) return true;

            string[] curSymbols = DefineSymbolUtil.GetCurrentSymbolsToArray();
            int length = cachedSymbolNames.Count;
            for (int i = 0; i < length; ++i)
            {
                if (!curSymbols.IsExists(cachedSymbolNames[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public override void OnReset(bool isNeedSave = false)
        {
            cachedSymbolNames = new List<string>();
            base.OnReset(isNeedSave);
        }
    }
}

#endif