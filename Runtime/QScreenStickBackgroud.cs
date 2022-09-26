using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.OnScreen;

namespace QTool.InputSystem
{
    /// <summary>
    /// 虚拟摇杆
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class QScreenStickBackgroud : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler
    {
        public RectTransform rectTransform => transform as RectTransform;
        public OnScreenStick stick;
        Vector2 startPos = Vector2.zero;
        float raudis=0;
        private void OnEnable()
        {
            rectTransform.pivot = Vector2.one * 0.5f;
            startPos = transform.position;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            rectTransform.anchoredPosition = eventData.position;
            stick.OnPointerDown(eventData);
        }
        public void OnDrag(PointerEventData eventData)
        {
            var offset = eventData.position - startPos;
            offset = Vector2.ClampMagnitude(offset, raudis);
            stick.OnDrag(eventData);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            transform.position = startPos;
            stick.OnPointerUp(eventData);
        }
    }
}