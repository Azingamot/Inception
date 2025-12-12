using UnityEngine;

public class FoodUsage : UsableItem
{
    public FoodUsage(Item item) : base(item)
    {
    }

    public override void Use()
    {
        if (item is FoodItem food)
        {
            playerItemInHand.TriggerAnimation("Use");
            HungerSystem.Instance.RaiseSaturation(food.Saturation);
            InventoryController.Instance.RemoveItem(1);
            AudioSystem.PlaySound(item.SoundType);
        }
    }
}
