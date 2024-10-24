using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BlockData
{
    public Vector3 position;
    public bool isHardBrick;
    public Color color;

    public BlockData(Vector3 pos, bool hard, Color col)
    {
        position = pos;
        isHardBrick = hard;
        color = col;
    }
}