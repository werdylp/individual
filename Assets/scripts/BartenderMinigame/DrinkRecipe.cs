using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DrinkRecipe
{
    public string drinkName;
    public List<IngredientAmount> ingredients;
}

[System.Serializable]

public class IngredientAmount
{
    public IngredientType type;
    public float amount;
}

