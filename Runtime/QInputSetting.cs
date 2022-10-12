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
        [SerializeField]
        private string tipInfo = "按下{Key}触发";
        protected string ViewKey
        {
            get
            {
                return action.action?.GetBindingDisplayString();
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
            OnChangeKey += OnChangeKeyCallBack;
            QInputSystem.OnControlSchemeChange += OnChange;
        }
        protected virtual void OnDisable()
        {
            OnChangeKey -= OnChangeKeyCallBack;
            QInputSystem.OnControlSchemeChange -= OnChange;
            CleanUp();
        }
        void OnChangeKeyCallBack(QInputSetting keySetting, bool start)
        {
            if (!start)
            {
                if (keySetting.action == action)
                {
                    OnChange();
                }
            }
        }
        #region 改键逻辑
        public static Action<QInputSetting, bool> OnChangeKey;
        private InputActionRebindingExtensions.RebindingOperation ChangeOperation;
        void CleanUp()
        {
            ChangeOperation?.Dispose();
            ChangeOperation = null;
        }
        private void PerformInteractiveRebind(InputAction action)
        {
            ChangeOperation?.Cancel();

            ChangeOperation = action.PerformInteractiveRebinding(action.GetBindingIndex())
                .OnCancel(
                    operation =>
                    {
                        OnChange();
                        CleanUp();
                        OnChangeKey?.Invoke(this, false);
                        action?.Enable();
                    })
                .OnComplete(
                    operation =>
                    {

                        OnChange();
                        CleanUp();
                        OnChangeKey?.Invoke(this, false);
                        action?.Enable();

                    });


            OnChangeKey?.Invoke(this, true);
            ChangeOperation.Start();
        }
        public void StartChange()
        {
            if (!Active) return;
            action.action.Disable();
            PerformInteractiveRebind(action);
        }

        #endregion
    }
}