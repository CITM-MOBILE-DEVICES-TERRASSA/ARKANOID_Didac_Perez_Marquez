using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickupType
    {
        ExtraLife,
        ExtraPoints,
        SpeedReduction
    }

    public PickupType pickupType;
    public Ball ball;

    void Start()
    {
        ball = FindObjectOfType<Ball>();

        pickupType = (PickupType)Random.Range(0, 3);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Paddle"))
        {
            switch (pickupType)
            {
                case PickupType.ExtraLife:
                    Manager.Instance.OnLifeLost(1);
                    Debug.Log("Extra life");
                    break;
                case PickupType.ExtraPoints:
                    Manager.Instance.OnBlockDestroyed(30);
                    Debug.Log("30 extra points");
                    break;
                case PickupType.SpeedReduction:
                    ball.speed = 2f;
                    Debug.Log("Ball speed reduced");
                    break;
            }

            AudioManager.instance.PlaySound("powerUp");

            Destroy(gameObject);
        }
    }
}