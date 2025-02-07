using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownController : MonoBehaviour
{
    [SerializeField] private int _countdownTime;
    [SerializeField] private int _visibleCountdownTime;
    [SerializeField] private TextMeshProUGUI _countdownDisplay;
    //[SerializeField] Animator _propellerAnimator; 
    private PlaneMovement _planeMovement;
    private PlayerInput _playerInput;
    private Timer _timer;
    [SerializeField] private TextMeshProUGUI _timeToBeatText;
    private string timeToBeatPrefix = "Time to beat: ";
    private GameManager _gameManager;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        StartCoroutine(CountdownToStart());
        _playerInput = FindObjectOfType<PlayerInput>();
        _planeMovement = FindObjectOfType<PlaneMovement>();
        _timer = FindObjectOfType<Timer>();
        _gameManager = GameManager.Instance;
        
    }

    IEnumerator CountdownToStart()
    {
        yield return new WaitForSeconds(0.1f);
        
        float startTime = Time.realtimeSinceStartup + 1f; 

        while (_countdownTime >= 1)
        {
            float elapsedRealTime = Time.realtimeSinceStartup - startTime;

            int displayTime = Mathf.Max(1, _countdownTime - Mathf.CeilToInt(elapsedRealTime));

            // Display "GO!" when countdown is completed
            if (displayTime == 1 && elapsedRealTime >= _countdownTime - 1)
            {
                _countdownDisplay.text = "GO!";
                //_propellerAnimator.enabled = true;
                AfterCountdown();
                //Time.timeScale = 1f;
                yield return new WaitForSeconds(1f);
                _countdownDisplay.gameObject.SetActive(false);
                yield break; 
            }
            else if (displayTime > 3) 
            {
                _countdownDisplay.text = "";
                //_timeToBeatText.text = "";
            }
            else
            {
                _countdownDisplay.text = displayTime.ToString();
            }

            BeforeCountdown();
            //Time.timeScale = 0f;

            yield return null;
        }
    }

    public void BeforeCountdown()
    {
        _timer.PauseTimer();
        _playerInput.DisableInput();
        _planeMovement.DisableMovement();
        string timeToBeatString = timeToBeatPrefix + FormatTime(_gameManager.GetTimeToComplete());
        //Debug.Log(timeToBeatString);
        _timeToBeatText.text = timeToBeatString;
    }

    public void AfterCountdown()
    {
        _playerInput.EnableInput();
        _planeMovement.EnableMovement();
        _timer.StartTimer();
        _timeToBeatText.text = "";

    }
    private string FormatTime(float timeValue)
    {
        int minutes = Mathf.FloorToInt(timeValue / 60);
        int seconds = Mathf.FloorToInt(timeValue % 60);
        int milliseconds = Mathf.FloorToInt((timeValue * 1000) % 1000);

        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds / 10);
        return formattedTime;
    }
}