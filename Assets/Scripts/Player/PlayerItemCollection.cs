using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollection : MonoBehaviour, IObservable
{
    [SerializeField] private List<IObserver> observers = new List<IObserver>();
    public List<IObserver> Observers => observers;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ICollectable>(out ICollectable collectable))
        {
            ItemPickupContext context = collectable.Collect();
            if (context != null)
            {          
                Notify(context);
            }  
            else
            {
                Debug.LogWarning("Инвентарь забит");
            }  
        }
    }
}
