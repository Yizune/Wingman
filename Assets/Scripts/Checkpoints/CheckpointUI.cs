using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI checkpointText;
    CheckpointManager checkpointManager;

    private void Start()
    {
        checkpointManager = FindObjectOfType<CheckpointManager>();
    }
    private void Update()
    {
        
        if (checkpointManager != null)
        {
            checkpointText.text = $"{checkpointManager.GetCheckpointsLeft()} / {checkpointManager.GetTotalCheckpoints()}";
        }
        else
        {
            Debug.LogWarning("CheckpointManager not found in the scene.");
        }
    }
}
