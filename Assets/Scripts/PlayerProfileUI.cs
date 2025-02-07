using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerProfileUI : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private PlayerProfile playerProfile;
    
    // Start is called before the first frame update
    void Start()
    {
        _inputField.text = playerProfile.PlayerName;
        _inputField.onEndEdit.AddListener(ChangeName);
        _nextButton.onClick.AddListener(Next);
        _backButton.onClick.AddListener(Back);
    }
    private void ChangeName(string name)
    {
        Debug.Log("Name  changed to " + name);
        playerProfile.ChangeName(name);
    }
    private void Next()
    {
        SceneLoader.Load(SceneLoader.Scene.LevelSelectScene);
    }
    private void Back()
    {
        SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
    }

    // Update is called once per frame
    private void OnDisable()
    {
        SaveSystem.SavePlayerName(playerProfile);
    }
}
