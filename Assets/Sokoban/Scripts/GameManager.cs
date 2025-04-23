using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isFrozen = false;
    private string message = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (isFrozen && Input.GetKeyDown(KeyCode.Space))
        {
            isFrozen = false;
            ClearMap();
            FindObjectOfType<GridManager>().GenerateRandomGrid();
        }
    }

    public void FreezeGame(string msg)
    {
        isFrozen = true;
        message = msg;
        Debug.Log(message);  // Later replace with UI
    }

    private void ClearMap()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Floor")) Destroy(obj);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Wall")) Destroy(obj);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Goal")) Destroy(obj);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Hazard")) Destroy(obj);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player")) Destroy(obj);
    }
}
