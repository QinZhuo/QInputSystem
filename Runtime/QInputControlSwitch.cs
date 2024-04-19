#if InputSystem
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace QTool.InputSystem
{
    public class QInputControlSwitch : MonoBehaviour
    {
        public QControlScheme controlScheme;
        public UnityEvent<bool> OnActive;
        private void Awake()
        {
            Fresh();
            QInputSystem.OnControlSchemeChange += Fresh;
        }
        private void OnDestroy()
        {
            QInputSystem.OnControlSchemeChange -= Fresh;
        }
        public void Fresh()
        {
            OnActive.Invoke(controlScheme.HasFlag(QInputSystem.ControlScheme));
        }
    }
}

#endif