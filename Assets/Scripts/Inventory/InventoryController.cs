using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryUI ui;
    [SerializeField] private InputActionReference inputNumber;

    public InventoryData data { get; private set; }
    private int selectedSlotIndex = -1;

    public int SelectedSlotIndex => selectedSlotIndex;

    public static InventoryController Instance { get; private set; }
    private Item previousItem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        data = new InventoryData();
        InitializeUI();
    }

    private void InitializeUI()
    {
        List<InventorySlotUI> uis = ui.InitializeSlots();
        Debug.Log(uis.Count);
        for (int i = 0; i < uis.Count; i++)
        {
            InventorySlotData slotData = new InventorySlotData();
            data.AddData(slotData);
            uis[i].Setup(slotData, i, this);
            InventoryInfo.inventorySlotInfos.Add(new InventorySlotInfo(slotData, i));
        }
    }

    private void OnEnable()
    {
        inputNumber.action.started += OnNumberPressed;
    }

    private void OnDisable()
    {
        inputNumber.action.started -= OnNumberPressed;
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
        if (newIndex < 0 || newIndex >= ui.TotalSlotsCount) return;

        if (selectedSlotIndex >= 0)
        {
            var item = GetSelectedItem();
            if (previousItem != item) item?.ItemUsage?.Stop();
            previousItem = item;
            ui.SetSlotSelected(selectedSlotIndex, false);
        }

        ui.SetSlotSelected(newIndex, true);
        selectedSlotIndex = newIndex;
        SelectionChangeHandler.instance.ChangeSelection(data.Slots[selectedSlotIndex]);
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
