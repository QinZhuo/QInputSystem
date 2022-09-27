using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTool;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace QTool.InputSystem {
  
    /// <summary>
    /// 快捷键提示脚本
    /// </summary>
    public class QInputTip : MonoBehaviour
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
        //public virtual string TipKeyReplce(string key)
        //{
        //    var keyCode = KeyCode.None;
        //    if (System.Enum.TryParse(key, out keyCode))
        //    {
        //        switch (keyCode)
        //        {
        //            case KeyCode.KeypadEnter: return "Enter";
        //            case KeyCode.Return: return "Enter";
        //            case KeyCode.LeftAlt: return "Alt";
        //            case KeyCode.RightAlt: return "Alt";
        //            case KeyCode.Escape: return "Esc";
        //            case KeyCode.LeftControl: return "Ctrl";
        //            case KeyCode.RightControl: return "Ctrl";
        //            case KeyCode.Backspace: return "Back";
        //            case KeyCode.LeftShift: return "Shift";
        //            case KeyCode.RightShift: return "Shift";
        //            case KeyCode.UpArrow: return "↑";
        //            case KeyCode.DownArrow: return "↓";
        //            case KeyCode.LeftArrow: return "←";
        //            case KeyCode.RightArrow: return "→";
        //        }
        //    }
        //    return key;
        //}

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
            QInputSetting.OnChangeKey += OnChangeKey;
            QInputSystem.OnControlSchemeChange += OnChange;
        }
        protected virtual void OnDisable()
        {
            QInputSetting.OnChangeKey -= OnChangeKey;
            QInputSystem.OnControlSchemeChange -= OnChange;
        }
        void OnChangeKey(QInputSetting keySetting, bool start)
        {
            if (!start)
            {
                if (keySetting.action == action)
                {
                    OnChange();
                }
            }
        }
    }
}