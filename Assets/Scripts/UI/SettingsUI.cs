using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private GameObject CreditsMenu;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Settings _settings;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _profileMenu;
    [SerializeField] private Button _backButton;
    // Start is called before the first frame update
    void Start()
    {
        _soundSlider.onValueChanged.AddListener(ChangeVolume);
        _backButton.onClick.AddListener(Back);
        _soundSlider.value = _settings.GetVolume();
    }
    private void Back()
    {
        _settingsMenu.SetActive(false);
        _profileMenu.SetActive(true);
        CreditsMenu.SetActive(false);
    }
    private void ChangeVolume(float volume)
    {
        _settings.ChangeVolume(volume);
    }
    public void UpdateVolumeSlider()
    {
        _soundSlider.value = _settings.GetVolume();
    }

    // Update is called once per frame
    private void OnDisable()
    {
        SaveSystem.SaveSettings(_settings);
    }
    public void LoadSettings()
    {
        SaveSystem.LoadSettings(_settings);
    }
}

