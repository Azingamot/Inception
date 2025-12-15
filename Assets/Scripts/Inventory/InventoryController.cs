using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryUI ui;
    [SerializeField] private float scrollThreshold = 0.3f;
    [SerializeField] private InputActionReference inputNumber, inputScroll;
    [SerializeField] private ItemDescription itemDescription;
    public InventoryData data { get; private set; }

    private int selectedSlotIndex = -1;

    public int SelectedSlotIndex => selectedSlotIndex;

    public static InventoryController Instance { get; private set; }
    private Item previousItem;

    public bool CanChangeSelection { get; set; } = true;

    public void Initialize(SaveData saveData = null)
    {
        if (Instance == null)
        {
            Instance = this;
        }

        data = new InventoryData();
        List<InventorySlotUI> uis = ui.InitializeSlots();

        if (saveData != null)
            LoadDataFromSave(uis, saveData.InventorySlotsInfo);
        else
            FirstInitialization(uis);
    }

    private void FirstInitialization(List<InventorySlotUI> uis)
    {
        for (int i = 0; i < uis.Count; i++)
        {
            InventorySlotData slotData = new InventorySlotData(i);
            data.AddData(slotData);
            uis[i].Setup(slotData, i, this, itemDescription);
        }
    }

    public void LoadDataFromSave(List<InventorySlotUI> uis, List<InventorySlotInfo> slots)
    {
        for (int i = 0; i < uis.Count; i++)
        {
            InventorySlotInfo slotInfo = slots[i];

            InventorySlotData inventorySlotData = new InventorySlotData(slotInfo.slotIndex) 
            { 
                ItemInSlot = ResourcesHelper.FindItemResource<Item>(slotInfo.itemUID),
                Count = slotInfo.count
            };
            data.AddData(inventorySlotData);
            uis[i].Setup(inventorySlotData, slots[i].slotIndex, this, itemDescription);
        }
    }

    public List<InventorySlotInfo> Save()
    {
        List<InventorySlotInfo> slotInfos = new List<InventorySlotInfo>();
        foreach (InventorySlotData slotData in data.Slots)
        {
            slotInfos.Add(new InventorySlotInfo(slotData.Index, slotData.ItemInSlot, slotData.Count));
        }
        return slotInfos;
    }

    private void OnEnable()
    {
        inputNumber.action.started += OnNumberPressed;
    }

    private void OnDisable()
    {
        inputNumber.action.started -= OnNumberPressed;
    }

    private void Update()
    {
        OnMouseScroll();
    }

    private void OnMouseScroll()
    {
        int scrollValue = MouseScrollValue();

        if (scrollValue != 0) ChangeSelectedSlot(ClampSlotIndex(SelectedSlotIndex + scrollValue));
    }

    private int MouseScrollValue()
    {
        if (UIHelper.IsPointerOverUI()) return 0;

        float scroll = inputScroll.action.ReadValue<float>();
        if (scroll > scrollThreshold)
            return 1;
        else if (scroll < -scrollThreshold)
            return -1;
        else
            return 0;
    }

    private void OnNumberPressed(InputAction.CallbackContext ctx)
    {
        var control = ctx.control;
        if (int.TryParse(control.name, out int number))
        {
            int index = number - 1;
            if (index >= 0 && index < ui.HotbarSlotsCount)
            {
                ChangeSelectedSlot(index);
            }
        }
    }

    public void ChangeSelectedSlot(int newIndex)
    {
        if (!CanChangeSelection)
            return;

        if (newIndex < 0 || newIndex >= ui.HotbarSlotsCount || Time.timeScale == 0) return;

        int prevIndex = SelectedSlotIndex;
        selectedSlotIndex = newIndex;

        if (selectedSlotIndex >= 0)
        {
            var item = GetSelectedItem();
            if (previousItem != null && !previousItem.Compare(item))
            {
                previousItem?.ItemUsage?.Stop();
            }
            previousItem = item;
            ui.SetSlotSelected(prevIndex, false);
        }

        ui.SetSlotSelected(selectedSlotIndex, true);
        SelectionChangeHandler.instance.ChangeSelection(data.Slots[selectedSlotIndex]);
    }

    private int ClampSlotIndex(int index)
    {
        if (index < 0) return ui.HotbarSlotsCount - 1;
        if (index >= ui.HotbarSlotsCount) return 0;
        return index;
    }

    public Item GetSelectedItem()
    {
        if (selectedSlotIndex < 0) return null;
        return data.Slots[selectedSlotIndex].ItemInSlot;
    }

    public void AddItem(Item item, int count = 1)
    {
        if (data.AddItem(item, count))
        {
            Refresh();
        }
    }

    public void RemoveItem(int slotIndex, int count)
    {
        data.RemoveItem(slotIndex, count);
        Refresh();
    }

    public void RemoveItem(Item item, int count)
    {
        InventorySlotData slotData = data.GetSlotWithItem(item, count);
        if (slotData != null)
        {
           data.RemoveItem(slotData, count);
        }
        Refresh();
    }

    public void RemoveItem(int count)
    {
        data.RemoveItem(selectedSlotIndex, count);
        ui.RefreshSlots(data.Slots);
        ChangeSelectedSlot(selectedSlotIndex);
    }

    public void OnItemPicked(ItemPickupContext ctx)
    {
        if (data.HaveSpaceForItem(ctx.InventoryItem, ctx.ItemsCount))
        {
            ChangeSelectedSlot(SelectedSlotIndex);
            AddItem(ctx.InventoryItem, ctx.ItemsCount);
            Destroy(ctx.ItemInstance);
        }
    }

    public void SwapItems(int fromIndex, int toIndex)
    {
        var fromSlot = data.Slots[fromIndex];
        var toSlot = data.Slots[toIndex];

        var tempItem = toSlot.ItemInSlot;
        var tempCount = toSlot.Count;

        toSlot.SetItem(fromSlot.ItemInSlot, fromSlot.Count);

        ChangeSelectedSlot(selectedSlotIndex);

        fromSlot.SetItem(tempItem, tempCount);

        Refresh();
    }

    private void Refresh()
    {
        ChangeSelectedSlot(SelectedSlotIndex);
        ui.RefreshSlots(data.Slots);
    }
}
