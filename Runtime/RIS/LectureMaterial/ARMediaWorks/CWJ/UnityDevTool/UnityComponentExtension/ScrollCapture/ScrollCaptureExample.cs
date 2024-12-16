#if UNITY_EDITOR

using UnityEngine;

using CWJ.UI;

public class ScrollCaptureExample : MonoBehaviour
{
    [CWJ.InvokeButton]
    public void Capture()
    {
        ScrollCaptureManager.Instance.Capture(ScrollCaptureManager.Instance.editor_ScrollRect);
    }
}

#endif