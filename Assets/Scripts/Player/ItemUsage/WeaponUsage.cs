using UnityEngine;

public class WeaponUsage : UsableItem, IDamageDealer
{
    protected DamageItem damageItem;

    public WeaponUsage(DamageItem item) : base(item)
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
        ItemsCooldown.Instance.SetOnCooldown(damageItem.ReloadTime);
    }
}
