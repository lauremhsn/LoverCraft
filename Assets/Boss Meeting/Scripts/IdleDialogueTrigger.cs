using UnityEngine;

public class IdleDialogueTrigger : MonoBehaviour
{
    public BossDialogueManager dialogueManager;

    // This function must be called from an Animation Event in the idle animation
    public void TriggerIdleDialogue()
    {
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue();
        }
        else
        {
            Debug.LogWarning("Dialogue Manager not assigned.");
        }
    }
}
