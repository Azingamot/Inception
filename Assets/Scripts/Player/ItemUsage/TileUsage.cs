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
            if (GroundCheck() && RadiusCheck())
            {
                TileItem tileItem = item as TileItem;
                TilePlacement.instance.PlaceGroundTile(tileItem.TileToPlace, mouseInWorld);
                playerItemInHand.TriggerAnimation("Use");
                InventoryController.Instance.RemoveItem(1);
            }
        }
    }

    public override void InHandTick()
    {
        if (stopped) return;

        mouseInWorld = Camera.main.ScreenToWorldPoint(MousePosition.GetData());

        bool groundCheck = GroundCheck();
        bool radiusCheck = RadiusCheck();

        if (groundCheck && radiusCheck)
        {
            ShowSelectedTile.instance.ActivateHighlight(Vector3Int.CeilToInt(mouseInWorld), Color.white);
        }
        else if (groundCheck)
        {
            ShowSelectedTile.instance.ActivateHighlight(Vector3Int.CeilToInt(mouseInWorld), Color.red);
        }
        else
        {
            ShowSelectedTile.instance.DeactivateHighlight();
        }
    }

    private bool GroundCheck()
    {
        return !TilePlacement.instance.CheckGround(mouseInWorld);
    }

    private bool RadiusCheck()
    {
        return PlayerPosition.CheckIfInRadius(mouseInWorld, 3);
    }

    public override void Initialize(PlayerItemInHand playerItemInHand)
    {
        base.Initialize(playerItemInHand);
        stopped = false;
    }

    public override void Stop()
    {
        base.Stop();
        stopped = true;
        ShowSelectedTile.instance.DeactivateHighlight();
    }
}
