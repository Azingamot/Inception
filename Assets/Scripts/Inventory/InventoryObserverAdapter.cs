using UnityEngine;

public class InventoryObserverAdapter : MonoBehaviour, IObserver
{
    [SerializeField] private InventoryController controller;

    private void Awake()
    {
        PlayerItemCollection playerItemCollection = GameObject.FindAnyObjectByType<PlayerItemCollection>();
        if (playerItemCollection != null) 
            playerItemCollection.AddObserver(this);
    }

    public void OnUpdate(object context)
    {
        if (context is ItemPickupContext pickupCtx)
        {
            controller.OnItemPicked(pickupCtx);
        }
    }
}
