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

    public static BlockGridGenerator Instance { get; private set; }

    [Header("Configuración de la cuadrícula")]
    public GameObject blockPrefab;
    public float blockSpacing = 0.2f;
    public Vector2 areaSize = new Vector2(10, 5);
    public Vector2 blockSize = new Vector2(1, 0.5f);
    [Range(0f, 1f)] public float hardRowChance = 0.1f;

    [Header("Configuración de color")]
    public Color baseColor = Color.blue;
    public Color gizmoColor = Color.green;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
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
        List<BlockData> blockDataList = new List<BlockData>();

        foreach (Transform child in transform)
        {
            Brick brick = child.GetComponent<Brick>();
            
            if (brick != null && brick.health > 0)
            {
                Vector3 position = child.position;
                bool isHard = brick.health > 1;
                Color color = child.GetComponent<Renderer>().material.color;

                blockDataList.Add(new BlockData(position, isHard, color));
            }
        }

        string json = JsonUtility.ToJson(new BlockListWrapper { blocks = blockDataList });

        PlayerPrefs.SetString("SavedBlocks", json);
        PlayerPrefs.Save();
    }

    public void LoadBlockData()
    {
        if (PlayerPrefs.HasKey("SavedBlocks"))
        {
            string json = PlayerPrefs.GetString("SavedBlocks");

            BlockListWrapper blockListWrapper = JsonUtility.FromJson<BlockListWrapper>(json);
            List<BlockData> blockDataList = blockListWrapper.blocks;

            ClearExistingBlocks();

            foreach (BlockData data in blockDataList)
            {
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