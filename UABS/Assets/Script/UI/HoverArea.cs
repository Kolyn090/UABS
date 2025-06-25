using UnityEngine;
using UnityEngine.EventSystems;

namespace UABS.Assets.Script.UI
{
    public class HoverArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public HoverDropdown parentDropdown;

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            parentDropdown.SendMessage("OnPointerEnter", eventData);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            parentDropdown.SendMessage("OnPointerExit", eventData);
        }
    }
}