using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plane : MonoBehaviour
{
    Rigidbody rb;

    public float horizontalInput;
    public float verticalInput;

    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private float speed_1;
    

    public float throttleIncrement = 0.1f;
    public float maxThrottle = 200f;
    public float responsiveness = 10f;

    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;
    private float responseModifier
    {
        get
        {
            return (rb.mass / 10f) * responsiveness;
        }
    }

    // Variables to clamp rotation
    public float maxRollAngle = 90f;
    private float currentRollAngle = 0f;

    public float maxPitchAngle = 90f;
    private float currentPitchAngle = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity
    }

    void Update()
    {
        Inputs();

        Vector2 moveDirection = moveAction.action.ReadValue<Vector2>();
        transform.Translate(moveDirection * speed_1 * Time.deltaTime);
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    void Inputs()
    {
        roll = horizontalInput;
        pitch = verticalInput;
        //roll = Input.GetAxis("Horizontal");
        //pitch = Input.GetAxis("Vertical");
        yaw = Input.GetAxis("Yaw");

        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }

    void HandleMovement()
    {
        // Always apply forward force based on throttle value
        rb.AddForce(transform.forward * maxThrottle * throttle);
        // speed forward 
        throttle = 5f;
    }


    void HandleRotation()
    {
        RotatePitch(pitch);
        RotateYawRoll(yaw, roll);
    }

    void RotatePitch(float pitchInput)
    {
        // Rotate around the local right axis for pitch
        transform.Rotate(Vector3.right, pitchInput * Time.fixedDeltaTime * responseModifier);

        // Rotate around the local forward axis for Pitch (visual purposes)
        float newPitchAngle = currentPitchAngle - pitchInput * Time.fixedDeltaTime * responseModifier;
        newPitchAngle = Mathf.Clamp(newPitchAngle, -maxPitchAngle, maxPitchAngle);

        transform.eulerAngles = new Vector3(newPitchAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        currentPitchAngle = newPitchAngle;
    }

    void RotateYawRoll(float yawInput, float rollInput)
    {
        // Rotate around the local up axis for yaw
        transform.Rotate(Vector3.up, yawInput * Time.fixedDeltaTime * responseModifier);

        // Rotate around the local forward axis for roll (visual purposes)
        float newRollAngle = currentRollAngle - rollInput * Time.fixedDeltaTime * responseModifier;
        newRollAngle = Mathf.Clamp(newRollAngle, -maxRollAngle, maxRollAngle);

        // Calculate the change in roll angle
        float deltaRollAngle = newRollAngle - currentRollAngle;

        // Rotate around the local forward axis by the change in roll angle
        transform.Rotate(transform.forward, deltaRollAngle);

        currentRollAngle = newRollAngle;
    }
}
