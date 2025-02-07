using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;


public class AudioManager2 : MonoBehaviour
{

    [Header("Finish")]
    public bool _MuteFinishSFX = false;
    public AudioSource _FinishSFX;

    [Header("Ambiance")]
    public bool _MuteAmbiance = false;
    public AudioSource _Ambiance;

    [Header("Checkpoint")]

    public AudioSource _Checkpoint;
    [Header("Engine")]
    public bool _MuteEngine = false;
    public AudioSource _Engine;
    [Header("PlaneStart")]
    public bool _MutePlaneStart = false;
    public AudioSource _PlaneStart;

    [Header("Crash sound effect")]
    [SerializeField] AudioSource _CrashSFX;

    [Header("Race Music")]
    public bool _MuteRaceMusic = false;
    public bool _RandomizeMusic = true;
    public int _MusicIndex = 0;
    [SerializeField] public List<AudioSource> _RaceMusic;

    [Header("Main Menu Music")]
    public bool _MuteMenuMusic = false;
    static GameObject _MenuMusicStatic = null;
    public GameObject _MainMenuObj;
    private AudioSource _MenuMusicSorce;
    [Header("Menu Select")]
    public bool _MuteMenuSelect = false;
    static GameObject _MenuSelectStatic = null;
    public GameObject _MenuSelectObj;
    private AudioSource _MenuSelectSFX;

    [Header("Main Menu Music Handler")]
    public bool _StartMenuMusic = false;
    public bool _StopMenuMusic = false;

    [Header("Lose Music")]
    [SerializeField] AudioSource _LoseMusic;
    [Header("Win Music")]
    [SerializeField] AudioSource _WinMusic;

    public static float _Volume;

    private void Start()
    {
        // sorry for whoever reads this becuase its bad, but it works
        //saves dontdestroy on load to static from main menu scene
        if (_MainMenuObj != null)
        {
            if (_MenuMusicStatic == null)
            {
                DontDestroyOnLoad(_MainMenuObj);
                _MenuMusicStatic = _MainMenuObj;
            }
        }
        if (_MenuSelectObj != null)
        {
            if (_MenuSelectStatic == null)
            {
                DontDestroyOnLoad(_MenuSelectObj);
                _MenuSelectStatic = _MenuSelectObj;
            }
        }
        // updates the private audio
        if (_MenuSelectStatic != null)
        {
            _MenuSelectSFX = _MenuSelectStatic.GetComponent<AudioSource>();
        }
        if (_MenuMusicStatic != null)
        {
            _MenuMusicSorce = _MenuMusicStatic.GetComponent<AudioSource>();
        }
        //plays main menu music
        if (_StartMenuMusic)
        {
            PlayMenuMusic();
        }
        //stops menu music
        if (_StopMenuMusic)
        {
            StopMenuMusic();
        }


        if (!_MuteAmbiance)
        {
            PlayAmbiance();
        }

        if (!_MuteEngine)
        {
            PlayEngineSounds();
        }

        if (!_MuteRaceMusic)
        {
            PlayRaceMusic();
        }

    }
    public void PlayAmbiance()
    {
        if (_Ambiance == null) return;
        _Ambiance.Play();
    }
    public void PlayEngineSounds()
    {
        if (_Engine == null) return;
        _Engine.Play();
    }
    public void PlayCheckpointSFX()
    {
        if (_Checkpoint == null) return;
        _Checkpoint.Play();
    }
    public void PlayRaceMusic()
    {
        if (_RaceMusic == null || _RaceMusic.Count <= 0) return;
        foreach (var musictrack in _RaceMusic) // changes music volume
        {
            musictrack.volume = _Volume;
        }
        if (_RandomizeMusic == true)
        {
            System.Random rand = new System.Random();
            int t = Random.Range(0, _RaceMusic.Count);
            _RaceMusic[t].Play();
        }
        else
        {
            _RaceMusic[_MusicIndex].Play();
        }
    }
    public void PlayMenuMusic()
    {
        if (_MenuMusicSorce == null) return;
        if (_MenuMusicSorce.isPlaying) return;
        _MenuMusicSorce.Play();
    }
    public void StopMenuMusic()
    {
        if (_MenuMusicSorce == null) return;
        _MenuMusicSorce.Stop();
    }
    public void PlayPlaneStart()
    {
        if (_PlaneStart == null) return;
        _PlaneStart.Play();
    }
    public void PlaySelectSFX()
    {
        if (_MenuSelectSFX == null) return;
        _MenuSelectSFX.Play();
    }
    public void PlayFinishSFX()
    {
        if (_FinishSFX == null) return;
        _FinishSFX.Play();
    }
    public void PlayCrashSFX()
    {

        if (_CrashSFX == null) return;
        _CrashSFX.Play();
    }
    public void PlayLoseMusic()
    {
        if (_LoseMusic == null) return;
        _LoseMusic.volume = _Volume;
        _LoseMusic.Play();
    }
    public void PlayWinMusic()
    {
        if (_WinMusic == null) return;
        _WinMusic.volume = _Volume;
        _WinMusic.Play();
    }
    public void UpdateVolume(float volume)
    {
        volume = volume * 0.2f;
        _Volume = volume;
        _MenuMusicSorce.volume = volume;

    }
    public float GetVolume()
    {
        return _MenuMusicSorce.volume / 0.2f;
    }
}
