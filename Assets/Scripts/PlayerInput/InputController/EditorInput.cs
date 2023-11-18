using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerInput.InputController
{
    public class EditorInput : MonoBehaviour
    {
        private const float DragDelta = 0.01f;

        private Camera _camera;
        private Vector2 _lastInputPosition;

        public Vector2 InputWorldPosition => _camera.ScreenToWorldPoint(Input.mousePosition);
        public Vector2 InputScreenPosition => Input.mousePosition;
        
        public bool Enabled { get; set; }

        public event Action<Vector2> Clicked;
        public event Action<Vector2> ClickReleased;
        public event Action<Vector2> Dragged;

        public void Initialize() => _camera = Camera.main;

        private void Update()
        {
            if (!Enabled)
                return;

            if (Input.GetButtonDown("Fire1"))
            {
                Clicked?.Invoke(InputWorldPosition);
                _lastInputPosition = InputWorldPosition;
                return;
            }

            if (Input.GetButton("Fire1"))
            {
                if (Vector2.Distance(_lastInputPosition, InputWorldPosition) < DragDelta)
                    return;

                _lastInputPosition = InputWorldPosition;
                Dragged?.Invoke(InputWorldPosition);
                return;
            }

            if (Input.GetButtonUp("Fire1"))
                ClickReleased?.Invoke(InputWorldPosition);
        }

        public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
    }
}