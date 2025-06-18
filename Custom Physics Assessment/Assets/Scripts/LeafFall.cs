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
    public float maxSpeedTreshold = 10f;
    public float speedDecayRate = 2f;
    public float upwardsSpeedDragRate = 50f;

    private Vector3 velocity = Vector3.zero;
    private float time;
    private bool isGrounded = false;

    float startY = 5f;

    void OnEnable()
    {
        isGrounded = false;
        time = Random.Range(0f, Mathf.PI * 2f);
        startY = transform.position.y;
    }

    // Public method to set the initial velocity when falling begins
    public void SetInitialVelocity(Vector3 initialVelocity)
    {
        this.velocity = initialVelocity;
    }

    void Update()
    {
        if (isGrounded)
            return;

        // Time-based drift
        time += Time.deltaTime;
        velocity.x += Mathf.Sin(time * driftFrequency) * horizontalDriftStrength * Time.deltaTime;

        // Inertia-based gravity modifier
        float g = -gravity;
        float vy = velocity.y;

        if (vy > 0f)
        {
            // Ascending → stronger gravity to push it back down
            g *= upwardsSpeedDragRate;
        }
        else if (vy < -maxSpeedTreshold * 0.5f)
        {
            // Fast falling → soften gravity as it nears terminal
            float factor = Mathf.InverseLerp(-maxSpeedTreshold, -maxSpeedTreshold * 0.5f, vy);
            g *= Mathf.Lerp(1f, 0.5f, factor);
        }

        // Apply to vertical velocity
        velocity.y += g * Time.deltaTime;

        // Cap overall speed smoothly (terminal velocity behavior)
        float currentSpeed = velocity.magnitude;
        if (currentSpeed > maxSpeedTreshold)
        {
            velocity = Vector3.Lerp(velocity, velocity.normalized * maxSpeedTreshold, speedDecayRate * Time.deltaTime);
        }

        // Flutter/fall curve
        float normalizedY = Mathf.InverseLerp(startY, groundY, transform.position.y);
        float fallMult = fallSpeedCurve.Evaluate(normalizedY);

        // Move and rotate
        Vector3 next = transform.position + velocity * Time.deltaTime * fallMult;
        if (next.y <= groundY)
        {
            next.y = groundY;
            velocity = Vector3.zero;
            isGrounded = true;
        }

        transform.position = next;
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime * velocity.x);
    }


    public bool IsGrounded()
    {
        return isGrounded;
    }
}