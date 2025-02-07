using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class LeaderBoardUI : MonoBehaviour
{
    
    [SerializeField] private Transform _entryContainer;
    [SerializeField] private ScoreEntryUI _scoreEntryUIPrefab;
    PlaybackManager playbackManager;
    LevelSelect levelSelect;
    private void Start()
    {
        playbackManager = PlaybackManager.instance;
        levelSelect = LevelSelect.Instance;
    }

    public void Initialize(LevelDataSO levelDataSO) {
        LevelLeaderboard levelLeaderboard = Leaderboard.GetLevelLeaderboard(levelDataSO);
        if (levelLeaderboard == null)
            return;
        string levelName = levelSelect.LevelToLoad.name;
        List<ScoreEntry> scoreEntryList = levelLeaderboard.ScoreEntryList;
        if (scoreEntryList == null)
            return;
        
        foreach (var scoreEntry in scoreEntryList) {
            ScoreEntryUI scoreEntryUI = Instantiate(_scoreEntryUIPrefab, _entryContainer);
            scoreEntryUI.Initialize(scoreEntryList.IndexOf(scoreEntry) + 1, scoreEntry.PlayerName, scoreEntry.TimeTaken);
            Button challengeButton = scoreEntryUI.GetComponentInChildren<Button>();
            if (challengeButton == null) { Debug.Log("ChallengeButton was NULL"); }
            if (playbackManager == null) { Debug.Log("playbackManager was NULL"); }
            if (RecordingAvailable(levelName, scoreEntry.PlayerName))
            {
                challengeButton.onClick.AddListener(() => LoadChallenge(levelName, scoreEntry.PlayerName, levelDataSO));
            }
            else { challengeButton.gameObject.SetActive(false); }
            
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(_entryContainer.GetComponent<RectTransform>());
    }
    private void LoadChallenge(string levelName, string playerName, LevelDataSO levelToLoad) 
    {
        playbackManager.Initialize(levelName, playerName);
        levelSelect.LoadLevel(levelToLoad);
    }
    private bool RecordingAvailable(string levelName, string playerName)
    {
        string pathName = Path.Combine(Application.persistentDataPath, levelName);
        if (pathName == null)
        {
            return false;
        }
        string fileName = playerName + ".dat";
        string pathAndFile = Path.Combine(pathName, fileName);
        if (!File.Exists(pathAndFile))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    
    
    /*
    private void CreateHighScoreEntryTransform(ScoreEntry scoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 20f;
        Transform entryTransform = Instantiate(_entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);
        
        int rank = transformList.Count + 1;
        string rankString;
        // choose the correct suffix for the rank
        switch (rank)
        {
            default: rankString = rank + "TH"; break;
            case 1:  rankString = "1ST"; break;
            case 2:  rankString = "2ND"; break;
            case 3:  rankString = "3RD"; break;
                    
        }
        entryTransform.Find("RankText").GetComponent<TextMeshProUGUI>().text = rank.ToString();
            
        // score placeholder
        int score = scoreEntry.TimeTaken;
        entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();
            
        // name placeholder
        string name = scoreEntry.PlayerName;
        entryTransform.Find("NameText").GetComponent<TextMeshProUGUI>().text = name;
        if (rank == 1)
        {
            //highlights firstplace
            
            entryTransform.Find("RankText").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("NameText").GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        transformList.Add(entryTransform);
    }*/

   
}

