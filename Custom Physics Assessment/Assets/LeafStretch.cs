using UnityEngine;

public class LeafStretch : MonoBehaviour
{
    [Header("Stretch Settings")]
    public float stretchFactor = 0.05f;
    public float maxStretch = 1.3f;

    // speed thresholds
    public float startStretchSpeed = 5f;   // speed where stretching begins
    public float fullStretchSpeed = 20f;   // speed at which maxStretch is reached

    private Vector3 restScale;
    private Vector3 lastPosition;
    private Vector3 currentVelocity;

    void Awake()
    {
        restScale = transform.localScale;
        lastPosition = transform.position;
    }

    void Update()
    {
        // compute velocity
        currentVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        float speed = currentVelocity.magnitude;

        // remap speed to stretch factor between 1 → maxStretch
        float t = Mathf.InverseLerp(startStretchSpeed, fullStretchSpeed, speed);
        float s = Mathf.Lerp(1f, maxStretch, t);

        // apply stretch and squash
        float inv = 1f / Mathf.Sqrt(s);
        transform.localScale = new Vector3(restScale.x * s, restScale.y * inv, 1f);
    }
}
