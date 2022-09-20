using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTool;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using System;

namespace QTool.InputSystem {
   
    public class QMouseControl : MonoBehaviour
    { 


        [Header("Motion")]
        [SerializeField] private float CursorSpeed = 400;
        [SerializeField] private float ScrollSpeed = 45;
        private void OnEnable()
        {
            InitCursor();
            UnityEngine.InputSystem.InputSystem.onAfterUpdate += OnAfterInputUpdate;
            StickAction.action?.Enable();
            SetActionCallback(LeftButtonAction,OnLeftButton);
        }
        private static void SetActionCallback(InputActionProperty field, Action<InputAction.CallbackContext> callback, bool install = true)
        {
            var action = field.action;
            if (action == null)
                return;
            if (install)
            {
                action.Enable();
            }
            if (install)
            {
                action.started += callback;
                action.canceled += callback;
            }
            else
            {
                action.started -= callback;
                action.canceled -= callback;
            }
        }
        void KeyCheck(InputAction.CallbackContext context,MouseButton key)
        {
            var isPressed = context.control.IsPressed();
            SystemMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(key, isPressed);
            InputState.Change(SystemMouse, mouseState);
            Debug.LogError(key+":"+isPressed);
        }
        void OnLeftButton(InputAction.CallbackContext context)
        {
            KeyCheck(context, MouseButton.Left);
        }
        private double m_LastTime;
        private Vector2 m_LastStickValue;
        void OnAfterInputUpdate()
        {
          
            if (SystemMouse == null)
                return;
            if (StickAction == null)
                return;
          
            var stickValue = StickAction.action.ReadValue<Vector2>();

            if (Mathf.Approximately(0, stickValue.x) && Mathf.Approximately(0, stickValue.y))
            {
                m_LastTime = default;
                m_LastStickValue = default;
            }
            else
            {
                var currentTime = InputState.currentTime;
                if (Mathf.Approximately(0, m_LastStickValue.x) && Mathf.Approximately(0, m_LastStickValue.y))
                {
                    m_LastTime = currentTime;
                }

                var deltaTime = (float)(currentTime - m_LastTime);
                var delta = new Vector2(CursorSpeed * stickValue.x * deltaTime,CursorSpeed * stickValue.y * deltaTime);
                QInputSystem.PointerPosition += delta;

                m_LastStickValue = stickValue;
                m_LastTime = currentTime;
               
            }
        }
      
        Mouse SystemMouse;
        void InitCursor()
        {
            var devices = UnityEngine.InputSystem.InputSystem.devices;
            for (var i = 0; i < devices.Count; ++i)
            {
                var device = devices[i];
                if (device.native && device is Mouse mouse)
                {
                    SystemMouse = mouse;
                    break;
                }
            }

        }
        [Space(10)]
        [SerializeField] private InputActionProperty StickAction;
        [SerializeField] private InputActionProperty LeftButtonAction;
        [SerializeField] private InputActionProperty MiddleButtonAction;
        [SerializeField] private InputActionProperty RightButtonAction;
        [SerializeField] private InputActionProperty ForwardButtonAction;
        [SerializeField] private InputActionProperty BackButtonAction;
        [SerializeField] private InputActionProperty ScrollWheelAction;


    }

}