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
        //��ȡ�ƶ�ҡ�����뷽��
        //var moveDir = QInput.Actions["�ƶ�"].Vector2 * 100 * Time.deltaTime;
        //transform.localPosition +=new Vector3(moveDir.x, moveDir.y,0);


        ////�����ƶ���Χ
        //transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, 300);
    }
}
