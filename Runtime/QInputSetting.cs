using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTool;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;

namespace QTool.InputSystem {

    using InputSystem = UnityEngine.InputSystem.InputSystem;
    /// <summary>
    /// 快捷键提示脚本
    /// </summary>
    public class QInputSetting : MonoBehaviour
    {
        public InputActionReference action;
        public int bindIndex = -1;
        [SerializeField]
        private string tipInfo = "按下{Key}触发";
        protected string ViewKey
        {
            get
            {
                if (action.action == null) return "";
                if (bindIndex < 0)
                {
                    if (action.action.controls.Count <= 1)
                    {
                        return action.action.GetBindingDisplayString(action.action.GetActiveBindingMask());
                    }
                    else
                    {
                        var view = "";
                        foreach (var control in action.action.controls)
                        {
                            view += control.displayName;
                        }
                        return view;
                    }
                }
                else
                {
                    return action.action.bindings[bindIndex].ToDisplayString();
                }
            }
        }
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
        public StringEvent OnValueChange;
        public bool Active
        {
            get
            {
                return action!=null&&action.action != null;
            }
        }

        protected void OnChange()
        {
            if (!Active) return;
            string tip = "";
            tip = tipInfo.Replace("{Key}", ViewKey);
            OnValueChange?.Invoke(tip);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
           
            OnChange();
        }
# endif
        protected virtual void OnEnable()
        {
            OnChange();
            QInputSystem.OnRebindingOver += OnRebindingOver;
            QInputSystem.OnControlSchemeChange += OnChange;
        }
        protected virtual void OnDisable()
        {
            QInputSystem.OnRebindingOver -= OnRebindingOver;
            QInputSystem.OnControlSchemeChange -= OnChange;
        }
        void OnRebindingOver(InputAction inputAction)
        {
            if (inputAction == action.action)
            {
                OnChange();
            }
        }
        #region 改键逻辑
        private async void PerformInteractiveRebind(InputAction action)
        {
            var index = bindIndex;
            if (action.controls.Count <= 1|| index >= 0)
            {
                OnValueChange?.Invoke("?");
                if (index < 0)
                {
                    index = action.GetBindingIndex(action.GetActiveBindingMask());
                }
                await action.RebindingAsync(index);
            }
            else
            {
                foreach (var control in action.controls)
                {
                    OnValueChange?.Invoke(ViewKey.Replace(control.displayName,"?"));
                    index = action.GetBindingIndexForControl(control);
                    await action.RebindingAsync(index);
                    await QTask.Wait(0.2f);
                }
            }

        }
        public void StartChange()
        {
            if (!Active) return;
            PerformInteractiveRebind(action);
        }

        #endregion
    }
}