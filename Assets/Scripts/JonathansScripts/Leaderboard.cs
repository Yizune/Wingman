using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public static class Leaderboard
{
    private const string FILE_NAME = "Leaderboard";
    private static string _path = Path.Combine(Application.persistentDataPath, "Data", "Leaderboard", FILE_NAME + ".json");
    

    public static LevelLeaderboard GetLevelLeaderboard(LevelDataSO levelDataSO) {
        AllLeaderboard allLeaderboard = GetAllLeaderboard();
        if (allLeaderboard == null)
            return null;
        
        LevelLeaderboard levelLeaderboard = allLeaderboard.levelLeaderboardList.Find(x => x.ID == levelDataSO.ID);
        return levelLeaderboard;
    }
    
    public static bool AddScoreEntry(LevelDataSO levelDataSO, string playerName, float timeTaken, out int ranking)
    {
        ranking = -1;
        
        // load saved leaderboard
        AllLeaderboard allLeaderboard = GetAllLeaderboard();
        
        if (allLeaderboard == null)
            allLeaderboard = new AllLeaderboard { levelLeaderboardList = new List<LevelLeaderboard>() };
        
        // find the level leaderboard
        LevelLeaderboard levelLeaderboard = allLeaderboard.levelLeaderboardList.Find(x => x.ID == levelDataSO.ID);
        if (levelLeaderboard == null)
        {
            // create new leaderboard
            levelLeaderboard = new LevelLeaderboard { ID = levelDataSO.ID, 
                                                      LevelName = levelDataSO.LevelName, 
                                                      ScoreEntryList = new List<ScoreEntry>() };
            allLeaderboard.levelLeaderboardList.Add(levelLeaderboard);
        }
        
        
        // HANDLES INSERTING NEW SCORE ENTRY
        {
            if (levelLeaderboard.ScoreEntryList == null)
                levelLeaderboard.ScoreEntryList = new List<ScoreEntry>();
            
            // find and remove the old score entry
            for (int i = 0; i < levelLeaderboard.ScoreEntryList.Count; i++)
            {
                if (levelLeaderboard.ScoreEntryList[i].PlayerName != playerName)
                    continue;
                    
                if (timeTaken > levelLeaderboard.ScoreEntryList[i].TimeTaken) {
                    return false;
                }
                    
                levelLeaderboard.ScoreEntryList.RemoveAt(i);
                break;
            }
            
            
            ScoreEntry newScoreEntry = new ScoreEntry { TimeTaken = timeTaken, PlayerName = playerName };
            InsertHighScoreEntry(newScoreEntry, levelLeaderboard.ScoreEntryList, out ranking);
            
        }
        
        
        // save updated highscores
        SaveLeaderboard(allLeaderboard);

        return true;
    }
    
    private static void InsertHighScoreEntry(ScoreEntry newScoreEntry, List<ScoreEntry> scoreEntryList, out int ranking) {
        int insertIndex = 0;

        // find the correct position for the new entry
        for (int i = 0; i < scoreEntryList.Count; i++)
        {
            if (newScoreEntry.TimeTaken > scoreEntryList[i].TimeTaken)
            {
                insertIndex = i+1;
            }
            else 
                break;
        }
        ranking = insertIndex + 1;
        
        scoreEntryList.Insert(insertIndex, newScoreEntry);
        GameRecorder.instance.SaveRecording();
    }
    
    public static AllLeaderboard GetAllLeaderboard()
    {
        string path = Path.Combine(Application.persistentDataPath, "Data", "Leaderboard", FILE_NAME + ".json");
        if (!File.Exists(_path))
            return null;
        
        string json = File.ReadAllText(_path);
        AllLeaderboard allLeaderboard = JsonUtility.FromJson<AllLeaderboard>(json);
        return allLeaderboard;
    }
    
    private static void SaveLeaderboard(AllLeaderboard allLeaderboard)
    {
        string json = JsonUtility.ToJson(allLeaderboard, true);
        
        // if directory doesn't exist, create it
        string directory = Path.GetDirectoryName(_path);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        
        File.WriteAllText(_path, json);
    }

}


[Serializable]
public class AllLeaderboard
{
    public List<LevelLeaderboard> levelLeaderboardList;
}

[Serializable]
public class LevelLeaderboard {
    public string ID;
    public string LevelName;
    public List<ScoreEntry> ScoreEntryList;
    
    
}

[System.Serializable]
public class ScoreEntry
{
    public float TimeTaken;
    public string PlayerName;
}
