using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Button;
using static UnityEngine.UI.Selectable;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
namespace QTool.QInputSystem
{
    abstract class TempHandlerList<T> where T:class
    {
        protected abstract void Action(T handler);
        public void Init(List< MonoBehaviour> monos)
        {
            HandlerList = new List<T>();
            foreach (var mono in monos)
            {
                if(mono is T)
                {
                    HandlerList.Add(mono as T);
                }
            }
        }
        List<T> HandlerList;
        public void Invoke()
        {
            if (HandlerList == null)
            {
                Debug.LogError("未初始化" + GetType());
                return;
            }
            foreach (var handler in HandlerList)
            {
                Action(handler);
            }
        }
    }
    class QEventTrigger
    {
        public ClickList click = new ClickList();
        public DonwList donw = new DonwList();
        public UpList up = new UpList();
       // public SelectList select = new SelectList();
      //  public DeselectList deselect = new DeselectList();
        public EnterList enter = new EnterList();
        public ExitList exit = new ExitList();

        internal void Init(MonoBehaviour baseMono)
        {
            var monos =new List<MonoBehaviour>( baseMono.GetComponents<MonoBehaviour>());
            monos.Remove(baseMono);
            click.Init(monos);
            donw.Init(monos);
            up.Init(monos);
            //select.Init(monos);
            //deselect.Init(monos);
            enter.Init(monos);
            exit.Init(monos);
        }
    }
    class EnterList : TempHandlerList<IPointerEnterHandler>
    {
        protected override void Action(IPointerEnterHandler handler)
        {
            handler.OnPointerEnter(EventCreater.TempEventData);
        }
    }
    class ExitList : TempHandlerList<IPointerExitHandler>
    {
        protected override void Action(IPointerExitHandler handler)
        {
            handler.OnPointerExit(EventCreater.TempEventData);
        }
    }

     class SelectList : TempHandlerList<ISelectHandler>
    {
        protected override void Action(ISelectHandler handler)
        {
            handler.OnSelect(EventCreater.TempEventData);
        }
    }
    class DeselectList : TempHandlerList<IDeselectHandler>
    {
        protected override void Action(IDeselectHandler handler)
        {
            handler.OnDeselect(EventCreater.TempEventData);
        }
    }
    class ClickList : TempHandlerList<IPointerClickHandler>
    {
        protected override void Action(IPointerClickHandler handler)
        {
            handler.OnPointerClick(EventCreater.TempEventData);
        }
    }
    class DonwList : TempHandlerList<IPointerDownHandler>
    {
        protected override void Action(IPointerDownHandler handler)
        {
            handler.OnPointerDown(EventCreater.TempEventData);
        }
    }
    class UpList : TempHandlerList<IPointerUpHandler>
    {
        protected override void Action(IPointerUpHandler handler)
        {
            handler.OnPointerUp(EventCreater.TempEventData);
        }
    }
    static class EventCreater
    {
        static PointerEventData _eventData;
        public static PointerEventData TempEventData
        {
            get
            {
                if (_eventData == null)
                {
                    _eventData = new PointerEventData(EventSystem.current);
                    _eventData.Reset();
                    _eventData.button = PointerEventData.InputButton.Left;
                    _eventData.clickCount = 1;
                    _eventData.pointerId = 123;
                }
                return _eventData;
            }
        }
    }
    [RequireComponent(typeof(Selectable))]
    public class QInputButton : MonoBehaviour
    {
        public InputActionProperty inputAction;
 
        QEventTrigger trigger = new QEventTrigger();
        public Selectable Selectable;
        public static string onlyInput = "";
        public float minInterval =0;
        private void Reset()
        {
            Selectable = GetComponent<Selectable>();
        }
        float time=0;
        public bool KeyActive => (string.IsNullOrWhiteSpace(onlyInput) || name.Equals(onlyInput));
        private void Awake()
        {
            if (Selectable == null)
            {
                Selectable = GetComponent<Selectable>();
            }
            trigger.Init(this);
            if (inputAction.action != null)
            {
                inputAction.action.Enable();
                inputAction.action.started += content =>
                {
                    if (Selectable.IsInteractable()&& KeyActive)
                    {
                        trigger.enter.Invoke();
                        trigger.donw.Invoke();
                    }
                };
                inputAction.action.performed += content =>
                {
                    if (Selectable.IsInteractable()&& KeyActive&& (Time.unscaledTime - time) > minInterval)
                    {
                        trigger.click.Invoke();
                        time = Time.unscaledTime;
                    }
                };
                inputAction.action.canceled += content =>
                {
                    if (Selectable.IsInteractable()&& KeyActive)
                    {
                        trigger.up.Invoke();
                        trigger.exit.Invoke();
                    }
                };
            }
        }
    }
}