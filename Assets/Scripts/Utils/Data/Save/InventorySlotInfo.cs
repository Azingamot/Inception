[System.Serializable]
public class InventorySlotInfo
{
    public InventorySlotData data;
    public int slotIndex;

    public InventorySlotInfo(InventorySlotData data, int slotIndex)
    {
        this.data = data;
        this.slotIndex = slotIndex;
    }
}
