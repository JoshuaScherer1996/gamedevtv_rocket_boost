using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
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
                break;
            default:
                Debug.Log("You crashed");
                ReloadLevel();
                break;
        }
    }

    // Method that loads the current scene.
    private void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
