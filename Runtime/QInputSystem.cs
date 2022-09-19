using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using System;
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
                if (Mouse.current == null)
                {
                    return Vector2.zero;
                }
                return Mouse.current.position.ReadValue();
            }
            set
            {
                if (Mouse.current == null)
                {
                    return;
                }
                var delta = value - MousePosition;
                InputState.Change(Mouse.current.position, value);
                InputState.Change(Mouse.current.delta, delta);
                Mouse.current.WarpCursorPosition(value);
            }
        }
        public static QDeviceType DeviceType { get; private set; } = QDeviceType.MouseKeyboard;
        public static InputControl InputControl { get; private set; } = null;
        public static event Action OnDeviceTypeChange;
        [RuntimeInitializeOnLoadMethod]
        private static void DeviceTypeCheck()
        {
            UnityEngine.InputSystem.InputSystem.onActionChange += (obj, type) =>
            {
                if (obj is  InputAction action &&action.activeControl!=null)
                {
                    InputControl = action.activeControl;
                    SetDeviceType(action.activeControl.path);
                }
            };
        }
        private static void SetDeviceType(string key)
        {
            var newDeviceType = ParseDeviceType(key);
            if (newDeviceType != DeviceType)
            {
                DeviceType = newDeviceType;
                OnDeviceTypeChange?.Invoke();
            }
        }
        public static QDeviceType ParseDeviceType(string key)
        {
            var typeStr = key.TrimStart('/').TrimStart('<').SplitStartString("/").TrimEnd('>');
            var type = QDeviceType.MouseKeyboard;
            switch (typeStr)
            {
                case "Gamepad":
                case "XInputController":
                case "XInputControllerWindows":
                    type = QDeviceType.XInputController;
                    break;
                case "Mouse":
                case "Keyboard":
                    type = QDeviceType.MouseKeyboard;
                    break;
                case "Touchscreen":
                    type = QDeviceType.Touchscreen;
                    break;
                default:
                    Debug.LogError("不支持设备检测[" + typeStr + "]");
                    break;
            }
            return type;
        }
        public static int GetDeviceIndex(this InputAction action)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                var bind = action.bindings[i];
                if (QInputSystem.ParseDeviceType(bind.effectivePath) == QInputSystem.DeviceType)
                {
                    return i;
                }
            }
            return 0;
        }
        public enum QDeviceType
        {
            MouseKeyboard,
            XInputController,
            Touchscreen,
        }
    }
}