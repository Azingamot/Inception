using UnityEngine;

/// <summary>
/// Предмет, который можно поднять
/// </summary>
public class CollectableItem : MonoBehaviour, ICollectable
{
    [SerializeField] private Item collectableItem;
    [SerializeField] private int itemsCount;
    public Item Item { get => collectableItem; set => collectableItem = value; }


    /// <summary>
    /// Событие поднятия предмета
    /// </summary>
    public ItemPickupContext Collect()
    {
        return new ItemPickupContext(gameObject, collectableItem, itemsCount);
    }
    
    /// <summary>
    /// Событие инициализации предмета
    /// </summary>
    /// <param name="item">Предмет для инициализации</param>
    public void Initialize(Item item)
    {
        collectableItem = item;
    }
}
