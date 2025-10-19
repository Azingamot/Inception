using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollection : MonoBehaviour, IObservable
{
    [SerializeField] private List<IObserver> observers = new List<IObserver>();
    [SerializeField] private LayerMask collectableMask;
    [SerializeField] private float pickupRadius = 3f;
    public List<IObserver> Observers => observers;

    private void Start()
    {
        StartCoroutine(FindCollectables());
    }

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Notify(object context)
    {
        foreach (var observer in observers)
        {
            observer.OnUpdate(context);
        }
    }

    public void Notify(IObserver observer, object context)
    {
        observer.OnUpdate(context);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    private IEnumerator FindCollectables()
    {
        while (true)
        {
            AttractItems();
            yield return new WaitForFixedUpdate();
        }
    }

    private void AttractItems()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius, collectableMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<CollectableItem>(out CollectableItem collectableItem))
            {
                collectableItem.FollowObject(transform);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ICollectable>(out ICollectable collectable))
        {
            ItemPickupContext context = collectable.Collect();       
            Notify(context);
        }
    }
}
