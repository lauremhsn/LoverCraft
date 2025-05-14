using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BossDialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public enum Speaker { Player, Boss }
        public Speaker speaker;
        [TextArea(2, 5)] public string text;
    }

    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

    public GameObject playerDialogueBox;
    public TMP_Text playerDialogueText;

    public GameObject bossDialogueBox;
    public TMP_Text bossDialogueText;

    private int currentLine = 0;
    private bool isTalking = false;

    void Start()
    {
        playerDialogueBox.SetActive(false);
        bossDialogueBox.SetActive(false);
    }

    public void StartDialogue()
    {
        if (isTalking) return;
        isTalking = true;
        currentLine = 0;
        ShowLine(dialogueLines[currentLine]);
    }

    void Update()
    {
        if (isTalking && Input.GetKeyDown(KeyCode.Space))
        {
            currentLine++;
            if (currentLine < dialogueLines.Count)
            {
                ShowLine(dialogueLines[currentLine]);
            }
            else
            {
                EndDialogue();
            }
        }
    }

    void ShowLine(DialogueLine line)
    {
        if (line.speaker == DialogueLine.Speaker.Player)
        {
            playerDialogueBox.SetActive(true);
            bossDialogueBox.SetActive(false);
            playerDialogueText.text = line.text;
        }
        else
        {
            bossDialogueBox.SetActive(true);
            playerDialogueBox.SetActive(false);
            bossDialogueText.text = line.text;
        }
    }

    void EndDialogue()
    {
        playerDialogueBox.SetActive(false);
        bossDialogueBox.SetActive(false);
        isTalking = false;
    }
}
