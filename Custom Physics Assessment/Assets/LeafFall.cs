using UnityEngine;

public class LeafFall : MonoBehaviour
{
    [Header("Fall Settings")]
    public float gravity = 1.5f;
    public float horizontalDriftStrength = 1f;
    public float driftFrequency = 1f;
    public float rotationSpeed = 100f;
    public float groundY = -4f;
    public AnimationCurve fallSpeedCurve = AnimationCurve.Linear(0, 1, 1, 1);
    public float maxSpeedTreshold = 10f; // NEW: Maximum speed the leaf can fall
    public float speedDecayRate = 2f; // NEW: How quickly speed reduces when over maxFallSpeed

    private Vector3 velocity = Vector3.zero;
    private float time;
    private bool isGrounded = false;

    void OnEnable() // Reset on enable
    {
        isGrounded = false;
        // velocity is now set by SetInitialVelocity, so we don't zero it out here
        time = Random.Range(0f, Mathf.PI * 2f);
    }

    // Public method to set the initial velocity when falling begins
    public void SetInitialVelocity(Vector3 initialVelocity)
    {
        this.velocity = initialVelocity;
    }

    void Update()
    {
        // If already on the ground, do nothing (LeafController will disable this script)
        if (isGrounded)
            return;

        // Falling behavior - apply gravity and drift
        time += Time.deltaTime;
        velocity.y -= gravity * Time.deltaTime;
        // Horizontal drift now adds to current velocity for smoother transitions
        velocity.x += Mathf.Sin(time * driftFrequency) * horizontalDriftStrength * Time.deltaTime;

        // NEW: Check for max speed and gradually reduce it if necessary
        float currentSpeed = velocity.magnitude;
        Debug.Log($"Current speed: {currentSpeed}");
        if (currentSpeed > maxSpeedTreshold)
        {
            // Lerp the velocity magnitude towards maxFallSpeed, preserving direction
            velocity = Vector3.Lerp(velocity, velocity.normalized * maxSpeedTreshold, speedDecayRate * Time.deltaTime);
        }

        // Apply overall fall speed curve based on height
        float normalizedY = Mathf.InverseLerp(5f, groundY, transform.position.y);
        float fallMult = fallSpeedCurve.Evaluate(normalizedY);
        Vector3 next = transform.position + velocity * Time.deltaTime * fallMult;

        // Check for ground collision
        if (next.y <= groundY)
        {
            next.y = groundY;
            velocity = Vector3.zero; // Stop all movement
            isGrounded = true;
        }

        transform.position = next;
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}