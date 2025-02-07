using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCollision : MonoBehaviour
{
    public event EventHandler OnPlaneCrashed;
    GameObject crashDebris;
    [SerializeField] GameObject _crashDebrisPrefab;
    private AudioManager2 _AudioManager;
    [SerializeField] GameObject planeModel;
    Collider[] planeColliders;
    [SerializeField] float invincibilityTime;
    private void Awake()
    {
        _AudioManager = FindObjectOfType<AudioManager2>();
        crashDebris = Instantiate(_crashDebrisPrefab, transform);
        planeColliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in planeColliders) { Debug.Log(collider.name); }
    }
    private void OnCollisionEnter(Collision other)
    {
        // check layers?
        foreach (Collider collider in planeColliders) {  collider.enabled = false; }
        _AudioManager.PlayCrashSFX();
        crashDebris.transform.SetParent(null);
        crashDebris.SetActive(true);
        crashDebris = Instantiate(_crashDebrisPrefab, transform);
        StartCoroutine(RemoveInvincibility());
        //Debug.Log("Started Invincibility");
        //foreach (Collider collider in planeColliders) { Debug.Log(collider.enabled); }
        OnPlaneCrashed?.Invoke(this, EventArgs.Empty);
    }
    IEnumerator RemoveInvincibility()
    {
        yield return new WaitForSeconds(invincibilityTime);
        //Debug.Log("Stopping Invincibility");
        foreach (Collider collider in planeColliders)
        {
            collider.enabled = true;
        }
        //foreach (Collider collider in planeColliders) { Debug.Log(collider.enabled); }
    }
}
