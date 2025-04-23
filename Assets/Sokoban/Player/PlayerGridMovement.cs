using UnityEngine;

public class PlayerGridMovement : MonoBehaviour
{
    public float moveTime = 0.2f;
    private bool isMoving = false;

    void Update()
    {
        if (isMoving || GameManager.Instance.isFrozen) return;

        Vector2 input = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) input = Vector2.up;
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) input = Vector2.down;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) input = Vector2.left;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) input = Vector2.right;

        if (input != Vector2.zero)
        {
            Vector3 start = transform.position;
            Vector3 target = start + (Vector3)input;

            // Wall detection without debug logs
            RaycastHit2D hit = Physics2D.Raycast(target, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Wall"))
            {
                return;
            }

            StartCoroutine(Move(start, target));
        }
    }

    System.Collections.IEnumerator Move(Vector3 start, Vector3 end)
    {
        isMoving = true;
        float elapsed = 0;

        while (elapsed < moveTime)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / moveTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        isMoving = false;
    }
}
