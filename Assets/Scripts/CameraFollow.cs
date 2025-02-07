using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _targetTf;
    [SerializeField] private Transform _cameraHolderTf;
    
    [SerializeField] private float _smoothTime = 0.3f;
    [SerializeField] private float _rotateSmoothTime = 0.3f;
    [SerializeField] private float _cameraHitDistance = 1f;
    [SerializeField] private float _cameraHitRadius = 0.5f;

    private Vector3 _offset;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _defaultRotation = new Vector3(0f, 0f, 0f);
    private bool _isPaused = false;
    RaycastHit cameraHit;
    private void Start()
    {
        _offset = _cameraHolderTf.position - _targetTf.position;
        _defaultRotation = _cameraHolderTf.rotation.eulerAngles;
        transform.position = _cameraHolderTf.position;
        transform.rotation = _cameraHolderTf.rotation;

        if (GameManager.Instance != null)
            GameManager.Instance.OnGameTogglePause += Instance_OnGameTogglePause;
    }
    
    private void Instance_OnGameTogglePause(object sender, GameManager.OnGameTogglePauseEventArgs e)
    {
        _isPaused = e.IsPaused;
    }
    
    private void FixedUpdate()
    {
        
        if (_isPaused) return;
        
        SmoothFollow();
        
    }
    private bool CheckRoof()
    {
        bool roofClose = Physics.SphereCast(transform.position, _cameraHitRadius, Vector3.up, out cameraHit, _cameraHitDistance);
        //if (roofClose) Debug.Log("Roof is " + cameraHit.distance + " away");
        return roofClose;
    }
    private void SmoothFollow()
    {
        Vector3 targetPositionRoof = Vector3.zero;
        if (CheckRoof())
        {
            targetPositionRoof.y = -(cameraHit.point.y - transform.position.y);
        }
        Vector3 targetPosition = _targetTf.position + (_targetTf.rotation * _offset) + targetPositionRoof;
        
        // use smoothdamp to smooth the camera movement
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
        
        // follow the target's rotation, but keep in mind the default rotation
        Quaternion targetRotation = Quaternion.LookRotation(_targetTf.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotateSmoothTime);
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, _cameraHitDistance, 0), _cameraHitRadius);
    }
}
