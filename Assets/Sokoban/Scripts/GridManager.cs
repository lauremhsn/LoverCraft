using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    public GameObject tileFloorPrefab;
    public GameObject tileWallPrefab;
    public GameObject tileGoalPrefab;
    public GameObject tileHazardPrefab;
    public GameObject playerPrefab;

    public int width = 10;
    public int height = 10;
    public int innerWallCount = 15;

    private TileType[,] grid;
    private Vector2Int playerPos;
    private Vector2Int goalPos;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        GenerateRandomGrid();
    }

    public void GenerateRandomGrid()
    {
        grid = new TileType[width, height];
        playerPos = new Vector2Int(0, 0);

        // Place goal
        do
        {
            goalPos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (goalPos == playerPos || InSpawnArea(goalPos.x, goalPos.y));

        grid[playerPos.x, playerPos.y] = TileType.Floor;
        grid[goalPos.x, goalPos.y] = TileType.Goal;

        // Outer walls
        for (int x = 0; x < width; x++)
        {
            grid[x, 0] = TileType.Wall;
            grid[x, height - 1] = TileType.Wall;
        }
        for (int y = 0; y < height; y++)
        {
            grid[0, y] = TileType.Wall;
            grid[width - 1, y] = TileType.Wall;
        }

        // Inner walls
        for (int i = 0; i < innerWallCount; i++)
        {
            Vector2Int wallPos;
            do
            {
                wallPos = new Vector2Int(Random.Range(1, width - 1), Random.Range(1, height - 1));
            } while (grid[wallPos.x, wallPos.y] != TileType.Floor || InSpawnArea(wallPos.x, wallPos.y));
            grid[wallPos.x, wallPos.y] = TileType.Wall;
        }

        // Hazards
        for (int i = 0; i < 6; i++)
        {
            Vector2Int hazardPos;
            do
            {
                hazardPos = new Vector2Int(Random.Range(1, width - 1), Random.Range(1, height - 1));
            } while (grid[hazardPos.x, hazardPos.y] != TileType.Floor || InSpawnArea(hazardPos.x, hazardPos.y));
            grid[hazardPos.x, hazardPos.y] = TileType.Hazard;
        }

        RenderGrid();
        CenterCamera();
    }

    private void RenderGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject prefab = tileFloorPrefab;
                switch (grid[x, y])
                {
                    case TileType.Wall: prefab = tileWallPrefab; break;
                    case TileType.Goal: prefab = tileGoalPrefab; break;
                    case TileType.Hazard: prefab = tileHazardPrefab; break;
                }

                Vector3 pos = new Vector3(x - width / 2f + 0.5f, y - height / 2f + 0.5f, 0);
                Instantiate(prefab, pos, Quaternion.identity);
            }
        }

        // Instantiate player
        Vector3 playerWorldPos = new Vector3(playerPos.x - width / 2f + 0.5f, playerPos.y - height / 2f + 0.5f, 0);
        Instantiate(playerPrefab, playerWorldPos, Quaternion.identity);
    }

    private void CenterCamera()
    {
        Camera.main.transform.position = new Vector3(0, 0, -10);
    }

    private bool InSpawnArea(int x, int y)
    {
        return x < 3 && y < 3;
    }

    public TileType[,] GetGrid() => grid;
    public Vector2Int GetPlayerPosition() => playerPos;
    public Vector2Int GetGoalPosition() => goalPos;
}