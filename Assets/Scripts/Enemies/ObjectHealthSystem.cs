using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ObjectHealthSystem : HealthSystem
{
    [Header("Validation")]
    [SerializeField] private DamageType requiredType;

    [Header("UI")]
    [SerializeField] private HealthIndicator healthIndicator;

    [Header("Effects")]
    [SerializeField] private Transform mainTransform;
    [SerializeField] private SpriteRenderer spriteRenderer;

    protected override void Initialize()
    {
        onInitialize?.Invoke(mainTransform, spriteRenderer);
    }

    protected override void ChangeHealth(float amount)
    {
        base.ChangeHealth(amount);
        healthIndicator.OnUpdate(hp, maxHp);
    }

    public override void ReceiveDamage(float amount, DamageItem item)
    {
        if (CheckType(item.DamageTypes))
        {
            ReceiveDamage(amount);
        }
    }

    private bool CheckType(DamageType[] types)
    {
        return types.Contains(requiredType); 
    }
}
