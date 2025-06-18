using UnityEngine;

public class LeafFall : MonoBehaviour
{
    [Header("Fall Settings")]
    public float gravity = 1.5f;
    public float horizontalDriftStrength = 1f;
    public float driftFrequency = 1f;
    public float rotationSpeed = 100f;
    public float groundY = -4f;
    public AnimationCurve fallSpeedCurve;

    private Vector3 velocity;
    private float time;
    private bool isGrounded = false;

    private LeafDrag dragScript;

    void Start()
    {
        dragScript = GetComponent<LeafDrag>();
        time = Random.Range(0f, 2f * Mathf.PI); // for variation in drift
    }

    void Update()
    {
        if (dragScript != null && dragScript.enabled && !Input.GetMouseButton(0))
        {
            dragScript.enabled = false; // stop dragging once dropped
            velocity = Vector3.zero;
        }

        if (dragScript != null && dragScript.enabled)
            return;

        if (isGrounded)
            return;

        time += Time.deltaTime;

        // Simulate gravity with vertical acceleration
        velocity.y -= gravity * Time.deltaTime;

        // Apply horizontal sinusoidal drift
        velocity.x = Mathf.Sin(time * driftFrequency) * horizontalDriftStrength;

        // Modify fall speed with a curve
        float fallMultiplier = fallSpeedCurve.Evaluate(Mathf.Clamp01((transform.position.y + 5f) / 10f));
        Vector3 newPosition = transform.position + velocity * Time.deltaTime * fallMultiplier;

        // Stop at ground level
        if (newPosition.y <= groundY)
        {
            newPosition.y = groundY;
            velocity = Vector3.zero;
            isGrounded = true;
        }

        transform.position = newPosition;

        // Slowly rotate as it falls
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
