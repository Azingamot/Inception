using UnityEngine;

/// <summary>
/// ��������� ��� ���������, ������� ����� ����� �������
/// </summary>
public interface ICollectable
{
    public Item Item { get; set; }
    public void Initialize(Item item);
    public ItemPickupContext Collect();
}
