[System.Serializable]
public class InventorySlotInfo
{
    public string itemUID;
    public int count;
    public int slotIndex;

    public InventorySlotInfo(int slotIndex, Item item, int count)
    {
        this.slotIndex = slotIndex;
        if (item != null)
            itemUID = item.UID;
        this.count = count;
    }
}
