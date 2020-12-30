using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameTile tileFrom, tileTo;
    Vector3 positionFrom, positionTo;
    float progress;

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
        Debug.Assert(tile.NextTileOnPath != null, "Nowhere to go!", this);
        tileFrom = tile;
        tileTo = tile.NextTileOnPath;
        positionFrom = tileFrom.transform.localPosition;
        positionTo = tileTo.transform.localPosition;
        progress = 0f;
        //transform.localPosition = tile.transform.localPosition;
    }
    public bool GameUpdate()
    {
        //transform.localPosition += Vector3.forward * Time.deltaTime;
        progress += Time.deltaTime;
        while (progress >= 1f)
        {
            tileFrom = tileTo;
            tileTo = tileFrom.NextTileOnPath;
            if (tileTo == null)
            {
                OriginFactory.Reclaim(this);
                return false;
            }
            positionFrom = positionTo;
            positionTo = tileTo.transform.localPosition;
            progress -= 1f;
        }
        transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, progress);
        return true;
    }
}