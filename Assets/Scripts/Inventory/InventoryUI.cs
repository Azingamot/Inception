using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform hotBarTransform;
    [SerializeField] private Transform inventoryTransform;
    [SerializeField] private GameObject inventoryItemPrefab;

    public List<InventorySlotUI> slotUIs = new();

    public int HotbarSlotsCount => hotBarTransform.childCount;
    public int TotalSlotsCount => slotUIs.Count;

    public void RefreshSlots(List<InventorySlotData> data)
    {
        for (int i = 0; i < slotUIs.Count && i < data.Count; i++)
        {
            slotUIs[i].Setup(data[i], i, InventoryController.Instance);
        }
    }

    public List<InventorySlotUI> InitializeSlots()
    {
        slotUIs.Clear();

        for (int i = 0; i < hotBarTransform.childCount; i++)
        {
            var slot = hotBarTransform.GetChild(i).GetComponentInChildren<InventorySlotUI>();
            if (slot != null) slotUIs.Add(slot);
        }

        for (int i = 0; i < inventoryTransform.childCount; i++)
        {
            var slot = inventoryTransform.GetChild(i).GetComponentInChildren<InventorySlotUI>();
            if (slot != null) slotUIs.Add(slot);
        }
        return slotUIs;
    }

    public void SetSlotSelected(int index, bool isSelected)
    {
        if (index >= 0 && index < slotUIs.Count)
        {
            slotUIs[index].SetSelected(isSelected);
        }
    }
}
