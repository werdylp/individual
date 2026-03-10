using UnityEngine;
using static GameState;

public class NPC : MonoBehaviour
{

    [Header("Item")]
    public ItemType rewardItem = ItemType.None;
    public Sprite rewardSprite;

    [Header("Dialogue")]
    public Dialogue1 beforeMinigameDialogue;
    public Dialogue1 afterFairDialogue;
    public Dialogue1 afterCheatDialogue;

    [Header("Minigame")]
    public MonoBehaviour minigameBehaviour;

    IMinigame minigame;

    public DialogueManager dialogueManager;

    bool rewardGiven = false;

    void Start()
    {
        if (minigameBehaviour != null)
        {
            minigame = minigameBehaviour.GetComponent<IMinigame>();

            if (minigame == null)
                Debug.LogError("Assigned minigameBehaviour does not implement IMinigame");
        }
    }
    public void OnNPCClicked()
    {
        Debug.Log("NPC clicked: " + gameObject.name);
        if (dialogueManager.IsDialogueOpen)
            return;

        if (minigame != null && minigame.IsRunning)
            return;

        TriggerDialogue();
    }

    void TriggerDialogue()
    {
        Debug.Log("TriggerDialogue called");
        Debug.Log("Minigame: " + minigame);
        if (minigame != null)
        {
            if (minigame.IsFinished)
            {
                if (minigame.PlayerCheated)
                {
                    dialogueManager.StartDialogue(afterCheatDialogue);
                }
                else
                {
                    dialogueManager.StartDialogue(afterFairDialogue);
                }

                GiveReward();
                return;
            }

            dialogueManager.onDialogueEnd = StartMinigame;
            dialogueManager.StartDialogue(beforeMinigameDialogue);
            return;
        }
    }

    void StartMinigame()
    {
        if (minigame != null)
            minigame.StartGame();
    }

    void GiveReward()
    {
        if (rewardGiven)
            return;

        if (rewardItem != ItemType.None)
        {
            InventoryManager.Instance.AddItem(rewardItem, rewardSprite);
        }

        rewardGiven = true;
    }
}