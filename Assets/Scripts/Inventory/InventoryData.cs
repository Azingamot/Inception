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
        count = CheckIfAlreadyExisting(item, count);
        if (count <= 0)
            return true;

        foreach (var slot in Slots)
        {
            if (slot.ItemInSlot == null)
            {
                SetNewItemToSlot(slot, item, count);
                count = 0;
                break;
            }
        }

        return count <= 0;
    }

    private int CheckIfAlreadyExisting(Item item, int count)
    {
        foreach (var slot in Slots)
        {
            if (slot.ItemInSlot != null && slot.ItemInSlot.Compare(item) && slot.Count < item.MaxStack)
            {
                int available = item.MaxStack - slot.Count;
                int toAdd = Mathf.Min(count, available);
                slot.Count += toAdd;
                count -= toAdd;
                return count;
            }
        }
        return count;
    }

    private bool SetNewItemToSlot(InventorySlotData slot, Item item, int count)
    {
        if (slot.ItemInSlot == null)
        {
            slot.SetItem(item, count);
            return true;
        }
        return false;
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
        RemoveItemFromSlot(slot, count);
    }

    public void RemoveItem(InventorySlotData slotData, int count)
    {
       RemoveItemFromSlot(slotData, count);
    }

    private void RemoveItemFromSlot(InventorySlotData slotData, int count)
    {
        if (slotData.ItemInSlot != null)
        {
            slotData.Count -= count;
            if (slotData.Count <= 0)
            {
                slotData.Clear();
            }
        }
    }
}
