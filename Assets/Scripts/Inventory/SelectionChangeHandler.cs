using UnityEngine;

public class SelectionChangeHandler : MonoBehaviour
{
    [SerializeField] private SelectionChangeUI selectionChangeUI;
    public static SelectionChangeHandler instance;
    private PlayerItemInHand playerItemInHand;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this, 0);
    }

    private void Start()
    {
        playerItemInHand = GameObject.FindAnyObjectByType<PlayerItemInHand>();
    }

    public void ChangeSelection(InventorySlot slot)
    {
        InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();
        Item itemInSlot = inventoryItem != null ? inventoryItem.ItemInSlot : null;
        selectionChangeUI.UpdateSelectionText(itemInSlot);
        ChangeItemInHand(itemInSlot);
    }

    private void ChangeItemInHand(Item item)
    {
        if (playerItemInHand != null && item != null)
        {
            playerItemInHand.ChangeItemSprite(item.ItemSprite);
            playerItemInHand.SetItemAnimatorController(item.Animator);
            if (item.ItemUsage != null) item.ItemUsage.Initialize(playerItemInHand);
        }   
    }
}
