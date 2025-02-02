using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls upward thrust and rotation for the attached GameObject,
/// handling audio and particle feedback accordingly.
/// </summary>
public class Movement : MonoBehaviour
{
    // Input Actions
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction rotation;

    // Movement Settings
    [SerializeField] private float thrustForce = 1000.0f;
    [SerializeField] private float rotationForce = 250.0f;

    // Audio & VFX
    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private ParticleSystem mainBoosterParticles;
    [SerializeField] private ParticleSystem leftBoosterParticles;
    [SerializeField] private ParticleSystem rightBoosterParticles;

    // Game Object Caches
    private Rigidbody rb;
    private AudioSource audioSource;

    /// <summary>
    /// Initializes references to Rigidbody and AudioSource.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Enables input actions when this object becomes active.
    /// </summary>
    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    /// <summary>
    /// Handles physics-based thrust and rotation each fixed frame.
    /// </summary>
    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    /// <summary>
    /// Applies upward thrust if input is pressed, otherwise stops thrust effects.
    /// </summary>
    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    /// <summary>
    /// Adds force for upward thrust and plays related audio and particles.
    /// </summary>
    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
        PlaySound();
        mainBoosterParticles.Play();
    }

    /// <summary>
    /// Stops engine audio and booster particles.
    /// </summary>
    private void StopThrusting()
    {
        audioSource.Stop();
        mainBoosterParticles.Stop();
    }

    /// <summary>
    /// Reads rotation input and applies rotation forces if any.
    /// </summary>
    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();

        if (rotationInput < 0)
        {
            RotateLeft();
        }
        else if (rotationInput > 0)
        {
            RotateRight();
        }
        else
        {
            StopRotationParticles();
        }
    }

    /// <summary>
    /// Applies rotation for turning left and triggers right booster effects.
    /// </summary>
    private void RotateLeft()
    {
        leftBoosterParticles.Stop();
        ApplyRotation(rotationForce);
        rightBoosterParticles.Play();
    }

    /// <summary>
    /// Applies rotation for turning right and triggers left booster effects.
    /// </summary>
    private void RotateRight()
    {
        rightBoosterParticles.Stop();
        ApplyRotation(-rotationForce);
        leftBoosterParticles.Play();
    }

    /// <summary>
    /// Stops both left and right booster particles.
    /// </summary>
    private void StopRotationParticles()
    {
        leftBoosterParticles.Stop();
        rightBoosterParticles.Stop();
    }

    /// <summary>
    /// Temporarily prevents physics from altering rotation, applies rotation, then re-enables it.
    /// </summary>
    private void ApplyRotation(float force)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * force * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }

    /// <summary>
    /// Plays the main engine sound if not already playing.
    /// </summary>
    private void PlaySound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }
}
