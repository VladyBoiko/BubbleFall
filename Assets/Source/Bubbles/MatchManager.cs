using System;
using System.Collections.Generic;
using Bubbles.Spawn;
using UnityEngine;

namespace Bubbles
{
    public class MatchManager : MonoBehaviour
    {
        public event Action<int> OnBubblesPoppedCountChanged; 
        
        [Header("References")]
        [SerializeField] private BubbleRowSpawner _rowSpawner;
        [SerializeField] private BubbleMaterialProvider _materialProvider;
        [SerializeField] private Transform _topCeiling;
        
        [Header("VFX")]
        [SerializeField] private GameObject _popEffectPrefab;
        
        [Header("Settings")]
        [SerializeField] private int _matchThreshold = 3;
        [SerializeField] private LayerMask _bubbleLayerMask;
        
        private float _matchCheckRadius;
        private int _bubblesPoppedCount;
        
        private readonly List<Bubble> _allBubbles = new();

        private void Start()
        {
            _matchCheckRadius = _rowSpawner.BubbleSpacing + 0.1f;
        }

        public void CheckForMatches(Bubble rootBubble)
        {
            var matched = new HashSet<Bubble>();
            
            FindMatchingNeighbors(rootBubble, matched);

            if (matched.Count >= _matchThreshold)
            {
                foreach (var bubble in matched)
                {
                    SpawnPopEffect(bubble.transform.position, _materialProvider.GetAimColor(bubble.Color));
                    _rowSpawner.BubblePool.Return(bubble);
                    UnregisterBubble(bubble);
                }
                
                CheckDisconnectedBubbles();
                
                OnBubblesPoppedCountChanged?.Invoke(_bubblesPoppedCount);
                _bubblesPoppedCount = 0;
            }
        }

        private void FindMatchingNeighbors(Bubble current, HashSet<Bubble> visited)
        {
            if (visited.Contains(current))
                return;

            visited.Add(current);

            Collider[] colliders = Physics.OverlapSphere(current.transform.position, _matchCheckRadius, _bubbleLayerMask);

            foreach (var col in colliders)
            {
                if (!col.CompareTag("Bubble")) continue;

                Bubble neighbor = col.GetComponent<Bubble>();
                if (neighbor == null || !neighbor.IsAttachedToGrid || neighbor.Color != current.Color)
                    continue;

                FindMatchingNeighbors(neighbor, visited);
            }
        }

        private void CheckDisconnectedBubbles()
        {
            var connected = new HashSet<Bubble>();
            
            foreach (var bubble in _allBubbles)
            {
                if (bubble == null || !bubble.IsAttachedToGrid)
                    continue;
                
                if (Mathf.Abs(bubble.transform.position.y - _topCeiling.position.y) < _matchCheckRadius)
                {
                    Debug.DrawLine(new Vector3(bubble.transform.position.x, bubble.transform.position.y, bubble.transform.position.z), 
                        new Vector3(bubble.transform.position.x, _topCeiling.position.y, bubble.transform.position.z), 
                        Color.blue, 
                        1f);
                    
                    FindConnectedBubbles(bubble, connected);
                }
            }
            
            List<Bubble> toRemove = new();

            foreach (var bubble in _allBubbles)
            {
                if (bubble == null || !bubble.IsAttachedToGrid)
                    continue;

                if (!connected.Contains(bubble))
                {
                    toRemove.Add(bubble);
                }
            }

            foreach (var bubble in toRemove)
            {
                SpawnPopEffect(bubble.transform.position, _materialProvider.GetAimColor(bubble.Color));
                _rowSpawner.BubblePool.Return(bubble);
                UnregisterBubble(bubble);
            }
        }

        private void FindConnectedBubbles(Bubble current, HashSet<Bubble> visited)
        {
            if (visited.Contains(current))
                return;

            visited.Add(current);

            Collider[] colliders = Physics.OverlapSphere(current.transform.position, _matchCheckRadius, _bubbleLayerMask);
            foreach (var col in colliders)
            {
                if (!col.CompareTag("Bubble")) continue;

                var neighbor = col.GetComponent<Bubble>();
                if (neighbor == null || !neighbor.IsAttachedToGrid)
                    continue;

                FindConnectedBubbles(neighbor, visited);
            }
        }
        
        public void RegisterBubble(Bubble bubble)
        {
            if (!_allBubbles.Contains(bubble))
                _allBubbles.Add(bubble);
        }

        public void UnregisterBubble(Bubble bubble)
        {
            _allBubbles.Remove(bubble);
            _bubblesPoppedCount++;
        }
        
        private void SpawnPopEffect(Vector3 position, Color color)
        {
            if (_popEffectPrefab == null) return;
            var effect = Instantiate(_popEffectPrefab, position, Quaternion.identity);
            var particle = effect.GetComponent<ParticleSystem>();

            if (particle != null)
            {
                var main = particle.main;
                main.startColor = color;
            }
                
            Destroy(effect, 1f);
        }
    }
}