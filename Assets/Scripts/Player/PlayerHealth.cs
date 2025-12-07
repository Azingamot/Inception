using UnityEngine;

public class PlayerHealth : HealthSystem
{
    [Header("UI")]
    [SerializeField] private FilledBarUI healthBar;

    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Player player;

    public void Load(Player player)
    {
        this.player = player;
    }

    protected override void Initialize()
    {
        onInitialize.Invoke(transform, spriteRenderer);
    }

    protected override void ChangeHealth(float amount, Transform source = null, DamageItem damageItem = null)
    {
        base.ChangeHealth(amount, source);
        healthBar.ChangeBarFill(hp, maxHp);
        if (source != null)
            player.Knockback(source, 3);
    }
}
