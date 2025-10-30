using System.Collections;
using UnityEngine;

public class BuildUsage : UsableItem
{
    public BuildUsage(BuildingItem item) : base(item) { }
    private Vector2 mouseInWorld;
    private bool stopped = false;
    private BuildingItem buildingItem;

    public override void Use()
    {
        if (TryPlaceObject(buildingItem, mouseInWorld))
        {
            InventoryController.Instance.RemoveItem(1);
            playerItemInHand.TriggerAnimation("Use");
        } 
    }

    public override void InHandTick()
    {
        if (stopped) return;

        mouseInWorld = Camera.main.ScreenToWorldPoint(MousePosition.GetData());

        if (CheckPlacement())
            ShowBuildingPlacement.instance.ActivateHighlight(ObjectsPlacement.Instance.PlacementPosition(mouseInWorld), Color.green, buildingItem.PreviewSprite);
        else
            ShowBuildingPlacement.instance.ActivateHighlight(ObjectsPlacement.Instance.PlacementPosition(mouseInWorld), Color.red, buildingItem.PreviewSprite);
    }
    
    private bool TryPlaceObject(BuildingItem item, Vector3 pos)
    {
        return ObjectsPlacement.Instance.PlaceObject(item, pos);
    }

    private bool CheckPlacement()
    {
        return ObjectsPlacement.Instance.CanPlaceObject(buildingItem, mouseInWorld);
    }

    public override void Initialize(PlayerItemInHand playerItemInHand)
    {
        base.Initialize(playerItemInHand);
        if (item is BuildingItem) 
            buildingItem = (BuildingItem)item;
        stopped = false;
    }

    public override void Stop()
    {
        stopped = true;
        ShowBuildingPlacement.instance.DeactivateHighlight();
    }
}
