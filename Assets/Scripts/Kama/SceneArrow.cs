using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneArrow : MonoBehaviour
{
    public Transform arrow;
    private Transform playerTransform;
    public CheckpointManager checkpointManager;

    public float HideArrow;

    void Start()
    {
        playerTransform = FindObjectOfType<PlaneMovement>().transform;
    }

    void Update()
    {
        if(checkpointManager != null)
        {
            Vector3 dir = playerTransform.InverseTransformPoint(checkpointManager.GetNextCheckpoint().transform.position);
            float a = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            a += 180;
            arrow.transform.localEulerAngles = new Vector3(0, 180, a);
            if(dir.magnitude < HideArrow)
            {
                SetChildrenActice(false);
            }
            else
            {
                SetChildrenActice(true);
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }    
        }
    }

    void SetChildrenActice(bool value)
    {
        foreach(Transform child in transform) 
        {
            child.gameObject.SetActive(value);
        }
    }
}

