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
    public class QVirtualButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
    {
        public GamepadButton gamepadButton = GamepadButton.Start;
        public enum QVirtualGamepadStick
        {
            None,
            LeftStick,
            RightStick,
        }
        public ButtonControl Control
        {
            get
            {
                switch (gamepadButton)
                {
                    case GamepadButton.DpadUp:
                        return QVirtualGamepad.Instance.dpad.up;
                    case GamepadButton.DpadDown:
                        return QVirtualGamepad.Instance.dpad.down;
                    case GamepadButton.DpadLeft:
                        return QVirtualGamepad.Instance.dpad.left;
                    case GamepadButton.DpadRight:
                        return QVirtualGamepad.Instance.dpad.right;
                    case GamepadButton.North:
                        return QVirtualGamepad.Instance.buttonNorth;
                    case GamepadButton.East:
                        return QVirtualGamepad.Instance.buttonEast;
                    case GamepadButton.South:
                        return QVirtualGamepad.Instance.buttonSouth;
                    case GamepadButton.West:
                        return QVirtualGamepad.Instance.buttonWest;
                    case GamepadButton.LeftStick:
                        return QVirtualGamepad.Instance.leftStickButton;
                    case GamepadButton.RightStick:
                        return QVirtualGamepad.Instance.rightStickButton;
                    case GamepadButton.LeftShoulder:
                        return QVirtualGamepad.Instance.leftShoulder;
                    case GamepadButton.RightShoulder:
                        return QVirtualGamepad.Instance.rightShoulder;
                    case GamepadButton.Start:
                        return QVirtualGamepad.Instance.startButton;
                    case GamepadButton.Select:
                        return QVirtualGamepad.Instance.selectButton;
                    case GamepadButton.LeftTrigger:
                        return QVirtualGamepad.Instance.leftTrigger;
                    case GamepadButton.RightTrigger:
                        return QVirtualGamepad.Instance.rightTrigger;
                    default:
                        return null;
                }
            }
        }
        public void Change(bool value)
        {
            QVirtualGamepad.Instance.MakeCurrent();
            QVirtualGamepad.Instance.CopyState<GamepadState>(out var state);
            state.WithButton(gamepadButton, value);
            InputState.Change(QVirtualGamepad.Instance, state);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            Change(true);
        }
      
        public void OnPointerUp(PointerEventData eventData)
        {
            Change(false);
        }
    }
}