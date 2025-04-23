using UnityEngine;

public class HazardTrigger : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
        {
            Vector2 playerPos = new Vector2(
                Mathf.Round(transform.position.x * 2f) / 2f,
                Mathf.Round(transform.position.y * 2f) / 2f
            );

            Vector2 hazardPos = new Vector2(
                Mathf.Round(other.transform.position.x * 2f) / 2f,
                Mathf.Round(other.transform.position.y * 2f) / 2f
            );

            if (playerPos == hazardPos)
            {
                GameManager.Instance.FreezeGame("YOU DIED!");
            }
        }
    }
}
