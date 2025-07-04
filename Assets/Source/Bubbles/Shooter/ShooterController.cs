using Input;
using UnityEngine;

namespace Bubbles.Shooter
{
    public class ShooterController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputController _input;
        [SerializeField] private AimController _aimController;
        [SerializeField] private BubbleShooter _bubbleShooter;

        private bool _isAiming = false;

        private void OnEnable()
        {
            _input.OnAim += HandleAim;
            _input.OnShoot += HandleShoot;
            _aimController.Initialize(_bubbleShooter.ShootPoint);
        }

        private void OnDisable()
        {
            _input.OnAim -= HandleAim;
            _input.OnShoot -= HandleShoot;
        }

        private void Start()
        {
            _bubbleShooter.PrepareNextBubble();
        }

        private void HandleAim(Vector2 screenPos)
        {
            if (!_isAiming) return;

            _aimController.SetAimScreenPosition(screenPos);
        }

        private void HandleShoot(bool isPressed)
        {
            _isAiming = isPressed;
            _aimController.EnableAimLine(isPressed);

            if (!isPressed)
            {
                Vector3 direction = _aimController.GetClampedShootDirection();
                _bubbleShooter.Shoot(direction);
            }
        }
    }
}