using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // Declaring the variables.
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction rotateLeft;
    [SerializeField] private InputAction rotateRight;
    [SerializeField] private float thrustForce = 1000;
    [SerializeField] private float rotationForce = 1000;

    private Rigidbody rb;

    // Gets and assigns the necessary components.
    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

// Event function that gets called everytime the object with script Movement is enabled.
    private void OnEnable()
    {
        // Enables the thrust input action.
        thrust.Enable();
        rotateLeft.Enable();
        rotateRight.Enable();
    }

    private void FixedUpdate()
    {
        if (thrust.IsPressed())
        {
            rb.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
        }

        if (rotateLeft.IsPressed())
        {
            rb.AddRelativeTorque(Vector3.forward * rotationForce * Time.fixedDeltaTime);
        }

        if (rotateRight.IsPressed())
        {
            rb.AddRelativeTorque(-Vector3.forward * rotationForce * Time.fixedDeltaTime);
        }
    }
}
