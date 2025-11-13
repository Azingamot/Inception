using UnityEngine;

public abstract class DamageItem : Item
{
    public float Damage = 5;
    public float ReloadTime = 2;
    public DamageType[] DamageTypes;
}
