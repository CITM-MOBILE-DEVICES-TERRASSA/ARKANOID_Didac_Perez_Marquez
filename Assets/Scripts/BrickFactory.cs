using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickFactory
{
    public static GameObject CreateBrick(GameObject prefab, Vector2 position, Vector2 blockSize, bool isHardBrick, Transform parent)
    {
        // Instancia el ladrillo en la posición deseada y lo asigna al padre (la cuadrícula)
        GameObject brick = Object.Instantiate(prefab, position, Quaternion.identity, parent);

        // Ajusta el tamaño del bloque
        brick.transform.localScale = new Vector3(blockSize.x, blockSize.y, 1);

        // Obtén el componente Brick y ajusta sus propiedades
        Brick brickComponent = brick.GetComponent<Brick>();
        if (brickComponent != null)
        {
            brickComponent.InitializeBrick(isHardBrick);
        }

        return brick;
    }
}
