using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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

    private Coroutine checkCoroutine;
    private Coroutine mergeCoroutine;

    private Vector3 defaultScale;

    private void Start()
    {
        defaultScale = transform.localScale;
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

    public ItemPickupContext Collect()
    {
        StopAllCoroutines();
        return new ItemPickupContext(gameObject, collectableItem, itemsCount);
    }
    
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
        if (isMerging || candidateForMerge)
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5, collectableMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<CollectableItem>(out CollectableItem collectable) && CanMerge(collectable) && collectable.CanMerge(this))
            {
                if (itemsCount + collectable.itemsCount > collectable.Item.MaxStack)
                    return;

                UnsetFromMerging();

                collectable.SetForMerging();

                StartCoroutine(MergeCoroutine(collectable));
            }
        }
    }

    public void SetForMerging()
    {
        candidateForMerge = true;
    }

    private void UnsetFromMerging()
    {
        candidateForMerge = false;
        isMerging = true;
        if (mergeCoroutine != null)
        {
            StopCoroutine(mergeCoroutine);
            transform.localScale = defaultScale;
        }
    }

    private IEnumerator MergeCoroutine(CollectableItem itemToMerge)
    {
        float scaleValue = itemToMerge.transform.localScale.y;
        float elapsedTime = 0;
        do
        {
            elapsedTime += Time.deltaTime;
            scaleValue = Mathf.Lerp(scaleValue, 0, elapsedTime * 3);
            if (itemToMerge == null)
                yield break;
            itemToMerge.transform.localScale = new Vector3(itemToMerge.transform.localScale.x, scaleValue, 0);
            yield return null;
        }
        while (scaleValue > 0.1f);
        Merge(itemToMerge);
    }

    private void Merge(CollectableItem itemToMerge)
    {
        if (itemToMerge == null)
            return;
        itemsCount += itemToMerge.itemsCount;
        countText.text = itemsCount.ToString();
        Destroy(itemToMerge.gameObject);
        isMerging = false;
        itemToMerge = null;
    }

    public bool CanMerge(CollectableItem collectable)
    {
        return !ReferenceEquals(this, collectable) && Item.Compare(collectable.Item) && !candidateForMerge && !isMerging;
    }
}
