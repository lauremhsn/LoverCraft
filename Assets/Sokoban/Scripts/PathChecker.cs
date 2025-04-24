using System.Collections.Generic;
using UnityEngine;

public class PathChecker : MonoBehaviour
{
    public static bool IsPathAvailable(TileType[,] grid, Vector2Int start, Vector2Int goal, int width, int height)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == goal)
                return true;

            foreach (var dir in directions)
            {
                Vector2Int neighbor = current + dir;

                if (neighbor.x >= 0 && neighbor.x < width &&
                    neighbor.y >= 0 && neighbor.y < height &&
                    !visited.Contains(neighbor) &&
                    grid[neighbor.x, neighbor.y] != TileType.Wall &&
                    grid[neighbor.x, neighbor.y] != TileType.Hazard)
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        return false;
    }
}
