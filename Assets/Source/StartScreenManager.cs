using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _pressAnyKeyText;
    [SerializeField] private TextMeshProUGUI _lastScoreText;
    [SerializeField] private float _blinkSpeed = 1f;

    private IDisposable _mEventListener;

    private void Awake()
    {
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        _lastScoreText.text = lastScore.ToString();
    }

    private void OnEnable()
    {
        _mEventListener = InputSystem.onAnyButtonPress.Call(LoadGameplayScene);
    }
    
    private void Start()
    {
        StartCoroutine(BlinkText());
    }

    private void OnDisable()
    {
        _mEventListener?.Dispose();
    }
    
    private IEnumerator BlinkText()
    {
        float alpha = 1f;
        bool fadingOut = true;

        while (true)
        {
            alpha += (fadingOut ? -1 : 1) * Time.deltaTime * _blinkSpeed;
            alpha = Mathf.Clamp01(alpha);

            Color color = _pressAnyKeyText.color;
            color.a = alpha;
            _pressAnyKeyText.color = color;

            if (alpha <= 0f)
                fadingOut = false;
            else if (alpha >= 1f)
                fadingOut = true;

            yield return null;
        }
    }

    private void LoadGameplayScene(InputControl control)
    {
        SceneManager.LoadScene("GameplayScene");
    }
}