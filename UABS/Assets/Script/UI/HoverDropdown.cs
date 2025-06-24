using UnityEngine;
using UnityEngine.EventSystems;


namespace UABS.Assets.Script.UI
{
    public class HoverDropdown : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        public RectTransform dropdownPanel;
        private bool isPointerInside = false;

        private void Start()
        {
            dropdownPanel.gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isPointerInside = true;
            dropdownPanel.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerInside = false;
            StartCoroutine(HideAfterDelay());
        }

        private System.Collections.IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(0.2f);
            if (!isPointerInside)
                dropdownPanel.gameObject.SetActive(false);
        }
    }

}