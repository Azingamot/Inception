using UnityEngine;

/// <summary>
/// �������� �������� �������� ��� ������ ������ ���������� � ��������-������������
/// </summary>
public class ItemPickupContext
{
    public Item InventoryItem;
    public GameObject ItemInstance;
    public int ItemsCount;

    public ItemPickupContext(Item item, int itemsCount)
    {
        this.InventoryItem = item;
        this.ItemsCount = itemsCount;
    }

    public ItemPickupContext(GameObject itemInstance, Item item, int itemsCount)
    {
        this.ItemInstance = itemInstance;
        this.ItemsCount = itemsCount;
        this.InventoryItem = item;
    }
}
