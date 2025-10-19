using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour, IObserver
{
    public static InventoryManager instance { get; private set; }

    [SerializeField] private GameObject hotBar, inventory;
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private InputActionReference inputNumber;

    [SerializeField] private List<InventorySlot> inventorySlots = new();

    private int selectedSlot = -1;

    public int SelectedSlot => selectedSlot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            GameObject.FindAnyObjectByType<PlayerItemCollection>().AddObserver(this);
        }
        else
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        InitializeSlots();
        ChangeSelectedSlot(0);
    }

    private void OnEnable()
    {
        inputNumber.action.started += ReadPressedInt;
    }

    private void OnDisable()
    {
        inputNumber.action.started -= ReadPressedInt;
    }

    private void ReadPressedInt(InputAction.CallbackContext callbackContext)
    {
        InputControl control = callbackContext.control;

        int number = GetNumberFromControl(control);

        if (number >= 0)
        {
            ChangeSelectedSlot(number);
        }
    }

    private int GetNumberFromControl(InputControl control)
    {
        if (int.TryParse(control.name, out int value))
            return value - 1;
        return -1;
    }

    public void ChangeSelectedSlot(int newValue)
    {
        if (newValue > 7) return;

        if (selectedSlot >= 0 && selectedSlot != newValue)
        {
            Item previous = GetSelectedItem();
            if (previous != null && previous.ItemUsage != null) previous.ItemUsage.Stop();
            inventorySlots[selectedSlot].SelectionChange(false);
        }
        inventorySlots[newValue].SelectionChange(true);
        //SelectionChangeHandler.instance.ChangeSelection(inventorySlots[newValue]);
        selectedSlot = newValue;
    }


    /// <summary>
    /// Возвращает выбранный предмет в панели инструментов
    /// </summary>
    /// <returns></returns>
    public Item GetSelectedItem()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem item = slot.GetComponentInChildren<InventoryItem>();

        if (item != null)
        {
            return item.ItemInSlot;
        }
        return null;
    }

    /// <summary>
    /// Загружает слоты для инвентаря из панели быстрого доступа и основного инвентаря
    /// </summary>
    private void InitializeSlots()
    {
        for (int i = 0; i < hotBar.transform.childCount; i++)
        {
            inventorySlots.Add(hotBar.transform.GetChild(i).GetComponentInChildren<InventorySlot>());
        }
        for (int i = 0; i < inventory.transform.childCount ;i++)
        {
            inventorySlots.Add(inventory.transform.GetChild(i).GetComponentInChildren<InventorySlot>());
        }
    }

    /// <summary>
    /// Добавляет предмет в инвентарь
    /// </summary>
    /// <param name="item">Предмет</param>
    public void AddItem(Item item, int count = 1)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemSlot == null)
            {
                SpawnNewItem(item, slot, count);
                ChangeSelectedSlot(selectedSlot);
                break;
            }
            else if (itemSlot.ItemInSlot == item && (itemSlot.Count + count) < item.MaxStack)
            {
                itemSlot.Count += count;
                itemSlot.RefreshCount();
                ChangeSelectedSlot(selectedSlot);
                break;
            }
        }
    }

    public void DecreaseItem(Item item, int count)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemSlot.ItemInSlot == item)
            {
                itemSlot.Count -= count;
                itemSlot.RefreshCount();
                if (itemSlot.Count <= 0)
                {
                    RemoveItem(itemSlot, slot);
                }  
                break;
            }
        }
    }

    public void RemoveItem(InventoryItem inventoryItem, InventorySlot slot)
    {
        if (inventoryItem.ItemInSlot.ItemUsage != null)
            inventoryItem.ItemInSlot.ItemUsage.Stop();
        Destroy(inventoryItem.gameObject);
        //if (CheckIfSelected(slot))
            //SelectionChangeHandler.instance.UpdateSelectionAfterRemove(inventorySlots[selectedSlot]);
    }

    public bool HaveSpaceForItem(Item item, int count = 1)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemSlot == null)
                return true;
            else if (itemSlot.ItemInSlot == item && (itemSlot.Count + count) < item.MaxStack)
                return true;
        }
        return false;
    }

    private bool CheckIfSelected(InventorySlot slot)
    {
        return inventorySlots[selectedSlot] == slot;

    }

    /// <summary>
    /// Загружает предмет в инвентарь
    /// </summary>
    /// <param name="item">Предмет</param>
    /// <param name="slot">Слот</param>
    private void SpawnNewItem(Item item, InventorySlot slot, int count = 1)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item, count);
    }

    public void OnUpdate(object context)
    {
        if (context is ItemPickupContext)
        {
            ItemPickupContext pickupContext = (ItemPickupContext)context;
            if (HaveSpaceForItem(pickupContext.InventoryItem, pickupContext.ItemsCount))
            {
                AddItem(pickupContext.InventoryItem, pickupContext.ItemsCount);
                Destroy(pickupContext.ItemInstance);
            }
        }
    }

    public void InventorySlotTouched(InventorySlot touchedSlot)
    {
        int slotIndex = -1;

        for (int i = 0; i < 8; i++)
        {
            if (inventorySlots[i].Equals(touchedSlot))
            {
                slotIndex = i;
            }
        }

        if (slotIndex != -1)
            ChangeSelectedSlot(slotIndex);
    }
}
