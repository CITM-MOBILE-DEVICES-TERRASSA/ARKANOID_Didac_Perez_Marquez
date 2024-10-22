using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState
{
    public int currentScore;
    public int totalBricks;
    public int lives;
    public string currentScene; // Nombre de la escena actual
    public List<BlockData> blockDataList; // Lista para almacenar la información de cada bloque

    [System.Serializable]
    public class BlockData
    {
        public Vector3 position;
        public int health;
        public Color color;
        public bool isDestroyed;
    }
}