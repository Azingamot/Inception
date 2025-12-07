using UnityEngine;

public class DamageDealerToPlayer : MonoBehaviour
{
    [SerializeField] private float damageAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.ReceiveDamage(damageAmount);
        }
    }
}
