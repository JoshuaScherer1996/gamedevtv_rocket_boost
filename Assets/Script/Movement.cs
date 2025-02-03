using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls upward thrust and rotation for the attached GameObject,
/// handling audio and particle feedback accordingly.
/// </summary>
public class Movement : MonoBehaviour
{
    // Input Actions used to detect player commands.
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction rotation;

    // Movement settings determining the forces applied.
    [SerializeField] private float thrustForce = 1000.0f;
    [SerializeField] private float rotationForce = 250.0f;

    // Audio and visual effects for engine thrust and rotational boosts.
    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private ParticleSystem mainBoosterParticles;
    [SerializeField] private ParticleSystem leftBoosterParticles;
    [SerializeField] private ParticleSystem rightBoosterParticles;

    // Cached references to essential components for performance.
    private Rigidbody rb;
    private AudioSource audioSource;

    /// <summary>
    /// Caches component references on startup for performance optimization.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();                 // Retrieve the Rigidbody component.
        audioSource = GetComponent<AudioSource>();      // Retrieve the AudioSource component.
    }

    /// <summary>
    /// Enables input actions when this GameObject becomes active.
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
    /// Checks for thrust input and triggers thrust start or stop accordingly.
    /// </summary>
    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            StartThrusting();   // Apply upward force and start effects.
        }
        else
        {
            StopThrusting();   // Halt effects when thrust input is released. 
        }
    }

    /// <summary>
    /// Applies upward force to simulate thrust, and plays related audio as well as particle effects.
    /// </summary>
    private void StartThrusting()
    {
        // Apply force in the local upward direction, scaled by fixed delta time.
        rb.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
        PlaySound();                    // Play engine sound if not already active.
        mainBoosterParticles.Play();    // Start the main booster particle effect.
    }

    /// <summary>
    /// Stops engine audio and booster particles.
    /// </summary>
    private void StopThrusting()
    {
        audioSource.Stop();             // Stop the engine sound
        mainBoosterParticles.Stop();    // Stop main booster particles.
    }

    /// <summary>
    /// Reads rotation input and applies corresponding rotation forces and visual effects.
    /// </summary>
    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();  // Get rotation input value.

        if (rotationInput < 0)
        {
            RotateLeft();   // Rotate left if the input is negative.
        }
        else if (rotationInput > 0)
        {
            RotateRight();  // Rotate right if the input is positive.
        }
        else
        {
            StopRotationParticles();    // Stop booster particles when there is no rotation input.
        }
    }

    /// <summary>
    /// Rotates the GameObject to the left and triggers the right booster effect.
    /// </summary>
    private void RotateLeft()
    {
        leftBoosterParticles.Stop();    // Stop left booster to avoid redundant effects.
        ApplyRotation(rotationForce);   // Apply rotation force for turning left.
        rightBoosterParticles.Play();   // Play right booster particles for visual feedback.
    }

    /// <summary>
    /// Rotates the GameObject to the right and triggers the left booster effect.
    /// </summary>
    private void RotateRight()
    {
        rightBoosterParticles.Stop();   // Stop right booster to avoid redundant effects.
        ApplyRotation(-rotationForce);  // Apply rotation force (negative for right turn).
        leftBoosterParticles.Play();    // Play left booster particles for visual feedback.
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
    /// Temporarily disables physics-based rotation, applies manual rotation, and re-enables it.
    /// </summary>
    /// <param name="force">The rotation force to be applied.</param>
    private void ApplyRotation(float force)
    {
        rb.freezeRotation = true;   // Disable automatic physics rotation.
        transform.Rotate(Vector3.forward * force * Time.fixedDeltaTime);    // Apply manual rotation.
        rb.freezeRotation = false;  // Re-enable physics rotation.
    }

    /// <summary>
    /// Plays the main engine sound if it is not already playing.
    /// Prevents overlapping audio playback.
    /// </summary>
    private void PlaySound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }
}
