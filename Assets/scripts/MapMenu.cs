using UnityEngine;

public class MapMenu : MonoBehaviour
{
    public GameObject mapMenu;             
    public LocationManager locationManager;
    public DialogueManager dialogueManager;

    public GameObject barButton;
    public GameObject stageButton;
    public bool IsOpen => mapMenu.activeSelf;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMap();
        }
    }

    void Start()
    {
        mapMenu.SetActive(false);
    }

    void UpdateMapButtons()
    {
        barButton.SetActive(GameState.Instance.IsBarUnlocked());
        stageButton.SetActive(GameState.Instance.IsStageUnlocked());
    }



    public void ToggleMap()
    {
        bool isOpen = mapMenu.activeSelf;
        mapMenu.SetActive(!isOpen);

        // Když otevøu mapu, schovám dialog
        if (!isOpen)
        {
            UpdateMapButtons();
            if (dialogueManager != null)
                dialogueManager.Hide();
        }
    }

    public void GoToBar()
    {
        locationManager.SetLocation(Location.Bar);
        CloseMap();
    }

    public void GoToToilet()
    {
        locationManager.SetLocation(Location.Toilet);
        CloseMap();
    }

    public void GoToStage()
    {
        if (InventoryManager.Instance.selectedItem != ItemType.StagePass)
        {
            Debug.Log("Potøebuješ Stage Pass");
            return;
        }

        InventoryManager.Instance.RemoveSelectedItem();
        locationManager.SetLocation(Location.Stage);
        CloseMap();
    }

    void CloseMap()
    {
        mapMenu.SetActive(false);
    }
}
