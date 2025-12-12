using UnityEngine;

public class HealUsage : UsableItem
{
    public HealUsage(Item item) : base(item)
    {
    }

    public override void Use()
    {
        if (item is HealItem heal)
        {
            playerItemInHand.TriggerAnimation("Use");
            HungerSystem.Instance.RaiseSaturation(heal.Saturation);
            playerItemInHand.GetComponentInParent<PlayerHealth>().AddHealth(heal.HealthRegeneration);
            InventoryController.Instance.RemoveItem(1);
            AudioSystem.PlaySound(item.SoundType);
        }
    }
}
