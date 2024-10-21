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
    public GameManager gameManager;

    public GameObject player;

    private void Start()
    {

        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>(); // Encuentra GameManager en la escena
        }

        rb = GetComponent<Rigidbody2D>(); // Obtener el componente Rigidbody2D
        LaunchBall(); // Lanzar la pelota al inicio del juego
    }

    private void LaunchBall()
    {
        // Aplicar la velocidad inicial en la dirección especificada
        initialDirection=new Vector2(Random.Range(-1,1), -1).normalized;
        rb.velocity = initialDirection * speed;
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
            gameManager.OnLifeLost(-1);
            ResetBall();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Incrementar la velocidad después de un impacto, si es menor al límite máximo
        if (collision.gameObject.CompareTag("Paddle") && speed < maxSpeed)
        {
            speed *= speedIncreaseFactor; // Aumentar la velocidad

            // Obtener la dirección actual de la pelota y normalizarla (para que solo afecte la magnitud de la velocidad)
            Vector2 currentDirection = rb.velocity.normalized;

            // Aplicar la nueva velocidad sin alterar la dirección
            rb.velocity = currentDirection * speed;
        }
    }
}