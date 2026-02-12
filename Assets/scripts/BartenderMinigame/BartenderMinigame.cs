using TMPro;
using UnityEngine;

public class BartenderMinigame : MonoBehaviour
{
    public GameObject panel;

    public TextMeshProUGUI currentMixText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI recipeText;

    public bool IsRunning => panel.activeSelf;

    public Sprite drinkSprite;

    int rum;
    int cola;

    void Start()
    {
        panel.SetActive(false);
    }

    public void StartGame()
    {
        recipeText.text = "Recept:\n2× Rum\n1× Cola";
        panel.SetActive(true);
        ResetMix();
        UpdateMixText();
        resultText.text = "";
    }

    void ResetMix()
    {
        rum = 0;
        cola = 0;
    }

    void UpdateMixText()
    {
        currentMixText.text = $"Rum: {rum}\nCola: {cola}";
    }

    public void AddRum()
    {
        rum++;
        UpdateMixText();
    }

    public void AddCola()
    {
        cola++;
        UpdateMixText();
    }



    public void UseCan()
    {
        GameState.Instance.bartenderCheated = true;
        Win();
    }

    public void Submit()
    {
        if (rum == 2 && cola == 1)
        {
            Win();
        }
        else
        {
            resultText.text = "Zkus to znovu.";
            ResetMix();
            UpdateMixText();
        }
    }

    void Win()
    {
        Debug.Log("MINIGAME WIN CALLED");

        GameState.Instance.bartenderMinigamePlayed = true;
        GameState.Instance.AdvanceProgress();
        InventoryManager.Instance.AddItem(ItemType.Drink, drinkSprite);

        panel.SetActive(false);
    }
}
