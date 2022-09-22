using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_SWITCH
using nn.hid;
#endif

namespace QTool.InputSystem
{

#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class QVirtualGamepad : QGamepad<QVirtualGamepad>
    {
#if UNITY_EDITOR
        static QVirtualGamepad()
        {
            Initialize();
        }
#endif
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        protected static void Initialize()
        {
            UnityEngine.InputSystem.InputSystem.RegisterLayout<QVirtualGamepad>();
        }
    }
    public abstract class QGamepad<T> : Gamepad where T : QGamepad<T>
    {

        protected static void RegisterLayout()
        {
            UnityEngine.InputSystem.InputSystem.RegisterLayout<QSwitchGamepad>();
        }
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = UnityEngine.InputSystem.InputSystem.GetDevice<T>();
                    if (_instance == null)
                    {
                        _instance = UnityEngine.InputSystem.InputSystem.AddDevice<T>();
                    }
                }
                return _instance;
            }
        }
    }
    #region Switch
#if UNITY_SWITCH
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    [InputControlLayout(displayName = "Switch Controller (on Switch)")]
    public class QSwitchGamepad : QGamepad<QSwitchGamepad>,IInputUpdateCallbackReceiver
    {

        [InputControl(name = "buttonNorth", displayName = "X", shortDisplayName = "X")]
        [InputControl(name = "buttonSouth", displayName = "B", shortDisplayName = "B")]
        [InputControl(name = "buttonWest", displayName = "Y", shortDisplayName = "Y")]
        [InputControl(name = "buttonEast", displayName = "A", shortDisplayName = "A")]
        [InputControl(name = "leftShoulder", displayName = "L", shortDisplayName = "L")]
        [InputControl(name = "rightShoulder", displayName = "R", shortDisplayName = "R")]
        [InputControl(name = "leftTrigger", displayName = "ZL", shortDisplayName = "ZL")]
        [InputControl(name = "rightTrigger", displayName = "ZR", shortDisplayName = "ZR")]
        [InputControl(name = "start", displayName = "Plus")]
        [InputControl(name = "select", displayName = "Minus")]
        public new ButtonControl aButton =>base.bButton;
        public new ButtonControl bButton =>base.aButton;
#if UNITY_EDITOR
        static QSwitchGamepad()
        {
            Initialize();
        }
#endif
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        protected static void Initialize()
        {
            RegisterLayout();
            var device = Instance;
            if (Application.isPlaying)
            {
                Npad.Initialize();
                Npad.SetSupportedIdType(new NpadId[] { NpadId.Handheld });
                Npad.SetSupportedStyleSet(NpadStyle.FullKey | NpadStyle.Handheld | NpadStyle.JoyDual);
            }
        }
        private NpadState npadState = new NpadState();
        public void OnUpdate()
        {
            Npad.GetState(ref npadState, NpadId.Handheld, Npad.GetStyleSet(NpadId.Handheld));
            var gampadState = new GamepadState();
            if (npadState.GetButton(NpadButton.A))
            {
                gampadState.buttons |= 1 << (int)GamepadButton.B;
            }
            if (npadState.GetButton(NpadButton.B))
            {
                gampadState.buttons |= 1 << (int)GamepadButton.A;
            }
            if (npadState.GetButton(NpadButton.X))
            {
                gampadState.buttons |= 1 << (int)GamepadButton.X;
            }
            if (npadState.GetButton(NpadButton.Y))
            {
                gampadState.buttons |= 1 << (int)GamepadButton.Y;
            }
            if (npadState.GetButton(NpadButton.StickL))
            {
                gampadState.buttons |= 1 << (int)GamepadButton.LeftStick;
            }
            if (npadState.GetButton(NpadButton.StickR))
            {
                gampadState.buttons |= 1 << (int)GamepadButton.RightStick;
            }
            if (npadState.GetButton(NpadButton.L))
            {
                gampadState.buttons |= 1 << (int)GamepadButton.LeftShoulder;
            }
            if (npadState.GetButton(NpadButton.R))
            {
                gampadState.buttons |= 1 << (int)GamepadButton.RightShoulder;
            }
            if (npadState.GetButton(NpadButton.ZL))
            {
                gampadState.buttons |= 1 << (int)GamepadButton.LeftTrigger;
            }
            if (npadState.GetButton(NpadButton.ZR))
            {
                gampadState.buttons |= 1 << (int)GamepadButton.RightTrigger;
            }
            if (npadState.GetButton(NpadButton.Plus))
            {
                gampadState.buttons |= 1 << (int)GamepadButton.Start;
            }
            if (npadState.GetButton(NpadButton.Minus))
            {
                gampadState.buttons |= 1 << (int)GamepadButton.Select;
            }
            gampadState.leftStick = new Vector2(npadState.analogStickL.fx, npadState.analogStickL.fy);
            gampadState.rightStick = new Vector2(npadState.analogStickR.fx, npadState.analogStickR.fy);
            UnityEngine.InputSystem.InputSystem.QueueStateEvent(this, gampadState);
        }
    }
#endif
    #endregion
}