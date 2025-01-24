using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // Declaring the variables.
    [SerializeField] private InputAction thrust;

    [SerializeField] private InputAction rotation;
    [SerializeField] private float thrustForce = 1000;

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
        rotation.Enable();
    }

    private void FixedUpdate()
    {
        ProcessThrust();

        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            rb.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
        }
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        Debug.Log($"We rotate with the value of: {rotationInput}");
    }
}
