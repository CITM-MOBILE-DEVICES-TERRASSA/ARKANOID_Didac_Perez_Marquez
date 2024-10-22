using UnityEngine;
using UnityEditor;

public class BlockGridGenerator : MonoBehaviour
{
    [Header("Configuración de la cuadrícula")]
    public GameObject blockPrefab; // Prefab del bloque
    public float blockSpacing = 0.2f; // Espacio entre bloques
    public Vector2 areaSize = new Vector2(10, 5); // Tamaño del área (ancho, alto)
    public Vector2 blockSize = new Vector2(1, 0.5f); // Tamaño de cada bloque (ancho, alto)
    [Range(0f, 1f)] public float hardRowChance = 0.1f; // Probabilidad de que toda la fila sea de ladrillos duros

    [Header("Configuración de color")]
    public Color baseColor = Color.blue; // Color base seleccionable desde el Inspector
    public Color gizmoColor = Color.green; // Color del gizmo en la escena

    private void Start()
    {
        
    }

    public void GenerateBlockGrid()
    {
        ClearExistingBlocks();

        int numberOfColumns = Mathf.FloorToInt((areaSize.x + blockSpacing) / (blockSize.x + blockSpacing));
        int numberOfRows = Mathf.FloorToInt((areaSize.y + blockSpacing) / (blockSize.y + blockSpacing));

        int totalBricks = numberOfColumns * numberOfRows;
        FindObjectOfType<GameManager>().SetTotalBricks(totalBricks);

        float totalBlockWidth = (numberOfColumns * blockSize.x) + ((numberOfColumns - 1) * blockSpacing);
        float totalBlockHeight = (numberOfRows * blockSize.y) + ((numberOfRows - 1) * blockSpacing);

        Vector3 startPosition = new Vector3(
            -totalBlockWidth / 2 + blockSize.x / 2,
            -totalBlockHeight / 2 + blockSize.y / 2,
            0
        );

        startPosition += transform.position;

        for (int row = 0; row < numberOfRows; row++)
        {
            bool isHardRow = Random.value < hardRowChance;
            Color rowColor = isHardRow ? Color.gray : GenerateVibrantColor();

            for (int column = 0; column < numberOfColumns; column++)
            {
                Vector3 position = new Vector3(
                    startPosition.x + column * (blockSize.x + blockSpacing),
                    startPosition.y + row * (blockSize.y + blockSpacing),
                    0
                );

                GameObject block = BrickFactory.CreateBrick(blockPrefab, position, blockSize, isHardRow, transform);
                block.GetComponent<Renderer>().material.color = rowColor;
            }
        }
    }

    private void ClearExistingBlocks()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private Color GenerateVibrantColor()
    {
        float hue, saturation, value;
        Color.RGBToHSV(baseColor, out hue, out saturation, out value);

        hue = Random.Range(hue - 0.1f, hue + 0.1f);
        saturation = Random.Range(0.8f, 1f);
        value = Random.Range(0.7f, 0.9f);

        return Color.HSVToRGB(hue, saturation, value);
    }

    // OnDrawGizmos will draw the area in the Scene view
    private void OnDrawGizmos()
    {
        // Set the Gizmo color
        Gizmos.color = gizmoColor;

        // Draw a wireframe cube representing the areaSize
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, areaSize.y, 1f));
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(BlockGridGenerator))]
    public class BlockGridGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            BlockGridGenerator blockGridGenerator = (BlockGridGenerator)target;

            if (GUILayout.Button("Regenerate Grid"))
            {
                blockGridGenerator.GenerateBlockGrid();
            }
        }
    }
#endif
}