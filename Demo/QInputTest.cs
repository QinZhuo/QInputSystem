using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTool.QInputSystem;

public class QInputTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //获取移动摇杆输入方向
        //var moveDir = QInput.Actions["移动"].Vector2 * 100 * Time.deltaTime;
        //transform.localPosition +=new Vector3(moveDir.x, moveDir.y,0);


        ////限制移动范围
        //transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, 300);
    }
}
