using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Ball Settings")]
    public float speed = 2f; // Velocidad inicial de la pelota
    public float speedIncreaseFactor = 1.02f; // Factor por el cual la velocidad aumenta después de un impacto
    public float maxSpeed = 4f; // Límite de velocidad máximo
    public Vector2 initialDirection; // Dirección inicial de la pelota
    private Rigidbody2D rb; // Referencia al componente Rigidbody2D
    public Manager manager;

    public GameObject player;

    private void Start()
    {

        if (manager == null)
        {
            manager = FindObjectOfType<Manager>(); // Encuentra GameManager en la escena
        }

        rb = GetComponent<Rigidbody2D>(); // Obtener el componente Rigidbody2D
        Invoke("LaunchBall", 1f);
    }

    private void LaunchBall()
{
    // Generate a random angle between -30 and 30 degrees
    float randomAngle = Random.Range(-30f, 30f);
    
    // Convert the angle to radians
    float angleInRadians = randomAngle * Mathf.Deg2Rad;

    // Calculate the direction based on the angle
    Vector2 initialDirection = new Vector2(Mathf.Sin(angleInRadians), -Mathf.Cos(angleInRadians)).normalized;

    // Apply the initial velocity in the calculated direction
    rb.velocity = initialDirection * speed; // Set the ball's velocity
}

    private void ResetBall()
    {
        // Resetear posición y velocidad de la pelota
        transform.position = new Vector3(0,player.transform.position.y+3,0); // Resetear a la posición central
        rb.velocity = Vector2.zero; // Detener la pelota
        speed = 2f; // Restablecer la velocidad inicial
        Invoke("LaunchBall", 1f); // Relanzar la pelota con un retraso de 1 segundo
    }

    private void Update()
    {
        // Verificar si la pelota ha salido de los límites de juego
        if (transform.position.y<player.transform.position.y-6) // Suponiendo que -6 es fuera del área de juego
        {
            Manager.Instance.OnLifeLost(-1);
            ResetBall();
        }
    }
   private void OnCollisionEnter2D(Collision2D collision)
    {
        // Incrementar la velocidad después de un impacto, si es menor al límite máximo
        if (collision.gameObject.CompareTag("Paddle"))
        {
            if (speed < maxSpeed)
            {
                speed *= speedIncreaseFactor; // Aumentar la velocidad

                // Obtener la dirección actual de la pelota y normalizarla (para que solo afecte la magnitud de la velocidad)
                Vector2 currentDirection = rb.velocity.normalized;

                // Aplicar la nueva velocidad sin alterar la dirección
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

        // Fix to prevent ball from getting stuck on X or Y axis
        CorrectBallDirection();
    }

    private void CorrectBallDirection()
    {
        // Minimum value for velocity components to avoid perfect horizontal/vertical movement
        float minVelocity = 0.2f;

        Vector2 velocity = rb.velocity;

        // Check if the X or Y velocity is too small, meaning it's close to being stuck on that axis
        if (Mathf.Abs(velocity.x) < minVelocity)
        {
            // Adjust the X component slightly to ensure the ball doesn't move perfectly vertically
            velocity.x = Mathf.Sign(velocity.x) * minVelocity;
        }
        if (Mathf.Abs(velocity.y) < minVelocity)
        {
            // Adjust the Y component slightly to ensure the ball doesn't move perfectly horizontally
            velocity.y = Mathf.Sign(velocity.y) * minVelocity;
        }

        // Apply the corrected velocity back to the ball
        rb.velocity = velocity;
    }
}