using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsHandler : MonoBehaviour
{
    public static ResultsHandler Instance;

    public enum Result
    {
       Win,
       TimeFail,
       CrashTooMuch
    }
    public Result ResultValue => _result;
    [SerializeField] private Result _result;

    public LevelDataSO LevelData;
    
    public int Ranking => _ranking;
    private int _ranking;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }
   
   public void SetResult(Result result)
   {
       _result = result;
   }
   
   public void SetRanking(int ranking)
   {
       _ranking = ranking;
   }
}
