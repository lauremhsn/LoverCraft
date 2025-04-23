using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject hazardPrefab;
    public GameObject goalPrefab;
    public GameObject playerPrefab;

    public int width = 10;
    public int height = 10;

    public float wallChance = 0.2f;
    public float hazardChance = 0.1f;

    void Start()
    {
        GenerateRandomGrid();
    }

    public void GenerateRandomGrid()
    {
        bool goalPlaced = false;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 💡 Center the tile grid visually around (0, 0)
                Vector3 spawnPos = new Vector3(
                    x - width / 2f + 0.5f,
                    y - height / 2f + 0.5f,
                    0
                );

                Instantiate(floorPrefab, spawnPos, Quaternion.identity);

                // Force walls on outer edges
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    Instantiate(wallPrefab, spawnPos, Quaternion.identity);
                    continue;
                }

                // Leave the spawn area (3x3 bottom left) empty
                if (x < 3 && y < 3) continue;

                float rand = Random.value;

                if (!goalPlaced && rand < 0.05f)
                {
                    Instantiate(goalPrefab, spawnPos, Quaternion.identity);
                    goalPlaced = true;
                }
                else if (rand < hazardChance)
                {
                    Instantiate(hazardPrefab, spawnPos, Quaternion.identity);
                }
                else if (rand < hazardChance + wallChance)
                {
                    Instantiate(wallPrefab, spawnPos, Quaternion.identity);
                }
            }
        }

    Vector3Int playerGridPos = new Vector3Int(1, 1, 0);
    Vector3 playerStartPos = new Vector3(
        playerGridPos.x - width / 2f + 0.5f,
        playerGridPos.y - height / 2f + 0.5f,
        0
    );


        Instantiate(playerPrefab, playerStartPos, Quaternion.identity);
    }
}
