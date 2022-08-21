using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
namespace QTool.InputSystem
{

    public class QJoystick : MonoBehaviour,IDragHandler,IEndDragHandler
    {
        public RectTransform back;
        public RectTransform JoystickButton;
        public float radius=100;
        public string joyStickName="虚拟摇杆";

        Vector2 startPos;
        Pointer joyStick;
        [ContextMenu("测试")]
        private void OnEnable()
        {
            if (joyStick == null)
            {
                joyStick = UnityEngine.InputSystem.InputSystem.AddDevice<Pointer>(joyStickName);
                UnityEngine.InputSystem.InputSystem.SetDeviceUsage(joyStick, joyStickName);
            }
            else
            {
                UnityEngine.InputSystem.InputSystem.AddDevice(joyStick);
            }
        }
        private void OnDisable()
        {
            UnityEngine.InputSystem.InputSystem.RemoveDevice(joyStick);
        }
        private void Awake()
        {
            startPos = JoystickButton.position;
        }
        private void Update()
        {
            InputState.Change(joyStick.position, JoystickButton.transform.position);
            InputState.Change(joyStick.delta, (JoystickButton.transform.position-back.transform.position)/radius-Vector3.one);
        }
        public void OnDrag(PointerEventData eventData)
        {
            var offset = eventData.position - startPos;
            offset = Vector2.ClampMagnitude(offset, radius);
            JoystickButton.position = startPos+offset;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            JoystickButton.position = startPos;
        }
    }
}