using UnityEngine;

public class AxeUsage : UsableItem, IDamageDealer
{
    private DamageItem damageItem;

    public AxeUsage(DamageItem item) : base(item)
    {
        damageItem = item;
    }

    public float DamageAmount { get => damageItem.Damage; }
    public DamageType[] TypesOfDamage { get => damageItem.DamageTypes; }

    public override void Use()
    {
        if (ItemsCooldown.Instance.IsOnCooldown()) 
            return;

        playerItemInHand.SetRotation(MousePosition.GetData());
        playerItemInHand.TriggerAnimation("Slash");

        if (item is Axe axe) 
            ItemsCooldown.Instance.SetOnCooldown(axe.SwingSpeed);
    }

    public override void Stop()
    {
        if (playerItemInHand != null) playerItemInHand.StopAnimation();
        CheckIfAround.instance.Disable();
    }

    public override void Initialize(PlayerItemInHand playerItemInHand)
    {
        base.Initialize(playerItemInHand);
        CheckIfAround.instance.Enable(); 
    }
}
