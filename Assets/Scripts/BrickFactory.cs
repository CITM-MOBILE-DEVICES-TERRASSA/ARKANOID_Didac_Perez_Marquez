using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickFactory
{
    public static GameObject CreateBrick(GameObject prefab, Vector2 position, Vector2 blockSize, bool isHardBrick, Transform parent)
    {
        GameObject brick = Object.Instantiate(prefab, position, Quaternion.identity, parent);

        brick.transform.localScale = new Vector3(blockSize.x, blockSize.y, 1);

        Brick brickComponent = brick.GetComponent<Brick>();
        if (brickComponent != null)
        {
            brickComponent.InitializeBrick(isHardBrick);
        }

        return brick;
    }
}
