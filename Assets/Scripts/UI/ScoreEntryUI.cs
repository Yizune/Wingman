using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rankText;
    [SerializeField] private TextMeshProUGUI _playerNameText;
    [SerializeField] private TextMeshProUGUI _timeTakenText;
    [SerializeField] private Image _backgroundImage;
    
    public void Initialize(int rank, string playerName, float timeTaken) {
        _rankText.text = rank.ToString();
        _playerNameText.text = playerName;
        
        
        int minutes = Mathf.FloorToInt(timeTaken / 60);
        int seconds = Mathf.FloorToInt(timeTaken % 60);
        int milliseconds = Mathf.FloorToInt((timeTaken * 1000) % 1000);
        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds / 10);
        _timeTakenText.text = formattedTime;
    }
    
    public void SetBackgroundColour(Color color) {
        _backgroundImage.color = color;
    }
}
