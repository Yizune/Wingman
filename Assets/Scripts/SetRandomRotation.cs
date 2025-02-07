using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomRotation : MonoBehaviour
{
    Transform tf;
    [SerializeField] int _numberOfBlades;
    private void Awake()
    {
        tf = transform;
        float randomAngle = Random.value * (360f / _numberOfBlades);
        transform.localEulerAngles = new Vector3(randomAngle, 0, 0);
    }
}
