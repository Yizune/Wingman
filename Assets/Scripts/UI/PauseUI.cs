using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _PauseButton;

    [SerializeField] private GameObject pauseContainer;
    [SerializeField] private AudioSource engineAudio;
    private bool paused;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        pauseContainer.SetActive(false);
        _PauseButton.onClick.AddListener(PauseGame);
        _resumeButton.onClick.AddListener(ResumeGame);
        _mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    /*private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }

        
    }*/

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
    }
    private void ResumeGame()
    {
        //OnApplicationFocus(true);
        //OnApplicationPause(false);
        Cursor.visible = false;
        pauseContainer.SetActive(false);
        PlayEngineAudio();
        _PauseButton.gameObject.SetActive(true);
        GameManager.Instance.SetPause(false);
        paused = false;
    }
    private void PauseGame()
    {
        //OnApplicationFocus(false);
        //OnApplicationPause(true);
        Cursor.visible = true;
        pauseContainer.SetActive(true);
        StopEngineAudio();
        _PauseButton.gameObject.SetActive(false);
        GameManager.Instance.SetPause(true);
        paused = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void PlayEngineAudio()
    {
        if (engineAudio != null)
        {
            engineAudio.Play();
        }
    }
    private void StopEngineAudio()
    {
        if (engineAudio != null)
        {
            engineAudio.Stop();
        }
    }
    public void OpenPauseMenu()
    {
        if (!paused)
        {
            PauseGame();
        }
        else if (paused)
        {
            ResumeGame();
        }
    }
}
