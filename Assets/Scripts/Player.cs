using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(PlaneMovement))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CheckpointManager))]
[RequireComponent(typeof(PlaneCollision))]
public class Player : MonoBehaviour
{
    public event EventHandler OnPlayerCrashed;
    // public event EventHandler OnPlayerCompletedLap;
    public event EventHandler OnPlayerFinishedRace;
    
    // listen to checkpoint manager
    // [SerializeField] private CheckpointManager _checkpointManager;
    [SerializeField] private PlaneMovement _planeMovement;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private CheckpointManager _checkpointManager;
    [SerializeField] private PlaneCollision _planeCollision;

    Timer _timer;
    
    private Coroutine _resetToLastCheckpointRoutine;

    private void Start() {
        // set player orientation to start checkpoint
        
        _planeMovement.DisableMovement();
        _playerInput.DisableInput();
        
        GameManager.Instance.OnGameStart += GameManager_OnGameStart;
        GameManager.Instance.OnGameEnd += GameManager_OnGameEnd;
        GameManager.Instance.OnGameTogglePause += GameManager_OnGameTogglePause;
        // listen to collision
        // listen to checkpoint manager on complete everything
        
        _checkpointManager.OnPlayerFinished += CheckpointManager_OnPlayerCheckedAllPoints;
        
        _planeCollision.OnPlaneCrashed += PlaneCollision_OnPlaneCrashed;
        _timer = FindObjectOfType<Timer>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            ResetToLastCheckpoint();
        }
    }

    private void CheckpointManager_OnPlayerCheckedAllPoints(object sender, EventArgs e) {
        // OnPlayerCompletedLap?.Invoke(this, EventArgs.Empty);
        // Debug.Log("Player completed lap");
        // _checkpointManager.ResetCheckpoints();
        OnPlayerFinishedRace?.Invoke(this, EventArgs.Empty);
    }
    
    private void PlaneCollision_OnPlaneCrashed(object sender, EventArgs e) {
        OnPlayerCrashed?.Invoke(this, EventArgs.Empty);
        ResetToLastCheckpoint();
    }

    private void GameManager_OnGameStart(object sender, EventArgs e) {
        _playerInput.EnableInput();
        _planeMovement.EnableMovement();
    }
    
    private void GameManager_OnGameEnd(object sender, EventArgs e) {
        
        // delay disabling input and movement
        
        _playerInput.DisableInput();
        _planeMovement.DisableMovement();
    }
    
    private void GameManager_OnGameTogglePause(object sender, GameManager.OnGameTogglePauseEventArgs e) {
        if (e.IsPaused) {
            _playerInput.DisableInput();
            _planeMovement.DisableMovement();
        } else {
            _playerInput.EnableInput();
            _planeMovement.EnableMovement();
        }
    }
    
    public void ResetToLastCheckpoint() {
        if (_resetToLastCheckpointRoutine != null) {
            StopCoroutine(_resetToLastCheckpointRoutine);
        }
        _timer.IncRespawnsSinceCheckpoint();
        _timer.SetTimerAfterRespawn();
        _resetToLastCheckpointRoutine = StartCoroutine(ResetToLastCheckpointRoutine());
    }
    
    private IEnumerator ResetToLastCheckpointRoutine() {
        Checkpoint lastCheckpoint = _checkpointManager.GetCurrentCheckpoint();
        if (lastCheckpoint == null) {
            Debug.LogError("No checkpoints found");
            
            _resetToLastCheckpointRoutine = null;
            yield break;
        }
        
        // reset player to last checkpoint
        _playerInput.DisableInput();
        _planeMovement.DisableMovement();
        
        _planeMovement.ResetPlane(lastCheckpoint.transform.position, lastCheckpoint.transform.forward);
        yield return new WaitForSeconds(1.5f);
        
        _playerInput.EnableInput();
        _planeMovement.EnableMovement();

        _resetToLastCheckpointRoutine = null;
    }

    public void ResetToStart(bool enableInputAfterReset = true) {
        Checkpoint startCheckpoint = _checkpointManager.GetStartCheckpoint();
        if (startCheckpoint == null) {
            Debug.LogError("No start checkpoint found");
            return;
        }
        
        // reset player to last checkpoint
        _playerInput.DisableInput();
        _planeMovement.DisableMovement();
        
        _planeMovement.ResetPlane(startCheckpoint.transform.position, startCheckpoint.transform.forward);
        if (_timer != null)
        {
            _timer.IncRespawnsSinceCheckpoint();
            _timer.SetTimerAfterRespawn();
        }
        if (!enableInputAfterReset) return;

        _playerInput.EnableInput();
        _planeMovement.EnableMovement();
    }
    
    public void SetupCheckpoints(List<Checkpoint> checkpointDataList, Checkpoint startCheckpointData, Checkpoint endCheckpointData) {
        _checkpointManager.SetupCheckpoints(checkpointDataList, startCheckpointData, endCheckpointData);
    }
   
    
}
