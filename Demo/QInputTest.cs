using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTool.InputSystem;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;

namespace QTool
{

    public class QInputTest : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
    {
        public InputActionReference input;

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.LogError(nameof(OnPointerClick));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.LogError(nameof(OnPointerDown));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.LogError(nameof(OnPointerEnter));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.LogError(nameof(OnPointerExit));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.LogError(nameof(OnPointerUp));
        }

        // Start is called before the first frame update
        void Start()
        {
            Debug.LogError("InputTest");
            input.action.Enable();
            input.action.started += (info) =>
            {
                Debug.LogError("started " + info.ReadValue<Vector2>());
            };
            input.action.performed += (info) =>
            {
                Debug.LogError("performed "+info.ReadValue<Vector2>());
            };
            input.action.canceled += (info) =>
            {
                Debug.LogError("canceled " + info.ReadValue<Vector2>());
            };
        }

        // Update is called once per frame
        void Update()
        {
            if (Gamepad.current.xButton.wasPressedThisFrame)
            {
                Debug.LogError(Gamepad.current.xButton);
            }
            if (Gamepad.current.aButton.wasPressedThisFrame)
            {
                Debug.LogError(Gamepad.current.aButton);
            }
            if (Gamepad.current.bButton.wasPressedThisFrame)
            {
                Debug.LogError(Gamepad.current.bButton);
            }
            if (Gamepad.current.yButton.wasPressedThisFrame)
            {
                Debug.LogError(Gamepad.current.yButton);
            }
            if (Gamepad.current.leftTrigger.wasPressedThisFrame)
            {
                Debug.LogError(Gamepad.current.leftTrigger);
            }
            if (Gamepad.current.rightTrigger.wasPressedThisFrame)
            {
                Debug.LogError(Gamepad.current.rightTrigger);
            }

            //Debug.LogError(input.action.ReadValue<Vector2>());
            //获取移动摇杆输入方向
            //var moveDir = QInput.Actions["移动"].Vector2 * 100 * Time.deltaTime;
            //transform.localPosition +=new Vector3(moveDir.x, moveDir.y,0);


            ////限制移动范围
            //transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, 300);
        }
    }
}
