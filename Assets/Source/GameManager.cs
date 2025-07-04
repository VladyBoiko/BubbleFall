using Bubbles;
using Bubbles.Spawn;
using Input;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Source
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BubbleGridManager _bubbleGridManager;
        [SerializeField] private MatchManager _matchManager;
        [SerializeField] private InputController _inputController;
        
        [Header("UI")]
        [SerializeField] private Canvas _hudCanvas;
        [SerializeField] private Canvas _pauseMenuCanvas;
        
        [Header("Buttons")]
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _quitButton;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _bubblesCounterText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        
        [Header("Score Settings")]
        [SerializeField] private int _scoreMultiplier = 10;

        private int _bubblesCounter;
        private int _score;

        private void Awake()
        {
            _bubblesCounterText.text = "0";
            _scoreText.text = "0";
            
            _pauseButton.onClick.AddListener(PauseGame);
            _resumeButton.onClick.AddListener(ResumeGame);
            _quitButton.onClick.AddListener(QuitGame);
            
            _pauseMenuCanvas.enabled = false;
        }
        
        private void Start()
        {
            _bubbleGridManager.OnGameOver += GameOverHandler;
            _matchManager.OnBubblesPoppedCountChanged += BubblesPoppedCountChangedHandler;
        }
        
        private void OnDestroy()
        {
            _bubbleGridManager.OnGameOver -= GameOverHandler;
            _matchManager.OnBubblesPoppedCountChanged -= BubblesPoppedCountChangedHandler;
        }

        private void PauseGame()
        {
            Time.timeScale = 0f;

            _inputController.DefaultMapLock();
            
            _hudCanvas.enabled = false;
            _pauseMenuCanvas.enabled = true;
        }

        private void ResumeGame()
        {
            Time.timeScale = 1f;

            _inputController.DefaultMapUnlock();
            
            _pauseMenuCanvas.enabled = false;
            _hudCanvas.enabled = true;
        }
        
        private void QuitGame()
        {
            SaveScore();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        
        private void GameOverHandler()
        {
            SaveScore();
            
            SceneManager.LoadScene("MainMenuScene");
        }

        private void SaveScore()
        {
            PlayerPrefs.SetInt("LastScore", _score);
            PlayerPrefs.Save();
        }
        
        private void BubblesPoppedCountChangedHandler(int count)
        {
            _bubblesCounter += count;
            _score = _bubblesCounter * _scoreMultiplier;
            
            _bubblesCounterText.text = _bubblesCounter.ToString();
            _scoreText.text = _score.ToString();
        }
    }
}