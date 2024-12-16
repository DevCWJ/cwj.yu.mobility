using CWJ.SceneHelper;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CWJ.YU.Mobility
{
    public class InteractableObj : MonoBehaviour, CWJ.SceneHelper.INeedSceneObj, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
    {
        [GetComponent] public Button button;

        private void Awake()
        {
            if(!button) button = GetComponent<Button>();
        }

        public SceneObjContainer sceneObjs { get; set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.LogError($"OnPointerClick - {gameObject.name} ",gameObject);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.LogError($"OnPointerDown - {gameObject.name} ",gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.LogError($"OnPointerEnter - {gameObject.name} ",gameObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.LogError($"OnPointerExit - {gameObject.name} ",gameObject);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.LogError($"OnPointerUp - {gameObject.name} ",gameObject);
        }
    }
}
