using System;
using Zenject;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : IInputManager, IInitializable, IDisposable
    {
        private Camera _camera;

        private readonly GameplayInput _input = new();

        public void Initialize()
        {
            _input.Enable();
            _input.Gameplay.Tap.performed += OnTap;
        }

        public void Dispose()
        {
            _input.Gameplay.Tap.performed -= OnTap;
            _input.Disable();
        }

        private void OnTap(InputAction.CallbackContext _)
        {
            if (_camera == null) _camera = Camera.main;
            
            var screenPosition = _input.Gameplay.PointerPosition.ReadValue<Vector2>();

            var ray = _camera.ScreenPointToRay(screenPosition);

            if (!Physics.Raycast(ray, out var hit))
                return;

            if (hit.collider.TryGetComponent<ITappable>(out var tappable))
            {
                tappable.OnTap();
            }
        }
    }
}