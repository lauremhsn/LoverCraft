using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 10;
    public int maxPlacementTries = 1000;
    public int maxMazeRetries = 100;
    public int hazardCount = 5;
    [Range(0f, 1f)] public float wallDensity = 0.4f;

    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject hazardPrefab;
    public GameObject goalPrefab;
    public GameObject playerPrefab;
    public TextMeshProUGUI bestStepsText;
    public TextMeshProUGUI loseText;

    private TileType[,] grid;
    private GameObject spawnedPlayer;
    private List<GameObject> spawnedTiles = new List<GameObject>();
    private int playerX;
    private int playerY;
    private bool isFrozen = false;
    private bool isMoving = false;
    private Vector3 targetPosition;
    public float moveSpeed = 5f;
    private int bestSteps = 0;
    private int remainingSteps = 0;

    void Start()
    {
        GenerateAndSpawnMaze();

        if (loseText != null)
        {
            loseText.gameObject.SetActive(false); // Hide Lose Text at start
        }
    }

    void Update()
    {
        if (isFrozen)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetGame();
            }
            return;
        }

        if (isMoving)
        {
            spawnedPlayer.transform.position = Vector3.MoveTowards(spawnedPlayer.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(spawnedPlayer.transform.position, targetPosition) < 0.01f)
            {
                spawnedPlayer.transform.position = targetPosition;
                isMoving = false;

                remainingSteps--;
                UpdateBestStepsUI();

                TileType currentTile = grid[playerX, playerY];

                if (currentTile == TileType.Hazard)
                {
                    Debug.Log("Player hit a hazard!");
                    ShowLoseScreen();
                }
                else if (currentTile == TileType.Goal)
                {
                    Debug.Log("Player won!");
                    FreezePlayer();
                }
                else if (remainingSteps <= 0)
                {
                    Debug.Log("Player ran out of moves!");
                    ShowLoseScreen();
                }
            }

            return;
        }

        // Move with WASD and Arrows
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) TryMove(0, 1);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) TryMove(0, -1);
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) TryMove(-1, 0);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) TryMove(1, 0);
    }

    void TryMove(int moveX, int moveY)
    {
        int newX = playerX + moveX;
        int newY = playerY + moveY;

        if (newX < 0 || newX >= gridWidth || newY < 0 || newY >= gridHeight)
            return;

        TileType tile = grid[newX, newY];

        if (tile == TileType.Wall)
        {
            return;
        }

        playerX = newX;
        playerY = newY;
        targetPosition = new Vector3(playerX, playerY, -1);
        isMoving = true;
    }

    void FreezePlayer()
    {
        isFrozen = true;
        spawnedPlayer.GetComponent<SpriteRenderer>().color = Color.cyan;
    }

    void ShowLoseScreen()
    {
        isFrozen = true;
        if (loseText != null)
        {
            loseText.gameObject.SetActive(true);
        }
    }

    void SpawnPlayer()
    {
        playerX = 1;
        playerY = 1;
        spawnedPlayer = Instantiate(playerPrefab, new Vector3(playerX, playerY, -1), Quaternion.identity);
    }

    void GenerateMaze()
    {
        grid = new TileType[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (x == 0 || y == 0 || x == gridWidth - 1 || y == gridHeight - 1)
                {
                    grid[x, y] = TileType.Wall;
                }
                else
                {
                    grid[x, y] = TileType.Floor;
                }
            }
        }

        int totalInnerTiles = (gridWidth - 2) * (gridHeight - 2);
        int numberOfWalls = Mathf.RoundToInt(totalInnerTiles * wallDensity);
        int wallsPlaced = 0;
        int wallTries = 0;

        while (wallsPlaced < numberOfWalls && wallTries < maxPlacementTries)
        {
            int wx = Random.Range(1, gridWidth - 1);
            int wy = Random.Range(1, gridHeight - 1);

            if (grid[wx, wy] == TileType.Floor && (wx != 1 || wy != 1))
            {
                grid[wx, wy] = TileType.Wall;
                wallsPlaced++;
            }
            wallTries++;
        }

        bool goalPlaced = false;
        int goalTries = 0;

        while (!goalPlaced && goalTries < maxPlacementTries)
        {
            int gx = Random.Range(gridWidth / 2, gridWidth - 2);
            int gy = Random.Range(gridHeight / 2, gridHeight - 2);

            if (grid[gx, gy] == TileType.Floor && (gx != 1 || gy != 1))
            {
                grid[gx, gy] = TileType.Goal;
                goalPlaced = true;
            }
            goalTries++;
        }

        int hazardsPlaced = 0;
        int hazardTries = 0;

        while (hazardsPlaced < hazardCount && hazardTries < maxPlacementTries)
        {
            int hx = Random.Range(1, gridWidth - 1);
            int hy = Random.Range(1, gridHeight - 1);

            if (grid[hx, hy] == TileType.Floor && (hx != 1 || hy != 1))
            {
                grid[hx, hy] = TileType.Hazard;
                hazardsPlaced++;
            }
            hazardTries++;
        }
    }

    (bool, int) CheckMazeSolvable()
    {
        Queue<(Vector2Int, int)> queue = new Queue<(Vector2Int, int)>();
        bool[,] visited = new bool[gridWidth, gridHeight];

        queue.Enqueue((new Vector2Int(1, 1), 0));
        visited[1, 1] = true;

        while (queue.Count > 0)
        {
            var (current, steps) = queue.Dequeue();

            if (grid[current.x, current.y] == TileType.Goal)
            {
                return (true, steps);
            }

            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, -1)
            };

            foreach (var dir in directions)
            {
                int newX = current.x + dir.x;
                int newY = current.y + dir.y;

                if (newX >= 0 && newX < gridWidth && newY >= 0 && newY < gridHeight)
                {
                    if (!visited[newX, newY] &&
                        (grid[newX, newY] == TileType.Floor || grid[newX, newY] == TileType.Goal))
                    {
                        visited[newX, newY] = true;
                        queue.Enqueue((new Vector2Int(newX, newY), steps + 1));
                    }
                }
            }
        }

        return (false, 0);
    }

    void GenerateAndSpawnMaze()
    {
        int tries = 0;
        bool foundMaze = false;

        while (tries < maxMazeRetries)
        {
            GenerateMaze();
            var (solvable, distance) = CheckMazeSolvable();

            if (solvable)
            {
                Debug.Log("Maze generated! Best distance: " + distance);
                grid = (TileType[,])grid.Clone();
                bestSteps = distance;
                remainingSteps = bestSteps;

                SpawnTiles();
                SpawnPlayer();
                CenterCamera();
                UpdateBestStepsUI();
                foundMaze = true;
                break;
            }

            tries++;
        }

        if (!foundMaze)
        {
            Debug.LogWarning("Failed to generate a solvable maze after " + maxMazeRetries + " tries!");
        }
    }

    void UpdateBestStepsUI()
    {
        if (bestStepsText != null)
        {
            bestStepsText.text = remainingSteps.ToString();
        }
    }

    void SpawnTiles()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GameObject prefabToSpawn = null;

                switch (grid[x, y])
                {
                    case TileType.Floor:
                        prefabToSpawn = floorPrefab;
                        break;
                    case TileType.Wall:
                        prefabToSpawn = wallPrefab;
                        break;
                    case TileType.Hazard:
                        prefabToSpawn = hazardPrefab;
                        break;
                    case TileType.Goal:
                        prefabToSpawn = goalPrefab;
                        break;
                }

                if (prefabToSpawn != null)
                {
                    GameObject tile = Instantiate(prefabToSpawn, new Vector3(x, y, 0), Quaternion.identity);
                    spawnedTiles.Add(tile);
                }
            }
        }
    }

    void CenterCamera()
    {
        Camera.main.transform.position = new Vector3(gridWidth / 2f - 0.5f, gridHeight / 2f - 0.5f, -10f);
        Camera.main.orthographicSize = Mathf.Max(gridWidth, gridHeight) / 2f + 2f;
    }

    void ResetGame()
    {
        foreach (GameObject tile in spawnedTiles)
        {
            Destroy(tile);
        }
        spawnedTiles.Clear();

        if (spawnedPlayer != null)
        {
            Destroy(spawnedPlayer);
        }

        if (loseText != null)
        {
            loseText.gameObject.SetActive(false);
        }

        isFrozen = false;
        isMoving = false;

        GenerateAndSpawnMaze();
    }
}
