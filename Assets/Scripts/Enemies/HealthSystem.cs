using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [Header("UI")]
    [SerializeField] private HealthIndicator healthIndicator;
    [Header("Health")]
    [SerializeField] private DamageType requiredType;
    [SerializeField] private float maxHp;
    [SerializeField] private float invisibilityTime = 0.1f;
    [Header("Events")]
    [SerializeField] private UnityEvent<Transform, SpriteRenderer> onInitialize;
    [SerializeField] private UnityEvent<float> onHit;
    [SerializeField] private UnityEvent<float> beforeDeath;
    [SerializeField] private UnityEvent<float> onDeath;
    [Header("Effects")]
    [SerializeField] private Transform mainTransform;
    [SerializeField] private float timeToDie;
    [SerializeField] private float timeToFinish;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float hp;
    public float MaxHealth { get => maxHp; set => maxHp = value; }
    public float Health { get => hp; set => hp = Mathf.Clamp(value, 0, maxHp); }
    private bool canBeDamaged = true;

    private void Start()
    {
        onInitialize?.Invoke(mainTransform, spriteRenderer);
        hp = maxHp;
    }

    public void ReceiveDamage(float amount)
    {
        ChangeHealth(amount);
    }

    public void ReceiveDamage(float amount, DamageItem item)
    {
        if (CheckType(item.DamageTypes))
        {
            ChangeHealth(amount);
        }
    }

    public void ReceiveDamage(DamageItem item)
    {
        if (!canBeDamaged) return;
        ChangeHealth(item.Damage);
    }

    private void ChangeHealth(float amount)
    {
        Health -= amount;
        if (Health == 0)
        {
            canBeDamaged = false;
            StopAllCoroutines();
            beforeDeath?.Invoke(timeToDie);
            StartCoroutine(DieInTime());
        }
        else
        {
            onHit?.Invoke(timeToFinish);
            StartCoroutine(Invisibility());
        }
        healthIndicator.UpdateFill(hp, maxHp);
    }

    private IEnumerator Invisibility()
    {
        canBeDamaged = false;
        yield return new WaitForSeconds(invisibilityTime);
        canBeDamaged = true;
    }

    private IEnumerator DieInTime()
    {
        yield return new WaitForSeconds(timeToDie);
        onDeath?.Invoke(timeToFinish);
    }

    private bool CheckType(DamageType[] types)
    {
        return types.Contains(requiredType); 
    }
}
