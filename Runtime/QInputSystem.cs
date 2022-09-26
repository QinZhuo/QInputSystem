using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace QTool.InputSystem
{
    public static class QInputSystem
    {
        public static bool IsGamepad => ControlScheme == QControlScheme.Gamepad;
        public static QControlScheme ControlScheme { get; private set; } = QControlScheme.None;
        public static event Action OnControlSchemeChange;
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
                    if (obj is InputAction action&&action.actionMap.asset.name!=nameof(DefaultInputActions))
                    {
                        Player.actions = action.actionMap.asset;
                        OnControlSchemeChange?.Invoke();
                    }
                }
                if (change == InputActionChange.BoundControlsChanged )
                {
                    if(obj is InputActionAsset asset)
                    {
                        if (Enum.TryParse<QControlScheme>(Player.currentControlScheme.RemveChars('&'), out var newScheme))
                        {
                            if (newScheme != ControlScheme)
                            {
                                ControlScheme = newScheme;
                                OnControlSchemeChange?.Invoke();
                            }
                        }
                        else
                        {
                            Debug.LogError("不支持环境 " + Player.currentControlScheme);
                        }
                    }
                }
            };
        }
        
        public enum QControlScheme
        {
            None,
            KeyboardMouse,
            Gamepad,
            Touchscreen,
        }

    }
}