using UnityEngine;

[CreateAssetMenu(fileName = "FishingRodItem", menuName = "Scriptable Objects/Fishing Rod Item")]
public class FishingRodItem : Item
{
    public float FishingPower = 5;
    public float Cooldown = 1;
    public float BaitFlightTime = 1;
    public Sprite BaitSprite;

    public FishingRodItem()
    {
        ItemUsage = new FishingRodUsage(this);
    }

    public override string FormatDescription()
    {
        return $"{Cooldown}s {NamesHelper.ReceiveName("Cooldown")}\n" + $"{FishingPower} {NamesHelper.ReceiveName("Fishing Power")}\n" + base.FormatDescription();
    }
}
