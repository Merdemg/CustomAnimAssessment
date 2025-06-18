using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class LeafDrag : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    private Vector3 restScale;
    private bool isDragging = false;
    private Vector3 offset;

    [Header("Spring Settings")]
    public float angularFrequency = 15f;
    [Range(0f, 2f)]
    public float dampingRatio = 0.8f;

    [Header("Stretch Settings")]
    public float stretchFactor = 0.05f;
    public float maxStretch = 1.3f;

    void Start()
    {
        restScale = transform.localScale;
        targetPosition = transform.position;
    }

    void Update()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;

        if (Input.GetMouseButtonDown(0))
        {
            var hit = Physics2D.OverlapPoint(mouse);
            if (hit && hit.transform == transform)
            {
                isDragging = true;
                offset = transform.position - mouse;
            }
        }

        if (isDragging)
        {
            targetPosition = mouse + offset;
        }

        // Apply spring regardless of dragging state
        Vector3 pos = transform.position;
        ApplyDampedSpring(ref pos, ref velocity, targetPosition, angularFrequency, dampingRatio);
        transform.position = pos;

        // Dynamic squash & stretch based on speed
        float speed = velocity.magnitude;
        float s = 1f + Mathf.Clamp(speed * stretchFactor, 0f, maxStretch - 1f);
        float inv = 1f / Mathf.Sqrt(s);
        transform.localScale = new Vector3(restScale.x * s, restScale.y * inv, 1f);

        if (Input.GetMouseButtonUp(0) && isDragging)
            isDragging = false;
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
