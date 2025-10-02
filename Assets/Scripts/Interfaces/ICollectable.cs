using UnityEngine;

/// <summary>
/// Интерфейс для предметов, которые игрок может поднять
/// </summary>
public interface ICollectable
{
    public Item Item { get; set; }
    public void Initialize(Item item);
    public ItemPickupContext Collect();
}
