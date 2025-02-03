using UnityEngine;

/// <summary>
/// Oscillator moves the GameObject back and forth between its starting position and an offset position.
/// The movement is smoothly interpolated using a PingPong value combined with Lerp.
/// </summary>
public class Oscillator : MonoBehaviour
{
    // The offset by which the GameObject will move from its start position.
    [SerializeField] private Vector3 movementVector;

    // The speed factor that controls the oscillation rate.
    [SerializeField] private float speed;

    // Cached start position of the GameObject.
    private Vector3 startPosition;

    // End position calculated based on the start position and movementVector.
    private Vector3 endPosition;
    
    // A factor that represents the current position in the oscillation.
    private float movementFactor;    
    
    /// <summary>
    /// Called on the frame when the script is enabled.
    /// Caches the start position and calculates the end position.
    /// </summary>
    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + movementVector;
    }
    
    /// <summary>
    /// Called once per frame.
    /// Updates the GameObject's position by interpolating between the start and end positions.
    /// </summary>
    void Update()
    {
        // Compute a value that oscillates between 0 and 1 using PingPong.
        movementFactor = Mathf.PingPong(Time.time * speed, 1f);

        // Smoothly interpolate the position between the start and end positions based on movementFactor.
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor);
    }
}
