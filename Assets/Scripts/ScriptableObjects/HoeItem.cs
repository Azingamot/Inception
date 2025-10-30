using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "HoeItem", menuName = "Scriptable Objects/Hoe Item")]
public class HoeItem : Item
{
    public TileBase WeededEarth;
    public float Cooldown;

    public HoeItem()
    {
        ItemUsage = new HoeUsage(this);
    }
}
