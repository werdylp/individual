using UnityEngine;


public class KuratorDialogue : MonoBehaviour
{

    public DialogueManager dialogueManager;

    [Header("Story Progress")]
    public int requiredItems = 5;

    [Header("Items")]
    public ItemType[] requiredItem;
    public ItemType[] rewardItem;
    public Sprite[] rewardSprite;
    public bool[] advanceLocation;

    [Header("Dialogue")]
    public Dialogue1[] storyDialogue;

    int progress = 0;

    public void OnNPCClicked()
    {
        if (dialogueManager.IsDialogueOpen)
            return;

        if (TryGiveItem())
            return;

        ShowDialogue();
    }

    bool TryGiveItem()
    {
        var inventory = InventoryManager.Instance;

        if (inventory.selectedItem == ItemType.None)
            return false;

        if (inventory.selectedItem != requiredItem[progress])
            return false;

        inventory.RemoveSelectedItem();

        GameState.Instance.curatorReceivedDrink = true;


        if (rewardItem[progress] != ItemType.None)
            inventory.AddItem(rewardItem[progress], rewardSprite[progress]);

        progress++;

        GameState.Instance.AdvanceProgress();

        ShowDialogue();

        return true;
    }

    void ShowDialogue()
    {
        if (progress < storyDialogue.Length)
        {
            dialogueManager.StartDialogue(storyDialogue[progress]);
        }
    }
}
