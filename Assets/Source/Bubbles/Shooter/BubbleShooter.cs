using UnityEngine;
using Bubbles.Spawn;

namespace Bubbles.Shooter
{
    public class BubbleShooter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BubblePool _bubblePool;
        [SerializeField] private BubbleGridManager _bubbleGridManager;
        [SerializeField] private AimController _aimController;
        [SerializeField] private BubbleMaterialProvider _bubbleMaterialProvider;
        [SerializeField] private Transform _shootPoint;
        
        [Header("Shooting Settings")]
        [SerializeField] private float _shootForce = 20f;
        
        public Transform ShootPoint => _shootPoint;
        
        private Bubble _currentBubble;

        public void PrepareNextBubble()
        {
            _currentBubble = _bubblePool.Get();
            _currentBubble.transform.position = _shootPoint.position;
            _currentBubble.transform.rotation = Quaternion.identity;

            _currentBubble.Collider.enabled = false;

            var randomColor = (BubbleColor)Random.Range(0, System.Enum.GetValues(typeof(BubbleColor)).Length);
            _currentBubble.Initialize(randomColor, _bubbleMaterialProvider, -1, false);

            _aimController.SetColor(_bubbleMaterialProvider.GetAimColor(randomColor));
        }

        public void Shoot(Vector3 direction)
        {
            if (_currentBubble == null)
                return;

            _currentBubble.Collider.enabled = true;
            _currentBubble.transform.position = _shootPoint.position;
            _currentBubble.Rigidbody.isKinematic = false;
            _currentBubble.Rigidbody.velocity = direction * _shootForce;

            _currentBubble.OnLanded += HandleBubbleLanded;
            
            PrepareNextBubble();
        }
        
        private void HandleBubbleLanded(Bubble bubble, Bubble otherBubble)
        {
            _bubbleGridManager.AttachBubbleToGrid(bubble);
            bubble.OnLanded -= HandleBubbleLanded;
        }
    }
}