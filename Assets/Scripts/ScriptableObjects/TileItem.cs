using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileItem", menuName = "Scriptable Objects/Tile Item")]
public class TileItem : Item
{
    public TileBase TileToPlace;

    public TileItem()
    {
        ItemUsage = new TileUsage(this);
    }
}
