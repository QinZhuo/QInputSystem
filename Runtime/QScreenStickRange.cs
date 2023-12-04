#if InputSystem
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;
namespace QTool.InputSystem
{
    /// <summary>
    /// 虚拟摇杆
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class QScreenStickRange : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler
    {
        private void Reset()
        {
            if (stick == null){
                stick = GetComponentInChildren<OnScreenStick>();
                if (stick == null){
                    var staickObj = transform.GetChild("StickBack.Stick");
                    stick = staickObj.gameObject.AddComponent<OnScreenStick>();
                }
            }
        }
        public RectTransform rectTransform => transform as RectTransform;
        public RectTransform Back => stick.transform.parent as RectTransform;
        public OnScreenStick stick;
        public BoolEvent OnActive;
        Vector2 startPos = Vector2.zero;
        float raudis=0;
        private void OnEnable()
        {
            Back.pivot = Vector2.one * 0.5f;
            startPos = Back.position;
            stick.GetComponent<Image>().raycastTarget = false; 
            OnActive.Invoke(false);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            Back.transform.position= eventData.position;
            stick.OnPointerDown(eventData);
            OnActive.Invoke(true);
        }
        public void OnDrag(PointerEventData eventData)
        {
            var offset = eventData.position - startPos;
            offset = Vector2.ClampMagnitude(offset, raudis);
            stick.OnDrag(eventData);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
           Back.transform.position = startPos;
           stick.OnPointerUp(eventData);
           OnActive.Invoke(false);
        }
    }
}
#endif