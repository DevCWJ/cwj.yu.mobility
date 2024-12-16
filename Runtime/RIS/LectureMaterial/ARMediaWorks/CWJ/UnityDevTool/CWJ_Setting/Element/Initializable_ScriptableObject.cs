#if UNITY_EDITOR

namespace CWJ
{
    public class Initializable_ScriptableObject : CWJScriptableObject
    {
        [Readonly] public bool isInitialized = false;

        public virtual bool IsAutoReset => true;
        
        public override sealed void OnConstruct()
        {
            base.OnConstruct();
            OnReset();
        }

        public virtual void OnReset(bool isNeedSave = false)
        {
            isInitialized = false;
            if (isNeedSave)
            {
                SaveScriptableObj();
            }
        }
    }
}

#endif