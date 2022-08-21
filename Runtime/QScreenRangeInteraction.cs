using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.InputSystem.Editor;
#endif
using UnityEngine;
using UnityEngine.InputSystem;
namespace QTool.InputSystem
{
    /// <summary>
    /// Êó±êÆÁÄ»Î»ÖÃ´¥·¢Âß¼­
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class QScreenRangeInteraction : IInputInteraction<Vector2>
    {
        public float up=0;
        public float donw = 0;
        public float left = 0;
        public float right = 0;
#if UNITY_EDITOR
        static QScreenRangeInteraction() => UnityEngine.InputSystem.InputSystem.RegisterInteraction<QScreenRangeInteraction>();
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        static void OnQScreenRangeInteraction() => UnityEngine.InputSystem.InputSystem.RegisterInteraction<QScreenRangeInteraction>();
#endif

        bool InRange(Vector2 pos)
        {
            if (pos.x >= left && pos.x <= right)
            {
                if (pos.y >= donw && pos.y <= up)
                {
                    return true;
                }
            }
            return false;
        }
        public void Process(ref InputInteractionContext context)
        {
            var control= context.control;
          
            var pos = context.ReadValue<Vector2>()/new Vector2(Screen.width,Screen.height);
            if (InRange(pos))
            {
                if(!context.isStarted)
                {
                    context.PerformedAndStayStarted();
                }
            }
            else
            {
                if(context.isStarted)
                {
                    context.Canceled();
                }
            }
        }
        public void Reset()
        {
        }
    }
}