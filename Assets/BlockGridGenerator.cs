using UnityEngine;
using UnityEditor; // Import the UnityEditor namespace for the custom editor

public class BlockGridGenerator : MonoBehaviour
{
    [Header("Configuración de la cuadrícula")]
    public GameObject blockPrefab; // Prefab del bloque
    public float blockSpacing = 0.2f; // Espacio entre bloques
    public Vector2 areaSize = new Vector2(10, 5); // Tamaño del área (ancho, alto)
    public Vector2 blockSize = new Vector2(1, 0.5f); // Tamaño de cada bloque (ancho, alto)

    private void Start()
    {
        GenerateBlockGrid();
    }

    public void GenerateBlockGrid() // Make this method public
    {
        // Eliminar bloques existentes si los hay
        ClearExistingBlocks();

        // Calcula el número de filas y columnas basados en el área y el tamaño de los bloques
        int numberOfColumns = Mathf.FloorToInt((areaSize.x + blockSpacing) / (blockSize.x + blockSpacing));
        int numberOfRows = Mathf.FloorToInt((areaSize.y + blockSpacing) / (blockSize.y + blockSpacing));

        // Calcula el tamaño total que ocuparán los bloques con el espaciado
        float totalBlockWidth = (numberOfColumns * blockSize.x) + ((numberOfColumns - 1) * blockSpacing);
        float totalBlockHeight = (numberOfRows * blockSize.y) + ((numberOfRows - 1) * blockSpacing);

        // Calcula la posición inicial para centrar la cuadrícula en el área
        Vector3 startPosition = new Vector3(
            -totalBlockWidth / 2 + blockSize.x / 2,
            -totalBlockHeight / 2 + blockSize.y / 2,
            0
        );

        for (int row = 0; row < numberOfRows; row++)
        {
            // Determine if the entire row should be hard bricks (for this example, 10% chance)
            bool isHardRow = Random.value < 0.1f; // 10% chance for the entire row to be hard bricks
            
            Color rowColor = isHardRow ? Color.gray : GenerateVibrantColor(); // Use gray for hard rows, vibrant color for normal rows

            for (int column = 0; column < numberOfColumns; column++)
            {
                // Calcula la posición de cada bloque
                Vector3 position = new Vector3(
                    startPosition.x + column * (blockSize.x + blockSpacing),
                    startPosition.y + row * (blockSize.y + blockSpacing),
                    0
                );

                // Instancia el bloque y lo posiciona
                GameObject block = Instantiate(blockPrefab, position, Quaternion.identity, transform);
                block.transform.localScale = new Vector3(blockSize.x, blockSize.y, 1); // Ajusta el tamaño del bloque
                
                // Get the Brick component and initialize with the color
                Brick brick = block.GetComponent<Brick>();
                if (brick != null)
                {
                    brick.InitializeBrick(isHardRow); // Initialize the brick as hard or normal
                }
                
                // Set the color of the block
                block.GetComponent<Renderer>().material.color = rowColor; // Assign the row color (either gray or vibrant)
            }
        }
    }

    // Método para eliminar bloques existentes
    private void ClearExistingBlocks()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject); // Elimina cada bloque existente
        }
    }

    // Method to generate a blue-derived vibrant color
    private Color GenerateVibrantColor()
    {
        float hue = Random.Range(0.5f, 0.7f); // Restrict hue to the blue range
        float saturation = Random.Range(0.8f, 1f); // High saturation for vibrant color
        float lightness = Random.Range(0.7f, 0.9f); // Higher lightness for brightness

        return Color.HSVToRGB(hue, saturation, lightness); // Convert HSL to RGB
    }

#if UNITY_EDITOR // Ensure the following code is only compiled in the editor
    [CustomEditor(typeof(BlockGridGenerator))]
    public class BlockGridGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector but remove specific inputs
            DrawDefaultInspector();

            // Get a reference to the BlockGridGenerator script
            BlockGridGenerator blockGridGenerator = (BlockGridGenerator)target;

            // Add a button to regenerate the grid
            if (GUILayout.Button("Regenerate Grid"))
            {
                blockGridGenerator.GenerateBlockGrid(); // Call the method to regenerate the grid
            }
        }
    }
#endif
}