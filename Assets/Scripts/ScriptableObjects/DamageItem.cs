using UnityEngine;

public abstract class DamageItem : Item
{
    public float Damage = 5;
    public float ReloadTime = 2;
	public float KnockbackValue = 1f;
    public DamageType[] DamageTypes;

	public override string FormatDescription()
	{
		return $"{Damage} {NamesHelper.ReceiveName("Damage")}" +
			$"\n{ReloadTime} {NamesHelper.ReceiveName("Cooldown")}" +
			$"\n{KnockbackValue} {NamesHelper.ReceiveName("Knockback")}\n" + base.FormatDescription();
	}
}
