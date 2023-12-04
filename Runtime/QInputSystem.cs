using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
#if InputSystem
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
namespace QTool.InputSystem
{
	public static class QInputSystem
	{
		static QControlScheme _ControlScheme = QControlScheme.None;
		public static QControlScheme ControlScheme
		{
			get => _ControlScheme;
			set
			{
				if (_ControlScheme != value)
				{
					_ControlScheme = value;
					if (_ControlScheme != QControlScheme.Touchscreen)
					{
						ActiveBindingMask = new InputBinding() { groups = ControlScheme.ToString() };
					}
					QDebug.Log("操作方式更改 " + ControlScheme);
					OnControlSchemeChange?.Invoke();
				}
			}
		}
		public static InputBinding ActiveBindingMask { get; private set; } = default;

		static QControlScheme newScheme;
		public static event Action OnControlSchemeChange;
		static PlayerInput _playerInput = null;
		public static PlayerInput Player
		{
			get
			{
				if (_playerInput == null && Application.isPlaying && QToolManager.Instance != null)
				{
					if (PlayerInput.all.Count == 0)
					{
						_playerInput = QToolManager.Instance.gameObject.AddComponent<PlayerInput>();
						_playerInput.actions = Resources.Load<InputActionAsset>(nameof(QInputSetting));
						if (_playerInput.actions == null)
						{
							QDebug.LogWarning(nameof(Resources)+ "找不到设置文件" + nameof(QInputSetting));
							_playerInput.actions = ScriptableObject.CreateInstance<InputActionAsset>();
						}
						foreach (var map in _playerInput.actions.actionMaps)
						{
							map.Enable();
							foreach (var action in map.actions)
							{
								action.Enable();
							}
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
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void DeviceTypeCheck()
		{
			var player = Player;
			UnityEngine.InputSystem.InputSystem.onActionChange += (obj, change) =>
			{
				if (Application.isPlaying)
				{
					switch (change)
					{
						case InputActionChange.ActionEnabled:
						case InputActionChange.ActionStarted:
						case InputActionChange.ActionPerformed:
							if (obj is InputAction action && action.activeControl != null && action.activeControl.device != null)
							{
								if (!Player.currentControlScheme.IsNull() && !Enum.TryParse(Player.currentControlScheme.RemveChars('&'), out newScheme))
								{
									Debug.LogWarning("不支持环境 " + Player.currentControlScheme);
								}
								if (newScheme != ControlScheme)
								{
									ControlScheme = newScheme;
								}
							}
							break;
						case InputActionChange.BoundControlsChanged:
							{
								if (obj is InputActionAsset asset)
								{
									if (!Player.currentControlScheme.IsNull() && !Enum.TryParse(Player.currentControlScheme.RemveChars('&'), out newScheme))
									{
										Debug.LogWarning("不支持环境 " + Player.currentControlScheme);
									}
								}
							}
							break;
						default:
							break;
					}


				}
			};
		}
		public static Action<InputAction, int> OnRebindingStart;
		public static Action<InputAction, int> OnRebindingOver;

	
		static InputActionRebindingExtensions.RebindingOperation ActiveRebinding;
		public static string ToQString(this InputBinding bind)
		{
			var view = bind.ToDisplayString();
			if (Application.platform == RuntimePlatform.Switch)
			{
				switch (view)
				{
					case "X": view = "Y"; break;
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
#endif