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
                if (!Active) return "";
                if (bindIndex < 0)
                {
                    var index= action.action.GetBindingIndex(QInputSystem.ActiveBindingMask);
                    if (index >= 0)
                    {
                        var bind= action.action.bindings[index];
                        if (!bind.isPartOfComposite)
                        {
                           return bind.ToDisplayString();
                        }
                        var view = "";
                        while (bind.isPartOfComposite)
                        {
                            view += bind.ToDisplayString();
                            index++;
                            bind = action.action.bindings[index];
                        }
                        return view;

                    }
                    return "";

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
        void OnRebindingOver(InputAction inputAction,int bindIndex)
        {
            if (inputAction == action?.action)
            {
                OnChange();
            }
        }
        #region 改键逻辑
        private async void PerformInteractiveRebind(InputAction action)
        {
            var index = bindIndex;
            if (bindIndex >=0)
            {
                OnValueChange?.Invoke("?");
                await action.RebindingAsync(index);
                return;
            }
            if (index < 0)
            {
                index = action.GetBindingIndex(QInputSystem.ActiveBindingMask);
            }
            if (index < 0)
            {
                action.AddBinding(QInputSystem.ActiveBindingMask);
                index = action.GetBindingIndex(QInputSystem.ActiveBindingMask);
            }
            var bind = action.bindings[index];
            if (!bind.isPartOfComposite)
            {
                OnValueChange?.Invoke("?");
                await action.RebindingAsync(index);
                return;
            }
            while (bind.isPartOfComposite)
            {
                OnValueChange?.Invoke(ViewKey.Replace(bind.ToDisplayString(), "?"));
                await action.RebindingAsync(index);
                await QTask.Wait(0.2f);
                index++;
                bind = action.bindings[index];
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