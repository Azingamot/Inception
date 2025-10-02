using UnityEngine;

public class InventoryTick : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private void Start()
    {
        inventoryManager = InventoryManager.instance;
    }

    private void Update()
    {
        Item itemInHand = inventoryManager.GetSelectedItem();    
        if (itemInHand != null)
        {
            itemInHand.ItemUsage.InHandTick();
        }
    }
}
