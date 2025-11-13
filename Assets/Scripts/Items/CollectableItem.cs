using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Предмет, который можно поднять
/// </summary>
[RequireComponent(typeof(SpriteRenderer),typeof(Collider2D), typeof(Rigidbody2D))]
public class CollectableItem : MonoBehaviour, ICollectable
{
    [SerializeField] private bool initializeOnStart = false;
    [SerializeField] private Item collectableItem;
    [SerializeField] private int itemsCount;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private LayerMask collectableMask;

    public Item Item { get => collectableItem; set => collectableItem = value; }

    private SpriteRenderer sr;
    private Collider2D ownCollider;
    private Rigidbody2D rb;

    private float timeToWait = 1.25f;
    private bool candidateForMerge = false;
    private bool isMerging = false;
    private bool candidateForCollection = false;

    private Coroutine checkCoroutine;
    private Coroutine mergeCoroutine;

    private void Start()
    {
        if (initializeOnStart)
            Initialize(collectableItem, itemsCount);
    }

    private void OnEnable()
    {
        checkCoroutine = StartCoroutine(CheckForCollectables());
    }

    private void OnDisable()
    {
        if (checkCoroutine != null)
            StopCoroutine(checkCoroutine);
    }

    private IEnumerator WaitForCollection()
    {
        yield return new WaitForSeconds(timeToWait);
        ownCollider.enabled = true;
    }

    /// <summary>
    /// Событие поднятия предмета
    /// </summary>
    public ItemPickupContext Collect()
    {
        return new ItemPickupContext(gameObject, collectableItem, itemsCount);
    }
    
    /// <summary>
    /// Событие инициализации предмета
    /// </summary>
    /// <param name="item">Предмет для инициализации</param>
    public void Initialize(Item item, int count = 1)
    {
        rb = GetComponent<Rigidbody2D>();   
        sr = GetComponent<SpriteRenderer>();
        ownCollider = GetComponent<Collider2D>();
        SetCollider();
        SetData(item, count);
    }

    public void LaunchUp(float power)
    {
        if (rb != null)
        {
            rb.AddForce(Vector2.up * power, ForceMode2D.Impulse);
            rb.AddForce(Random.insideUnitCircle * power * 1.5f, ForceMode2D.Impulse);
            StartCoroutine(GravityControl());
        }
    }

    public void FollowObject(Transform followTransform)
    {
        if (rb == null) return;

        if (!candidateForCollection)
            candidateForCollection = true;

        Vector2 direction = (followTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * 3f;
    }

    private IEnumerator GravityControl()
    {
        float gravity = 2f;
        do
        {
            gravity -= 0.08f;
            rb.gravityScale = gravity;
            yield return new WaitForFixedUpdate();
        }
        while (gravity > 0);

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        ownCollider.enabled = true;
    }

    private void SetCollider()
    {
        ownCollider.enabled = false;
        StartCoroutine(WaitForCollection());
    }

    private void SetData(Item item, int count)
    {
        sr.sprite = item.ItemSprite;
        collectableItem = item;
        itemsCount = count;
        if (count > 1)
            countText.text = count.ToString();
    }

    private IEnumerator CheckForCollectables()
    {
        while (true)
        {
            CheckForOtherCollectables();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void CheckForOtherCollectables()
    {
        if (isMerging)
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5, collectableMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<CollectableItem>(out CollectableItem collectable) && !ReferenceEquals(this,collectable) && collectable.CanMerge(this))
            {
                if (itemsCount + collectable.itemsCount > collectable.Item.MaxStack)
                    return;

                collectable.SetForMerging(this);
                UnsetFromMerging();
            }
        }
    }

    public void SetForMerging(CollectableItem source)
    {
        candidateForMerge = true;
        mergeCoroutine = StartCoroutine(MergeCoroutine(source));
    }

    private void UnsetFromMerging()
    {
        candidateForMerge = false;
        isMerging = true;
        if (mergeCoroutine != null)
            StopCoroutine(mergeCoroutine);
    }

    private IEnumerator MergeCoroutine(CollectableItem source)
    {
        float scaleValue = transform.localScale.y;
        float elapsedTime = 0;
        do
        {
            elapsedTime += Time.deltaTime;
            scaleValue = Mathf.Lerp(scaleValue, 0, elapsedTime * 2);
            transform.localScale = new Vector3(transform.localScale.x, scaleValue, 0);
            yield return null;
        }
        while (scaleValue > 0.3f);
        source.Merge(this);
    }

    private void Merge(CollectableItem collectable)
    {
        itemsCount += collectable.itemsCount;
        countText.text = itemsCount.ToString();
        Destroy(collectable.gameObject);
        isMerging = false;
    }

    public bool CanMerge(CollectableItem collectable)
    {
        return Item.Compare(collectable.Item) && !candidateForMerge;
    }
}
