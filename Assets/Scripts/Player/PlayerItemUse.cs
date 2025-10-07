using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemUse : MonoBehaviour
{
    public void UseItem()
    {
        Item selected = InventoryManager.instance.GetSelectedItem();
        if (selected != null && selected.ItemUsage != null)
            selected.ItemUsage.Use();
    }
}
