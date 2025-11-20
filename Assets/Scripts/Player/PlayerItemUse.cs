using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemUse : MonoBehaviour
{
    private InventoryController inventoryController;

    private void Start()
    {
        inventoryController = InventoryController.Instance;
    }

    public void UseItem()
    {
        Item selected = inventoryController.GetSelectedItem();
        if (selected != null && selected.ItemUsage != null && !UIHelper.IsPointerOverUI())
            selected.ItemUsage.Use();
    }
}
