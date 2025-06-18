using UnityEngine;

public class LeafDrag : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            if (hit != null && hit.gameObject == gameObject)
            {
                isDragging = true;
                offset = transform.position - mousePos;
            }
        }

        if (isDragging)
        {
            transform.position = mousePos + offset;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            // Optional: trigger bounce / release behavior here
        }
    }
}
