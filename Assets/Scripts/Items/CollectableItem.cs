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
        return new ItemPickupContext(gameObject, collectableItem, itemsCount);
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
