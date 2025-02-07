using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointVisualizer : MonoBehaviour
{
    private Camera cam;
    private Vector3 checkpointPos;
    private Vector2 indicatorPos;
    [SerializeField] float xOffsetBorder = 150f;
    [SerializeField] float yOffsetBorder = 20f;
    private Vector2 screenSize;
    private CheckpointManager checkpointManager;
    Image indicatorImage;
    [SerializeField] Sprite freeIndicatorSprite;
    [SerializeField] Sprite upArrowSprite;
    [SerializeField] Sprite downArrowSprite;
    bool firstFrameDone = false;
    CanvasGroup canvasGroup;
    float distance;
    [SerializeField] float minFadeDistance = 100f;
    [SerializeField] float maxFadeDistance = 200f;
    [SerializeField] bool freeIcon;
    [SerializeField] float freeSize = 75f;
    [SerializeField] float edgeSize = 150f;
    float screenRatio;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        indicatorImage = GetComponent<Image>();
        checkpointManager = FindObjectOfType<CheckpointManager>();
        screenSize = new Vector2(Screen.width, Screen.height);
        Debug.Log("Screen size: " + screenSize.x + ", " + screenSize.y);
        canvasGroup = GetComponent<CanvasGroup>();
        if (!freeIcon) { indicatorImage.sprite = upArrowSprite; }
        screenRatio = Screen.height / Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        if (!firstFrameDone)
        {
            checkpointPos = checkpointManager.GetNextCheckpoint().transform.position;
            //Debug.Log(checkpointPos);
            firstFrameDone = true;
        }

        Vector3 objectPos = cam.WorldToScreenPoint(checkpointPos);
        distance = Vector3.Distance(cam.transform.position, checkpointPos);
        //Debug.Log(distance);
        Vector3 imagePos = Vector3.zero;

        if (freeIcon)
        {


            //Debug.Log(objectPos);
            if (objectPos.z < 0) //|| Mathf.Abs(objectPos.x - (screenSize.x - offsetBorder)) > screenSize.x - offsetBorder || Mathf.Abs(objectPos.y - (screenSize.y - offsetBorder)) > screenSize.y - offsetBorder)
            {
                if (Mathf.Abs(screenSize.x / 2 - (screenSize.x - objectPos.x)) > screenRatio * Mathf.Abs(screenSize.y / 2 - (screenSize.y - objectPos.y))) //(Mathf.Abs(screenSize.x / 2 - (screenSize.x - objectPos.x)) > Mathf.Abs(screenSize.y / 2 - (screenSize.y - objectPos.y)))
                {
                    if (screenSize.x - objectPos.x > screenSize.x / 2) { imagePos.x = screenSize.x; }
                    else { imagePos.x = 0f; }
                    imagePos.y = screenSize.y - objectPos.y;
                }
                else
                {
                    if (screenSize.y - objectPos.y > screenSize.y / 2) { imagePos.y = screenSize.y; }
                    else { imagePos.y = 0f; }
                    imagePos.x = screenSize.x - objectPos.x;
                }
                canvasGroup.alpha = 1;
                //indicatorImage.rectTransform.sizeDelta = new Vector2(edgeSize, edgeSize);
                /*if (screenSize.x - objectPos.x > )
                    imagePos.x = (screenSize.x - objectPos.x) * 10000;
                imagePos.y = (screenSize.y - objectPos.y) * 10000;*/
                /* if (screenSize.x - objectPos.x > screenSize.x / 2) { imagePos.x = screenSize.x; }
                 else { imagePos.x = 0; }*/
            }
            else
            {
                imagePos.x = objectPos.x;
                imagePos.y = objectPos.y;
                canvasGroup.alpha = Mathf.Clamp((distance - minFadeDistance) / (maxFadeDistance - minFadeDistance), 0, 1);
                //indicatorImage.rectTransform.sizeDelta = new Vector2(freeSize, freeSize);
            }
            if (imagePos.x > screenSize.x || imagePos.y > screenSize.y || imagePos.x < 0 || imagePos.y < 0) { canvasGroup.alpha = 1; }

        }
        else
        {
            if (objectPos.z < 0)
            {
                Vector3 adjustedPos = new Vector3(screenSize.x - objectPos.x, screenSize.y - objectPos.y, objectPos.z);
                if (adjustedPos.x >= screenSize.x / 2) { adjustedPos.x = screenSize.x; }
                else { adjustedPos.x = 0f; }
                adjustedPos.z = 0f;
                imagePos.y = adjustedPos.y;
                imagePos.x = adjustedPos.x;
            }
            else
            {
                imagePos.x = objectPos.x;
                imagePos.y = objectPos.y;
                imagePos.z = 0f;
            }
            if (imagePos.y >= screenSize.y / 2) { indicatorImage.sprite = upArrowSprite; }
            else { indicatorImage.sprite = downArrowSprite; }
            imagePos.y = screenSize.y;

        }
        imagePos.x = Mathf.Clamp(imagePos.x, xOffsetBorder, screenSize.x - xOffsetBorder);
        imagePos.y = Mathf.Clamp(imagePos.y, yOffsetBorder, screenSize.y - yOffsetBorder);
        if (imagePos.x == screenSize.x - xOffsetBorder || imagePos.y == screenSize.y - yOffsetBorder || imagePos.x == xOffsetBorder || imagePos.y == yOffsetBorder)
        {
            indicatorImage.rectTransform.sizeDelta = new Vector2(edgeSize, edgeSize);
        }
        else
        {
            indicatorImage.rectTransform.sizeDelta = new Vector2(freeSize, freeSize);
        }
        transform.position = imagePos;
    }
    public void UpdateCheckPointPos(Vector3 checkpointPosition)
    {
        checkpointPos = checkpointPosition;
    }

}
