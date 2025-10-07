using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickupNotifications : MonoBehaviour, IObserver
{
    [SerializeField] private Notification notificationPrefab;
    [SerializeField] private LayoutGroup notificationLayout;

    private void Awake()
    {
        PlayerItemCollection playerItemCollection = GameObject.FindAnyObjectByType<PlayerItemCollection>();
        playerItemCollection.AddObserver(this);
    }

    public void OnUpdate(object context)
    {
        ItemPickupContext itemContext = (ItemPickupContext)context;
        if (InventoryManager.instance.HaveSpaceForItem(itemContext.InventoryItem))
        {
            Notification notificationInstance = Instantiate<Notification>(notificationPrefab, notificationLayout.transform, false);
            notificationInstance.Initialize(itemContext);
        }
    }
}
