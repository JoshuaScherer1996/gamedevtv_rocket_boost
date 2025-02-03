using UnityEngine;
using UnityEngine.InputSystem;

public class QuitApplication : MonoBehaviour
{

    private void Update()
    {
        Quitting();
    }

    private void Quitting()
    {
        if (Keyboard.current.escapeKey.isPressed)
        {
            Application.Quit();
        }
    }
}
