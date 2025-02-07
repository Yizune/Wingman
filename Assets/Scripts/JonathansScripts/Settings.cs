using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Settings : MonoBehaviour
{

    public readonly float DEFAULT_SOUND_VOLUME = 0.1f;

    public float SoundVolume => _soundVolume;
    private float _soundVolume; // = 0.1f
    [SerializeField] public AudioManager2 _audioManager;
    // Start is called before the first frame update
    void Start()
    {
        SaveSystem.LoadSettings(this);


    }

    // Update is called once per frame
    void Update()
    {

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
        _audioManager.UpdateVolume(volume);
        return _soundVolume;
    }
    public float GetVolume()
    {
        return _audioManager.GetVolume();
    }
}

