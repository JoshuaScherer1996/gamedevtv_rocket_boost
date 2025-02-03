using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Monitors for the Escape key input and quits the application when pressed.
/// </summary>
public class QuitApplication : MonoBehaviour
{
    /// <summary>
    /// Called once per frame.
    /// Checks for the quit input.
    /// </summary>
    private void Update()
    {
        Quitting();
    }

    /// <summary>
    /// Checks if the Escape key is pressed and quits the application if true.
    /// </summary>
    private void Quitting()
    {
        // If the Escape key is pressed, quit the application.
        if (Keyboard.current.escapeKey.isPressed)
        {
            Application.Quit();
        }
    }
}
