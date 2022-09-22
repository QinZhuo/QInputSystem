using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTool.InputSystem;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace QTool
{

    public class QInputTest : MonoBehaviour
    {
        public InputActionReference input;
        // Start is called before the first frame update
        void Start()
        {
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
            if (Gamepad.current.xButton.isPressed)
            {
                Debug.LogError(Gamepad.current.xButton);
            }
            if (Gamepad.current.aButton.isPressed)
            {
                Debug.LogError(Gamepad.current.aButton);
            }
            if (Gamepad.current.bButton.isPressed)
            {
                Debug.LogError(Gamepad.current.bButton);
            }
            if (Gamepad.current.yButton.isPressed)
            {
                Debug.LogError(Gamepad.current.yButton);
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
