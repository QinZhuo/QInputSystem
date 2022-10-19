using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

namespace QTool.InputSystem
{
    public static class QInputSystem
    {
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
                        _playerInput.actions = Resources.Load<InputActionAsset>(nameof(QInputSetting));
                        foreach (var action in _playerInput.actions)
                        {
                            action.Enable();
                        }
                        OnControlSchemeChange?.Invoke();
                    }
                    else
                    {
                        _playerInput = PlayerInput.all[0];
                    }
                
                }
                return _playerInput;
            }
        }
        public static InputActionAsset QInputSetting => Player.actions;
        [RuntimeInitializeOnLoadMethod]
        private static void DeviceTypeCheck()
        {
            UnityEngine.InputSystem.InputSystem.onActionChange += (obj, change) =>
            {
                if (Application.isPlaying)
                {
                    if (change == InputActionChange.BoundControlsChanged)
                    {
                        if (obj is InputActionAsset asset)
                        {
                            if (!Player.currentControlScheme.IsNullOrEmpty() && !Enum.TryParse<QControlScheme>(Player.currentControlScheme.RemveChars('&'), out newScheme))
                            {
                                Debug.LogError("不支持环境 " + Player.currentControlScheme);
                            }
                        }
                    }else if(obj is InputAction action && action.activeControl != null)
                    {
                        if (newScheme != ControlScheme  )
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
                   
                }
            };
        }

        public static Action<InputAction> OnRebindingOver;

    }
    [System.Flags]
    public enum QControlScheme
    {
        None=0,
        KeyboardMouse=1<<1,
        Gamepad=1<<2,
        Touchscreen=1<<3,
    }

    public static partial class Tool
    {
        public static InputBinding GetActiveBindingMask(this InputAction inputAction)
        {
            if (inputAction.bindingMask.HasValue)
                return inputAction.bindingMask.Value;

            if (inputAction.actionMap?.bindingMask != null)
                return inputAction.actionMap.bindingMask.Value;

            if(inputAction.actionMap?.asset?.bindingMask != null)
            {
                return inputAction.actionMap.asset.bindingMask.Value;
            }
            return default;
        }

        static InputActionRebindingExtensions.RebindingOperation ActiveRebinding;
        
        public static async Task<bool> RebindingAsync(this InputAction action,int bindIndex)
        {
            if (ActiveRebinding != null)
            {
                ActiveRebinding.Cancel();
            }
            var enable= action.enabled ;
            if (enable)
            {
                action.Disable();
            }
            var rebinding = action.PerformInteractiveRebinding(bindIndex);
            ActiveRebinding = rebinding;
            rebinding.Start();
            await QTask.Wait(() => rebinding.completed || rebinding.canceled);
            rebinding.Dispose();
            if (enable)
            {
                action.Enable();
            }
            if (rebinding.completed)
            {
                QInputSystem.OnRebindingOver?.Invoke(action);
            }
            if (rebinding == ActiveRebinding)
            {
                ActiveRebinding = null;
            }
            return rebinding.completed;
        }
    }

}