#if InputSystem
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTool.InputSystem;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace QTool
{

    public class QInputTest : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
    {
        public InputActionReference input;
        public Text text;
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.LogError(nameof(OnPointerClick));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.LogError(nameof(OnPointerDown));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.LogError(nameof(OnPointerEnter));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.LogError(nameof(OnPointerExit));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.LogError(nameof(OnPointerUp));
        }

        // Start is called before the first frame update
        void Start()
        {
            Debug.LogError("InputTest");
            input.action.Enable();
            input.action.started += (info) =>
            {
                text.text=("started " + info.ReadValue<Vector2>());
            };
            input.action.performed += (info) =>
            {
                text.text = ("performed "+info.ReadValue<Vector2>());
            };
            input.action.canceled += (info) =>
            {
                text.text = ("canceled " + info.ReadValue<Vector2>());
            }; input.action.Disable();
        }

       
    }
}
#endif