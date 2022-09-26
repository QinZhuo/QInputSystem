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
        public static bool IsGamepad => ControlScheme == QControlScheme.Gamepad;
        public static QControlScheme ControlScheme { get; private set; } = QControlScheme.KeyboardMouse;
        public static event Action OnControlSchemeChange;
        static bool ControlSchemeFresh = true;
        static PlayerInput _playerInput=null;
        public static PlayerInput Player
        {
            get
            {
                if (_playerInput == null)
                {
                    if (PlayerInput.all.Count == 0)
                    {
                        _playerInput = QToolManager.Instance.gameObject.AddComponent<PlayerInput>();
                    }
                    else
                    {
                        _playerInput = PlayerInput.all[0];
                    }
                }
                return _playerInput;
            }
        }
        [RuntimeInitializeOnLoadMethod]
        private static void DeviceTypeCheck()
        {
            UnityEngine.InputSystem.InputSystem.onActionChange += (obj, change) =>
            {
                if (PlayerInput.all.Count == 0)
                {
                    if (obj is InputAction action)
                    {
                        Player.actions = action.actionMap.asset;
                    }
                }
             
                if (change == InputActionChange.BoundControlsChanged || ControlSchemeFresh)
                {
                    if (obj is InputAction action)
                    {
                        if (action.activeControl != null&& action.actionMap.asset!=null)
                        {
                            var scheme =  FindControlSchemeForDevices(new ReadOnlyArray<InputDevice>(
                                new InputDevice[] { action.activeControl.device }), action.actionMap.asset.controlSchemes, null, true).Value.name;
                            if(Enum.TryParse<QControlScheme>(scheme.RemveChars('&'),out var newScheme))
                            {
                                if(newScheme != ControlScheme && !action.activeControl.device.description.empty)
                                {
                                    ControlScheme = newScheme;
                                    OnControlSchemeChange?.Invoke();
                                }
                            }
                            else
                            {
                                Debug.LogError("不支持环境 "+scheme);
                            }
                            ControlSchemeFresh = false;
                        }
                    }
                    else if(obj is InputActionAsset asset)
                    {
                        ControlSchemeFresh = true;
                    }
                }
            };
        }
        
        public enum QControlScheme
        {
            KeyboardMouse,
            Gamepad,
            Touchscreen,
        }

    }
}