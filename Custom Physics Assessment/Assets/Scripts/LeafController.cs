using UnityEngine;

public class LeafController : MonoBehaviour
{
    [SerializeField] float inertiaMultiplier = 4f;

    private LeafDrag leafDrag;
    private LeafFall leafFall;

    private void Awake()
    {
        leafDrag = GetComponent<LeafDrag>();
        leafFall = GetComponent<LeafFall>();

        // Initially, the leaf should be falling
        SetState(LeafState.Falling);
    }

    private void Update()
    {
        // Check for mouse click to start dragging
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0f;

            var hit = Physics2D.OverlapPoint(mouse);
            if (hit && hit.transform == transform)
            {
                SetState(LeafState.Dragging);
            }
        }

        // If currently dragging, check for mouse release
        if (leafDrag.enabled && Input.GetMouseButtonUp(0))
        {
            // Get the current velocity from LeafDrag before switching to falling
            Vector3 currentDragVelocity = leafDrag.GetCurrentVelocity() * -inertiaMultiplier; // NEW: Get velocity from LeafDrag
            leafFall.SetInitialVelocity(currentDragVelocity); // NEW: Pass velocity to LeafFall
            SetState(LeafState.Falling);
        }

        // If the leaf is falling and reaches the ground, stop falling
        if (leafFall.enabled && leafFall.IsGrounded())
        {
            SetState(LeafState.Grounded);
        }
    }

    public void SetState(LeafState newState)
    {
        switch (newState)
        {
            case LeafState.Falling:
                leafDrag.enabled = false;
                leafFall.enabled = true;
                break;
            case LeafState.Dragging:
                leafDrag.enabled = true;
                leafFall.enabled = false;
                break;
            case LeafState.Grounded:
                leafDrag.enabled = false;
                leafFall.enabled = false;
                break;
        }
    }
}

public enum LeafState
{
    Falling,
    Dragging,
    Grounded
}