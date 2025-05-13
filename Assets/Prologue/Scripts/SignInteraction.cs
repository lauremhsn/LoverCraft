using UnityEngine;
using UnityEngine.UI;

public class SignInteraction : MonoBehaviour
{
    public GameObject signDialogue;
    public GameObject signBarrier;
    public PlayerMovementPrologue playerMovement;

    private bool isPlayerNearby = false;
    private bool hasInteracted = false;
    private bool isDialogueActive = false;

    void Update()
    {
        if (isPlayerNearby && !hasInteracted && Input.GetKeyDown(KeyCode.E))
        {
            signDialogue.SetActive(true);
            playerMovement.enabled = false;
            isDialogueActive = true;
        }

        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            signDialogue.SetActive(false);
            playerMovement.enabled = true;
            isDialogueActive = false;
            hasInteracted = true;

            if (signBarrier != null)
            {
                Destroy(signBarrier);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}