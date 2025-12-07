using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyHealth : HealthSystem
{
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private SpriteRenderer mainRenderer;
    private EnemyController enemyController;

    protected override void Initialize()
    {
        enemyController = GetComponent<EnemyController>();
        MaxHealth = enemyController.Stats.MaxHealth;
        Health = MaxHealth;
        onInitialize.Invoke(spriteTransform, mainRenderer);
    }

    protected override void ChangeHealth(float amount, Transform source, DamageItem damageItem = null)
    {
        base.ChangeHealth(amount, source);
        float weaponKnockbackValue = damageItem == null ? 1 : damageItem.KnockbackValue;
        if (source != null) 
            enemyController.Knockback(source.position, enemyController.Stats.KnockbackValue + weaponKnockbackValue);
    }
}
