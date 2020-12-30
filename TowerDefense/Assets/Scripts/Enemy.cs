using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyFactory originFactory;
    public EnemyFactory OriginFactory
    {
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redifined origin factory!");
            originFactory = value;
        }
    }
    public void SpwanOn(GameTile tile)
    {
        transform.localPosition = tile.transform.localPosition;
    }
}