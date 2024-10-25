using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOrientationManager : MonoBehaviour
{
    private Paddle paddle;
    private Ball ball;
    public BlockGridGenerator blockGridGenerator;
    public GameObject[] borders;

    private Vector2 lastScreenSize;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        lastScreenSize = new Vector2(Screen.width, Screen.height);
        paddle = FindAnyObjectByType<Paddle>();
        ball = FindAnyObjectByType<Ball>();
        AdjustLayout();
    }

    void Update()
    {
        if (Screen.width != lastScreenSize.x || Screen.height != lastScreenSize.y)
        {
            lastScreenSize = new Vector2(Screen.width, Screen.height);
            AdjustLayout();
        }
    }

    void AdjustLayout()
    {
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        AdjustPaddle();
        AdjustBall();
        AdjustBorders();
        AdjustBrickGrid();
    }

    void AdjustBall()
    {
        Vector3 ballPosition = ball.transform.position;
        ballPosition.x = 0f;
        ball.transform.position=ballPosition;
    }

    void AdjustPaddle()
    {
        Vector3 leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));
        Vector3 rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));
        Paddle paddleScript = paddle.GetComponent<Paddle>();
        paddleScript.minX = leftEdge.x;
        paddleScript.maxX = rightEdge.x;

        Vector3 paddlePosition = paddle.transform.position;
        if (paddlePosition.x < paddleScript.minX)
        {
            paddlePosition.x = paddleScript.minX;
        }
        else if (paddlePosition.x > paddleScript.maxX)
        {
            paddlePosition.x = paddleScript.maxX;
        }

        paddle.transform.position = paddlePosition;
    }

    void AdjustBorders()
    {
        Vector3 leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));
        Vector3 rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));
        Vector3 topEdge = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 1, 0));

        borders[0].transform.position = new Vector3(leftEdge.x, 0, 0);
        borders[1].transform.position = new Vector3(rightEdge.x, 0, 0);
        borders[2].transform.position = new Vector3(0, topEdge.y, 0);
    }

    void AdjustBrickGrid()
    {
        Vector3 leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));
        Vector3 rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));
        float screenWidth = rightEdge.x - leftEdge.x;
        blockGridGenerator.areaSize = new Vector2(screenWidth, blockGridGenerator.areaSize.y);
        RepositionBricksToFitGrid(leftEdge.x, rightEdge.x);
    }

    void RepositionBricksToFitGrid(float leftBound, float rightBound)
    {
        float gridWidth = rightBound - leftBound;
        float startX = leftBound + (blockGridGenerator.blockSize.x / 2);
        int numberOfColumns = Mathf.FloorToInt(gridWidth / (blockGridGenerator.blockSize.x + blockGridGenerator.blockSpacing));
        float totalBlockWidth = (numberOfColumns * blockGridGenerator.blockSize.x) + ((numberOfColumns - 1) * blockGridGenerator.blockSpacing);
        float horizontalOffset = (gridWidth - totalBlockWidth) / 2;

        int column = 0;

        foreach (Transform child in blockGridGenerator.transform)
        {
            Brick brick = child.GetComponent<Brick>();
            if (brick != null && brick.health > 0)
            {
                float newXPosition = startX + (column * (blockGridGenerator.blockSize.x + blockGridGenerator.blockSpacing)) + horizontalOffset;
                Vector3 newPosition = new Vector3(newXPosition, child.localPosition.y, child.localPosition.z);
                child.localPosition = newPosition;

                column++;

                if (column >= numberOfColumns)
                {
                    column = 0;
                }
            }
        }
    }
}