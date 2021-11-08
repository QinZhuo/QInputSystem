using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace QTool.QInputSystem
{
    public static class QInputSystem
    {
        public static Vector2 MousePosition
        {
            get
            {
                return Mouse.current.position.ReadValue();
            }
            set
            {
                Debug.LogWarning("不允许更改鼠标位置");
                return;
                var delta = value - MousePosition;
                InputState.Change(Mouse.current.position, value);
                InputState.Change(Mouse.current.delta, delta);
                Mouse.current.WarpCursorPosition(value);
            }
        }
    }
}