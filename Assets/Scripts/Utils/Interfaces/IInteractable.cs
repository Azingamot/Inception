using UnityEngine;

public interface IInteractable
{
    public Transform Transform { get; }
    public void OnInteract(GameObject interactor);
}