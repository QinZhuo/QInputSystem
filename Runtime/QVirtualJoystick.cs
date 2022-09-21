using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
namespace QTool.InputSystem
{
    /// <summary>
    /// 虚拟摇杆
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class QVirtualJoystick : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler
    {
        public RectTransform back;
        public RectTransform stick;
        public QVirtualGamepadStick gamepadStick = QVirtualGamepadStick.LeftStick;
        public enum QVirtualGamepadStick
        {
            None,
            LeftStick,
            RightStick,
        }
        public StickControl Stick
        {
            get
            {
                switch (gamepadStick)
                {
                    case QVirtualGamepadStick.LeftStick:
                        return QVirtualGamepad.Instance.leftStick;
                    case QVirtualGamepadStick.RightStick:
                        return QVirtualGamepad.Instance.leftStick;
                    default:
                        return null;
                }
            }
        }
        Vector2 startPos = Vector2.zero;
        float raudis=0;
        private void OnEnable()
        {
            startPos = stick.position;
            raudis=(back.sizeDelta.x / 2);
        }
        private void Update()
        {
            InputState.Change(Stick, (stick.transform.position-transform.position)/ raudis - Vector3.one);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }
        public void OnDrag(PointerEventData eventData)
        {
            var offset = eventData.position - startPos;
            offset = Vector2.ClampMagnitude(offset, raudis);
            stick.position = startPos + offset;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            stick.position = startPos;
        }
    }
}