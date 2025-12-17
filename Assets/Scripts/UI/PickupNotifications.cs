using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickupNotifications : MonoBehaviour, IObserver
{
    [SerializeField] private Notification notificationPrefab;
    [SerializeField] private LayoutGroup notificationLayout;
    private InventoryController inventoryController;

    private void Start()
    {
        PlayerItemCollection playerItemCollection = GameObject.FindAnyObjectByType<PlayerItemCollection>();
        playerItemCollection.AddObserver(this);
        inventoryController = InventoryController.Instance;
    }

    public void OnUpdate(object context)
    {
        ItemPickupContext itemContext = (ItemPickupContext)context;
        if (inventoryController.Data.HaveSpaceForItem(itemContext.InventoryItem))
        {
            Notification notificationInstance = Instantiate<Notification>(notificationPrefab, notificationLayout.transform, false);
            notificationInstance.Initialize(itemContext);
        }
    }
}
