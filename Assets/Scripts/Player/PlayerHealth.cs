using UnityEngine;

public class PlayerHealth : HealthSystem
{
    [Header("UI")]
    [SerializeField] private FilledBarUI healthBar;

    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    protected override void Initialize()
    {
        onInitialize.Invoke(transform, spriteRenderer);
    }

    protected override void ChangeHealth(float amount)
    {
        base.ChangeHealth(amount);
        healthBar.ChangeBarFill(hp, maxHp);
    }
}
