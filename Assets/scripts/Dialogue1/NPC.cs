using UnityEngine;
using static GameState;

public class NPC : MonoBehaviour
{
    public Dialogue1 firstDialogue;
    public Dialogue1 retryDialogue;
    public Dialogue1 afterDialogue;

    public Dialogue1 beforeMinigameDialogue;
    public Dialogue1 afterFairDialogue;
    public Dialogue1 afterCheatDialogue;

    public bool hasMinigame = false;
    public BartenderMinigame minigame;

    public DialogueManager dialogueManager;

    void OnMouseDown()
    {
        TriggerDialogue();
    }

    void TriggerDialogue()
    {
        Debug.Log("NPC clicked: " + gameObject.name);

        if (hasMinigame)
        {
            //after minigame
            if (GameState.Instance.bartenderMinigamePlayed)
            {
                dialogueManager.onDialogueEnd = null;

                if (GameState.Instance.bartenderCheated)
                    dialogueManager.StartDialogue(afterCheatDialogue);
                else
                    dialogueManager.StartDialogue(afterFairDialogue);

                return;
            }

            //before minigame
            dialogueManager.onDialogueEnd = StartMinigame;
            dialogueManager.StartDialogue(beforeMinigameDialogue);
            return;
        }

        if (GameState.Instance.IsBarUnlocked())
        {
            dialogueManager.StartDialogue(afterDialogue);
            return;
        }

        if (!GameState.Instance.talkedToCurator)
        {
            GameState.Instance.talkedToCurator = true;
            dialogueManager.StartDialogue(firstDialogue);
            return;
        }

        dialogueManager.StartDialogue(retryDialogue);
    }


    void StartMinigame()
    {
        if (minigame != null)
            minigame.StartGame();
    }
}