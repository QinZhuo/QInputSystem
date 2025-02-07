using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTool;
using UnityEngine.Events;
using System;
using System.Threading.Tasks;

#if InputSystem
using UnityEngine.InputSystem;
namespace QTool.InputSystem {
    using InputSystem = UnityEngine.InputSystem.InputSystem;
    /// <summary>
    /// 快捷键提示脚本
    /// </summary>
    public class QInputSetting : MonoBehaviour
    {
        [UnityEngine.Serialization.FormerlySerializedAs("action")]
        [SerializeField]
        private InputActionReference defaultAction;
        private InputAction _action;
        public InputAction Action
        {
            get => _action;
            set
            {
                _action = value;
                OnChange();
            }
        }
        public int bindIndex = -1;
        [SerializeField]
        private string tipInfo = "按下{Key}触发";
        public string TipInfo
        {
            get
            {
                return tipInfo;
            }
            set
            {
                tipInfo = value;
                OnChange();
            }
        }
        public UnityEvent<string> OnValueChange;
        public bool Active
        {
            get
            {
                return Action!=null;
            }
        }

        protected void OnChange()
        {
            if (!Active) return;
            OnValueChange?.Invoke(tipInfo.Replace("{Key}", Action.ToViewString(bindIndex)));
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            OnChange();
        }
#endif
		protected virtual void Awake()
        {
			Action = defaultAction?.action;
			QInputSystem.OnRebindingOver += OnRebindingOver;
			QInputSystem.OnControlSchemeChange += OnChange;
		}
		protected virtual void OnDestroy()
		{
			QInputSystem.OnRebindingOver -= OnRebindingOver;
			QInputSystem.OnControlSchemeChange -= OnChange;
		}
        void OnRebindingOver(InputAction inputAction,int bindIndex)
        {
            if (inputAction == Action)
            {
                OnChange();
            }
        }
#region 改键逻辑
        public async void StartChange()
        {
            if (!Active) return;
            var index = bindIndex;
            if (bindIndex >= 0)
            {
                OnValueChange?.Invoke("?");
                await Action.RebindingAsync(index);
                return;
            }
            if (index < 0)
            {
                index = Action.GetBindingIndex(QInputSystem.ActiveBindingMask);
            }
            if (index < 0)
            {
                Action.AddBinding(QInputSystem.ActiveBindingMask);
                index = Action.GetBindingIndex(QInputSystem.ActiveBindingMask);
            }
            var bind = Action.bindings[index];
            if (!bind.isPartOfComposite)
            {
                OnValueChange?.Invoke("?");
                await Action.RebindingAsync(index);
                return;
            }
            while (bind.isPartOfComposite)
            {
                OnValueChange?.Invoke(Action.ToViewString().Replace(bind.ToQString(), "?"));
                await Action.RebindingAsync(index);
                await Task.Delay(200);
                index++;
                bind = Action.bindings[index];
            }
        }

#endregion
    }
}
#endif