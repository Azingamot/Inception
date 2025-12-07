using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class HealthSystem : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] protected float maxHp;
    [SerializeField] protected float invisibilityTime = 0.1f;
    [Header("Events")]
    [SerializeField] protected UnityEvent<Transform, SpriteRenderer> onInitialize;
    [SerializeField] protected UnityEvent<float> onHit;
    [SerializeField] protected UnityEvent<float> beforeDeath;
    [SerializeField] protected UnityEvent<float> onDeath;
    [Header("Timers")]
    [SerializeField] protected float timeToDie;
    [SerializeField] protected float timeToFinish;

    protected float hp = -1;
    public float MaxHealth { get => maxHp; set => maxHp = value; }
    public float Health { get => hp; set => hp = Mathf.Clamp(value, 0, maxHp); }
    protected bool canBeDamaged = true;

    private void Start()
    {
        if (hp == -1) hp = maxHp;
        Initialize();
    }

    protected virtual void Initialize()
    {

    }

    public void ReceiveDamage(float amount, Transform source = null)
    {
        if (!canBeDamaged) return;
        ChangeHealth(amount, source);
    }

    protected virtual void ChangeHealth(float amount, Transform source, DamageItem damageItem = null)
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

    public void ReceiveDamage(DamageItem item, Transform source = null)
    {
        ReceiveDamage(item.Damage, source);
    }

    public virtual void ReceiveDamage(float amount, DamageItem item, Transform source = null)
    {
        ReceiveDamage(amount, source);
    }
}
