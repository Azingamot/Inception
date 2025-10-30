using UnityEngine;
using System.Collections.Generic;

public class CraftItem : MonoBehaviour
{
    public void CreateItem(CraftRecipe recipe)
    {
        List<InventorySlotData> slotData = new();
        foreach (RecipeElement element in recipe.Elements)
        {
            InventorySlotData inventorySlotData = InventoryController.Instance.data.GetSlotWithItem(element.DropItem, element.Count);
            if (inventorySlotData == null)
                return;
            slotData.Add(inventorySlotData);
        }
        Debug.Log("Success");
        InventoryController.Instance.AddItem(recipe.Result, recipe.Count);
    }
}
