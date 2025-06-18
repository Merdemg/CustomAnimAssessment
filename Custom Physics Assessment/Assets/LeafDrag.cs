using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LeafDrag : MonoBehaviour
{
    bool isDragging = false;
    Vector3 offset;
    Vector3 restScale;
    Vector3 velocity;
    float spring = 100f;      // Higher spring for snappier movement
    float damping = 15f;      // Smoothing factor
    float maxStretch = 1.3f;  // Max scale multiplier

    void Start()
    {
        restScale = transform.localScale;
    }

    void Update()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;

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
            Vector3 target = mouse + offset;
            Vector3 diff = target - transform.position;

            velocity += (diff * spring - velocity * damping) * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;

            // Dynamic stretch based on current speed
            float speed = velocity.magnitude;
            float stretchFactor = 1f + Mathf.Clamp(speed * 0.05f, 0f, maxStretch - 1f);
            float inv = 1f / Mathf.Sqrt(stretchFactor);
            transform.localScale = new Vector3(restScale.x * stretchFactor, restScale.y * inv, 1);
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
        }

        if (!isDragging)
        {
            // Smoothly decelerate to rest
            velocity = Vector3.Lerp(velocity, Vector3.zero, damping * Time.deltaTime);
            transform.position += velocity * Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, restScale, Time.deltaTime * 20f);
        }
    }
}
