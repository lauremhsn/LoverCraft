using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuardInteraction : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public enum Speaker { Player, SignGuard, CastleGuard0, CastleGuard1 }
        public Speaker speaker;
        [TextArea] public string text;
        public bool isAutomatic;
        public float displayDuration = 2f;
    }

    public List<DialogueLine> dialogueLines;

    public GameObject playerDialogueBox;
    public TextMeshProUGUI playerDialogueText;
    public GameObject signGuardDialogueBox;
    public TextMeshProUGUI signGuardDialogueText;
    public GameObject castleGuard0DialogueBox;
    public TextMeshProUGUI castleGuard0DialogueText;
    public GameObject castleGuard1DialogueBox;
    public TextMeshProUGUI castleGuard1DialogueText;

    private bool playerInRange = false;
    private bool isTalking = false;
    private int currentLineIndex = 0;
    private PlayerMovementPrologue playerMovement;

    void Start()
    {
        HideAllDialogue();
        playerMovement = FindObjectOfType<PlayerMovementPrologue>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isTalking)
        {
            isTalking = true;
            currentLineIndex = 0;
            StartCoroutine(ShowDialogue(currentLineIndex));
        }

        if (isTalking && Input.GetKeyDown(KeyCode.Space))
        {
            if (currentLineIndex < dialogueLines.Count)
            {
                if (!dialogueLines[currentLineIndex].isAutomatic)
                {
                    currentLineIndex++;
                    if (currentLineIndex < dialogueLines.Count)
                        StartCoroutine(ShowDialogue(currentLineIndex));
                    else
                        EndDialogue();
                }
            }
        }
    }

    IEnumerator ShowDialogue(int index)
    {
        HideAllDialogue();

        DialogueLine line = dialogueLines[index];
        SetMovementAllowed(line.isAutomatic);

        GameObject box = GetDialogueBox(line.speaker);
        TextMeshProUGUI text = GetDialogueText(line.speaker);

        box.SetActive(true);
        text.text = line.text;

        if (line.isAutomatic)
        {
            yield return new WaitForSeconds(line.displayDuration);
            currentLineIndex++;
            if (currentLineIndex < dialogueLines.Count)
                StartCoroutine(ShowDialogue(currentLineIndex));
            else
                EndDialogue();
        }
    }

    void EndDialogue()
    {
        HideAllDialogue();
        isTalking = false;
        SetMovementAllowed(true); // Re-enable movement after dialogue
    }

    void HideAllDialogue()
    {
        playerDialogueBox.SetActive(false);
        signGuardDialogueBox.SetActive(false);
        castleGuard0DialogueBox.SetActive(false);
        castleGuard1DialogueBox.SetActive(false);
    }

    void SetMovementAllowed(bool allowed)
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = allowed;
        }
    }

    GameObject GetDialogueBox(DialogueLine.Speaker speaker)
    {
        return speaker switch
        {
            DialogueLine.Speaker.Player => playerDialogueBox,
            DialogueLine.Speaker.SignGuard => signGuardDialogueBox,
            DialogueLine.Speaker.CastleGuard0 => castleGuard0DialogueBox,
            DialogueLine.Speaker.CastleGuard1 => castleGuard1DialogueBox,
            _ => null
        };
    }

    TextMeshProUGUI GetDialogueText(DialogueLine.Speaker speaker)
    {
        return speaker switch
        {
            DialogueLine.Speaker.Player => playerDialogueText,
            DialogueLine.Speaker.SignGuard => signGuardDialogueText,
            DialogueLine.Speaker.CastleGuard0 => castleGuard0DialogueText,
            DialogueLine.Speaker.CastleGuard1 => castleGuard1DialogueText,
            _ => null
        };
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = false;
    }
}