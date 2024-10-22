using UnityEngine;

public class Brick : MonoBehaviour
{
    [Header("Propiedades del ladrillo")]
    public int health; // Salud del ladrillo
    private int points; // Puntos otorgados al destruir el ladrillo
    public GameManager gameManager;
    
    private BlockGridGenerator blockGridGenerator;

    [Header("Propiedades del PowerUp")]
    public GameObject pickupPrefab; // Prefab para el power-up
    [Range(0f, 1f)]
    public float pickupSpawnChance = 0.5f; // Probabilidad de generar un power-up

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
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>(); // Encuentra GameManager si no está asignado
            blockGridGenerator = FindObjectOfType<BlockGridGenerator>(); // Encuentra GameManager si no está asignado
        }
    }

    // Método para que el ladrillo reciba daño
    public void TakeDamage()
    {
        health--;

        // Cambiar color cuando recibe daño
        gameObject.GetComponent<Renderer>().material.color = blockGridGenerator.baseColor;

        // Si la salud es menor o igual a cero, destruir el ladrillo
        if (health <= 0)
        {
            Die();
        }
    }

    // Método llamado cuando la vida del ladrillo llega a cero
    private void Die()
    {
        // Si corresponde, genera un power-up
        if (Random.value <= pickupSpawnChance)
        {
            SpawnPickup();
        }

        Destroy(gameObject); // Destruye el ladrillo

        gameManager.totalBricks-=1;

        gameManager.OnBlockDestroyed(points); // Notifica al GameManager que el bloque fue destruido
    }

    // Método para generar un power-up
    private void SpawnPickup()
    {
        Instantiate(pickupPrefab, transform.position, Quaternion.identity); // Instancia el power-up
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TakeDamage();
    }
}