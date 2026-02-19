using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BartenderMinigame : MonoBehaviour
{
    public GameObject panel;

    bool isPouring;
    IngredientType currentPouringType;


    [Header("Glass")]
    public Image glassFill;
    public Image limeImage;

    [Header("Recipes")]
    public List<DrinkRecipe> easyRecipes;
    public List<DrinkRecipe> mediumRecipes;
    public List<DrinkRecipe> hardRecipes;

    DrinkRecipe currentRecipe;

    [Header("Settings")]
    public float pourAmount = 0.5f;
    public float tolerance = 0.15f;

    Dictionary<IngredientType, float> currentMix
    = new Dictionary<IngredientType, float>();



    public TextMeshProUGUI currentMixText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI recipeText;

    public bool IsRunning => panel.activeSelf;

    public Sprite drinkSprite;


    void Start()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        if (isPouring)
        {
            AddIngredient(currentPouringType);
        }
    }

    public void StartPour(int typeIndex)
    {
        currentPouringType = (IngredientType)typeIndex;
        isPouring = true;
    }

    public void StopPour()
    {
        isPouring = false;
    }

    public void StartGame()
    {
        SelectRecipe();
        panel.SetActive(true);
        ResetMix();
    }

    void ResetMix()
    {
        currentMix.Clear();

        glassFill.fillAmount = 0f;
        glassFill.color = Color.clear;
        limeImage.enabled = false;
        UpdateCurrentMixText();
    }

    void SelectRecipe()
    {
        List<DrinkRecipe> pool = null;

        switch (GameState.Instance.currentDifficulty)
        {
            case Difficulty.Easy:
                pool = easyRecipes;
                break;
            case Difficulty.Medium:
                pool = mediumRecipes;
                break;
            case Difficulty.Hard:
                pool = hardRecipes;
                break;
        }

        currentRecipe = pool[Random.Range(0, pool.Count)];

        DisplayRecipe();
    }

    void DisplayRecipe()
    {
        recipeText.text = "Recept:\n";

        foreach (var ingredient in currentRecipe.ingredients)
        {
            recipeText.text += $"{ingredient.type} : {ingredient.amount}\n";
        }
    }


    public void AddIngredient(IngredientType type)
    {
        if (type == IngredientType.Lime)
        {
            limeImage.enabled = true;
            currentMix[type] = 1f;
            return;
        }

        if (!currentMix.ContainsKey(type))
        {
            currentMix[type] = 0f;
        }

        currentMix[type] += pourAmount * Time.deltaTime;
        currentMix[type] = Mathf.Clamp01(currentMix[type]);

        UpdateVisual();
        UpdateCurrentMixText();

        
    }

    void UpdateCurrentMixText()
    {
        currentMixText.text = "Nalito:\n";

        foreach (var pair in currentMix)
        {
            currentMixText.text += $"{pair.Key} : {pair.Value:F2}\n";
        }
    }

    public void AddIngredientInt(int typeIndex)
    {
        IngredientType type = (IngredientType)typeIndex;
        AddIngredient(type);
    }

    void UpdateVisual()
    {
        float rum = GetAmount(IngredientType.Rum);
        float cola = GetAmount(IngredientType.Cola);
        float vodka = GetAmount(IngredientType.Vodka);

        float total = rum + cola + vodka;

        if (total <= 0f)
        {
            glassFill.fillAmount = 0f;
            return;
        }

        glassFill.fillAmount = Mathf.Clamp01(total);

        Color rumColor = new Color(0.4f, 0.2f, 0.1f);
        Color colaColor = new Color(0.1f, 0.1f, 0.1f);
        Color vodkaColor = new Color(0.8f, 0.8f, 0.9f);

        Color finalColor =
            (rum / total) * rumColor +
            (cola / total) * colaColor +
            (vodka / total) * vodkaColor;

        glassFill.color = finalColor;
    }



    public void Shake()
    {
        EvaluateDrink();
    }

    void EvaluateDrink()
    {
        float totalPoured = 0f;

        foreach (var pair in currentMix)
            totalPoured += pair.Value;

        foreach (var ingredient in currentRecipe.ingredients)
        {
            float targetRatio = ingredient.amount;
            float poured = GetAmount(ingredient.type);

            float pouredRatio = poured / totalPoured;

            if (Mathf.Abs(pouredRatio - targetRatio) > tolerance)
            {
                resultText.text = "Špatný drink!";
                ResetMix();
                return;
            }
        }

        Win();
    }

    float GetAmount(IngredientType type)
    {
        if (currentMix.ContainsKey(type))
            return currentMix[type];

        return 0f;
    }

    public void UseCan()
    {
        GameState.Instance.bartenderCheated = true;
        Win();
    }


    void Win()
    {
        Debug.Log("MINIGAME WIN CALLED");
        resultText.text = "Drink hotový!";

        GameState.Instance.bartenderMinigamePlayed = true;
        InventoryManager.Instance.AddItem(ItemType.Drink, drinkSprite);

        panel.SetActive(false);
    }
}
