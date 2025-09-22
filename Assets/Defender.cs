using UnityEngine;

public class Defender : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of defender movement
    public Vector3 moveLimits;    // Z limits to confine movement

    public Camera playerCamera;   // Camera associated with this defender
    public string horizontalInput = "Horizontal";  // Input axis for X movement
    public string forwardBackwardInput = "Vertical";  // Input axis for forward/backward movement

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Get movement input from assigned axes
        float moveX = Input.GetAxis(horizontalInput);  // Horizontal movement (left/right)
        float moveZ = Input.GetAxis(forwardBackwardInput);  // Forward/backward movement

        // Calculate new position based on input
        Vector3 movement = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + movement;

        // Clamp movement to the defender's side of the field
        newPosition.x = Mathf.Clamp(newPosition.x, -moveLimits.x, moveLimits.x);
        newPosition.z = Mathf.Clamp(newPosition.z, -moveLimits.z, moveLimits.z);  // Now clamped on Z-axis

        // Update position
        transform.position = newPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Reflect the ball's velocity upon collision
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                Vector3 reflection = Vector3.Reflect(ballRb.velocity, collision.contacts[0].normal);
                ballRb.velocity = reflection;
            }
        }
    }
}
