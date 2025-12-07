using System.Collections;
using UnityEngine;

public class TileUsage : UsableItem
{
    public TileUsage(Item item) : base(item) { }
    private Vector2 mouseInWorld;
    private bool stopped = false;
    private TileItem tileItem;

    public override void Use()
    {
        if (GroundCheck() && RadiusCheck())
        {
            if (tileItem.TileType == TileType.Ground) TilePlacement.Instance.PlaceGroundTile(tileItem.TileToPlace, mouseInWorld);
            else TilePlacement.Instance.PlaceObjectTile(tileItem.TileToPlace, mouseInWorld);
                playerItemInHand.TriggerAnimation("Use");
            InventoryController.Instance.RemoveItem(1);
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
        return (tileItem.TileType == TileType.Ground && !TilePlacement.Instance.CheckGround(mouseInWorld)) ||
            (tileItem.TileType == TileType.Object && TilePlacement.Instance.CheckGround(mouseInWorld));
    }

    private bool RadiusCheck()
    {
        return PlayerPosition.CheckIfInRadius(mouseInWorld, 3);
    }

    public override void Initialize(PlayerItemInHand playerItemInHand)
    {
        base.Initialize(playerItemInHand);
        tileItem = item as TileItem;
        stopped = false;
    }

    public override void Stop()
    {
        base.Stop();
        stopped = true;
        ShowSelectedTile.instance.DeactivateHighlight();
    }
}
