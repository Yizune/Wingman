using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] private GameObject CreditsContainer;
    [SerializeField] private GameObject MainMenuContainer;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject _settingsContainer;
    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(BackToMainMenu);
    }
    private void BackToMainMenu()
    {
        CreditsContainer.SetActive(false);
        MainMenuContainer.SetActive(true);
        _settingsContainer.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
