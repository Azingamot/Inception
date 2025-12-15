using UnityEngine;

[System.Serializable]
public class FishingLootRarity
{
    public string Name;
    public float Speed = 180f;
    public float Size = 30f;
    public float LowerThreshold = 0;
    public float UpperThreshold = 100;
    public LootTable LootTable;
}
