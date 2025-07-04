using System.Collections.Generic;
using UnityEngine;

namespace Bubbles.Spawn
{
    public class BubblePool : MonoBehaviour
    {
        [Header("Bubble Pool Settings")]
        [SerializeField] private GameObject _bubblePrefab;
        [SerializeField] private Transform _bubblePlatform;
        [SerializeField] private int _initialSize;

        private Queue<Bubble> _pool;

        private void Awake()
        {
            float volume = _bubblePlatform.localScale.x * _bubblePlatform.localScale.y *
                           _bubblePlatform.localScale.z;
            _initialSize += Mathf.CeilToInt(volume);

            _pool = new Queue<Bubble>();

            for (int i = 0; i < _initialSize; i++)
            {
                Bubble bubble = CreateNewBubble();
                Return(bubble);
            }
        }

        public Bubble Get()
        {
            if (_pool.Count > 0)
            {
                var bubble = _pool.Dequeue();
                bubble.gameObject.SetActive(true);
                return bubble;
            }
            else
            {
                return CreateNewBubble();
            }
        }

        public void Return(Bubble bubble)
        {
            bubble.gameObject.SetActive(false);
            bubble.transform.SetParent(transform);
            bubble.transform.localPosition = Vector3.zero;
            _pool.Enqueue(bubble);
        }

        private Bubble CreateNewBubble()
        {
            var obj = Instantiate(_bubblePrefab, transform, true);
            var bubble = obj.GetComponent<Bubble>();
            return bubble;
        }
    }
}