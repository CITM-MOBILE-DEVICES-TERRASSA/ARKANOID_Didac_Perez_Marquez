using UnityEngine;
using UnityEngine.SceneManagement; // For restarting or quitting the game

[System.Serializable]
public class BlockData
{
    public Vector3 position; // Position of the block
    public bool isHardBrick;  // Whether the block is hard or soft
    public Color color;       // Color of the block

    public BlockData(Vector3 pos, bool hard, Color col)
    {
        position = pos;
        isHardBrick = hard;
        color = col;
    }
}