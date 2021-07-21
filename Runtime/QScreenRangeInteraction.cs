using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.InputSystem.Editor;
#endif
using UnityEngine;
using UnityEngine.InputSystem;
namespace QTool.QInputSystem
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class QScreenRangeInteraction : IInputInteraction<Vector2>
    {
        public float up=0;
        public float donw = 0;
        public float left = 0;
        public float right = 0;

        static QScreenRangeInteraction()
        {
             InputSystem.RegisterInteraction<QScreenRangeInteraction>();
        }
        //[RuntimeInitializeOnLoadMethod]
        //private static void Initialize()
        //{

        //}
      
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
        //Vector2? lastPos = null;
        public void Process(ref InputInteractionContext context)
        {
            var control= context.control;
          
            var pos = context.ReadValue<Vector2>()/new Vector2(Screen.width,Screen.height);
            //  Debug.LogError(pos + " : " +lastPos.Value);
            if (InRange(pos))
            {
                if(!context.isStarted)
                {
                    context.Performed();
                    context.PerformedAndStayStarted();
                }
            }
            else
            {
                if(!context.isStarted)
                {
                    context.Canceled();
                }
            }
        }
       // InputControl m_Control;
        public void Reset()
        {
           // lastPos = null;
          //  m_Control = null;
        }
    }
}