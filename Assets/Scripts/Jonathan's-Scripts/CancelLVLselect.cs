using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancelLVLselect : MonoBehaviour
{
    [SerializeField] private Button _cancelButton;
    // Start is called before the first frame update
    void Start()
    {
        _cancelButton.onClick.AddListener(MainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 
    private void MainMenu()
    {
        SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
    }
}
