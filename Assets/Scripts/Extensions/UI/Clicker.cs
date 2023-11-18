using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Extensions.UI
{
    public class Clicker : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
    {
        public event Action Clicked;
        public event Action DragStarted;

        private bool _selected;

        public bool Interactable { get; set; } = true;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Interactable)
                return;

            _selected = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!Interactable || !_selected)
                return;

            DragStarted?.Invoke();
            _selected = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Interactable || !_selected)
                return;

            Clicked?.Invoke();
            _selected = false;
        }
    }
}