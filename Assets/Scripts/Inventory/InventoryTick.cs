using UnityEngine;

public class InventoryTick : MonoBehaviour
{
    private InventoryController inventoryController;

    private void Start()
    {
       inventoryController = InventoryController.Instance;
    }
    private void Update()
    {
        Item itemInHand = inventoryController.GetSelectedItem();    
        if (itemInHand != null && itemInHand.ItemUsage != null)
        {
            itemInHand.ItemUsage.InHandTick();
        }
    }
}
