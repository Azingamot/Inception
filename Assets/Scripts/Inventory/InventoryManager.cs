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
            inventorySlots[selectedSlot].SelectionChange(false);
        }
        inventorySlots[newValue].SelectionChange(true);
        SelectionChangeHandler.instance.ChangeSelection(inventorySlots[newValue]);
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
    public void AddItem(Item item)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemSlot == null)
            {
                SpawnNewItem(item, slot);
                ChangeSelectedSlot(selectedSlot);
                break;
            }
            else if (itemSlot.ItemInSlot == item && itemSlot.Count < item.MaxStack)
            {
                itemSlot.Count++;
                itemSlot.RefreshCount();
                ChangeSelectedSlot(selectedSlot);
                break;
            }
        }
    }

    public bool HaveSpaceForItem(Item item)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemSlot == null)
                return true;
            else if (itemSlot.ItemInSlot == item && itemSlot.Count < item.MaxStack)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Загружает предмет в инвентарь
    /// </summary>
    /// <param name="item">Предмет</param>
    /// <param name="slot">Слот</param>
    private void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    public void OnUpdate(object context)
    {
        if (context is ItemPickupContext)
        {
            ItemPickupContext pickupContext = (ItemPickupContext)context;
            if (HaveSpaceForItem(pickupContext.InventoryItem))
            {
                AddItem(pickupContext.InventoryItem);
                Destroy(pickupContext.ItemInstance);
            }
        }
    }
}
