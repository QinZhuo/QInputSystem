using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using System;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;
using static UnityEngine.InputSystem.InputControlScheme;

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

        public static bool IsGamepad => ControlScheme.Value.bindingGroup=="";
        public static InputControlScheme? ControlScheme { get; private set; } = null;
        public static event Action OnControlSchemeChange;

        
        [RuntimeInitializeOnLoadMethod]
        private static void DeviceTypeCheck()
        {
            UnityEngine.InputSystem.InputSystem.onActionChange += (obj, change) =>
            {
                if (change == InputActionChange.BoundControlsChanged || ControlScheme == null)
                {
                    if (obj is InputAction action)
                    {
                        if (action.activeControl != null)
                        {
                            ControlScheme = FindControlSchemeForDevice(action.activeControl.device, action.actionMap.asset.controlSchemes);
                            Debug.LogError(ControlScheme.Value.bindingGroup);
                        }
                    }
                    else if(obj is InputActionAsset asset)
                    {
                        ControlScheme = null;
                    }
                }
            };
        }
        public static InputControlScheme? FindControlSchemeForDevice<TSchemes>(InputDevice device, TSchemes schemes)
         where TSchemes : IEnumerable<InputControlScheme>
        {
            if (schemes == null)
                throw new ArgumentNullException(nameof(schemes));
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            return FindControlSchemeForDevices( new ReadOnlyArray<InputDevice>(new InputDevice[] {device}), schemes,null,true);
        }


        private static void SetDevice(InputDevice device)
        {
            //var newControlScheme= InputControlScheme.FindControlSchemeForDevice(device, InputSetting.controlSchemes);
            //if (!newControlScheme.Equals( ControlScheme))
            //{
            //    ControlScheme = newControlScheme;
            //    OnControlSchemeChange?.Invoke();
            //    Debug.LogError(ControlScheme.ToQData());
            //    QEventManager.Trigger("输入设备类型", ControlScheme.ToString());
            //}
        }

        public enum QControlScheme
        {
            KeyboardMouse,
            Gamepad,
            Touchscreen,
        }

    }
}