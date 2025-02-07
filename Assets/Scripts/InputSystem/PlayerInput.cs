using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Slider _thrustSlider;
    [SerializeField] private BoostButton _boostButton;
    [SerializeField] private SlowButton _slowButton;
    [SerializeField] private bool _buttonEnabled;
    [SerializeField] private PauseUI _pauseUI;
    private Player player;
    [SerializeField] private float _thrustChangeSpeed = 1f;
    private PlayerInputActions _playerInputActions;
    
    
    [SerializeField] private float _thrustInput;
    
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        player = GetComponent<Player>();
        
        if (_thrustSlider == null) 
            Debug.LogWarning("OnScreenSlider is null");
        if (_buttonEnabled) { _thrustInput = 0.5f; }
        
    }

    private void Update() 
    {
        UpdateThrottleInput();
        if (_playerInputActions.Plane.Menu.IsPressed())
        {
            _pauseUI.OpenPauseMenu();
        }
        if (_playerInputActions.Plane.Reset.IsPressed() )
        {
            player.ResetToLastCheckpoint();
        }
    }
    public Vector2 GetInversetRotationInput()
    {
        return _playerInputActions.Plane.InRotate.ReadValue<Vector2>();
    }

    private void UpdateThrottleInput() 
    {
        if (!_buttonEnabled)
        {
            float input = _playerInputActions.Plane.Throttle.ReadValue<float>();

            if (input != 0) {
                _thrustInput = Mathf.Clamp(_thrustInput + input * _thrustChangeSpeed * Time.deltaTime * -1f, 0, 1);
            
                if (_thrustSlider != null) {
                    _thrustSlider.value = _thrustInput;
                }
            }
        }
        else
        {
            if (_boostButton != null && _slowButton != null) 
            {
                float input = 0.5f;
                if (_boostButton.IsPressed())
                {
                    Debug.Log("Boost button is pressed");
                    input = 1f;
                }
                else if (_slowButton.IsPressed()) 
                {
                    Debug.Log("Slow button is pressed");
                    input = 0f;
                }
            
                _thrustInput = input;
                //Debug.Log(_thrustInput);
                
            }
        }
    }

    public Vector2 GetRotationInput() {
        return _playerInputActions.Plane.Rotate.ReadValue<Vector2>();
    }
    
    
    public float GetThrustInput() {
        if (_thrustSlider != null && !_buttonEnabled) 
        {
            _thrustInput = _thrustSlider.value;
            
            return _thrustSlider.value;
        }
        else if (_buttonEnabled)
        {
            /*float input = 0.5f;
            if (_boostButton.IsPressed())
            {
                input = 1f;
            }
            else if (_slowButton.IsPressed())
            {
                input = 0f;
            }
            _thrustInput = input;*/
            return _thrustInput;
        }
        return _thrustInput;
    }
    
    
    public void DisableInput() {
        _playerInputActions.Disable();
    }
    
    public void EnableInput() {
        _playerInputActions.Enable();
    }
    
    private void OnEnable() {
        _playerInputActions.Plane.Enable();
    }

    private void OnDisable() {
        _playerInputActions.Plane.Disable();
    }
}
