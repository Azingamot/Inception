using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform hotBarTransform;
    [SerializeField] private Transform inventoryTransform;
    [SerializeField] private GameObject inventoryItemPrefab;

    public List<InventorySlotUI> slotUis = new();

    public int HotbarSlotsCount => hotBarTransform.childCount;
    public int TotalSlotsCount => slotUis.Count;

    public void RefreshSlots(List<InventorySlotData> data)
    {
        for (int i = 0; i < slotUis.Count && i < data.Count; i++)
        {
            slotUis[i].Setup(data[i], i, InventoryController.Instance);
        }
    }

    public List<InventorySlotUI> InitializeSlots()
    {
        slotUis.Clear();

        for (int i = 0; i < hotBarTransform.childCount; i++)
        {
            var slot = hotBarTransform.GetChild(i).GetComponentInChildren<InventorySlotUI>();
            if (slot != null) slotUis.Add(slot);
        }

        for (int i = 0; i < inventoryTransform.childCount; i++)
        {
            var slot = inventoryTransform.GetChild(i).GetComponentInChildren<InventorySlotUI>();
            if (slot != null) slotUis.Add(slot);
        }
        return slotUis;
    }

    public void SetSlotSelected(int index, bool isSelected)
    {
        if (index >= 0 && index < slotUis.Count)
        {
            slotUis[index].SetSelected(isSelected);
        }
    }
}
