using UnityEngine;

public class Paddle : MonoBehaviour
{
    private bool isDragging = false; // Is the player dragging the paddle?
    private Vector3 dragOffset; // Offset between the mouse position and paddle's position
    private Camera mainCamera; // Reference to the main camera for converting mouse position

    [Header("Paddle Settings")]
    public float minX = -7f; // Minimum X position limit
    public float maxX = 7f;  // Maximum X position limit

    private void Start()
    {
        mainCamera = Camera.main; // Get the main camera reference
    }

    private void Update()
    {
        // Handle mouse input and dragging logic
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast from the mouse position to check if it hits the paddle
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // If the paddle is clicked, start dragging
                isDragging = true;

                // Calculate the offset between the mouse position and the paddle's position
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                dragOffset = transform.position - mousePosition;
            }
        }

        // If dragging is active, move the paddle
        if (isDragging)
        {
            DragPaddle();
        }

        // Stop dragging when the left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private void DragPaddle()
    {
        // Get the current mouse position in world coordinates
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the new X position based on the mouse position and offset
        float newXPosition = mousePosition.x + dragOffset.x;

        // Clamp the new X position within the defined minX and maxX limits
        newXPosition = Mathf.Clamp(newXPosition, minX, maxX);

        // Set the new position of the paddle, keeping the Y and Z position unchanged
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
    }
}