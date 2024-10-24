using UnityEngine;

public class Brick : MonoBehaviour
{
    [Header("Propiedades del ladrillo")]
    public int health;
    private int points;
    
    private BlockGridGenerator blockGridGenerator;

    [Header("Propiedades del PowerUp")]
    public GameObject pickupPrefab;
    [Range(0f, 1f)]
    public float pickupSpawnChance = 0.5f;

    public void InitializeBrick(bool isHardBrick)
    {
        if (isHardBrick)
        {
            health = 2;
            points = 20;
        }
        else
        {
            health = 1;
            points = 10;
        }
    }

    private void Start()
    {
        blockGridGenerator = FindObjectOfType<BlockGridGenerator>();
    }

    public void TakeDamage()
    {
        health--;

        gameObject.GetComponent<Renderer>().material.color = blockGridGenerator.baseColor;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (Random.value <= pickupSpawnChance)
        {
            SpawnPickup();
        }

        Destroy(gameObject);

        Manager.Instance.totalBricks-=1;

        Manager.Instance.OnBlockDestroyed(points);
    }

    private void SpawnPickup()
    {
        Instantiate(pickupPrefab, transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ball")){
            TakeDamage();
        }
    }
}