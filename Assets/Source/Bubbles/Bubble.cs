using System;
using UnityEngine;

namespace Bubbles
{
    public class Bubble : MonoBehaviour
    {
        public event Action<Bubble, Bubble> OnLanded;
        
        [SerializeField] private MeshRenderer _visualRenderer;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        
        public Rigidbody Rigidbody => _rigidbody;
        public Collider Collider => _collider;

        public BubbleColor Color { get; private set; }
        public int RowIndex { get; private set; }
        public bool IsAttachedToGrid { get; private set; }
        
        public void Initialize(BubbleColor color, BubbleMaterialProvider materialProvider, int rowIndex, bool isAttachedToGrid)
        {
            Color = color;
            _visualRenderer.material = materialProvider.GetMaterial(color);
            RowIndex = rowIndex;
            IsAttachedToGrid = isAttachedToGrid;
        }
        
        public void Pop()
        {
            Destroy(gameObject);
        }
        
        public void Fix(int rowIndex)
        {
            RowIndex = rowIndex;
            IsAttachedToGrid = true;
            Rigidbody.isKinematic = true;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Bubble")) return;
            var otherBubble = collision.gameObject.GetComponent<Bubble>();
            if (otherBubble == null) return;
            if(!otherBubble.IsAttachedToGrid) return;
            
            OnLanded?.Invoke(this, otherBubble);
            OnLanded = null;
        }
    }
}