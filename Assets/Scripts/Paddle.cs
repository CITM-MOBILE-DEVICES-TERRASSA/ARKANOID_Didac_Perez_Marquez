using UnityEngine;

public class Paddle : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 dragOffset;
    private Camera mainCamera;

    [Header("Paddle Settings")]
    public float minX = -7f;
    public float maxX = 7f;
    public bool AiControlledPaddle=false;
    private Ball ball;

    private void Start()
    {
        mainCamera = Camera.main;
        ball = FindAnyObjectByType<Ball>();
    }

    private void Update()
    {
        if(AiControlledPaddle){
            AiPaddleMovement();
        }
        else{
            HandleMouseInput();
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;

                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                dragOffset = transform.position - mousePosition;
            }
        }

        if (isDragging)
        {
            DragPaddle();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private void DragPaddle()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        float newXPosition = mousePosition.x + dragOffset.x;

        newXPosition = Mathf.Clamp(newXPosition, minX, maxX);

        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
    }

    public void AiPaddleMovement()
    {
        transform.position = new Vector3(ball.transform.position.x, transform.position.y, transform.position.z);
    }

    public void ChangeMovementType()
    {
        AiControlledPaddle=!AiControlledPaddle;
        AudioManager.instance.PlaySound("menuSelect");
    }
}