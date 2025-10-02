using UnityEngine;

/// <summary>
/// �������� �������� �������� ��� ������ ������ ���������� � ��������-������������
/// </summary>
public class ItemPickupContext
{
    public Item InventoryItem;
    public int ItemsCount;

    public ItemPickupContext(Item item, int itemsCount)
    {
        this.InventoryItem = item;
        this.ItemsCount = itemsCount;
    }
}
