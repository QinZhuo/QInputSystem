using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;
namespace QTool.InputSystem
{
    public static class QInputSystem
    {
        public static bool IsGamepad => ControlScheme == QControlScheme.Gamepad;
        public static QControlScheme ControlScheme { get; private set; } = QControlScheme.None;
        static QControlScheme newScheme;
        public static event Action OnControlSchemeChange;
        static PlayerInput _playerInput=null;
        public static PlayerInput Player
        {
            get
            {
                if (_playerInput == null&&Application.isPlaying)
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
                if (Application.isPlaying)
                {
                    if (Player.actions==null)
                    {
                        if (obj is InputAction action && action.actionMap.asset.name != "DefaultInputActions")
                        {
                            Player.actions = action.actionMap.asset;
                            OnControlSchemeChange?.Invoke();
                        }
                    }
                    if (change == InputActionChange.BoundControlsChanged)
                    {
                        if (obj is InputActionAsset asset)
                        {
                            if (!Player.currentControlScheme.IsNullOrEmpty() && !Enum.TryParse<QControlScheme>(Player.currentControlScheme.RemveChars('&'), out newScheme))
                            {
                                Debug.LogError("不支持环境 " + Player.currentControlScheme);
                            }
                        }
                    }
                    else if (newScheme != ControlScheme && obj is InputAction action && action.activeControl != null)
                    {
                        if (action.activeControl.device.description.empty)
                        {
                            if (action.activeControl.device.name != "QSwitchGamepad")
                            {
                                return;
                            }
                        }
                        ControlScheme = newScheme;
                        OnControlSchemeChange?.Invoke();
                    }
                }
            };
        }
        
        
    }
    [System.Flags]
    public enum QControlScheme
    {
        None=0,
        KeyboardMouse=1<<1,
        Gamepad=1<<2,
        Touchscreen=1<<3,
    }

}