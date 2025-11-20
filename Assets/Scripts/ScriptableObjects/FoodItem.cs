using UnityEngine;

[CreateAssetMenu(fileName = "FoodItem", menuName = "Scriptable Objects/Food Item")]
public class FoodItem : Item
{
    public int Saturation = 1;

	public override string FormatDescription()
	{
		return $"{Saturation} {NamesHelper.ReceiveName("Saturation")}\n" + base.FormatDescription();
	}
}
