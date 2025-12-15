using System.Collections;
using UnityEngine;

public class FishingRodUsage : UsableItem
{
    public FishingRodUsage(Item item) : base(item) { }
    private Vector2 mouseInWorld;
    private bool stopped = false;
    private FishingRodItem rodItem;
    private Player player;

    public override void Use()
    {
        if (!ItemsCooldown.Instance.IsOnCooldown() && WaterCheck() && RadiusCheck())
        {
            playerItemInHand.TriggerAnimation("Use");
            AudioSystem.PlaySound(item.SoundType);

            if (!playerItemInHand.FishingLine.IsCasted)
                Cast();
            else
                Uncast(); 
        }
    }

    private void Cast()
    {
        Vector3 centeredMousePos = TilePlacement.Instance.GetWaterTileCenter(mouseInWorld);
        playerItemInHand.FishingLine.Cast(centeredMousePos, rodItem.BaitSprite, rodItem.BaitFlightTime);
        playerItemInHand.StartCoroutine(WaitToStartFishing(centeredMousePos, rodItem.BaitFlightTime));

        player.DisableMovement();
        InventoryController.Instance.CanChangeSelection = false;
        ItemsCooldown.Instance.SetOnCooldown(rodItem.BaitFlightTime);
    }

    private void Uncast(bool result = false)
    {
        playerItemInHand.FishingLine.Uncast();
        playerItemInHand.StartCoroutine(WaitToStopFishing(rodItem.BaitFlightTime));
        ItemsCooldown.Instance.SetOnCooldown(rodItem.Cooldown + rodItem.BaitFlightTime);
    }

    private IEnumerator WaitToStartFishing(Vector2 position, float time)
    {
        yield return new WaitForSeconds(time);
        FishingSystem.Instance.StartFishing(position, Uncast, rodItem.FishingPower);
    }

    private IEnumerator WaitToStopFishing(float time)
    {
        yield return new WaitForSeconds(time);
        player.EnableMovement();
        InventoryController.Instance.CanChangeSelection = true;
    }

    public override void InHandTick()
    {
        if (stopped) return;

        mouseInWorld = Camera.main.ScreenToWorldPoint(MousePosition.GetData());

        bool groundCheck = WaterCheck();
        bool radiusCheck = RadiusCheck();

        if (groundCheck && radiusCheck)
        {
            ShowSelectedTile.instance.ActivateHighlight(Vector3Int.CeilToInt(mouseInWorld), Color.white);
        }
        else
        {
            ShowSelectedTile.instance.DeactivateHighlight();
        }
    }

    private bool WaterCheck()
    {
        return TilePlacement.Instance.CheckWater(mouseInWorld);
    }

    private bool RadiusCheck()
    {
        return PlayerPosition.CheckIfInRadius(mouseInWorld, 5);
    }

    public override void Initialize(PlayerItemInHand playerItemInHand)
    {
        base.Initialize(playerItemInHand);
        rodItem = item as FishingRodItem;
        stopped = false;
        player = playerItemInHand.GetComponentInParent<Player>();
    }

    public override void Stop()
    {
        base.Stop();
        stopped = true;
        ShowSelectedTile.instance.DeactivateHighlight();
    }
}
