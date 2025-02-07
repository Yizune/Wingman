using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event EventHandler<OnGameTogglePauseEventArgs> OnGameTogglePause;
    public class OnGameTogglePauseEventArgs : EventArgs {
        public bool IsPaused;
    }

    public event EventHandler OnGameStart;
    public event EventHandler OnGameEnd;

    [SerializeField] GameRecorder gameRecorder;
    
    // reference to player
    [SerializeField] private Player _player;
    
    // countdown timer
    [SerializeField] private float _countdownTime = 3f;
    private float _currentCountdownTime;
    
    // timer
    public Timer GameTimer => _gameTimer;
    public Timer _gameTimer;
    private GameTimerUI _gameTimerUI;
    private bool _isPaused;

    //crash count
    [SerializeField] private TextMeshProUGUI crashCountText;

    // map settings
    private float _timeToComplete = 45f;
    [SerializeField] public int _maxCrashCount = 5;
    private int _playerCrashCount;
    // private int _playerLapCount;
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        _gameTimer = FindObjectOfType<Timer>();
        _gameTimerUI = FindObjectOfType<GameTimerUI>();
        gameRecorder = GameRecorder.instance;
    }

    private void Start() {
        _player.OnPlayerCrashed += Player_OnPlayerCrashed;
        _player.OnPlayerFinishedRace += Player_OnPlayerFinishedRace;
        
        SetupLevel();
        UpdateCrashCountText();

        _player.ResetToStart(false);
        // start countdown
        _currentCountdownTime = _countdownTime;
        //StartCoroutine(Countdown());
    }

    private void SetupLevel() {
        LevelDataSO levelData = LevelSelect.Instance.LevelToLoad;
        
        // GameObject checkpointPrefab = levelData.CheckpointPrefab;
        
        // cleanup all checkpoints in the scene if any
        Checkpoint[] existingCheckpoints = FindObjectsOfType<Checkpoint>();
        foreach (Checkpoint checkpoint in existingCheckpoints) {
            Destroy(checkpoint.gameObject);
        }
        
        GameObject parent = new GameObject("Checkpoints");
        
        // instantiate the checkpoints
        Checkpoint startCheckpoint = CreateCheckpoint(levelData.StartCheckpointData);
        startCheckpoint.name = "Start Checkpoint";
        startCheckpoint.transform.SetParent(parent.transform, true);
        
        Checkpoint endCheckpoint = CreateCheckpoint(levelData.EndCheckpointData);
        endCheckpoint.name = "End Checkpoint";
        endCheckpoint.transform.SetParent(parent.transform, true);
        
        List<Checkpoint> checkpoints = new List<Checkpoint>();
        for (int i = 0; i < levelData.CheckpointDataList.Count; i++) {
            Checkpoint checkpoint = CreateCheckpoint(levelData.CheckpointDataList[i]);
            checkpoint.name = "Checkpoint " + i;
            
            checkpoint.transform.SetParent(parent.transform, true);
            checkpoints.Add(checkpoint);
        }

        _player.SetupCheckpoints(checkpoints, startCheckpoint, endCheckpoint);

        _timeToComplete = levelData.TimeToComplete;
        _gameTimerUI.SetEndTime(_timeToComplete);
        _maxCrashCount = levelData.MaxCrashes;
    }

    public float GetTimeToComplete()
    {
        return _timeToComplete;
    }
    
    private Checkpoint CreateCheckpoint(CheckpointData checkpointData) {
        if (checkpointData.CheckpointPrefab == null) {
            Debug.LogError("Checkpoint prefab not found");
            return null;
        }
        
        GameObject checkpointPrefab = checkpointData.CheckpointPrefab;
        
        Checkpoint checkpoint = Instantiate(checkpointPrefab, 
            checkpointData.Position,
            checkpointData.Rotation).GetComponent<Checkpoint>();
        checkpoint.transform.localScale = checkpointData.Scale;
        
        return checkpoint;
    }
    private void UpdateCrashCountText()
    {
        if (crashCountText != null)
        {
            crashCountText.text = $"{_playerCrashCount} / {_maxCrashCount}";
        }
    }

    private void Player_OnPlayerCrashed(object sender, EventArgs e) {
        _playerCrashCount++;
        UpdateCrashCountText();

        if (_playerCrashCount >= _maxCrashCount) {
            EndGame();
        }
    }
    
    private void Player_OnPlayerFinishedRace(object sender, EventArgs e) {
        EndGame();
    }
    
    private IEnumerator Countdown() {
        Debug.Log("Countdown started");

        while (_currentCountdownTime > 0) {
            if (!_isPaused) {
                _currentCountdownTime -= Time.deltaTime;
            }
            
            yield return null;
        }
        
        Debug.Log("Countdown ended");
        StartGame();
    }

    private void StartGame() {
        // enable player inputs
        // enable movement
        // trigger timer start
        
        _gameTimer.RestartTimer();
        Debug.Log("Starting game");
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }
    
    private void EndGame() {
        gameRecorder.StopRecording();
        Cursor.visible = true;
        _gameTimer.PauseTimer();
        OnGameEnd?.Invoke(this, EventArgs.Empty);
        ProcessResult();
     
        // load result scene
        SceneLoader.Load(SceneLoader.Scene.EndGameScene);
    }

    private void ProcessResult() {
        // if player crashed more than x times
        if (_playerCrashCount >= _maxCrashCount) {
            Debug.Log("Player crashed too many times");
            ResultsHandler.Instance.SetResult(ResultsHandler.Result.CrashTooMuch);
            ResultsHandler.Instance.LevelData = LevelSelect.Instance.LevelToLoad;
            return;
        }
        
        // if player did not finish within time limit
        float completionTime = _gameTimer.TimeElapsed;
        if (completionTime > _timeToComplete) {
            Debug.Log("Player failed to finish within time limit");
            ResultsHandler.Instance.SetResult(ResultsHandler.Result.TimeFail);
            ResultsHandler.Instance.LevelData = LevelSelect.Instance.LevelToLoad;
            return;
        }
        
        // if above conditions aren't met then the player succeeded
        
        Leaderboard.AddScoreEntry(LevelSelect.Instance.LevelToLoad, SaveSystem.GetPlayerName(), completionTime, out int ranking);
        
        ResultsHandler.Instance.SetResult(ResultsHandler.Result.Win);
        ResultsHandler.Instance.LevelData = LevelSelect.Instance.LevelToLoad;
        ResultsHandler.Instance.SetRanking(ranking);
        gameRecorder.SaveRecording();
        
        Debug.Log("Player succeeded");
    }
    
    
    public void SetPause(bool isPaused) {
        if (_isPaused == isPaused) return;
        
        _isPaused = isPaused;
        
        if (_isPaused) {
            _gameTimer.PauseTimer();
            Time.timeScale = 0;
        } else {
            _gameTimer.StartTimer();
            Time.timeScale = 1;
        }
        
        OnGameTogglePause?.Invoke(this, new OnGameTogglePauseEventArgs {IsPaused = _isPaused});
    }
}
