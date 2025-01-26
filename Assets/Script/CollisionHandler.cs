using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // Declaring the variables.
    [SerializeField] private float delay = 2f;

    // Switch statements uses the tag of the object we collided with.
    private void OnCollisionEnter(Collision other)
    {
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

    // Method executes the NextLevel Method with a delay.
    private void StartSuccessSequence()
    {
        GetComponent<Movement>().enabled = false;
        Invoke("NextLevel", delay);
    }

    // Method executes the ReloadLevel Method with a delay.
    private void StartCrashSequence()
    {
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", delay);
    }

    // Method loads the next level based on the build index.
    private void NextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = ++currentScene;

        // Resets the scene to the first one if the player finishes the last level.
        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }

    // Method loads the current scene.
    private void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
