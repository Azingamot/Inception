using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryData
{
    public List<InventorySlotData> Slots { get; private set; } = new();
    public int HotbarSize { get; private set; } = 8;

    public void AddData(InventorySlotData data)
    {
        this.Slots.Add(data);
    }

    public InventorySlotData GetSlotWithItem(Item item, int count = 1)
    {
        return Slots.Where(u => u.ItemInSlot != null && u.ItemInSlot.Compare(item) && u.Count >= count).FirstOrDefault();
    }

    public bool AddItem(Item item, int count = 1)
    {
        foreach (var slot in Slots)
        {
            if (slot.ItemInSlot != null && slot.ItemInSlot.Compare(item) && slot.Count < item.MaxStack)
            {
                int available = item.MaxStack - slot.Count;
                int toAdd = Mathf.Min(count, available);
                slot.Count += toAdd;
                count -= toAdd;
                if (count <= 0) return true;
            }
            else if (slot.ItemInSlot == null)
            {
                slot.SetItem(item, count);
                count = 0;
                break;
            }
        }

        return count <= 0;
    }

    public bool HaveSpaceForItem(Item item, int count = 1)
    {
        int needed = count;
        foreach (var slot in Slots)
        {
            if (slot.ItemInSlot == null)
            {
                needed = 0;
                break;
            }
            else if (slot.ItemInSlot == item && slot.Count < item.MaxStack)
            {
                needed -= (item.MaxStack - slot.Count);
                if (needed <= 0) break;
            }
        }
        return needed <= 0;
    }

    public void RemoveItem(int slotIndex, int count)
    {
        var slot = Slots[slotIndex];
        if (slot.ItemInSlot != null)
        {
            slot.Count -= count;
            if (slot.Count <= 0)
            {
                slot.Clear();
            }
        }
    }
}
