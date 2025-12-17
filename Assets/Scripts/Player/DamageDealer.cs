using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private Transform source;
    private DamageItem item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out IDamageable component))
        {
            Item selected = InventoryController.Instance.GetSelectedItem();
            if (selected is DamageItem)
            {
                item = (DamageItem)selected;
                component.ReceiveDamage(item.Damage, item, source);
            }
        }
    }
}
