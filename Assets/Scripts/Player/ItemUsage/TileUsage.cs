using System.Collections;
using UnityEngine;

public class TileUsage : UsableItem
{
    public TileUsage(Item item) : base(item) { }
    private Vector2 mouseInWorld;
    private bool stopped = false;

    public override void Use()
    {
        if (item is TileItem)
        {
            if (!TilePlacement.instance.CheckGround(mouseInWorld))
            {
                TileItem tileItem = item as TileItem;
                TilePlacement.instance.PlaceTile(tileItem.TileToPlace, mouseInWorld);
                InventoryManager.instance.DecreaseItem(this.item, 1);
            }
        }
    }

    public override void InHandTick()
    {
        if (stopped) return;

        mouseInWorld = Camera.main.ScreenToWorldPoint(MousePosition.GetData());

        if (!TilePlacement.instance.CheckGround(mouseInWorld) && PlayerPosition.CheckIfInRadius(mouseInWorld, 3))
        {
            ShowSelectedTile.instance.ActivateHighlight(Vector3Int.CeilToInt(mouseInWorld), Color.white);
        }
        else
        {
            ShowSelectedTile.instance.ActivateHighlight(Vector3Int.CeilToInt(mouseInWorld), Color.red);
        }
    }

    public override void Initialize(PlayerItemInHand playerItemInHand)
    {
        base.Initialize(playerItemInHand);
        stopped = false;
    }

    public override void Stop()
    {
        ShowSelectedTile.instance.DeactivateHighlight();
        stopped = true;
    }
}
