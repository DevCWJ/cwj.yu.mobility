using CWJ.Singleton;

using UnityEngine;

#if CWJ_EXISTS_NEWINPUTSYSTEM
using UnityEngine.InputSystem;
#endif

namespace CWJ.AccessibleEditor.DebugSetting
{
    public class UnityDevConsoleVisible : SingletonBehaviourDontDestroy<UnityDevConsoleVisible>, IDontPrecreatedInScene
    {
#if CWJ_LOG_SAVE
        void Update()
        {
#if !CWJ_EXISTS_NEWINPUTSYSTEM
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                if (Input.GetKeyDown(KeyCode.F10))
                {
                    SwitchingConsoleVisible();
                }
            }
            //else
            //{
            //    if (Input.touchCount > 2)
            //    {
            //        SwitchingConsoleVisible();
            //    }
            //}
#else

#endif
        }

        void SwitchingConsoleVisible()
        {
            UnityEngine.Debug.developerConsoleVisible = !UnityEngine.Debug.developerConsoleVisible;
            //Debug.LogError("DebugConsole: " + (UnityEngine.Debug.developerConsoleVisible ? "Show" : "Hide"));
        }
#endif
    }
}