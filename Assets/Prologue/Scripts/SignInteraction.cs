using UnityEngine;

public class SignInteraction : MonoBehaviour
{
    public GameObject signDialogue;
    public GameObject signBarrier; // NEW: Reference to the separate barrier
    private bool playerInRange = false;
    private bool isReadingSign = false;
    private bool hasReadSign = false;
    private PlayerMovementFixed playerMovement;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovementFixed>();
    }

    void Update()
    {
        if (!hasReadSign)
        {
            if (playerInRange && !isReadingSign && Input.GetKeyDown(KeyCode.E))
            {
                signDialogue.SetActive(true);
                isReadingSign = true;
                if (playerMovement != null)
                {
                    playerMovement.enabled = false;
                }
            }
            else if (isReadingSign && Input.GetKeyDown(KeyCode.Space))
            {
                signDialogue.SetActive(false);
                isReadingSign = false;
                hasReadSign = true;

                if (playerMovement != null)
                {
                    playerMovement.enabled = true;
                }

                if (signBarrier != null)
                {
                    Destroy(signBarrier); // Destroy the barrier GameObject
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasReadSign && collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!hasReadSign && collision.CompareTag("Player"))
        {
            playerInRange = false;
            if (signDialogue != null && signDialogue.activeInHierarchy)
            {
                signDialogue.SetActive(false);
            }
            if (playerMovement != null)
            {
                playerMovement.enabled = true;
            }
            isReadingSign = false;
        }
    }
}