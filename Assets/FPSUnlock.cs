using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSUnlock : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
