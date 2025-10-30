using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CraftingStation : MonoBehaviour, IInteractable
{
    [SerializeField] private string pathToRecipes;
    private List<CraftRecipe> recipes = new List<CraftRecipe>();
    private bool uiOpened = false;
    private CraftingUI crafting;
    public Transform Transform => transform;

    public void Initialize()
    {
        if (recipes.Count == 0)
            recipes = ReceiveCraftingRecipes();

        if (crafting == null)
            crafting = GameObject.FindAnyObjectByType<CraftingUI>();

        if (!uiOpened) 
            OpenRecipesUI();
        else 
            CloseRecipesUI();
    }       

    private void OpenRecipesUI()
    {
        if (crafting != null)
        {
            crafting.Open(recipes);
        }
        uiOpened = true;
    }

    private void CloseRecipesUI()
    {
        if (crafting != null)
        {
            crafting.Close();
        }
        uiOpened = false;
    }

    public void OnInteract(GameObject interactor)
    {
        Initialize();
    }

    public List<CraftRecipe> ReceiveCraftingRecipes()
    {
        CraftRecipe[] recipes = Resources.LoadAll<CraftRecipe>(pathToRecipes);
        return recipes.ToList();
    }
}