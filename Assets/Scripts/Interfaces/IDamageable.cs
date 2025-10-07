using UnityEngine;

public interface IDamageable
{
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public void ReceiveDamage(float amount, DamageItem item);
    public void ReceiveDamage(float amount);
}
