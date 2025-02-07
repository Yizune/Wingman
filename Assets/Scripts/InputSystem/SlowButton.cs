using UnityEngine;
using UnityEngine.EventSystems;

public class SlowButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    public bool IsHeldDown => isHeldDown;
    public bool isHeldDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHeldDown = true;
        Debug.Log("Slowing");
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
        if (isHeldDown) { return 0f; }
        else { return 0f; }

    }
    public bool IsPressed() { return isHeldDown; }
}