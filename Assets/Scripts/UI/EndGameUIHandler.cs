using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUIHandler : MonoBehaviour
{
    [Header("Titles")]
    [SerializeField] private TextMeshProUGUI _titleText;

    [Header("Result Text Settings")]
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private string _winText = "Level Complete!";
    [SerializeField] private string _timeFailText = "You're out of time!";
    [SerializeField] private string _crashTooMuchText = "You're a bad pilot, period.";

    [Header("Button Settings")]
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _levelSelectButton;

    [Header("Leaderboard Settings")]
    [SerializeField] private LeaderBoardUI _leaderBoardUI;
    [SerializeField] private LevelDataSO _levelDataSO;

    private AudioManager2 _audioManager;

    private void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager2>(); // may not work becuase Visual Studio is not working with me 
        _retryButton.onClick.AddListener(RetryGame);
        _mainMenuButton.onClick.AddListener(MainMenu);
        _levelSelectButton.onClick.AddListener(LevelSelect);
    }

    private void Start()
    {
        if (ResultsHandler.Instance == null)
        {
            Debug.LogError("EndGameUIHandler: ResultsHandler.Instance is null!");
            _resultText.text = "You're not loading this scene from game scene";

            if (_levelDataSO != null)
            {
                _leaderBoardUI.Initialize(_levelDataSO);
            }

            return;
        }

        _levelDataSO = ResultsHandler.Instance.LevelData;

        if (_levelDataSO != null)
        {
            _leaderBoardUI.Initialize(_levelDataSO);
        }
        else
            Debug.LogError("EndGameUIHandler: _levelDataSO is null!");

        EndGameChoice(ResultsHandler.Instance.ResultValue);
        _titleText.text = _levelDataSO.LevelName;
    }

    public void EndGameChoice(ResultsHandler.Result result)
    {
        _retryButton.gameObject.SetActive(true);
        _mainMenuButton.gameObject.SetActive(true);

        switch (result)
        {
            case ResultsHandler.Result.Win:
                _levelSelectButton.gameObject.SetActive(true);
                _resultText.text = _winText;
                _audioManager.PlayWinMusic();
                break;

            case ResultsHandler.Result.TimeFail:
                _levelSelectButton.gameObject.SetActive(false);
                _resultText.text = _timeFailText;
                _audioManager.PlayLoseMusic();
                break;

            case ResultsHandler.Result.CrashTooMuch:
                _levelSelectButton.gameObject.SetActive(false);
                _resultText.text = _crashTooMuchText;
                _audioManager.PlayLoseMusic();
                break;
        }
    }

    private void RetryGame()
    {
        Debug.Log("RetryGame");
        SceneLoader.Load(SceneLoader.Scene.Beta_GameScene);
    }

    private void MainMenu()
    {
        SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
    }

    private void LevelSelect()
    {
        SceneLoader.Load(SceneLoader.Scene.LevelSelectScene);
        // SceneLoader.Load(SceneLoader.Scene.LevelSelectScene);
    }
}