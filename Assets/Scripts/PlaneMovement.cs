using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaneMovement : MonoBehaviour
{


    private bool _isRightStickControl = false;
    private bool _isLeftStickControl = true;
    public float CurrentSpeed;

    [SerializeField] private PlayerInput _playerInput;

    [Header("Throttle settings")]
    [SerializeField] private float _minThrottle = 20;
    [SerializeField] private float _maxThrottle = 100f;
    [SerializeField] private bool _invertThrottle = false;

    [Header("Movement settings")]
    [SerializeField] private float _acceleration = 300f;
    [SerializeField] private float _minSpeed = 5f;
    [SerializeField] private float _maxSpeed = 100f;


    [Header("Toggles for each axis")]
    [SerializeField] private bool _usePitch = true;
    [SerializeField] private bool _useYaw = true;
    [SerializeField] private bool _useRoll = true;

    [Header("Pitch settings")]
    [SerializeField] private bool _enableMaxPitch = true;
    [SerializeField] private bool _invertPitch = false;
    [Range(0f, 90f)]
    [SerializeField] private float _maxPitchAngle = 30f;
    [SerializeField] private float _pitchResponsiveness = 10f;
    [SerializeField] private float _pitchConstant = 0.1f;


    [Header("Yaw settings")]
    [SerializeField] private float _yawResponsiveness = 5f;

    [Header("Roll settings")]
    [SerializeField] private Transform _modelTf;
    // [Range(0f, 90f)] 
    // [SerializeField] private float _maxRollAngle = 90f;
    [Header("Engine Sound")]
    [SerializeField] private float _minSoundPitch = 0.75f;
    [SerializeField] private float _maxSoundPitch = 1.25f;

    private float _maxRollAngle = 90f;

    private float _throttle;
    private float _pitch;
    private float _yaw;
    private float _pitchModifier => (rb.mass / 10f) * _pitchResponsiveness;
    private float _yawModifier => (rb.mass / 10f) * _yawResponsiveness;

    private Rigidbody rb;

    private bool _canMove = true;
    private Vector3 _currentVelocity;
    private Vector3 _currentAngularVelocity;
    private AudioSource engineNoise;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity
        engineNoise = GetComponent<AudioSource>();
    }
    private void Start()
    {
        if (engineNoise != null)
        {
            Inputs();
            SetEnginePitch();
            engineNoise.Play();
        }
    }

    void Update()
    {
        if (!_canMove) return;
        Inputs();
    }

    void FixedUpdate()
    {
        SetEnginePitch();
        if (!_canMove) return;
        HandleRotation();
        HandleMovement();

        CurrentSpeed = rb.velocity.magnitude;
    }

    public void DisableMovement()
    {
        _canMove = false;
        _currentVelocity = rb.velocity;
        _currentAngularVelocity = rb.angularVelocity;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void EnableMovement()
    {
        _canMove = true;
        rb.velocity = _currentVelocity;
        rb.angularVelocity = _currentAngularVelocity;
    }

    public void ResetPlane(Vector3 worldPosition, Vector3 facingDirection)
    {
        transform.position = worldPosition;
        transform.forward = facingDirection;

        _currentVelocity = Vector3.zero;
        _currentAngularVelocity = Vector3.zero;

        rb.velocity = _currentVelocity;
        rb.angularVelocity = _currentAngularVelocity;

        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        _modelTf.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void Inputs()
    {
        if (_isRightStickControl)
        {
            // Handle input from the right stick
            Vector2 rotationInput = _playerInput.GetInversetRotationInput();
            _yaw = rotationInput.x;
            _pitch = rotationInput.y;
            _throttle = Mathf.Lerp(_minThrottle, _maxThrottle, _playerInput.GetThrustInput());
        }
        else
        {
            // Handle input from the left stick
            Vector2 rotationInput = _playerInput.GetRotationInput();
            _yaw = rotationInput.x;
            _pitch = rotationInput.y;

            _throttle = Mathf.Lerp(_minThrottle, _maxThrottle, _playerInput.GetThrustInput());
        }

        // if (Input.GetKey(KeyCode.Space)) 
        //     _throttle += throttleIncrement;
        //
        // else if (Input.GetKey(KeyCode.LeftControl)) 
        //     _throttle -= throttleIncrement;
        //
        // _throttle = Mathf.Clamp(_throttle, _minThrottle, _maxThrottle);
    }

    private void HandleMovement()
    {
        rb.AddForce(transform.forward * _throttle * _acceleration);

        // clamp the speed
        // float speed = rb.velocity.magnitude;
        // if (speed > _maxSpeed) rb.velocity = rb.velocity.normalized * _maxSpeed;
        // else if (speed < _minSpeed) rb.velocity = rb.velocity.normalized * _minSpeed;
    }

    private void HandleRotation()
    {
        if (_usePitch) HandlePitch(_pitch);
        if (_useYaw) HandleYaw(_yaw);

        // for visual
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
        if (_useRoll) HandleRoll(_yaw);
    }

    // Handle X Rotation of the plane
    private void HandlePitch(float pitchInput)
    {
        if (pitchInput == 0) return;


        // todo: make this a settings variable
        if (_invertPitch) pitchInput *= -1f;

        // clamp the pitch angle
        if (_enableMaxPitch)
        {
            float currentPitchAngle = transform.eulerAngles.x;
            float newPitchAngle = currentPitchAngle + pitchInput * Time.fixedDeltaTime * _pitchModifier * 0.1f;

            float maxSin = Mathf.Sin(_maxPitchAngle * Mathf.Deg2Rad);
            float minSin = Mathf.Sin(-_maxPitchAngle * Mathf.Deg2Rad);

            float newSin = Mathf.Sin(newPitchAngle * Mathf.Deg2Rad);

            if (newSin > maxSin)
            {
                transform.eulerAngles = new Vector3(_maxPitchAngle, transform.eulerAngles.y, transform.eulerAngles.z);
                return;
            }

            if (newSin < minSin)
            {
                transform.eulerAngles = new Vector3(-_maxPitchAngle, transform.eulerAngles.y, transform.eulerAngles.z);
                return;
            }
        }

        transform.Rotate(new Vector3(pitchInput * Time.fixedDeltaTime * _pitchModifier * _pitchConstant, 0, 0), Space.Self);
    }


    // Handle Y Rotation of the plane
    private void HandleYaw(float yawInput)
    {
        if (yawInput == 0) return;

        if (_invertThrottle) yawInput *= -1f;

        // rb.AddTorque(transform.up * yawInput * _responseModifier, ForceMode.Force);

        // the higher the speed, the less responsive the yaw is
        transform.Rotate(new Vector3(0, yawInput * Time.fixedDeltaTime * _yawModifier, 0), Space.World);

    }


    // Handle Z Rotation of the plane
    // only rotates the model and does not use torque
    private void HandleRoll(float yawInput)
    {
        if (_modelTf == null) return;

        // lerp towards the target rotation

        float targetZRotation = -yawInput * _maxRollAngle;
        float currentZRotation = _modelTf.localEulerAngles.z;

        float newZRotation = Mathf.LerpAngle(currentZRotation, targetZRotation, 0.075f);

        _modelTf.localRotation = Quaternion.Euler(0f, 0f, newZRotation);
    }
    private void SetEnginePitch()
    {
        if (engineNoise != null)
        {
            float pitchlevel = (_throttle - _minThrottle) / (_maxThrottle - _minThrottle) * (1 - (_maxSoundPitch - _minSoundPitch) / 2) + 1;
            engineNoise.pitch = pitchlevel;
        }
    }
    public void ToggleInvert()
    {
        _invertPitch = !_invertPitch;
    }
    public bool IsInvert() { return _invertPitch; }

    public void ToggleThrottle()
    {
        _invertThrottle = !_invertThrottle;
    }
    public bool IsThrottle() { return _invertThrottle; }

    // Method to toggle between Left Stick and Right Stick control
    public void ToggleControlScheme()
    {
        _isRightStickControl = !_isRightStickControl;
        _isLeftStickControl = !_isLeftStickControl;

    }

    // Method to check if the current control scheme is Right Stick control
    public bool IsRightStickControl()
    {
        return _isRightStickControl;
    }

    // Method to check if the current control scheme is Left Stick control
    public bool IsLeftStickControl()
    {
        return _isLeftStickControl;
    }
}
