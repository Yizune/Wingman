using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStick : MonoBehaviour
{

    [SerializeField] private GameObject rightStickScene;
    [SerializeField] private GameObject leftStickScene;

    private PlaneMovement planeMovement;

    private void Start()
    {
        planeMovement = FindObjectOfType<PlaneMovement>();
        UpdateSceneVisibility();
    }

    public void OnControlSchemeChanged(bool isRightStick )
    {
        planeMovement.ToggleControlScheme();

        UpdateSceneVisibility();
    }

    private void UpdateSceneVisibility()
    {
        if (planeMovement.IsRightStickControl())
        {
            rightStickScene.SetActive(true);
            leftStickScene.SetActive(false);
        }
        else
        {
            rightStickScene.SetActive(false);
            leftStickScene.SetActive(true);
        }
    }
}

