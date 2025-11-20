using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIControl : MonoBehaviour
{
    [SerializeField] private InputActionReference inventoryAction;
    [SerializeField] private ItemDescription itemDescription;
    [SerializeField] private GameObject inventoryUI, inventoryBackground;
    public static InventoryUIControl Instance { get; private set; }

    [HideInInspector] public bool InventoryActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        inventoryUI.SetActive(false);
        inventoryBackground.SetActive(false);
    }

    private void OnEnable()
    {
        inventoryAction.action.started += SwitchInventory;
    }

    private void OnDisable()
    {
        inventoryAction.action.started -= SwitchInventory;
    }

    private void SwitchInventory(InputAction.CallbackContext context)
    {
        InventoryActive = !InventoryActive;

        inventoryBackground.SetActive(InventoryActive);
        inventoryUI.SetActive(InventoryActive);
        if (!InventoryActive)
            itemDescription.HideDescription();
    }

    public void ActivateInventory()
    {
        inventoryBackground.SetActive(InventoryActive);
        inventoryUI.SetActive(InventoryActive);
    }  
}
