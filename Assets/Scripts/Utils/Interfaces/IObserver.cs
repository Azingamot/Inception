/// <summary>
/// Наблюдающий объект для реализации паттерна "наблюдатель"
/// </summary>
public interface IObserver
{
    public void OnUpdate(object context);
}
