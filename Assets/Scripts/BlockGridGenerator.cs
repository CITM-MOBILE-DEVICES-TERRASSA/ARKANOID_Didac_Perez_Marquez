using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BlockGridGenerator : MonoBehaviour
{

    [System.Serializable]
    public class BlockListWrapper
    {
        public List<BlockData> blocks;
    }

    // Singleton instance
    public static BlockGridGenerator Instance { get; private set; }

    [Header("Configuración de la cuadrícula")]
    public GameObject blockPrefab; // Prefab del bloque
    public float blockSpacing = 0.2f; // Espacio entre bloques
    public Vector2 areaSize = new Vector2(10, 5); // Tamaño del área (ancho, alto)
    public Vector2 blockSize = new Vector2(1, 0.5f); // Tamaño de cada bloque (ancho, alto)
    [Range(0f, 1f)] public float hardRowChance = 0.1f; // Probabilidad de que toda la fila sea de ladrillos duros

    [Header("Configuración de color")]
    public Color baseColor = Color.blue; // Color base seleccionable desde el Inspector
    public Color gizmoColor = Color.green; // Color del gizmo en la escena

    private void Awake()
    {
        // Singleton pattern: Ensure only one instance of BlockGridGenerator exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        Instance = this;
    }

    public void GenerateBlockGrid()
    {
        ClearExistingBlocks();

        int numberOfColumns = Mathf.FloorToInt((areaSize.x + blockSpacing) / (blockSize.x + blockSpacing));
        int numberOfRows = Mathf.FloorToInt((areaSize.y + blockSpacing) / (blockSize.y + blockSpacing));

        int totalBricks = numberOfColumns * numberOfRows;
        Manager.Instance.SetTotalBricks(totalBricks);

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

    public void SaveBlockData()
    {
        // Create a list to store data for all blocks that are still alive
        List<BlockData> blockDataList = new List<BlockData>();

        // Iterate over each child block (only existing blocks)
        foreach (Transform child in transform)
        {
            // Get the Brick component from the block
            Brick brick = child.GetComponent<Brick>();
            
            // If the block exists and is still alive, save its data
            if (brick != null && brick.health > 0) // Only save blocks that still have health
            {
                // Gather the block's position, type, and color
                Vector3 position = child.position;
                bool isHard = brick.health > 1; // If the health is more than 1, it's a hard brick
                Color color = child.GetComponent<Renderer>().material.color;

                // Add this block's data to the list
                blockDataList.Add(new BlockData(position, isHard, color));
            }
        }

        // Convert the block data list to JSON format
        string json = JsonUtility.ToJson(new BlockListWrapper { blocks = blockDataList });

        // Save the JSON string to PlayerPrefs (or a file if needed)
        PlayerPrefs.SetString("SavedBlocks", json);
        PlayerPrefs.Save();
    }

    public void LoadBlockData()
    {
        // Check if there is saved block data
        if (PlayerPrefs.HasKey("SavedBlocks"))
        {
            // Get the saved block data from PlayerPrefs
            string json = PlayerPrefs.GetString("SavedBlocks");

            // Convert the JSON string back into a list of block data
            BlockListWrapper blockListWrapper = JsonUtility.FromJson<BlockListWrapper>(json);
            List<BlockData> blockDataList = blockListWrapper.blocks;

            // Clear any existing blocks in the scene
            ClearExistingBlocks();

            // Instantiate each saved block in the scene
            foreach (BlockData data in blockDataList)
            {
                // Create a block using the saved data
                GameObject block = BrickFactory.CreateBrick(blockPrefab, data.position, blockSize, data.isHardBrick, transform);
                block.GetComponent<Renderer>().material.color = data.color;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
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