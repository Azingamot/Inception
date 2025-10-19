using UnityEngine;

public class SelectionChangeHandler : MonoBehaviour
{
    [SerializeField] private SelectionChangeUI selectionChangeUI;
    public static SelectionChangeHandler instance;
    private PlayerItemInHand playerItemInHand;

    private void Awake()
    {
        playerItemInHand = GameObject.FindAnyObjectByType<PlayerItemInHand>();
        if (instance == null)
            instance = this;
        else
            Destroy(this, 0);
    }

    public void ChangeSelection(InventorySlotData data)
    {
        Item itemInSlot = data != null ? data.ItemInSlot : null;

        selectionChangeUI.UpdateSelectionText(itemInSlot);
        ChangeItemInHand(itemInSlot);
    }

    public void UpdateSelectionAfterRemove(InventorySlotData slot)
    {
        selectionChangeUI.UpdateSelectionText(null);
        ChangeItemInHand(null);
    }

    private void ChangeItemInHand(Item item)
    {
        if (playerItemInHand != null && item != null)
        {
            playerItemInHand.ChangeItemSprite(item.ItemSprite);
            playerItemInHand.SetItemAnimatorController(item.Animator);
            if (item.ItemUsage != null) item.ItemUsage.Initialize(playerItemInHand);
        }   
        else if (playerItemInHand != null)
        {
            playerItemInHand.ChangeItemSprite(null);
        }
    }
}
