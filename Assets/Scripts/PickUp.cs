using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // Enum to define pickup types
    public enum PickupType
    {
        ExtraLife,
        ExtraPoints,
        SpeedReduction
    }

    public PickupType pickupType; // Type of the pickup
    public Ball ball;

    // Start is called before the first frame update
    void Start()
    {
        ball = FindObjectOfType<Ball>();

        // Randomly assign a pickup type
        pickupType = (PickupType)Random.Range(0, 3); // 0, 1, or 2 for three types
    }

    // Update is called once per frame
    void Update()
    {
        // You can add any additional behavior for the pickup here if needed
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collision is with the player
        if (collider.gameObject.CompareTag("Paddle"))
        {
            // Handle the pickup based on its type
            switch (pickupType)
            {
                case PickupType.ExtraLife:
                    // Implement logic to give the player an extra life
                    Manager.Instance.OnLifeLost(1);
                    Debug.Log("Extra life");
                    break;
                case PickupType.ExtraPoints:
                    // Implement logic to give the player extra points
                    Manager.Instance.OnBlockDestroyed(30);
                    Debug.Log("30 extra points");
                    break;
                case PickupType.SpeedReduction:
                    // Implement logic to reduce the speed of the ball
                    ball.speed = 2f;
                    Debug.Log("Ball speed reduced");
                    break;
            }

            AudioManager.instance.PlaySound("powerUp");

            // Destroy the pickup after it has been collected
            Destroy(gameObject);
        }
    }
}