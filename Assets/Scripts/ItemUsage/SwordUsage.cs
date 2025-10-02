using UnityEngine;

public class SwordUsage : UsableItem
{
    public SwordUsage(Item item) : base(item) { }

    public override void Use()
    {
        Debug.Log($"Sword of type {item.name} was used");
    }
}
