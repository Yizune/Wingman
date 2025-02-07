using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scene
    {
        MainMenuScene,
        LevelSelectScene,
        ActualGameScene,
        EndGameScene,
        Beta_GameScene,
        PlayerProfileScene,
        LeaderboardScene
    }

    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
    
    public static void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
