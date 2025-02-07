using UnityEngine;
using TMPro;

public class GameTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;

    private Timer _gameTimer; // Reference to the TimerLogic instance

    private GameManager _gameManager;

    float _endTime = 45f;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        //_endTime = _gameManager.GetTimeToComplete();
        _gameTimer = GameManager.Instance.GameTimer;
        _gameTimer.OnTimeElapsedChanged += GameTimer_OnTimeElapsedChanged;
    }

    private void OnDestroy()
    {
        _gameTimer.OnTimeElapsedChanged -= GameTimer_OnTimeElapsedChanged;
    }

    private void HandleGameEnd(float _timeElapsed)
    {
        // Update the timer text when the game ends
        int minutes = Mathf.FloorToInt(_timeElapsed / 60);
        int seconds = Mathf.FloorToInt(_timeElapsed % 60);
        int milliseconds = Mathf.FloorToInt((_timeElapsed * 1000) % 1000);

        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds / 10);
        _timerText.text = formattedTime;
    }
    
    private void GameTimer_OnTimeElapsedChanged(object sender, Timer.OnTimeElapsedChangedEventArgs e)
    {
        // Update the timer text when the time changes
        int minutes = Mathf.FloorToInt(e.TimeElapsed / 60);
        int seconds = Mathf.FloorToInt(e.TimeElapsed % 60);
        int milliseconds = Mathf.FloorToInt((e.TimeElapsed * 1000) % 1000);
        if (minutes * 60 + seconds >= _endTime)
        {
            _timerText.color = Color.red;
        }
        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds / 10);
        _timerText.text = formattedTime;
    }
    public void SetEndTime(float endTime)
    {
        _endTime = endTime;
    }
}
