using System.Collections.Generic;
using UnityEngine;

public static class Rarities
{
	public enum Rarity
	{
		Basic,
		Uncommon,
		Rare,
		Legendary,
		Mythical
	}

	private static Dictionary<Rarity, Color> rarityMap = new Dictionary<Rarity, Color>()
	{
		{ Rarity.Basic, Color.white },
		{ Rarity.Uncommon, Color.dodgerBlue },
		{ Rarity.Rare, Color.lightGreen },
		{ Rarity.Legendary, Color.lightGoldenRod },
		{ Rarity.Mythical, Color.pink }
	};

	public static Color ItemColor(Item item)
	{
		return rarityMap[item.Rarity];
	}
}