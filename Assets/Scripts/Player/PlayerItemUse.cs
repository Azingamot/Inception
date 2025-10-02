using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemUse : MonoBehaviour
{
    [SerializeField] private InputActionReference useInput;

    private void OnEnable()
    {
        useInput.action.started += UseItem;
    }

    private void OnDisable()
    {
        useInput.action.started -= UseItem;
    }

    private void UseItem(InputAction.CallbackContext context)
    {
        Item selected = InventoryManager.instance.GetSelectedItem();
        if (selected != null && selected.ItemUsage != null)
            selected.ItemUsage.Use();
    }
}
