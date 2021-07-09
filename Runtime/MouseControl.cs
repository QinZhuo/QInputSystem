using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTool;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using System;

namespace QTool.InputSystem {
   
    using InputSystem = UnityEngine.InputSystem.InputSystem;
    public class MouseControl : MonoBehaviour
    { 

        //[Header("Cursor")]
        //[SerializeField] private Graphic CursorGraphic;
        //[SerializeField] private RectTransform CursorTransform;

        [Header("Motion")]
        [SerializeField] private float CursorSpeed = 400;
        [SerializeField] private float ScrollSpeed = 45;
        private void OnEnable()
        {
            InitCursor();
            InputSystem.onAfterUpdate += OnAfterInputUpdate;
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
     //   MouseButton? button = null;
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
                // Motion has stopped.
                m_LastTime = default;
                m_LastStickValue = default;
            }
            else
            {
                var currentTime = InputState.currentTime;
                if (Mathf.Approximately(0, m_LastStickValue.x) && Mathf.Approximately(0, m_LastStickValue.y))
                {
                    // Motion has started.
                    m_LastTime = currentTime;
                }

                // Compute delta.
                var deltaTime = (float)(currentTime - m_LastTime);
                var delta = new Vector2(CursorSpeed * stickValue.x * deltaTime,CursorSpeed * stickValue.y * deltaTime);
                var newPosition= SystemMouse.position.ReadValue();
                newPosition +=delta;
                InputState.Change(SystemMouse.position, newPosition);
                InputState.Change(SystemMouse.delta, delta);

                m_LastStickValue = stickValue;
                m_LastTime = currentTime;

                // Update hardware cursor. Debug.LogError("Update" + stickValue);
              
                SystemMouse.WarpCursorPosition(newPosition);

              
               
            }
            //if (LeftButtonAction.action.Start()|| LeftButtonAction.action.Canceled())
            //{
            //    button = MouseButton.Left;
            //}
            //Debug.LogError(LeftButtonAction.action.GetPhase());
            //var action = context.action;
            //if (action == m_LeftButtonAction.action)

            //else if (action == m_RightButtonAction.action)
            //    button = MouseButton.Right;
            //else if (action == m_MiddleButtonAction.action)
            //    button = MouseButton.Middle;
            //else if (action == m_ForwardButtonAction.action)
            //    button = MouseButton.Forward;
            //else if (action == m_BackButtonAction.action)
            //    button = MouseButton.Back;
            //if (button != null)
            //{
            //    SystemMouse.CopyState<MouseState>(out var mouseState);
            //    mouseState.WithButton(button.Value, LeftButtonAction.action.activeControl.IsPressed());
            //    InputState.Change(SystemMouse, mouseState);
            //    button = null;
            //}
        }
      
        Mouse SystemMouse;
        void InitCursor()
        {
            var devices =InputSystem.devices;
            for (var i = 0; i < devices.Count; ++i)
            {
                var device = devices[i];
                if (device.native && device is Mouse mouse)
                {
                    SystemMouse = mouse;
                    break;
                }
            }
            //if (SystemMouse != null)
            //{
            //    var pos=new Vector2(Screen.width, Screen.height)/2;
            //    InputState.Change(SystemMouse.position,pos);
            //    SystemMouse.WarpCursorPosition(pos);

            //}

        }
        [Space(10)]
        [SerializeField] private InputActionProperty StickAction;
        [SerializeField] private InputActionProperty LeftButtonAction;
        [SerializeField] private InputActionProperty MiddleButtonAction;
        [SerializeField] private InputActionProperty RightButtonAction;
        [SerializeField] private InputActionProperty ForwardButtonAction;
        [SerializeField] private InputActionProperty BackButtonAction;
        [SerializeField] private InputActionProperty ScrollWheelAction;


        //private void Awake()
        //{
        //    Cursor.visible = false;
        //}
        //void Update()
        //{
        //    transform.position = QInput.MousePosition;
        //    if (onlyMouseControlShow)
        //    {
        //        gameObject.InvokeEvent("显示",QInput.MouseControl);
        //    }

        //}
    }

}