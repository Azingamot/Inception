using UnityEngine;

/// <summary>
/// �������, ������� ����� �������
/// </summary>
public class CollectableItem : MonoBehaviour, ICollectable
{
    [SerializeField] private Item collectableItem;
    [SerializeField] private int itemsCount;
    public Item Item { get => collectableItem; set => collectableItem = value; }


    /// <summary>
    /// ������� �������� ��������
    /// </summary>
    public ItemPickupContext Collect()
    {
        if (InventoryManager.instance.AddItem(Item))
        {
            Destroy(gameObject, 0);
            return new ItemPickupContext(collectableItem, itemsCount);
        }
        return null;
    }
    
    /// <summary>
    /// ������� ������������� ��������
    /// </summary>
    /// <param name="item">������� ��� �������������</param>
    public void Initialize(Item item)
    {
        collectableItem = item;
    }
}
