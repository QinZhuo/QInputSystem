using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace QTool.QInputSystem
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
            ChangeOperation?.Cancel(); // Will null out m_RebindOperation.

          

            // Configure the rebind.
            ChangeOperation = action.PerformInteractiveRebinding()
                .OnCancel(
                    operation =>
                    {
                        OnChangeKey?.Invoke(this, false);
                        OnChange();
                        CleanUp();
                        action?.Enable();
                    })
                .OnComplete(
                    operation =>
                    {

                        OnChangeKey?.Invoke(this, false);
                        OnChange();
                        CleanUp();
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
