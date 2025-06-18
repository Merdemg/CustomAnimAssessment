using UnityEngine;

public class LeafStretch : MonoBehaviour
{
    [Header("Stretch Settings")]
    public float stretchFactor = 0.05f;
    public float maxStretch = 1.3f;

    // Speed thresholds
    public float startStretchSpeed = 5f;
    public float fullStretchSpeed = 20f;

    // Lerp smoothing speed (higher = faster response)
    public float stretchLerpSpeed = 15f;

    private Vector3 restScale;
    private Vector3 lastPosition;
    private Vector3 currentVelocity;
    private Vector3 targetScale;

    void Awake()
    {
        restScale = transform.localScale;
        lastPosition = transform.position;
        targetScale = restScale;
    }

    void Update()
    {
        // Compute velocity
        currentVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        float speed = currentVelocity.magnitude;

        // Map speed to stretch amount (1→maxStretch)
        float t = Mathf.InverseLerp(startStretchSpeed, fullStretchSpeed, speed);
        float s = Mathf.Lerp(1f, maxStretch, t);

        // Apply squash compensation
        float inv = 1f / Mathf.Sqrt(s);
        targetScale = new Vector3(restScale.x * s, restScale.y * inv, 1f);

        // Smoothly interpolates towards targetScale
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            stretchLerpSpeed * Time.deltaTime
        );
    }
}
