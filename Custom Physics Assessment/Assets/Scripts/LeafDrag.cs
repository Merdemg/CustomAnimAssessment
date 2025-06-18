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

    static void ApplyDampedSpring(ref Vector3 p, ref Vector3 v, Vector3 target, float w, float z)
    {
        float dt = Time.deltaTime;
        float f = 1f + 2f * dt * z * w;
        float w2 = w * w;
        float dt_w2 = dt * w2;
        float invDet = 1f / (f + dt_w2);
        Vector3 diff = p - target;
        Vector3 newV = (v + diff * dt_w2) * invDet;
        p = target + (diff + newV * dt) * invDet;
        v = newV;
    }
}