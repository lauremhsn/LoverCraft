using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("You Win!");
            GameManager.Instance.FreezeGame(true);
        }
    }
}
