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
                        if (_playerInput.actions == null)
                        {
                            Debug.LogWarning("Resources�²�����" + nameof(QInputSetting));
                            _playerInput.actions = new InputActionAsset();
                        }
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
                                Debug.LogWarning("��֧�ֻ��� " + Player.currentControlScheme);
                            }
                        }
                    }else if(obj is InputAction action && action.activeControl != null&& action.activeControl.device!=null)
                    {
                        if (!Player.currentControlScheme.IsNullOrEmpty() && !Enum.TryParse<QControlScheme>(Player.currentControlScheme.RemveChars('&'), out newScheme))
                        {
                            Debug.LogWarning("��֧�ֻ��� " + Player.currentControlScheme);
                        }
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
                            QDebug.Log("������ʽ���� " + ControlScheme);
                            OnControlSchemeChange?.Invoke();
                        }
                    }
                   
                }
            };
        }
        public static Action<InputAction, int> OnRebindingStart;
        public static Action<InputAction,int> OnRebindingOver;

        public static InputBinding ActiveBindingMask => ControlScheme== QControlScheme.None?default : new InputBinding() { groups =ControlScheme.ToString() };

        static InputActionRebindingExtensions.RebindingOperation ActiveRebinding;
        public static string ToQString(this InputBinding bind)
        {
            var view = bind.ToDisplayString();
            if(Application.platform== RuntimePlatform.Switch)
            {
                switch (view)
                {
                    case "X":view = "Y";break;
                    case "Y": view = "X"; break;
                    case "A": view = "B"; break;
                    case "B": view = "A"; break;
                    default:
                        break;
                }
            }
            return view;
        }
        public static string ToViewString(this InputAction action,int bindIndex=-1)
        {
            if (action == null) return "";
            if (bindIndex < 0)
            {
                var index = action.GetBindingIndex(QInputSystem.ActiveBindingMask);
                if (index >= 0)
                {
                    var bind = action.bindings[index];
                    if (!bind.isPartOfComposite)
                    {
                        return bind.ToQString();
                    }
                    var view = "";
                    while (bind.isPartOfComposite)
                    {
                        view += bind.ToQString();
                        index++;
                        bind = action.bindings[index];
                    }
                    return view;
                }
                return "";
            }
            else
            {
                return action.bindings[bindIndex].ToQString();
            }
           
        }
        public static InputBinding GetActiveBindingMask(this InputAction inputAction)
        {
            if (inputAction.bindingMask.HasValue)
                return inputAction.bindingMask.Value;

            if (inputAction.actionMap?.bindingMask != null)
                return inputAction.actionMap.bindingMask.Value;

            if (inputAction.actionMap?.asset?.bindingMask != null)
            {
                return inputAction.actionMap.asset.bindingMask.Value;
            }
            return default;
        }
        public static async Task<bool> RebindingAsync(this InputAction action, int bindIndex)
        {
            if (ControlScheme == QControlScheme.Touchscreen||ControlScheme== QControlScheme.None) return false;
            if (ActiveRebinding != null)
            {
                ActiveRebinding.Cancel();
            }
            var enable = action.enabled;
            if (enable)
            {
                action.Disable();
            }
            OnRebindingStart?.Invoke(action, bindIndex);
            var rebinding = action.PerformInteractiveRebinding(bindIndex);
            ActiveRebinding = rebinding;
            rebinding.Start();
            await QTask.Wait(() => rebinding.completed || rebinding.canceled);
            rebinding.Dispose();
            if (enable)
            {
                action.Enable();
            }
            OnRebindingOver?.Invoke(action, bindIndex);
            if (rebinding == ActiveRebinding)
            {
                ActiveRebinding = null;
            }
            return rebinding.completed;
        }

    }
    [Flags]
    public enum QControlScheme
    {
        None=0,
        KeyboardMouse=1<<1,
        Gamepad=1<<2,
        Touchscreen=1<<3,
    }

}