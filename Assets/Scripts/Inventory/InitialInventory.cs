using System.Collections.Generic;
using UnityEngine;

public class InitialInventory : MonoBehaviour
{
    [SerializeField] private List<RecipeElement> initialItems;
    private bool isInitialized = false;
    
    public void InitializeInventory()
    {
        if (isInitialized) 
            return;

        foreach (var item in initialItems)
        {
            InventoryController.Instance.AddItem(item.Item, item.Count);
        }
        isInitialized = true;
    }
}
