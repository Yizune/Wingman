using UnityEngine;
using UnityEngine.EventSystems;

public class BoostButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    public bool IsHeldDown => isHeldDown;
    public bool isHeldDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHeldDown = true;
        Debug.Log("Boosting");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isHeldDown = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public float GetValue()
    {
        if (isHeldDown) { return 1f; }
        else { return 0f; }

    }
    public bool IsPressed() { return isHeldDown; }
}