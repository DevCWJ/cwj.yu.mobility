using UnityEngine;
using UnityEngine.UI;

namespace CWJ
{
    [DisallowMultipleComponent]
    public class EzExplorerItem : MonoBehaviour
    {
        [SerializeField] Text nameTxt;
        [SerializeField] Image iconImg;
        [SerializeField] Button clickBtn;

        public void Initialized(string name, Sprite icon, UnityEngine.Events.UnityAction clickAction, UnityEngine.Events.UnityAction doubleClickAction = null)
        {
            nameTxt.text = name;
            iconImg.sprite = icon;
            clickBtn.onClick.AddListener(clickAction);
            if (doubleClickAction != null)
                clickBtn.AddDoubleClickEvent(doubleClickAction);
        }
    }
}