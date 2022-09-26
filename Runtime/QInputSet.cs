using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace QTool.InputSystem
{
    using InputSystem = UnityEngine.InputSystem.InputSystem;
    public class QInputSet : QInputTip
    {
        public static Action<QInputSet, bool> OnChangeKey;
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
        protected override  void OnDisable()
        {
            base.OnDisable();
            CleanUp();
        }
    }
}
