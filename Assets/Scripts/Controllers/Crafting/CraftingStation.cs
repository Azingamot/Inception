using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CraftingStation : MonoBehaviour
{
    public string RecipesFolder;

    public List<CraftRecipe> ReceiveCraftingRecipes()
    {
        CraftRecipe[] recipes = Resources.LoadAll<CraftRecipe>("Recipes/" + RecipesFolder);
        return recipes.ToList();
    }
}