using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

/// <summary>
/// Handles collision events for the attached GameObject, including crash and success sequences.
/// </summary>
public class CollisionHandler : MonoBehaviour
{
    // General settings.
    [SerializeField] private float delay = 2f;

    // Audio clips for collision events.
    [SerializeField] private AudioClip crashSFX;
    [SerializeField] private AudioClip successSFX;

    // Particle systems for visual effects.
    [SerializeField] private ParticleSystem crashParticles;
    [SerializeField] private ParticleSystem successParticles;

    // Cached component for audio playback.
    private AudioSource audioSource;

     // State flags controlling collision handling.
    bool isControllable = true; // Determines if the player can control the GameObject.
    bool isCollidable = true;   // Determines if collisions should be processed.

    /// <summary>
    /// Obtains the AudioSource reference on start.
    /// </summary>
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    /// <summary>
    /// Checks for debug key input on every frame.
    /// </summary>
    private void Update()
    {
        RespondToDebugKeys();
    }

    /// <summary>
    /// Listens for debug key presses:
    /// - "L" loads the next level.
    /// - "C" toggles the collidability (useful for testing).
    /// </summary>
    private void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            NextLevel();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
        }
    }

    /// <summary>
    /// Determines the collision outcome based on the other object's tag.
    /// </summary>
    /// <param name="other">Collision data for the current collision event.</param>
    private void OnCollisionEnter(Collision other)
    {
        // Ignore collisions if controls are already disabled (e.g., after a crash or success) or collisions are turned off.
        if (!isControllable || !isCollidable) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                // No action required for friendly collisions.
                break;
            case "Fuel":
                // Fuel collisions are intentionally ignored. Could be implemented for later levels.
                break;
            case "Finish":
                // Trigger success sequence when reaching the finish line.
                StartSuccessSequence();
                break;
            default:
                // Any other collision is treated as a crash.
                StartCrashSequence();
                break;
        }
    }

    /// <summary>
    /// Initiates the success sequence, stopping current audio and playing success effects.
    /// </summary>
    private void StartSuccessSequence()
    {
        ToggleControls();           // Disable further input to prevent interference.
        audioSource.Stop();         // Stop any current audio.
        audioSource.PlayOneShot(successSFX);    // Play the success sound effect.
        successParticles.Play();    // Start success particle effects.

        // Disable the Movement script to prevent further player control.
        GetComponent<Movement>().enabled = false;

        // Load next level after a short delay.
        Invoke("NextLevel", delay);
    }

    /// <summary>
    /// Initiates the crash sequence, stopping current audio and playing crash effects.
    /// </summary>
    private void StartCrashSequence()
    {
        ToggleControls();           // Disable further input to prevent interference.
        audioSource.Stop();         // Stop any current audio.
        audioSource.PlayOneShot(crashSFX);      // Play the crash sound effect.
        crashParticles.Play();      // Start crash particle effects.

        // Disable the movement script to prevent further player control.
        GetComponent<Movement>().enabled = false;

        // Reload current level after a short delay.
        Invoke("ReloadLevel", delay);
    }

    /// <summary>
    /// Loads the next scene in the build index, or loops back to the first scene if at the last.
    /// </summary>
    private void NextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = ++currentScene;

        // Loop back to the first scene if the next scene index exceeds build settings.
        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }

    /// <summary>
    /// Reloads the currently active scene.
    /// </summary>
    private void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    /// <summary>
    /// Toggles whether the player can control the GameObject.
    /// </summary>
    private void ToggleControls()
    {
        isControllable = !isControllable;
    }
}
