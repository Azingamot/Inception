using System.Collections;
using UnityEngine;

public class HoeUsage : UsableItem
{
    public HoeUsage(Item item) : base(item) { }
    private Vector2 mouseInWorld;
    private bool stopped = false;

    public override void Use()
    {
        if (item is HoeItem hoe)
        {
            if (GroundCheck() && RadiusCheck() && !ItemsCooldown.Instance.IsOnCooldown())
            {
                TilePlacement.instance.PlaceAbovegroundTile(hoe.WeededEarth, mouseInWorld);
                playerItemInHand.TriggerAnimation("Use");
                ItemsCooldown.Instance.SetOnCooldown(hoe.Cooldown);
            }
        }
    }

    public override void InHandTick()
    {
        if (stopped) return;

        mouseInWorld = Camera.main.ScreenToWorldPoint(MousePosition.GetData());

        bool groundCheck = GroundCheck();
        bool radiusCheck = RadiusCheck();
        bool cooldownCheck = !ItemsCooldown.Instance.IsOnCooldown();

        if (groundCheck && radiusCheck && cooldownCheck)
        {
            ShowSelectedTile.instance.ActivateHighlight(Vector3Int.CeilToInt(mouseInWorld), Color.white);
        }
        else if (groundCheck && cooldownCheck)
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
        return TilePlacement.instance.CheckGround(mouseInWorld);
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
        stopped = true;
        ShowSelectedTile.instance.DeactivateHighlight();
    }
}
