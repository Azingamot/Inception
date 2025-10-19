using UnityEngine;

[System.Serializable]
public class InventorySlotData
{
    public Item ItemInSlot;
    public int Count;

    public void SetItem(Item item, int count)
    {
        ItemInSlot = item;
        Count = count;
    }

    public void Clear()
    {
        if (ItemInSlot != null && ItemInSlot.ItemUsage != null)
            ItemInSlot.ItemUsage.Stop();
        ItemInSlot = null;
        Count = 0;
    }
}
