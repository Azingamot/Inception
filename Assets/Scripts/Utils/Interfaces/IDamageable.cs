using UnityEngine;

public interface IDamageable
{
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public void ReceiveDamage(float amount);
    public virtual void ReceiveDamage(float amount, DamageItem item) { }
}
