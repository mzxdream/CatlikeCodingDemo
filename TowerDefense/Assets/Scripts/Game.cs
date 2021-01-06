using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    Vector2Int boardSize = new Vector2Int(11, 11);
    [SerializeField]
    GameBoard board = default;
    [SerializeField]
    GameTileContentFactory tileContentFactory = default;
    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    //[SerializeField]
    //EnemyFactory enemyFactory = default;
    [SerializeField]
    WarFactory warFactory = default;
    //[SerializeField, Range(0.1f, 10f)]
    //float spawnSpeed = 1f;
    //float spawnProgress;
    GameBehaviorCollection enemies = new GameBehaviorCollection();
    GameBehaviorCollection nonEmenies = new GameBehaviorCollection();
    TowerType selectedTowerType;
    static Game instance;
    [SerializeField]
    GameScenario scenario = default;
    GameScenario.State activeScenario;
    [SerializeField, Range(0, 100)]
    int startingPlayerHealth = 10;

    int playerHealth;
    void Awake()
    {
        board.Initialize(boardSize, tileContentFactory);
        board.ShowGrid = true;
        activeScenario = scenario.Begin();
        playerHealth = startingPlayerHealth;
    }
    void OnValidate()
    {
        if (boardSize.x < 2)
        {
            boardSize.x = 2;
        }
        if (boardSize.y < 2)
        {
            boardSize.y = 2;
        }
    }
    void OnEnable()
    {
        instance = this;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleAlternativeTouch();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            board.ShowPaths = !board.ShowPaths;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            board.ShowGrid = !board.ShowGrid;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedTowerType = TowerType.Laser;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedTowerType = TowerType.Mortar;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            BeginNewGame();
        }
        //spawnProgress += spawnSpeed * Time.deltaTime;
        //while (spawnProgress >= 1f)
        //{
        //    spawnProgress -= 1f;
        //    SpawnEnemy();
        //}
        if (playerHealth <= 0 && startingPlayerHealth > 0)
        {
            Debug.Log("Defeat!");
            BeginNewGame();
        }
        if (!activeScenario.Progress() && enemies.IsEmpty)
        {
            Debug.Log("Victory!");
            BeginNewGame();
            activeScenario.Progress();
        }
        enemies.GameUpdate();
        nonEmenies.GameUpdate();
        Physics.SyncTransforms();
        board.GameUpdate();
    }
    void BeginNewGame()
    {
        enemies.Clear();
        nonEmenies.Clear();
        board.Clear();
        activeScenario = scenario.Begin();
        playerHealth = startingPlayerHealth;
    }
    void HandleAlternativeTouch()
    {
        GameTile tile = board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                board.ToggleDestination(tile);
            }
            else
            {
                board.ToggleSpwanPoint(tile);
            }
        }
    }
    void HandleTouch()
    {
        GameTile tile = board.GetTile(TouchRay);
        if (tile != null)
        {
            //tile.Content = tileContentFactory.Get(GameTileContentType.Destination);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                board.ToggleTower(tile, selectedTowerType);
            }
            else
            {
                board.ToggleWall(tile);
            }
        }
    }
    public static void SpawnEnemy(EnemyFactory factory, EnemyType type)
    {
        //GameTile spawnPoint = board.GetSpawnPoint(Random.Range(0, board.SpawnPointCount));
        //Enemy enemy = enemyFactory.Get((EnemyType)(Random.Range(0, 3)));
        //enemy.SpwanOn(spawnPoint);
        //enemies.Add(enemy);
        GameTile spawnPoint = instance.board.GetSpawnPoint(Random.Range(0, instance.board.SpawnPointCount));
        Enemy enemy = factory.Get(type);
        enemy.SpwanOn(spawnPoint);
        instance.enemies.Add(enemy);
    }
    public static Shell SpawnShell()
    {
        Shell shell = instance.warFactory.Shell;
        instance.nonEmenies.Add(shell);
        return shell;
    }
    public static Explosion SpawnExplosion()
    {
        Explosion explosion = instance.warFactory.Explosion;
        instance.nonEmenies.Add(explosion);
        return explosion;
    }
    public static void EnemyReachedDestination()
    {
        instance.playerHealth -= 1;
    }
}