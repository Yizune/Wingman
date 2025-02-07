using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : MonoBehaviour
{
    public readonly string DEFAULT_PLAYER_NAME = "Player";
    
    public string  PlayerName => _playerName;
    private string _playerName;
    // Start is called before the first frame update
    void Awake()
    {
        SaveSystem.LoadPlayerName(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string ChangeName(string name)
    {
        if (_playerName != name)
        {
            _playerName = name;
            SaveSystem.SavePlayerName(this);
            
        }
        return _playerName;
       
    }
}

