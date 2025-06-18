using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class LeafDrag : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero; // Revert to direct velocity management
    private Vector3 targetPosition;
    private Vector3 offset;

    [Header("Spring Settings")]
    public float angularFrequency = 15f;
    [Range(0f, 2f)]
    public float dampingRatio = 0.8f;

    void Start()
    {
        targetPosition = transform.position;
    }

    // Public method to initiate drag
    public void StartDrag(Vector3 mousePosition)
    {
        targetPosition = transform.position;
        offset = transform.position - mousePosition;
    }

    void Update()
    {
        // Only update position if dragging is active (this script is enabled)
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;

        targetPosition = mouse + offset;

        // Apply spring to transform.position
        Vector3 pos = transform.position;
        ApplyDampedSpring(ref pos, ref velocity, targetPosition, angularFrequency, dampingRatio);
        transform.position = pos;
    }

    public Vector3 GetCurrentVelocity()
    {
        Debug.Log($"velocity during release: {velocity}");
        return velocity;
    }

    static void ApplyDampedSpring(ref Vector3 position, ref Vector3 velocity, Vector3 targetPosition, float angularFreq, float dampingRatio)
    {
        float dt = Time.deltaTime;

        // f = damping term that counteracts velocity and maintains critical damping balance
        float dampingFactor = 1f + 2f * dt * dampingRatio * angularFreq;
        float angularFreqSq = angularFreq * angularFreq;
        float dtAngularSq = dt * angularFreqSq;

        // invDet normalizes the update, stabilizing for large dt
        float invDeterminant = 1f / (dampingFactor + dtAngularSq);

        // Difference from target influences spring pull
        Vector3 displacement = position - targetPosition;

        // v_new = (oldVelocity + springForce * dt) / normalization
        Vector3 newVelocity = (velocity + displacement * dtAngularSq) * invDeterminant;

        // position update includes spring and velocity contributions, normalized
        position = targetPosition + (displacement + newVelocity * dt) * invDeterminant;

        velocity = newVelocity;
    }
}