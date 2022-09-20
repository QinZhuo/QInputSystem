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
        static Mouse _virtualMouse;
        public static Mouse VirtualMouse
        {
            get
            {
                if (_virtualMouse == null)
                {
                    _virtualMouse = UnityEngine.InputSystem.InputSystem.GetDevice(nameof(VirtualMouse)) as Mouse;
                    if (_virtualMouse == null)
                    {
                        _virtualMouse = UnityEngine.InputSystem.InputSystem.AddDevice<Mouse>(nameof(VirtualMouse));

                    }
                }
                return _virtualMouse;
            }
        }
        static QInputSystem()
        {
            _ = VirtualMouse;
            if (UnityEngine.InputSystem.InputSystem.devices.Count > 0)
            {
                foreach (var device in UnityEngine.InputSystem.InputSystem.devices)
                {
                    SetDevice(device);
                }
            }
        }
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

        public static Vector2 PointerPosition
        {
            get
            {
                return Pointer.current.position.ReadValue();
            }
            set
            {
                var delta = value - PointerPosition;
                InputState.Change(Pointer.current.position, value);
                InputState.Change(Pointer.current.delta, delta);
                if (Pointer.current is Mouse mouse)
                {
                    mouse.WarpCursorPosition(value);
                }
            }
        }
        public static bool IsGamepad => DeviceType == QDeviceType.XInputController || DeviceType == QDeviceType.DualShockGamepad;
        public static QDeviceType DeviceType { get; private set; } = QDeviceType.None;
        public static event Action OnDeviceTypeChange;
        [RuntimeInitializeOnLoadMethod]
        private static void DeviceTypeCheck()
        {
            UnityEngine.InputSystem.InputSystem.onActionChange += (obj, type) =>
            {
                if (obj is  InputAction action &&action.activeControl!=null)
                {
                    SetDevice(action.activeControl.device);
                }
            };
        }
        private static void SetDevice(InputDevice device)
        {
            var newDeviceType = ParseDeviceType(device.displayName, device);
            if (newDeviceType != DeviceType)
            {
                DeviceType = newDeviceType;
                OnDeviceTypeChange?.Invoke();
                QEventManager.Trigger("输入设备类型", DeviceType.ToString());
            }
        }

        public static QDeviceType ParseDeviceType(string key,InputDevice device=null)
        {
            var typeStr = key.TrimStart('/').TrimStart('<').SplitStartString("/").TrimEnd('>');
            if (typeStr.Length > 0 && char.IsNumber(typeStr[typeStr.Length - 1]))
            {
                typeStr = typeStr.Substring(0, typeStr.Length - 1);
            }
            var type = QDeviceType.MouseKeyboard;
            switch (typeStr)
            {
                case "XInputController":
                case "XInputControllerWindows":
                    type = QDeviceType.XInputController;
                    break;
                case "Wireless Controller":
                case "DualShockGamepad":
                case "DualSenseGamepadHID":
                    type = QDeviceType.DualShockGamepad;
                    break;
                case "Mouse":
                case "Keyboard":
                    type = QDeviceType.MouseKeyboard;
                    break;
                case "Touchscreen":
                    type = QDeviceType.Touchscreen;
                    break; 
                case "VirtualMouse":
                    break;
                default:
                    if (device != null)
                    {
                        if (device is Gamepad)
                        {
                            type = QDeviceType.XInputController;
                        }
                        else if (device is Mouse || device is Keyboard)
                        {
                            type = QDeviceType.MouseKeyboard;
                        }
                        else if (device is Touchscreen)
                        {
                            type = QDeviceType.Touchscreen;
                        }
                        else
                        {
                            Debug.LogError("不支持设备检测[" + device + "]");
                        }
                    }
                    else
                    {
                        Debug.LogError("不支持设备检测[" + typeStr + "]");
                    }
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
            None,
            MouseKeyboard,
            XInputController,
            DualShockGamepad,
            Touchscreen,
        }
    }
}