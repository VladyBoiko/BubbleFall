using System;
using GlobalSource;
using Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bubbles.Spawn
{
    public class BubbleGridManager : MonoBehaviour
    {
        public event Action OnGameOver;
        
        [Header("References")]
        [SerializeField] private InputController _inputController;
        [SerializeField] private BubbleRowSpawner _rowSpawner;
        [SerializeField] private PatternManager _patternManager;
        [SerializeField] private MatchManager _matchManager;
        [SerializeField] private Transform _gridRoot;

        [Header("Settings")]
        [SerializeField] private float _dropSpeedMin = 0.3f;
        [SerializeField] private float _dropSpeedMax = 1.0f;
        [SerializeField] private float _timeToReachMaxSpeed = 300f;
        
        [Header("Trigger Detectors")]
        [SerializeField] private TriggerDetector _spawnTriggerDetector;
        [SerializeField] private TriggerDetector _gameOverTriggerDetector;
        
        private float _timeSinceStart;
        
        private int _lastSpawnTriggerRowIndex = -1;
        private int _lastGameOverTriggerRowIndex = -1;
        
        private void Start()
        {
            _patternManager.InitPatternEntries();
            _patternManager.EnqueueInitialPatterns();
            _patternManager.SpawnNextRow();

            _spawnTriggerDetector.OnTriggerExited += SpawnTriggerExitedHandler;
            _gameOverTriggerDetector.OnTriggerExited += GameOverTriggerExitedHandler;
        }

        private void Update()
        {
            _timeSinceStart += Time.deltaTime;
            MoveGridDown();
        }

        private void OnDestroy()
        {
            _spawnTriggerDetector.OnTriggerExited -= SpawnTriggerExitedHandler;
            _gameOverTriggerDetector.OnTriggerExited -= GameOverTriggerExitedHandler;
        }

        private void MoveGridDown()
        {
            float t = Mathf.Clamp01(_timeSinceStart / _timeToReachMaxSpeed);
            float speed = Mathf.Lerp(_dropSpeedMin, _dropSpeedMax, t);
            
            _gridRoot.position += Vector3.down * (speed * Time.deltaTime);
        }

        private void SpawnTriggerExitedHandler(Collider collider)
        {
            if (!collider.gameObject.CompareTag("Bubble")) return;

            var bubble = collider.GetComponent<Bubble>();
            if (bubble == null) return;

            if (bubble.RowIndex == _lastSpawnTriggerRowIndex || bubble.RowIndex == -1)
                return;

            _lastSpawnTriggerRowIndex = bubble.RowIndex;
            _patternManager.SpawnNextRow();
        }

        private void GameOverTriggerExitedHandler(Collider collider)
        {
            if (!collider.gameObject.CompareTag("Bubble")) return;

            var bubble = collider.GetComponent<Bubble>();
            if (bubble == null) return;

            if (bubble.RowIndex == _lastGameOverTriggerRowIndex || bubble.RowIndex == -1)
                return;

            _lastGameOverTriggerRowIndex = bubble.RowIndex;
            
            OnGameOver?.Invoke();
        }
        
        public void AttachBubbleToGrid(Bubble bubble)
        {
            bubble.Fix(_rowSpawner.CurrentRowIndex);
            bubble.transform.SetParent(_gridRoot);
            _matchManager.RegisterBubble(bubble);
            
            _matchManager.CheckForMatches(bubble);
        }
    }
}