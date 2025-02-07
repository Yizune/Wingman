using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUIHandler : MonoBehaviour
{
    [SerializeField] private LeaderBoardUI _leaderBoardUI;
    [SerializeField] private TextMeshProUGUI _levelNameText;
    
    [SerializeField] private Button _backButton;
    
    private LevelDataSO _levelDataSO;

    private void Awake() {
        _backButton.onClick.AddListener(BackToLevelSelect);
    }

    private void Start() {
        
        _levelDataSO = LevelSelect.Instance.LevelToLoad;
        
        if (_levelDataSO == null) {
            Debug.LogError("LeaderboardUIHandler: _levelDataSO is null!");
            return;
        }
        _levelNameText.text = _levelDataSO.LevelName;
        _leaderBoardUI.Initialize(_levelDataSO);
    }
    
    private void BackToLevelSelect() {
        SceneLoader.Load(SceneLoader.Scene.LevelSelectScene);
    }
}
