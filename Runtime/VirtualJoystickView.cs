using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QTool.InputSystem {
    public class VirtualJoystickView : MonoBehaviour
    {
        public string actionKey="动作名";
        public string inputKey="虚拟摇杆";
       
        //public InputAction Action
        //{
        //    get
        //    {
        //        return QInput.Actions[actionKey];
        //    }
        //}
        //public VirtualJoystick Joystick
        //{
        //    get
        //    {
        //        var input = Action.InputList.Get(inputKey);
        //        if (input == null)
        //        {
        //            input = new VirtualJoystick { DownDsitance=back.rect.width/2,maxDistance=back.rect.width/2 };
        //            input.Key=inputKey;
        //            Action.Add(input);
        //        }
        //        return input as VirtualJoystick;
        //    }
        //}
        //public Vector2 Vector2
        //{
        //    get
        //    {
        //        if (Joystick != null)
        //        {
        //            return Action.Vector2*Joystick.maxDistance;
        //        }
        //        else
        //        {
        //            return Vector2.zero;
        //        }
        //    }
        //}
        //private void Awake()
        //{
        //    ChangeDefaultCenter();
        //}
        //public RectTransform back;
        //public RectTransform JoystickButton;
        //private void LateUpdate()
        //{
        //    if (back == null || JoystickButton == null) return;
        //    back.transform.position = Joystick.Center;
        //    JoystickButton.localPosition = Vector2;
        //}
        //public void ChangeDefaultCenter()
        //{
        //    Joystick.defaultCenterPos = new Vector2(back.transform.position.x / Screen.width, back.transform.position.y / Screen.height);
        //}
        //private void OnDrawGizmosSelected()
        //{
        //    if (!Application.isPlaying) return;
        //    if (back == null || JoystickButton == null) return;
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(QInput.MousePosition,10);
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawWireSphere(back.position, Joystick.maxDistance);
        //    Gizmos.color = Color.white;
        //    Gizmos.DrawWireSphere(back.position, Joystick.DownDsitance);
        //}
    }
}