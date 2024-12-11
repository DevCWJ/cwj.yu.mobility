using System.Collections;

using NewResolutionDialog.Scripts.Controller;

using UnityEngine;
#if CWJ_EXISTS_NEWINPUTSYSTEM
using UnityEngine.InputSystem;
#endif
#pragma warning disable 0649

namespace NewResolutionDialog.Scripts
{
    public class DefaultInputsHandler : MonoBehaviour
    {
        //NewResolutionDialog는 root에 있어야하지만 특정 오브젝트가 활성화되어있어야만 사용할수있게끔 condition 추가
        [SerializeField] private Transform parentTrf;
        [SerializeField]
        private Settings settings;

        [SerializeField]
        private Canvas dialogCanvas;

#if !CWJ_EXISTS_NEWINPUTSYSTEM
        [SerializeField]
        private KeyCode popupKeyCode = KeyCode.Escape;
#else
        [SerializeField] private Key popupKeyCode = Key.Escape;
#endif
        private void Awake()
        {
            if (settings == null) Debug.LogError($"Serialized Field {nameof(settings)} is missing!");
        }

        private void Start()
        {
            if (settings.dialogStyle == ResolutionDialogStyle.PopupDialog)
                StartCoroutine(WaitForActivation());
        }

        private IEnumerator WaitForActivation()
        {
            WaitUntil waitToggleCanvasEvent =
#if !CWJ_EXISTS_NEWINPUTSYSTEM
            new WaitUntil(() => (parentTrf == null || parentTrf.gameObject.activeInHierarchy) && Input.GetKeyUp(popupKeyCode));
#else
            new WaitUntil(() => (parentTrf == null || parentTrf.gameObject.activeInHierarchy) && Keyboard.current != null && Keyboard.current[popupKeyCode].wasPressedThisFrame);
#endif
            while (true)
            {   
                yield return waitToggleCanvasEvent;

                ToggleCanvas();

                // wait twice (into next frame) to prevent the hotkey from being recognized again in the same frame
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
            }
        }

        public void ToggleCanvas()
        {
            dialogCanvas.enabled = !dialogCanvas.enabled;
        }

        public void InstantiateCanvas()
        {
            dialogCanvas.enabled = false;
            Canvas newObj= GameObject.Instantiate(dialogCanvas);

            var closeBtn = newObj.GetComponentInChildren<UnityEngine.UI.Button>();
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.AddListener(() => Destroy(newObj.gameObject));
            newObj.enabled = true;
        }
    }
}