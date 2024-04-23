using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Button;
using static UnityEngine.UI.Selectable;
using UnityEngine.UI;
using System;
#if InputSystem
using UnityEngine.InputSystem;
namespace QTool.InputSystem
{

    public class QInputButton : MonoBehaviour
	{
		public static float LongTouchTime { get; set; } = 0.3f;
		[SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("inputAction")]
        private InputActionReference defaultAction;
        private InputAction _action;
        public InputAction Action
        {
            get => _action;
            set
            {
                if (_action != value)
                {
                    ClearAction();
					_action = value;
                    InitAction();
                }
            }
        }
		[QName("触发UI事件"),SerializeField]
		private bool triggerUIEvent = false;
		[QName("长按切换",nameof(IsToggle)),SerializeField]
		private bool longTouchSwitch = false;
		QTimer longTouchTimer = new QTimer(LongTouchTime);
		public bool IsToggle => Selectable is Toggle;
		private QInputSetting _Setting = null;
		public QInputSetting Setting => _Setting ??= GetComponent<QInputSetting>();

        UIEventTrigger trigger = new UIEventTrigger();
        Selectable _selectable;
        public Selectable Selectable => _selectable??= GetComponent<Selectable>();
        public static string onlyInput = "";
        private void Reset()
        {
            Selectable.navigation = new Navigation
            {
                mode = Navigation.Mode.None
            };
        }
        bool press = false;

        public bool KeyActive => (string.IsNullOrWhiteSpace(onlyInput) || name.Equals(onlyInput));
        public bool ActiveAndInteractable
        {
            get
            {
                return Selectable.IsInteractable() && Selectable.IsActive() && (parenGroup == null ? true : parenGroup.interactable);
            }
        }
        CanvasGroup parenGroup;
      
        public void InputPerformed(InputAction.CallbackContext context)
		{
			if (ActiveAndInteractable && KeyActive && !press)
			{
				press = true;
				longTouchTimer.Clear();
				if (triggerUIEvent)
				{
					trigger.enter.Invoke();
					trigger.donw.Invoke();
				}
			}
        }
		public void Click()
		{
			if (triggerUIEvent)
			{
				trigger.click.Invoke();
			}
			else if (Selectable is Button button)
			{
				button.onClick.Invoke();
			}
			else if (Selectable is Toggle toggle)
			{
				toggle.isOn = !toggle.isOn;
			}
		}
        public void InputCanceled(InputAction.CallbackContext context)
        {
            if (KeyActive && press)
			{
				Click();
				press = false;
				if (triggerUIEvent)
				{
					trigger.up.Invoke();
					trigger.exit.Invoke();
				}
            }
        }
        private void InitAction()
        {
            if (Action != null)
            {
                parenGroup = GetComponentInParent<CanvasGroup>();
                trigger.Init(this);
                Action.performed += InputPerformed;
                Action.canceled += InputCanceled;
				if (Setting != null)
				{
					Setting.Action = Action;
				}
			}
        }
        private void Awake()
        {
			if (Action == null)
            {
				Action = defaultAction?.action;
			}
        }
		private void Start()
		{
			if (Selectable == null)
			{
				Debug.LogError(transform.GetPath() + " 缺少可点击组件");
			}
		}
		public void ClearAction()
        {
            if (Action != null)
            {
                Action.performed -= InputPerformed;
                Action.canceled -= InputCanceled;
                if (Setting != null)
                {
					Setting.Action = null;
                }
            }
        }
		private void Update()
		{
			if (longTouchSwitch&&press&&!longTouchTimer.IsOver)
			{
				if(longTouchTimer.Check(Time.unscaledDeltaTime, false))
				{
					Click();
				}
			}
		}
		private void OnDestroy()
        {
            ClearAction();
        }
    }

#region UI事件处理工具

	class UIEventTrigger
	{
		public ClickList click = new ClickList();
		public DonwList donw = new DonwList();
		public UpList up = new UpList();
		public EnterList enter = new EnterList();
		public ExitList exit = new ExitList();

		internal void Init(MonoBehaviour baseMono)
		{
			var monos = new List<MonoBehaviour>(baseMono.GetComponents<MonoBehaviour>());
			monos.Remove(baseMono);
			click.Init(monos);
			donw.Init(monos);
			up.Init(monos);
			enter.Init(monos);
			exit.Init(monos);
		}
	}
	abstract class TempHandlerList<T> where T : class
	{
		protected abstract void Action(T handler);
		public void Init(List<MonoBehaviour> monos)
		{
			HandlerList = new List<T>();
			foreach (var mono in monos)
			{
				if (mono is T)
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

#endregion
}
#endif