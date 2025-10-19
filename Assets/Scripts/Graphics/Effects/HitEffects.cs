using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffects: MonoBehaviour
{
    [Header("Effects List")]
    [SerializeField] private EffectType[] effectsOnHit;
    [SerializeField] private EffectType[] effectsOnDeath;
    [Header("Effects Data")]
    [SerializeField] private Material hitMaterial;
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private float shrinkAmount;

    private Transform mainTransform;
    private SpriteRenderer mainRenderer;
    private Dictionary<SpriteRenderer, Material> baseMaterials = new();
    private Coroutine afterHitCoroutine, shrinkCoroutine;
    private Vector3 baseScale;

    private void Start()
    {
        mainTransform = transform;
        TryGetComponent<SpriteRenderer>(out mainRenderer);
    }

    public void Initialize(Transform mainTransform, SpriteRenderer mainRenderer)
    {
        this.mainTransform = mainTransform;
        this.mainRenderer = mainRenderer;

        baseScale = transform.localScale;
        GatherMaterials();
    }

    private void GatherMaterials()
    {
        baseMaterials.Add(mainRenderer, mainRenderer.material);

        for (int i = 0; i < mainTransform.transform.childCount; i++)
        {
            mainTransform.transform.GetChild(i).TryGetComponent<SpriteRenderer>(out SpriteRenderer childRenderer);
            if (childRenderer != null && !baseMaterials.ContainsKey(childRenderer))
                baseMaterials.Add(childRenderer, childRenderer.material);
        }
    }

    public void HitEvent(float timeToFinish = 0.5f)
    {
        IterateThroughEffects(effectsOnHit, timeToFinish);
    }
    
    public void DeathEvent(float timeToFinish = 0.5f)
    {
        IterateThroughEffects(effectsOnDeath, timeToFinish);
    }

    private void IterateThroughEffects(EffectType[] effects, float timeToFinish)
    {
        foreach (EffectType effect in effects)
        {
            PlayEffect(effect, timeToFinish);
        }
    }

    private void PlayEffect(EffectType effectType, float timeToFinish)
    {
        switch (effectType)
        {
            case EffectType.CameraShake:
                CameraEffects.Instance.ApplyShake();
                break;
            case EffectType.Particles:
                hitParticles.Play();
                break;
            case EffectType.Blink:
                BlinkEffect(timeToFinish);
                break;
            case EffectType.Shrink:
                ShrinkEffect(timeToFinish);
                break;
            case EffectType.Dissipate:
                StartCoroutine(DissipateTimer(timeToFinish));
                break;
            default:
                break;
        }
    }

    private void BlinkEffect(float timeToFinish)
    {
        if (afterHitCoroutine != null)
            StopCoroutine(afterHitCoroutine);
        afterHitCoroutine = StartCoroutine(AfterHitBlink(timeToFinish));
    }

    private void ShrinkEffect(float timeToFinish)
    {
        if (shrinkCoroutine != null)
            StopCoroutine(shrinkCoroutine);
        shrinkCoroutine = StartCoroutine(ShrinkOnHit(timeToFinish));
    }

    private void ChangeMaterial(Material material)
    {
        mainRenderer.material = material ?? baseMaterials[mainRenderer];
        for (int i = 0; i < mainTransform.transform.childCount; i++)
        {
            mainTransform.transform.GetChild(i).TryGetComponent<SpriteRenderer>(out SpriteRenderer childRenderer);
            if (childRenderer != null)
                childRenderer.material = material ?? baseMaterials[childRenderer];
        }
    }

    private IEnumerator AfterHitBlink(float timeToFinish)
    {
        ChangeMaterial(hitMaterial);
        yield return new WaitForSeconds(timeToFinish);
        ChangeMaterial(null);
    }

    private IEnumerator DissipateTimer(float timeToDeath)
    {
        float elapsedTime = 0;
        float alpha = 1;
        while (alpha > 0)
        {
            elapsedTime += Time.deltaTime;
            alpha = Mathf.Lerp(1f, 0f, elapsedTime / timeToDeath);
            mainRenderer.color = new Color(mainRenderer.color.r, mainRenderer.color.g, mainRenderer.color.b, alpha);
            for (int i = 0; i < mainTransform.transform.childCount; i++)
            {
                mainTransform.transform.GetChild(i).TryGetComponent<SpriteRenderer>(out SpriteRenderer childRenderer);
                if (childRenderer != null)
                    childRenderer.color = new Color(childRenderer.color.r, childRenderer.color.g, mainRenderer.color.b, alpha);
            }
            yield return null;
        }
    }

    private IEnumerator ShrinkOnHit(float timeToFinish)
    {
        float elapsedTime = 0;
        float shrinkValue = baseScale.x * shrinkAmount;
        float currentValue = baseScale.x;

        while (currentValue != shrinkValue)
        {
            elapsedTime += Time.deltaTime;
            currentValue = LerpScaleTo(currentValue, shrinkValue, elapsedTime / timeToFinish);
            yield return null;
        }

        elapsedTime = 0;

        while (currentValue != baseScale.x)
        {
            elapsedTime += Time.deltaTime;
            currentValue = LerpScaleTo(currentValue, baseScale.x, elapsedTime / timeToFinish);
            yield return null;
        }
    }

    private float LerpScaleTo(float from, float to, float strength)
    {
        float objectScale = Mathf.Lerp(from, to, strength);
        mainTransform.localScale = new Vector2(objectScale, mainTransform.localScale.y);
        return objectScale;
    }
}
