using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAudio2 : MonoBehaviour
{
    public AudioManager2 audioManager2;
    private float _soundVolume;

    // UI Buttons
    public Button toggleMusicButton;
    public Button toggleSFXButton;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    void Start()
    {
        audioManager2 = GetComponent<AudioManager2>();
        // Add click listeners to the buttons
        /*toggleMusicButton.onClick.AddListener(ToggleMusic);
        toggleSFXButton.onClick.AddListener(ToggleSFX);
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);*/
    }

    /*void ToggleMusic()
    {
        audioManager2.ToggleMusic();
    }

    void ToggleSFX()
    {
        audioManager2.ToggleSFX();
    }

    void ChangeMusicVolume(float volume)
    {
        audioManager2.MusicVolume(volume);
        ChangeVolume(musicVolumeSlider.value);
    }

    void ChangeSFXVolume(float volume)
    {
        audioManager2.SFXVolume(volume);
        ChangeVolume(sfxVolumeSlider.value);
    }
    public float ChangeVolume(float volume)
    {
        if (volume > 1)
        {
            volume = 1;
        }
        else if (volume < 0)
        {
            volume = 0;
        }
        _soundVolume = volume;
        return _soundVolume;
    }*/
}
