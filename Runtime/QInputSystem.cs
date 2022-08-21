using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace QTool.QInputSystem
{
    public static class QInputSystem
    {
        static InputActionAsset _inputSetting;
        public static InputActionAsset QInputSetting
        {
            get
            {
                if (_inputSetting == null)
                {
                    _inputSetting = Resources.Load<InputActionAsset>(nameof(QInputSetting));
#if UNITY_EDITOR
                    if (_inputSetting == null)
                    {
                        _inputSetting = ScriptableObject.CreateInstance<InputActionAsset>();
                        UnityEditor.AssetDatabase.CreateAsset(_inputSetting, ("Assets/Resources/" + nameof(QInputSetting) + ".inputactions").CheckFolder());
                        if (!Application.isPlaying)
                        {
                            UnityEditor.AssetDatabase.Refresh();
                        }
                    }
#endif
                }
                return _inputSetting;
            }
        }
        public static Vector2 MousePosition
        {
            get
            {
                return Mouse.current.position.ReadValue();
            }
            set
            {
                var delta = value - MousePosition;
                InputState.Change(Mouse.current.position, value);
                InputState.Change(Mouse.current.delta, delta);
                Mouse.current.WarpCursorPosition(value);
            }
        }
    }
}