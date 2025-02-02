using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // Declaring the variables.
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction rotation;
    [SerializeField] private float thrustForce = 1000.0f;
    [SerializeField] private float rotationForce = 250.0f;
    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private ParticleSystem mainBoosterParticles;
    [SerializeField] private ParticleSystem leftBoosterParticles;
    [SerializeField] private ParticleSystem rightBoosterParticles;

    private Rigidbody rb;
    private AudioSource audioSource;

    // Gets and assigns the necessary components.
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Event function that gets called everytime the object with script Movement is enabled.
    private void OnEnable()
    {
        // Enables the input actions.
        thrust.Enable();
        rotation.Enable();
    }

    // Using FixedUpdate to call the physics based methods.

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    // Method that propels the object upwards and controls the audio.
    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            // Uses physics based movement.
            rb.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
            PlaySound();
            mainBoosterParticles.Play();
        }
        else
        {
            audioSource.Stop();
            mainBoosterParticles.Stop();
        }
    }

    // Method that rotates the object.
    private void ProcessRotation()
    {
        // Creates a variable that chaches the value of the player input.
        float rotationInput = rotation.ReadValue<float>();

        // Passes the necessary force as an argument based on the player input.
        if (rotationInput < 0)
        {
            ApplyRotation(rotationForce);
            leftBoosterParticles.Stop();
            rightBoosterParticles.Play();
        }
        else if (rotationInput > 0)
        {
            ApplyRotation(-rotationForce);
            rightBoosterParticles.Stop();
            leftBoosterParticles.Play();
        }
        else
        {
            leftBoosterParticles.Stop();
            rightBoosterParticles.Stop();
        }
    }

    // Method that allows to adjust the rotation based on its parameter.
    private void ApplyRotation(float force)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * force * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }

    // Method starts the audio if it isn't playing already.
    private void PlaySound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }
}
