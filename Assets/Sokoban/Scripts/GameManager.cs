using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isFrozen = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void FreezeGame(bool shouldFreeze)
    {
        isFrozen = shouldFreeze;
        Debug.Log($"Game Frozen: {shouldFreeze}");
    }

    public void Win()
    {
        Debug.Log("🎉 You Win!");
        FreezeGame(true);
    }

    public void Lose()
    {
        Debug.Log("💀 You Lose!");
        FreezeGame(true);
    }
}
