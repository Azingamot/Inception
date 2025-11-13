using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Notification : MonoBehaviour
{
    [SerializeField] private TMP_Text itemText;
    [SerializeField] private Image itemImage;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Initialize(ItemPickupContext itemPickupContext)
    {
        itemText.text = $"{itemPickupContext.InventoryItem.name} ({itemPickupContext.ItemsCount})";
        itemImage.sprite = itemPickupContext.InventoryItem.ItemSprite;
    }

    public void Initialize(string text, Sprite sprite)
    {
        itemText.text = text;
        itemImage.sprite = sprite;
    }

    public void Initialize(string text)
    {
        itemText.text = text;
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
