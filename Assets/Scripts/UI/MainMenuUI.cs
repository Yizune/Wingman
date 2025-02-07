using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject Credits;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _profileMenu;
    //make a button reference to game scene
    [SerializeField] private Button _CreditsButton;
    [SerializeField] Button _playButton;
    [SerializeField] Button _settingsButton;
    [SerializeField] Button _profileButton;
    [SerializeField] TextMeshProUGUI _profileName;
    [SerializeField] private SettingsUI _settingsUI;
    // Start is called before the first frame update
    void Start()
    {
        _CreditsButton.onClick.AddListener(OpenCredits);
       _playButton.onClick.AddListener(PlayGame);
       _profileButton.onClick.AddListener(OpenProfile);
       _settingsButton.onClick.AddListener(OpenSettings);
       _profileName.text = SaveSystem.GetPlayerName();
    }
    private void OpenCredits()
    {
        Credits.SetActive(true);
        _profileMenu.SetActive(false);
    }
    private void OpenSettings()
    {
        _settingsMenu.SetActive(true);
        _profileMenu.SetActive(false);
        //_settingsUI.LoadSettings();
        _settingsUI.UpdateVolumeSlider();

    }
    private void PlayGame()
    {
        SceneLoader.Load(SceneLoader.Scene.LevelSelectScene);
    }
    private void OpenProfile()
    {
        SceneLoader.Load(SceneLoader.Scene.PlayerProfileScene);
    }
}
