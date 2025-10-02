using UnityEngine;

public class AxeUsage : UsableItem
{
    public AxeUsage(Item item) : base(item) { }

    public override void Use()
    {
        Debug.Log($"Axe of type {item.name} was used");
    }
}
