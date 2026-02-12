using UnityEngine;
using UnityEngine.UI;

public class InventoryItemButton : MonoBehaviour
{
    public ItemType itemType;
    public Sprite icon;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        InventoryManager.Instance.SelectItem(itemType, icon);
    }
}