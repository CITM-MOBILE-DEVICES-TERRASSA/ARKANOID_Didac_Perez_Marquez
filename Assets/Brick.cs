using UnityEngine;

public class Brick : MonoBehaviour
{
    [Header("Brick Properties")]
    public int health; // Health of the brick
    private float timer = 2f;
    private int points;
    public GameManager gameManager;

    [Header("Pickup Properties")]
    public GameObject pickupPrefab; // Prefab for the pickup
    [Range(0f, 1f)]
    public float pickupSpawnChance = 0.5f; // Chance to spawn a pickup (0 to 1)

    // Método para inicializar las propiedades del ladrillo
    public void InitializeBrick(bool isHardBrick)
    {
        if (isHardBrick)
        {
            health = 2; // Ladrillo duro tiene 2 de vida
            points = 20;
        }
        else
        {
            health = 1; // Ladrillo normal tiene 1 de vida
            points = 10;
        }
    }

    private void Start()
    {
        // Buscar GameManager automáticamente si no está asignado
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>(); // Encuentra GameManager en la escena
        }
    }

    // Método para que el ladrillo reciba daño
    public void TakeDamage()
    {
        health--;

        // Cambiar color cuando recibe daño
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

        // Verificar si la vida es menor o igual a cero
        if (health <= 0)
        {
            Die(); // Llama al método Die
        }
    }

    private void Update()
    {
        timer -= 2 * Time.deltaTime;
    }

    // Método llamado cuando la vida del ladrillo llega a cero
    private void Die()
    {
        //Debug.Log($"{gameObject.name} ha sido destruido!");

        // Decide if a pickup should be spawned
        if (Random.value <= pickupSpawnChance)
        {
            SpawnPickup();
        }

        Destroy(gameObject); // Destruir el ladrillo

        gameManager.OnBlockDestroyed(points); // Notifica al GameManager que el bloque fue destruido
    }

    // Method to spawn a pickup
    private void SpawnPickup()
    {
        // Instantiate the pickup prefab at the brick's position with no rotation
        GameObject pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
    }

    // Método de Unity llamado cuando un colisionador entra en contacto con este objeto
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (timer <= 0)
        {
            timer = 2f;
            TakeDamage();
        }
    }
}