using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class RaceFailedUI : MonoBehaviour
{
    [SerializeField]
    private Button retryButton;
    [SerializeField]
    private Button mainMenuButton;
    // Start is called before the first frame update
    private void returnToMainMenu()
    {
        SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
    }
private void retry()
    {
        SceneLoader.Load(SceneLoader.Scene.Beta_GameScene);
    }
    

    // Update is called once per frame
    void Start()
    {
        retryButton.onClick.AddListener(retry);
        mainMenuButton.onClick.AddListener(returnToMainMenu);
    }
}
