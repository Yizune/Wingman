using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public event EventHandler<OnTimeElapsedChangedEventArgs> OnTimeElapsedChanged;
    public class OnTimeElapsedChangedEventArgs : EventArgs {
        public float TimeElapsed;
    }
    
    public float TimeElapsed => _timeElapsed;
    private float _timeElapsed;
    private float _lastCheckpointTime = 0;
    private int _respawnsSinceCheckpoint = 0;
    [SerializeField] private int _penaltyForRespawn = 3;
    [SerializeField] private float _penaltyTextDisplayTime = 1f;
    [SerializeField] private GameObject _penaltyTimeObject;
    [SerializeField]private TMP_Text _penaltyText;
    private bool _isPenaltyDisplayed = false;
    private bool _isRunning = false;
    GameRecorder gameRecorder;
    PlayBackPlayer playbackPlayer;

    private void Start()
    {
        //_penaltyText = _penaltyTimeObject.GetComponent<TMP_Text>();
        //Debug.Assert(_penaltyText != null);
        gameRecorder = GameRecorder.instance;
        playbackPlayer = FindObjectOfType<PlayBackPlayer>();
    }
    private void Update() {
        if (!_isRunning)
            return;

        _timeElapsed += Time.deltaTime;
        OnTimeElapsedChanged?.Invoke(this, new OnTimeElapsedChangedEventArgs{TimeElapsed = _timeElapsed});
    }

    public void StartTimer()
    {
        _isRunning = true;
        gameRecorder.StartRecording();
        playbackPlayer.StartPlayback();
    }

    public void PauseTimer()
    {
        _isRunning = false;
        gameRecorder.StopRecording();
        playbackPlayer.StopPlayback();
    }
    
    public void ResetTimer()
    {
        _timeElapsed = 0f;
        OnTimeElapsedChanged?.Invoke(this, new OnTimeElapsedChangedEventArgs{TimeElapsed = _timeElapsed});
    }
    
    public void RestartTimer()
    {
        ResetTimer();
        StartTimer();
    }
    public void SetLastCheckPointTime(float timePlayerPassedCheckpoint)
    {
        _lastCheckpointTime = timePlayerPassedCheckpoint;
    }
    public void CheckpointPassed()
    {
        _respawnsSinceCheckpoint = 0;
        _lastCheckpointTime = _timeElapsed;
    }
    public void IncRespawnsSinceCheckpoint()
    {
        _respawnsSinceCheckpoint++;
    }
    public void EnablePenalty()
    {
         _isPenaltyDisplayed = true;
         _penaltyTimeObject.SetActive(true);
    }

    private void DisablePenalty()
    {
        _isPenaltyDisplayed = false;
        _penaltyTimeObject.SetActive(false);
    }

    private IEnumerator PenaltyTimer()
    {
        yield return new WaitForSeconds(1f);
        DisablePenalty();
    }

    public void SetTimerAfterRespawn()
    {
        if (_penaltyText == null)
        {
            Debug.LogError("Penalty Time Text reference is null!");
            return;
        }

        _timeElapsed = _lastCheckpointTime + _penaltyForRespawn * _respawnsSinceCheckpoint;
        _penaltyText.gameObject.SetActive(true);
        _isPenaltyDisplayed = true;
        _penaltyText.text = ("Penalty: +" + _penaltyForRespawn + " seconds") as string;

        StartCoroutine(PenaltyTimer());
    }
    public bool TimerIsRunning()
    {
        return _isRunning;
    }
}
