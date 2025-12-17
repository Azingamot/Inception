using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OpenCraftingStation : MonoBehaviour
{
    [SerializeField] private CraftingStation defaultCraftingStation;
    [SerializeField] private CraftingUI craftingUI;
    [SerializeField] private LayerMask stationMask;
    private List<CraftingStation> currentStations = new();
    private List<CraftRecipe> currentRecipes = new();
    private Coroutine checkCoroutine;

    private void Awake()
    {
        CloseRecipesUI();
    }

    private void OnEnable()
    {
        checkCoroutine = StartCoroutine(CheckForCraftingStations());
    }

    private void OnDisable()
    {
        if (checkCoroutine != null)
            StopCoroutine(checkCoroutine);
    }

    private void OpenRecipesUI(List<CraftRecipe> recipes, bool firstOpen)
    {
        craftingUI.Open(recipes, firstOpen);
    }

    private void CloseRecipesUI()
    {
        craftingUI.Close();
    }

    private IEnumerator CheckForCraftingStations()
    {
        while (true)
        {
            if (craftingUI.IsOpened)
            {
                ReloadRecipes();
            }
            yield return new WaitForSeconds(1);
        }
    }

    private List<CraftingStation> FindCraftingStations(Vector2 pos)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 3, stationMask);
        List<CraftingStation> result = new();
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<CraftingStation>(out CraftingStation station))
            {
                result.Add(station);
            }
        }
        return result.DistinctBy(u => u.RecipesFolder).ToList();
    }

    public void Open()
    {
        if (!craftingUI.IsOpened)
        {
            ReloadRecipes();
        }
        else
        {
            CloseRecipesUI();
        }
    }

    private void ReloadRecipes()
    {
        Vector2 playerPos = PlayerPosition.GetPosition();
        List<CraftingStation> craftingStations = FindCraftingStations(playerPos);

        bool stationsListChanged = !CompareLists(craftingStations, currentStations);

        if (stationsListChanged)
        {
            currentStations = craftingStations;
        }

        currentRecipes = ReceiveRecipesFromStations(currentStations);

        OpenRecipesUI(currentRecipes, stationsListChanged);
    }
    
    private bool CompareLists(List<CraftingStation> list1, List<CraftingStation> list2)
    {
        List<CraftingStation> result = list1.Except(list2).Union(list2.Except(list1)).ToList();
        return result.Count == 0;
    }

    private List<CraftRecipe> ReceiveRecipesFromStations(List<CraftingStation> craftingStations)
    {
        List<CraftRecipe> result = defaultCraftingStation.ReceiveCraftingRecipes();
        foreach (var craft in craftingStations)
        {
            result.AddRange(craft.ReceiveCraftingRecipes());
        }
        return result.OrderBy(u => u.Result.Rarity).ToList();
    }
}
