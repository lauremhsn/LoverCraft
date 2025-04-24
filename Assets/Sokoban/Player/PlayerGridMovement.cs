using UnityEngine;

public class PlayerGridMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector2Int currentGridPos;
    private bool isMoving = false;
    private Vector3 targetWorldPos;

    private void Start()
    {
        currentGridPos = GridManager.Instance.GetPlayerPosition();
        transform.position = GridToWorldPosition(currentGridPos);
        targetWorldPos = transform.position;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
            {
                transform.position = targetWorldPos;
                isMoving = false;

                TileType[,] grid = GridManager.Instance.GetGrid();
                TileType tile = grid[currentGridPos.x, currentGridPos.y];

                if (tile == TileType.Goal)
                {
                    Debug.Log("You Win!");
                }
                else if (tile == TileType.Hazard)
                {
                    Debug.Log("You Lose!");
                }
            }
            return;
        }

        Vector2Int input = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W)) input = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S)) input = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A)) input = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D)) input = Vector2Int.right;

        if (input != Vector2Int.zero)
        {
            TryMove(input);
        }
    }

    private void TryMove(Vector2Int direction)
    {
        Vector2Int newGridPos = currentGridPos + direction;
        TileType[,] grid = GridManager.Instance.GetGrid();

        if (newGridPos.x < 0 || newGridPos.x >= grid.GetLength(0) ||
            newGridPos.y < 0 || newGridPos.y >= grid.GetLength(1))
            return;

        TileType tile = grid[newGridPos.x, newGridPos.y];
        if (tile == TileType.Wall)
            return;

        currentGridPos = newGridPos;
        targetWorldPos = GridToWorldPosition(currentGridPos);
        isMoving = true;
    }

    private Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        int width = GridManager.Instance.GetGrid().GetLength(0);
        int height = GridManager.Instance.GetGrid().GetLength(1);
        return new Vector3(gridPos.x - width / 2f + 0.5f, gridPos.y - height / 2f + 0.5f, 0);
    }
}
