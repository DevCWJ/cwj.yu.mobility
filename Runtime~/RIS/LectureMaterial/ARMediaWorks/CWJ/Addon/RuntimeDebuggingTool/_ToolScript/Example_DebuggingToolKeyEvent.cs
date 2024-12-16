
using UnityEngine;

#if CWJ_EXISTS_NEWINPUTSYSTEM
using UnityEngine.InputSystem;
#endif

using CWJ;

public class Example_DebuggingToolKeyEvent : MonoBehaviour
{
#if !CWJ_RUNTIMEDEBUGGING_DISABLED
#if !CWJ_EXISTS_NEWINPUTSYSTEM
    public KeyCode supportKey = KeyCode.LeftShift;
    public KeyCode pauseKey = KeyCode.P;
    public KeyCode allShowKey = KeyCode.Tab;
    public KeyCode fpsShowKey = KeyCode.F1;
    public KeyCode fpsResetKey = KeyCode.F5;
#else
    public Key supportKey = Key.LeftShift;
    public Key pauseKey = Key.P;
    public Key allShowKey = Key.Tab;
    public Key fpsShowKey = Key.F1;
    public Key fpsResetKey = Key.F5;
#endif

    private void Start()
    {
        RuntimeDebuggingTool.Instance.allVisibleMultipleEvent += OnAllShowKeyEvent;

        RuntimeDebuggingTool.Instance.fpsVisibleSingleEvent += OnFpsShowKeyEvent;

        RuntimeDebuggingTool.Instance.fpsResetSingleEvent += OnResetFpsKeyEvent;

        RuntimeDebuggingTool.Instance.timePauseSingleEvent += OnTimePauseKeyEvent;
    }

    private bool OnTimePauseKeyEvent()
    {
#if !CWJ_EXISTS_NEWINPUTSYSTEM
        return Input.GetKey(supportKey) && Input.GetKeyDown(pauseKey);
#else
        return Keyboard.current != null && Keyboard.current[supportKey].isPressed && Keyboard.current[pauseKey].wasPressedThisFrame;

#endif
    }

    private bool OnAllShowKeyEvent()
    {
#if !CWJ_EXISTS_NEWINPUTSYSTEM
        return Input.GetKey(supportKey) && Input.GetKeyDown(allShowKey);
#else
        return Keyboard.current != null && Keyboard.current[supportKey].isPressed && Keyboard.current[allShowKey].wasPressedThisFrame;
#endif
    }

    private bool OnFpsShowKeyEvent()
    {
#if !CWJ_EXISTS_NEWINPUTSYSTEM
        return Input.GetKey(supportKey) && Input.GetKeyDown(fpsShowKey);
#else
        return Keyboard.current != null && Keyboard.current[supportKey].isPressed && Keyboard.current[fpsShowKey].wasPressedThisFrame;
#endif
    }

    private bool OnResetFpsKeyEvent()
    {
#if !CWJ_EXISTS_NEWINPUTSYSTEM
        return Input.GetKey(supportKey) && Input.GetKeyDown(fpsResetKey);
#else
        return Keyboard.current != null && Keyboard.current[supportKey].isPressed && Keyboard.current[fpsResetKey].wasPressedThisFrame;
#endif
    }
#endif
}