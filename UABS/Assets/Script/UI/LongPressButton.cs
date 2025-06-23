using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UABS.Assets.Script.UI
{
    public class LongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public float triggeringDelay = 1.0f;
        public float repeatRate = 0.1f;
        public UnityEvent onLongPress;
        public UnityEvent onClick;

        private bool isPointerDown = false;
        private float pointerDownTimer = 0f;

        private float holdTime = 0f;
        private float nextInvokeTime = 0f;
        private bool repeating = false;

        public void OnPointerDown(PointerEventData eventData)
        {
            isPointerDown = true;
            pointerDownTimer = 0f;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isPointerDown && pointerDownTimer > triggeringDelay)
            {
                onClick?.Invoke(); // If released before duration, treat as normal click
            }
            Reset();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Reset(); // Cancel if pointer leaves button
        }

        private void Update()
        {
            if (!isPointerDown)
            return;

            holdTime += Time.deltaTime;
            pointerDownTimer += Time.deltaTime;

            if (holdTime >= nextInvokeTime)
            {
                // onLongPress?.Invoke();

                if (!repeating)
                {
                    // First time triggered
                    repeating = true;
                    nextInvokeTime = holdTime + repeatRate;
                }
                else
                {
                    // Continue repeating
                    nextInvokeTime += repeatRate;
                }
            }
        }

        private void Reset()
        {
            isPointerDown = false;
            pointerDownTimer = 0f;
        }
    }
}