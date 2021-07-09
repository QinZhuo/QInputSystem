using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace QTool.InputSystem
{
    using InputSystem = UnityEngine.InputSystem.InputSystem;
    public class QInputKeySet : QInputKeyTip
    {
        public static Action<QInputKeySet, bool> OnChangeKey;
        private InputActionRebindingExtensions.RebindingOperation ChangeOperation;
        void CleanUp()
        {
            ChangeOperation?.Dispose();
            ChangeOperation = null;
        }
        private void PerformInteractiveRebind(InputAction action)
        {
            ChangeOperation?.Cancel(); // Will null out m_RebindOperation.

          

            // Configure the rebind.
            ChangeOperation = action.PerformInteractiveRebinding()
                .OnCancel(
                    operation =>
                    {
                        OnChangeKey?.Invoke(this, false);
                        OnChange();
                        CleanUp();
                    })
                .OnComplete(
                    operation =>
                    {
                        Debug.LogError(operation.action.GetBindingDisplayString()+":"+action.GetBindingDisplayString());

                        OnChangeKey?.Invoke(this, false);
                        OnChange();
                        CleanUp();
                      
                    });


            OnChangeKey?.Invoke(this, true);
            ChangeOperation.Start();
        }
        public void StartChange()
        {
            if (!Active) return;
            PerformInteractiveRebind(action);
        }
        protected override  void OnDisable()
        {
            base.OnDisable();
            CleanUp();
        }
    }
}
