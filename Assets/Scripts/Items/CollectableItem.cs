using System.Collections;
using TMPro;
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
    public Item Item { get => collectableItem; set => collectableItem = value; }
    private SpriteRenderer sr;
    private Collider2D ownCollider;
    private Rigidbody2D rb;
    private float timeToWait = 1.25f;

    private void Start()
    {
        if (initializeOnStart)
            Initialize(collectableItem, itemsCount);
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
}
