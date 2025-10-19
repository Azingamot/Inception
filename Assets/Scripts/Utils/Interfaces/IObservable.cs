using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Наблюдаемый объект для реализации паттерна "наблюдатель"
/// </summary>
public interface IObservable
{
    List<IObserver> Observers { get; }
    public void Notify(object context);
    public void Notify(IObserver observer, object context);
    public void AddObserver(IObserver observer);
    public void RemoveObserver(IObserver observer);
}
