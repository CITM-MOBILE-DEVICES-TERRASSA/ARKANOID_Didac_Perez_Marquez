using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Ball Settings")]
    public float speed = 2f;
    public float speedIncreaseFactor = 1.02f;
    public float maxSpeed = 4f;
    public Vector2 initialDirection;
    private Rigidbody2D rb;
    public Manager manager;

    public GameObject player;

    private void Start()
    {

        if (manager == null)
        {
            manager = FindObjectOfType<Manager>();
        }

        rb = GetComponent<Rigidbody2D>();
        Invoke("LaunchBall", 1f);
    }

    private void LaunchBall()
{
    float randomAngle = Random.Range(-30f, 30f);
    
    float angleInRadians = randomAngle * Mathf.Deg2Rad;

    Vector2 initialDirection = new Vector2(Mathf.Sin(angleInRadians), -Mathf.Cos(angleInRadians)).normalized;

    rb.velocity = initialDirection * speed;
}

    private void ResetBall()
    {
        transform.position = new Vector3(0,player.transform.position.y+3,0);
        rb.velocity = Vector2.zero;
        speed = 2f;
        Invoke("LaunchBall", 1f);
    }

    private void Update()
    {
        if (transform.position.y<player.transform.position.y-6)
        {
            Manager.Instance.OnLifeLost(-1);
            ResetBall();
        }
    }
   private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            if (speed < maxSpeed)
            {
                speed *= speedIncreaseFactor;

                Vector2 currentDirection = rb.velocity.normalized;

                rb.velocity = currentDirection * speed;
            }

            AudioManager.instance.PlaySound("ballHitsPaddle");
        }
        else if (collision.gameObject.CompareTag("Brick"))
        {
            AudioManager.instance.PlaySound("ballHitsBrick");
        }
        else
        {
            AudioManager.instance.PlaySound("ballHitsBarrier");
        }

        CorrectBallDirection();
    }

    private void CorrectBallDirection()
    {
        float minVelocity = 0.2f;

        Vector2 velocity = rb.velocity;

        if (Mathf.Abs(velocity.x) < minVelocity)
        {
            velocity.x = Mathf.Sign(velocity.x) * minVelocity;
        }
        if (Mathf.Abs(velocity.y) < minVelocity)
        {
            velocity.y = Mathf.Sign(velocity.y) * minVelocity;
        }

        rb.velocity = velocity;
    }
}