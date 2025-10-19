using System.Collections.Generic;
using UnityEngine;

public class CropsGrowController : MonoBehaviour, IObservable
{
    public static CropsGrowController Instance { get; private set; }

    private List<IObserver> observers = new();

    public List<IObserver> Observers => observers;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void OnGrowTick(ClockContext context)
    {
        Notify(new CropGrowContext(1));
    }

    public void Notify(object context)
    {
        for (int i = 0; i < observers.Count; i++)
        {
            if (i > observers.Count) break;
            observers[i].OnUpdate(context);
        }
    }

    public void Notify(IObserver observer, object context)
    {
        observer?.OnUpdate(context);
    }

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }
}
