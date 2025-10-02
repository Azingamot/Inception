using System.Collections;
using UnityEngine;

public class TileUsage : UsableItem
{
    public TileUsage(Item item) : base(item) { }
    private Vector2 mouseInWorld;

    public override void Use()
    {
        if (item is TileItem)
        {
            if (!TilePlacement.instance.CheckGround(mouseInWorld))
            {
                TileItem tileItem = item as TileItem;
                TilePlacement.instance.PlaceTile(tileItem.TileToPlace, mouseInWorld);
            }
        }
        
        Debug.Log($"Tile of type {item.name} was used");
    }

    public override void InHandTick()
    {
        mouseInWorld = Camera.main.ScreenToWorldPoint(MousePosition.GetData());

        if (!TilePlacement.instance.CheckGround(mouseInWorld))
        {
            ShowSelectedTile.instance.ActivateHighlight(Vector3Int.CeilToInt(mouseInWorld));
        }
        else
        {
            ShowSelectedTile.instance.DeactivateHighlight();
        }
    }
}
