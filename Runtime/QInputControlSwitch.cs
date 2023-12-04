#if InputSystem
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QTool.InputSystem
{
    public class QInputControlSwitch : MonoBehaviour
    {
        public QControlScheme controlScheme;
        public BoolEvent OnActive;
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