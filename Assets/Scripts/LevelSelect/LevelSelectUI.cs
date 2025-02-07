using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour {
    [SerializeField] private List<LevelDataSO> _levelDataList;
    [SerializeField] public AudioManager2 audioManager;
    [SerializeField] private Transform _levelButtonPrefab;
    [SerializeField] private Transform _levelButtonsContainer;
    [SerializeField] private GameObject _tutorialCanvas;
    [SerializeField] private GameObject[] _tutorialImages;
    PlaybackManager playbackManager;
    int tutorialIndex = 0;

    private void Start() {
        foreach (LevelDataSO levelData in _levelDataList) {
            if (levelData == null)
                continue;
            
            Transform newLevelButton = Instantiate(_levelButtonPrefab, _levelButtonsContainer);

            Button loadLevelButton = newLevelButton.GetChild(0).GetComponent<Button>();
            loadLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = levelData.LevelName;
            loadLevelButton.onClick.AddListener(() => LevelClick(levelData));
            
            Button loadLeaderboardButton = newLevelButton.GetChild(1).GetComponent<Button>();
            loadLeaderboardButton.onClick.AddListener(() => LoadLeaderboard(levelData));
        }
        playbackManager = PlaybackManager.instance;
        playbackManager.Clear();
    }
    
    private void LevelClick(LevelDataSO levelData)
    {
        audioManager.PlaySelectSFX();
        LevelSelect.Instance.LoadLevel(levelData);
    }
    
    private void LoadLeaderboard(LevelDataSO levelData)
    {
        audioManager.PlaySelectSFX();
        LevelSelect.Instance.LoadLeaderboard(levelData);
    }
    public void ShowTutorial()
    {
        _tutorialCanvas.SetActive(true);
        tutorialIndex = 0;
        _tutorialImages[tutorialIndex].SetActive(true);
    }
    public void ShowNext()
    {
        if (tutorialIndex < _tutorialImages.Length - 1) 
        {
            tutorialIndex++;
            _tutorialImages[tutorialIndex].SetActive(true);
            _tutorialImages[tutorialIndex - 1].SetActive(false);
        }
        else { CloseTutorial(); }
    }
    public void CloseTutorial()
    {
        _tutorialImages[tutorialIndex].SetActive(false);
        _tutorialCanvas.SetActive(false);
        tutorialIndex = 0;
    }
}
