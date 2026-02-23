using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public ItemType selectedItem = ItemType.None;

    public Image cursorImage;
    public Sprite defaultCursor;

    public Transform slotsParent;
    public GameObject itemPrefab;

    private void Start()
    {
        Cursor.visible = false;
    }

    void Awake()
    {
        Instance = this;
        ClearSelection();
    }


    public void SelectItem(ItemType item, Sprite icon)
    {
        selectedItem = item;
        cursorImage.sprite = icon;
        cursorImage.enabled = true;
    }

    public void ClearSelection()
    {
        selectedItem = ItemType.None;
        cursorImage.sprite = defaultCursor;
        cursorImage.enabled = true;
    }

    void Update()
    {
        cursorImage.transform.position = Input.mousePosition;
    }


    public void AddItem(ItemType itemType, Sprite icon)
    {
        Debug.Log("Adding item: " + itemType);
        Debug.Log(icon);

        foreach (Transform slot in slotsParent)
        {
            if (slot.childCount == 0)
            {
                GameObject item = Instantiate(itemPrefab);
                item.transform.SetParent(slot, false);

                RectTransform rt = item.GetComponent<RectTransform>();
                rt.anchoredPosition = Vector2.zero;
                rt.localScale = Vector3.one;

                var itemButton = item.GetComponent<InventoryItemButton>();
                itemButton.itemType = itemType;
                itemButton.icon = icon;

                item.GetComponent<Image>().sprite = icon;
                return;
            }
        }

        Debug.Log("Inventáø je plný!");
    }

    public void RemoveSelectedItem()
    {
        if (selectedItem == ItemType.None)
            return;

        foreach (Transform slot in slotsParent)
        {
            if (slot.childCount > 0)
            {
                InventoryItemButton btn = slot.GetChild(0).GetComponent<InventoryItemButton>();
                if (btn != null && btn.itemType == selectedItem)
                {
                    Destroy(slot.GetChild(0).gameObject);
                    break;
                }
            }
        }

        ClearSelection();
    }

    public bool HasItem(ItemType type)
    {
        foreach (Transform slot in slotsParent)
        {
            if (slot.childCount > 0)
            {
                InventoryItemButton btn = slot.GetChild(0).GetComponent<InventoryItemButton>();
                if (btn != null && btn.itemType == type)
                    return true;
            }
        }
        return false;
    }

}
