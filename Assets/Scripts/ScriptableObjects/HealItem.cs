using UnityEngine;

[CreateAssetMenu(fileName = "HealItem", menuName = "Scriptable Objects/Heal Item")]
public class HealItem : Item
{
    public int Saturation = 1;
	public int HealthRegeneration = 1;

	public HealItem()
	{
		ItemUsage = new HealUsage(this);
	}

	public override string FormatDescription()
	{
		return $"{Saturation} {NamesHelper.ReceiveName("Saturation")}\n" + $"{HealthRegeneration} {NamesHelper.ReceiveName("Regeneration") }\n" + base.FormatDescription();
	}
}
