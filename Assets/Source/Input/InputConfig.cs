using Attributes.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    [CreateAssetMenu(fileName = "InputConfig", menuName = "Data/Input/InputConfig", order = 0)]
    public class InputConfig : ScriptableObject
    {
        [Header("Input Action Asset")]
        [SerializeField] private InputActionAsset _inputActionAsset;
        
        [Header("Input Action Map")]
        [SerializeField, ActionMapDropdown] private string _inputActionMapName;
        
        [Header("Input Actions")]
        [SerializeField, ActionInputDropdown] private string _aimActionName;
        [SerializeField, ActionInputDropdown] private string _shootActionName;
        [SerializeField, ActionInputDropdown] private string _pauseActionName;
        
        public InputActionAsset InputActionAsset => _inputActionAsset;
        
        public string InputActionMapName => _inputActionMapName;
        
        public string AimActionName => _aimActionName;
        public string ShootActionName => _shootActionName;
        public string PauseActionName => _pauseActionName;
    }
}