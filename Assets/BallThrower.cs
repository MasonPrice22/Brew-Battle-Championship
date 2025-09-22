using UnityEngine;

public class BallThrower : MonoBehaviour
{
    public GameObject Ball;            // The ball for this player (assign in Inspector)
    public Camera playerCamera;        // The camera for this player (assign in Inspector)
    public int playerID = 1;           // 1 for Player 1, 2 for Player 2

    private Rigidbody rb;
    private Vector2 startPos, endPos;
    private float startTime, endTime, swipeDistance, swipeTime;
    private bool holding, thrown;

    public float MaxBallSpeed = 350f;  // Maximum speed of the ball

    // Store each player's unique starting position
    public Vector3 startBallPosition;

    void Start()
    {
        rb = Ball.GetComponent<Rigidbody>();
        startBallPosition = Ball.transform.position;  // Save the starting position
        ResetBall();
    }

    public void ResetBall()
    {
        // Reset only this player's ball to its unique starting position
        Ball.transform.position = startBallPosition;
        Ball.transform.rotation = Quaternion.identity;

        // Stop movement and spinning
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Disable gravity until the ball is thrown
        rb.useGravity = false;

        thrown = false;
        holding = false;
    }

    void PickupBall()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = playerCamera.nearClipPlane * 5f;  // Adjust based on camera
        Ball.transform.position = playerCamera.ScreenToWorldPoint(mousePos);
    }

    void Update()
    {
        if (thrown) return;  // Ignore input if the ball has already been thrown

        if (holding) PickupBall();  // Move the ball with the mouse while holding

        // Detect input based on the correct player
        if ((playerID == 1 && Input.GetMouseButtonDown(0)) ||  // Player 1: Left Click
            (playerID == 2 && Input.GetMouseButtonDown(1)))    // Player 2: Right Click
        {
            // Raycast to see if the player clicked their ball
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == Ball.transform)
            {
                startTime = Time.time;
                startPos = Input.mousePosition;
                holding = true;
            }
        }
        else if ((playerID == 1 && Input.GetMouseButtonUp(0)) ||  // Player 1: Release Left Click
                 (playerID == 2 && Input.GetMouseButtonUp(1)))    // Player 2: Release Right Click
        {
            endTime = Time.time;
            endPos = Input.mousePosition;
            swipeDistance = (endPos - startPos).magnitude;
            swipeTime = endTime - startTime;

            if (swipeTime < 0.5f && swipeDistance > 30f)
            {
                ThrowBall();
            }
            else
            {
                ResetBall();  // Invalid swipe, reset the ball
            }
        }
    }

    void ThrowBall()
    {
        // Calculate throw direction
        Vector3 direction = playerCamera.ScreenToWorldPoint(new Vector3(
            endPos.x, endPos.y, playerCamera.nearClipPlane + 5f)) - Ball.transform.position;

        // Apply force to the ball
        direction.y += 1f;
        rb.AddForce(direction.normalized * MaxBallSpeed * 1.5f);

        // Enable gravity after throwing
        rb.useGravity = true;

        holding = false;
        thrown = true;

        // Reset the ball after 4 seconds
        Invoke("ResetBall", 3f);
    }
}
