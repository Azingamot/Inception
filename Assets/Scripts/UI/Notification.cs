using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Notification : MonoBehaviour
{
    [SerializeField] private TMP_Text itemText;
    [SerializeField] private Image itemImage;
    [SerializeField] private float destroyTimeScale = 1;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = destroyTimeScale;
    }

    public void Initialize(ItemPickupContext itemPickupContext)
    {
        itemText.text = $"{itemPickupContext.InventoryItem.Name} ({itemPickupContext.ItemsCount})";
        itemText.color = Rarities.ItemColor(itemPickupContext.InventoryItem);
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
