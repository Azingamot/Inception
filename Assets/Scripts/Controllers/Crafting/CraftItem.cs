using UnityEngine;
using System.Collections.Generic;

public class CraftItem : MonoBehaviour
{
    public bool CreateItem(CraftRecipe recipe, DropItems dropItems, Vector2 pos)
    {
        if (CanCraftItem(recipe))
        {
            if (dropItems != null)
                dropItems.SpawnDrop(pos, recipe.Result, recipe.Count);
            else
                InventoryController.Instance.AddItem(recipe.Result, recipe.Count);
            RemoveItemsForCraft(recipe);
            return true;
        }
        return false;
    }

    private bool CanCraftItem(CraftRecipe recipe)
    {
        List<InventorySlotData> slotData = new();
        foreach (RecipeElement element in recipe.Elements)
        {
            InventorySlotData inventorySlotData = InventoryController.Instance.data.GetSlotWithItem(element.Item, element.Count);
            if (inventorySlotData == null)
                return false;
            slotData.Add(inventorySlotData);
        }
        return true;
    }

    private void RemoveItemsForCraft(CraftRecipe recipe)
    {
        foreach (RecipeElement element in recipe.Elements)
        {
            InventoryController.Instance.RemoveItem(element.Item, element.Count);
        }
    }
}
