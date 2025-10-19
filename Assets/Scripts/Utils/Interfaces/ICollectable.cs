using UnityEngine;

/// <summary>
/// Интерфейс для предметов, которые игрок может поднять
/// </summary>
public interface ICollectable
{
    public Item Item { get; set; }
    public void Initialize(Item item, int count = 1);
    public ItemPickupContext Collect();
}
