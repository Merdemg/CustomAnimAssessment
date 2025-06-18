using UnityEngine;

public class LeafStretch : MonoBehaviour
{
    [Header("Stretch Settings")]
    public float stretchFactor = 0.05f;
    public float maxStretch = 1.3f;

    private Vector3 restScale;
    private Vector3 lastPosition;
    private Vector3 currentVelocity; // To store the calculated velocity

    void Awake()
    {
        restScale = transform.localScale;
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate velocity based on position change
        currentVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position; // Update last position for the next frame

        // Dynamic squash & stretch based on calculated speed
        float speed = currentVelocity.magnitude;
        float s = 1f + Mathf.Clamp(speed * stretchFactor, 0f, maxStretch - 1f);
        float inv = 1f / Mathf.Sqrt(s);
        transform.localScale = new Vector3(restScale.x * s, restScale.y * inv, 1f);
    }
}