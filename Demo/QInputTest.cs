using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTool.InputSystem;
using UnityEngine.InputSystem;
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
            //Debug.LogError(input.action.ReadValue<Vector2>());
            //��ȡ�ƶ�ҡ�����뷽��
            //var moveDir = QInput.Actions["�ƶ�"].Vector2 * 100 * Time.deltaTime;
            //transform.localPosition +=new Vector3(moveDir.x, moveDir.y,0);


            ////�����ƶ���Χ
            //transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, 300);
        }
    }
}
