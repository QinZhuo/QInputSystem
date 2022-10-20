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

    public abstract class QGamepad<T> : Gamepad where T : QGamepad<T>
    {
        protected static void RegisterLayout()
        {
            UnityEngine.InputSystem.InputSystem.RegisterLayout<T>();
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

#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    [InputControlLayout(displayName = "Switch Controller (on Switch)")]
    public class QSwitchGamepad : QGamepad<QSwitchGamepad>
#if UNITY_SWITCH
        , IInputUpdateCallbackReceiver
#endif

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
#if UNITY_SWITCH
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
            if (Application.platform== RuntimePlatform.Switch)
            {
                var device = Instance;
                Npad.Initialize();
                Npad.SetSupportedIdType(new NpadId[] { NpadId.Handheld });
                Npad.SetSupportedStyleSet(NpadStyle.FullKey | NpadStyle.Handheld | NpadStyle.JoyDual);
            }
        }
        private NpadState npadState = new NpadState();
        private GamepadState lastState = new GamepadState();
        public void OnUpdate()
        {
            if(Application.platform== RuntimePlatform.Switch)
            {
                var style = Npad.GetStyleSet(NpadId.Handheld);
                Npad.GetState(ref npadState, NpadId.Handheld, style);
             
                var gampadState = new GamepadState();
                if (style != NpadStyle.Invalid)
                {
                    if (npadState.GetButton(NpadButton.A))
                    {
                        gampadState.WithButton(GamepadButton.B);
                    }
                    if (npadState.GetButton(NpadButton.B))
                    {
                        gampadState.WithButton(GamepadButton.A);
                    }
                    if (npadState.GetButton(NpadButton.X))
                    {
                        gampadState.WithButton(GamepadButton.X);
                    }
                    if (npadState.GetButton(NpadButton.Y))
                    {
                        gampadState.WithButton(GamepadButton.Y);
                    }
                    if (npadState.GetButton(NpadButton.StickL))
                    {
                        gampadState.WithButton(GamepadButton.LeftStick);
                    }
                    if (npadState.GetButton(NpadButton.StickR))
                    {
                        gampadState.WithButton(GamepadButton.RightStick);
                    }
                    if (npadState.GetButton(NpadButton.L))
                    {
                        gampadState.WithButton(GamepadButton.LeftShoulder);
                    }
                    if (npadState.GetButton(NpadButton.R))
                    {
                        gampadState.WithButton(GamepadButton.RightShoulder);
                    }
                    if (npadState.GetButton(NpadButton.ZL))
                    {
                        gampadState.leftTrigger = 1;
                    }
                    if (npadState.GetButton(NpadButton.ZR))
                    {
                        gampadState.rightTrigger = 1;
                    }
                    if (npadState.GetButton(NpadButton.Plus))
                    {
                        gampadState.WithButton(GamepadButton.Start);
                    }
                    if (npadState.GetButton(NpadButton.Minus))
                    {
                        gampadState.WithButton(GamepadButton.Select);
                    }
                    gampadState.leftStick = new Vector2(npadState.analogStickL.fx, npadState.analogStickL.fy);
                    gampadState.rightStick = new Vector2(npadState.analogStickR.fx, npadState.analogStickR.fy);
                    MakeCurrent();
                }
                if(!lastState.Equals(gampadState))
                {
                    lastState = gampadState;
                    UnityEngine.InputSystem.InputSystem.QueueStateEvent(this, gampadState);
                }
              
            }

        }
#endif
    }
    #endregion
}