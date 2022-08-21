using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace QTool.InputSystem
{
    public static class QInputSystem
    {
        static InputActionAsset _inputSetting;
        public static InputActionAsset InputSetting
        {
            get
            {
                if (_inputSetting == null)
                {
                    _inputSetting = Resources.Load<InputActionAsset>(nameof(InputSetting));
#if UNITY_EDITOR
                    if (_inputSetting == null)
                    {
                        Debug.LogError(nameof(Resources) + "下不存在 " + nameof(InputSetting) + " 输入设置");
                    }
#endif
                }
                return _inputSetting;
            }
        }
        public static Vector2 MousePosition
        {
            get
            {
                return Mouse.current.position.ReadValue();
            }
            set
            {
                Debug.LogError("不可更改鼠标位置");
                return;
                var delta = value - MousePosition;
                InputState.Change(Mouse.current.position, value);
                InputState.Change(Mouse.current.delta, delta);
                Mouse.current.WarpCursorPosition(value);
            }
        }
    }
}