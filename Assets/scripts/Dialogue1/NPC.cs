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



    public void OnNPCClicked()
    {
        if (dialogueManager != null && dialogueManager.IsDialogueOpen)
            return;

        if (minigame != null && minigame.IsRunning)
            return;

        if (TryUseItem())
            return;

        TriggerDialogue();
    }

    bool TryUseItem()
    {
        var inventory = InventoryManager.Instance;

        if (inventory == null)
            return false;

        if (inventory.selectedItem == ItemType.None)
            return false;

        if (inventory.selectedItem == ItemType.Drink)
        {
            Debug.Log("NPC dostal drink");

            inventory.RemoveSelectedItem();
            Debug.Log("Item used");

            return true;
        }

        //wrong NPC
        return false;
    }


    void TriggerDialogue()
    {
        //bartender answers only in designated location
        if (GameState.Instance.currentLocation != Location.Bar && hasMinigame)
            return;



        if (minigame != null && minigame.panel.activeSelf)
            return;

        Debug.Log("NPC clicked: " + gameObject.name);

        if (hasMinigame)
        {
            //after minigame
            if (GameState.Instance.bartenderMinigamePlayed)
            {
                dialogueManager.onDialogueEnd = null;

                if (GameState.Instance.bartenderCheated)
                {
                    dialogueManager.StartDialogue(afterCheatDialogue);
                }
                else
                {
                    dialogueManager.StartDialogue(afterFairDialogue);
                }
                    


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

    public void OnClickNPC()
    {
        if (InventoryManager.Instance.selectedItem == ItemType.Drink)
        {
            Debug.Log("NPC dostal drink");
            InventoryManager.Instance.ClearSelection();
            return;
        }

        TriggerDialogue();
    }



    void StartMinigame()
    {
        if (minigame != null)
            minigame.StartGame();
    }
}