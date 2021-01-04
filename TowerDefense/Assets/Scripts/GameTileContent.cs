using UnityEngine;

public enum GameTileContentType
{
    Empty, Destination, Wall, SpawnPoint, Tower
}

public class GameTileContent : MonoBehaviour
{
    [SerializeField]
    GameTileContentType type = default;
    public GameTileContentType Type => type;
    GameTileContentFactory originFactory;
    public GameTileContentFactory OriginFactory
    {
        get => originFactory;
        set {
            Debug.Assert(originFactory == null, "Redefined origin factory");
            originFactory = value;
        }
    }
    public void Recycle()
    {
        originFactory.Reclaim(this);
    }
    public bool BlocksPath => Type == GameTileContentType.Wall || Type == GameTileContentType.Tower;
}