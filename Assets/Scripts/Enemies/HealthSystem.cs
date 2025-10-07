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
    [Header("Events")]
    [SerializeField] private UnityEvent onDeath;
    [Header("Effects")]
    [SerializeField] private Transform renderersRoot;
    [SerializeField] private SpriteRenderer mainRenderer;
    [SerializeField] private Material hitMaterial;
    [SerializeField] private float blinkTime;
    private float hp;
    public float MaxHealth { get => maxHp; set => maxHp = value; }
    public float Health { get => hp; set => hp = Mathf.Clamp(value, 0, maxHp); }
    private Material baseMaterial;
    private Coroutine afterHitCoroutine;

    private void Start()
    {
        hp = maxHp;
        baseMaterial = mainRenderer.material;
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
        ChangeHealth(item.Damage);
    }

    private void ChangeHealth(float amount)
    {
        Health -= amount;
        if (Health == 0)
            onDeath?.Invoke();
        else
            healthIndicator.UpdateFill(hp, maxHp);
        healthIndicator.UpdateFill(hp, maxHp);
        DamageEffects();
    }

    private bool CheckType(DamageType[] types)
    {
        return types.Contains(requiredType); 
    }

    private void DamageEffects()
    {
        CameraEffects.Instance.ApplyShake();
        if (afterHitCoroutine != null)
            StopCoroutine(afterHitCoroutine);
        afterHitCoroutine = StartCoroutine(AfterHitBlink());
    }

    private IEnumerator AfterHitBlink()
    {
        ChangeMaterial(hitMaterial);
        yield return new WaitForSeconds(blinkTime);
        ChangeMaterial(baseMaterial);
    }

    private void ChangeMaterial(Material material)
    {
        mainRenderer.material = material;
        for (int i = 0; i < renderersRoot.transform.childCount; i++)
        {
            renderersRoot.transform.GetChild(i).TryGetComponent<SpriteRenderer>(out SpriteRenderer childRenderer);
            if (childRenderer != null)
                childRenderer.material = material;
        }
    }
}
