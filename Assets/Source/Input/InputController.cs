using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputController : MonoBehaviour
    {
        public event Action<Vector2> OnAim;
        public event Action<bool> OnShoot;
        public event Action OnPause;
        
        [Header("References")]
        [SerializeField] private InputConfig _config;
        
        private InputActionMap _defaultInputMap;
        private InputAction _aimAction;
        private InputAction _shootAction;
        private InputAction _pauseAction;
        
        private void Awake()
        {
            _config.InputActionAsset.Enable();
            InitializeActions();
            SubscribeToEvents();
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void InitializeActions()
        {
            _defaultInputMap = _config.InputActionAsset?.FindActionMap(_config.InputActionMapName);
            if (_defaultInputMap == null)
                throw new InvalidOperationException($"Action map '{_config.InputActionMapName}' not found.");
            
            _aimAction = _defaultInputMap.FindAction(_config.AimActionName);
            if (_aimAction == null)
                throw new InvalidOperationException($"Aim action '{_config.AimActionName}' not found.");
            
            _shootAction = _defaultInputMap.FindAction(_config.ShootActionName);
            if (_shootAction == null)
                throw new InvalidOperationException($"Shoot action '{_config.ShootActionName}' not found.");
            
            _pauseAction = _defaultInputMap.FindAction(_config.PauseActionName);
            if (_pauseAction == null)
                throw new InvalidOperationException($"Pause action '{_config.PauseActionName}' not found.");
        }
        
        private void SubscribeToEvents()
        {
            _aimAction.performed += AimActionPerformedHandler;
            
            _shootAction.performed += ShootActionPerformedHandler;
            _shootAction.canceled += ShootActionCanceledHandler;
            
            _pauseAction.performed += PauseActionPerformedHandler;
        }
        
        private void UnsubscribeFromEvents()
        {
            _aimAction.performed -= AimActionPerformedHandler;
            
            _shootAction.performed -= ShootActionPerformedHandler;
            _shootAction.canceled -= ShootActionCanceledHandler;
            
            _pauseAction.performed -= PauseActionPerformedHandler;
            
            _config.InputActionAsset.Disable();
        }

        public void DefaultMapLock()
        {
            _defaultInputMap.Disable();
        }
        
        public void DefaultMapUnlock()
        {
            _defaultInputMap.Enable();
        }
        
        private void AimActionPerformedHandler(InputAction.CallbackContext context)
        {
            OnAim?.Invoke(context.ReadValue<Vector2>());
        }
        
        private void ShootActionPerformedHandler(InputAction.CallbackContext context)
        {
            OnShoot?.Invoke(true);
        }
        private void ShootActionCanceledHandler(InputAction.CallbackContext context)
        {
            OnShoot?.Invoke(false);
        }
        
        private void PauseActionPerformedHandler(InputAction.CallbackContext context)
        {
            OnPause?.Invoke();
        }
    }
}
