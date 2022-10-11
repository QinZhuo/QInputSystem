using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.InputSystem.LowLevel;

namespace QTool.InputSystem
{
    public static class QInputSystem
    {
        public static Mouse QVirtualMouse { get; private set; }
        public static bool IsGamepad => ControlScheme == QControlScheme.Gamepad;
        public static QControlScheme ControlScheme { get; private set; } = QControlScheme.None;
        static QControlScheme newScheme;
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
            QVirtualMouse = UnityEngine.InputSystem.InputSystem.GetDevice(nameof(QVirtualMouse)) as Mouse;
            if (QVirtualMouse == null)
            {
                QVirtualMouse = UnityEngine.InputSystem.InputSystem.AddDevice<Mouse>(nameof(QVirtualMouse));

            }
            QToolManager.Instance.OnUpdate += () =>
            {
                if(Pointer.current!= QVirtualMouse)
                {
                    var pos = Pointer.current.position.ReadValue();
                    InputState.Change(QVirtualMouse.position, pos);
                    InputState.Change(Pointer.current.delta, Pointer.current.delta.ReadValue());
                    QVirtualMouse.WarpCursorPosition(pos);
                }
                
            };
            UnityEngine.InputSystem.InputSystem.onActionChange += (obj, change) =>
            {
                if (PlayerInput.all.Count == 0)
                {
                    if (obj is InputAction action&&action.actionMap.asset.name!="DefaultInputActions")
                    {
                        Player.actions = action.actionMap.asset;
                        OnControlSchemeChange?.Invoke();
                    }
                }
                if (change == InputActionChange.BoundControlsChanged )
                {
                    if(obj is InputActionAsset asset)
                    {
                        if ( !Player.currentControlScheme.IsNullOrEmpty()&& !Enum.TryParse<QControlScheme>(Player.currentControlScheme.RemveChars('&'), out newScheme))
                        {
                            Debug.LogError("不支持环境 " + Player.currentControlScheme);
                        }
                    }
                }
                else if (newScheme != ControlScheme&& obj is InputAction action&& action.activeControl!=null)
                {
                    if (!(action.activeControl.device.description.empty && action.activeControl.device.name == nameof(QVirtualMouse)))
                    {
                        newScheme = QControlScheme.Touchscreen;
                        OnControlSchemeChange?.Invoke();
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