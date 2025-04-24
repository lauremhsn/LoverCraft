using UnityEngine;

public class HazardTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("You Lose!");
            GameManager.Instance.FreezeGame(true);
        }
    }
}