using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextNotificationUI : MonoBehaviour
{
    [SerializeField] private Notification textNotification;
    [SerializeField] private LayoutGroup notificationLayout;
    public static TextNotificationUI Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Notify(string text)
    {
        if (notificationLayout != null)
        {
            Notification notification = Instantiate<Notification>(textNotification, notificationLayout.transform, false);
            notification.Initialize(text);
        }    
    }
}
