using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
        {
            Vector2 playerPos = new Vector2(
                Mathf.Round(transform.position.x * 2f) / 2f,
                Mathf.Round(transform.position.y * 2f) / 2f
            );

            Vector2 goalPos = new Vector2(
                Mathf.Round(other.transform.position.x * 2f) / 2f,
                Mathf.Round(other.transform.position.y * 2f) / 2f
            );

            if (playerPos == goalPos)
            {
                GameManager.Instance.FreezeGame("YOU WIN!");
            }
        }
    }
}
