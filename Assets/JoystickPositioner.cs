using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPositioner : MonoBehaviour
{
    RectTransform allowedArea;
    private void Awake()
    {
        allowedArea = GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
           
        }*/
    }
}
