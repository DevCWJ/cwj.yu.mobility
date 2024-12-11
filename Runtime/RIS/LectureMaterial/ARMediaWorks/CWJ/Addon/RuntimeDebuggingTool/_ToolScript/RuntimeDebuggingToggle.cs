using UnityEngine;
using CWJ;

public class RuntimeDebuggingToggle : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button button;

    [SerializeField] int maxClick = 5;
    private float clickCnt = 0;
    public static bool IsRuntimeDebuggingDisabled =>
#if CWJ_RUNTIMEDEBUGGING_DISABLED
        true;
#else
        false;
#endif

    private void Start()
    {
        if (!IsRuntimeDebuggingDisabled)
        {
            button.onClick.AddListener(OnClickBtn);
            RuntimeDebuggingTool.Instance.allVisibleMultipleEvent += OnAllVisibleKeyEvent;
        }
    }

    private void OnClickBtn()
    {
        ++clickCnt;
    }

    private bool OnAllVisibleKeyEvent()
    {
        float check = clickCnt;
        clickCnt = Mathf.Repeat(clickCnt, maxClick); //0~(maxClick-1)  
        return check == maxClick;
    }
}
