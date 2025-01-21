using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // Declaring the variables.
    [SerializeField] private InputAction thrust;
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
    }

    private void FixedUpdate()
    {
        if (thrust.IsPressed())
        {
            rb.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
        }
    }
}
