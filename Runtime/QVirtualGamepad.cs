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
namespace QTool.InputSystem
{

#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class QVirtualGamepad:QGamepad<QVirtualGamepad>
    {
#if UNITY_EDITOR
        static QVirtualGamepad()
        {
            Initialize();
        }
#endif
    }
    public abstract class QGamepad<T> : Gamepad, IInputUpdateCallbackReceiver where T:QGamepad<T>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        protected static void Initialize()
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
                        _instance=UnityEngine.InputSystem.InputSystem.AddDevice<T>();
                    }
                }
                return _instance;
            }
        }

        public virtual void OnUpdate()
        {

        }
    }
}