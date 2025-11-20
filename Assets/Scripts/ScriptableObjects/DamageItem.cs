using UnityEngine;

public abstract class DamageItem : Item
{
    public float Damage = 5;
    public float ReloadTime = 2;
    public DamageType[] DamageTypes;

	public override string FormatDescription()
	{
		return $"{Damage} {NamesHelper.ReceiveName("Damage")}\n{ReloadTime} {NamesHelper.ReceiveName("Cooldown")}\n" + base.FormatDescription();
	}
}
