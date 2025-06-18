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

    private Vector3 velocity = Vector3.zero;
    private float time;
    private bool isGrounded = false; // Made private again, exposed via IsGrounded()

    void OnEnable() // Reset on enable
    {
        isGrounded = false;
        velocity = Vector3.zero;
        time = Random.Range(0f, Mathf.PI * 2f); // Randomize start time for varied fall
    }

    void Update()
    {
        // If already on the ground, do nothing (LeafController will disable this script)
        if (isGrounded)
            return;

        // Falling behavior
        time += Time.deltaTime;
        velocity.y -= gravity * Time.deltaTime;
        velocity.x = Mathf.Sin(time * driftFrequency) * horizontalDriftStrength;

        float normalizedY = Mathf.InverseLerp(5f, groundY, transform.position.y);
        float fallMult = fallSpeedCurve.Evaluate(normalizedY);
        Vector3 next = transform.position + velocity * Time.deltaTime * fallMult;

        if (next.y <= groundY)
        {
            next.y = groundY;
            velocity = Vector3.zero;
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