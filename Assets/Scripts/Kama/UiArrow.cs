using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiArrow : MonoBehaviour
{
    public Image image;
    public Transform target;
    public TextMeshProUGUI meter;
    public Vector3 offset;
    public float hideArrowDistance = 100f;

    public CheckpointManager checkpointManager;


    private void Start()
    {
        checkpointManager = GetComponent<CheckpointManager>();
    }
    public void Update()
    {
        float minX = image.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = image.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.width - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(checkpointManager.GetNextCheckpoint().transform.position + offset);

            float distanceToTarget = Vector3.Distance(checkpointManager.GetNextCheckpoint().transform.position, transform.position);


        if (distanceToTarget < hideArrowDistance)
        {
            image.gameObject.SetActive(false);
        }
        else
        {
            image.gameObject.SetActive(true);
            if (Vector3.Dot((checkpointManager.GetNextCheckpoint().transform.position - transform.position), transform.forward) < 0)
            {
                if (pos.x < Screen.width / 2)
                {
                    pos.x = maxX;
                }
                else
                {
                    pos.x = minX;
                }
            }

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            image.transform.position = pos;
            meter.text = ((int)distanceToTarget).ToString() + "m";
        }
     }
}

