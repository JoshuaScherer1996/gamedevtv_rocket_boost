using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

/// <summary>
/// Handles collision events for the attached GameObject, including crash and success sequences.
/// </summary>
public class CollisionHandler : MonoBehaviour
{
    // Declaring the variables.
    [SerializeField] private float delay = 2f;
    [SerializeField] private AudioClip crashSFX;
    [SerializeField] private AudioClip successSFX;
    [SerializeField] private ParticleSystem crashParticles;
    [SerializeField] private ParticleSystem successParticles;

    // Declaring the caches.
    private AudioSource audioSource;

    // Declaring the states.
    bool isControllable = true;
    bool isCollidable = true;

    /// <summary>
    /// Obtains the AudioSource reference on start.
    /// </summary>
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {
        RespondToDebugKeys();
    }

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
        // Ignore collisions if controls are already disabled (e.g., after a crash or success).
        if (!isControllable || !isCollidable) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("The object is friendly");
                break;
            case "Fuel":
                Debug.Log("The object is fuel");
                break;
            case "Finish":
                Debug.Log("The object is the finish line");
                StartSuccessSequence();
                break;
            default:
                Debug.Log("You crashed");
                StartCrashSequence();
                break;
        }
    }

    /// <summary>
    /// Initiates the success sequence, stopping current audio and playing success effects.
    /// </summary>
    private void StartSuccessSequence()
    {
        ToggleControls();
        audioSource.Stop();
        audioSource.PlayOneShot(successSFX);
        successParticles.Play();

        // Disable the movement script to prevent further control.
        GetComponent<Movement>().enabled = false;

        // Load next level after a short delay.
        Invoke("NextLevel", delay);
    }

    /// <summary>
    /// Initiates the crash sequence, stopping current audio and playing crash effects.
    /// </summary>
    private void StartCrashSequence()
    {
        ToggleControls();
        audioSource.Stop();
        audioSource.PlayOneShot(crashSFX);
        crashParticles.Play();

        // Disable the movement script to prevent further control.
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
