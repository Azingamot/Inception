using System.Collections.Generic;
using UnityEngine;

public class InitialBuilding : MonoBehaviour
{
    [SerializeField] private List<InitialObject> objectsList = new();

    public void LoadPositions()
    {
        foreach (var obj in objectsList)
        {
            ObjectsPlacement.Instance.PlaceObject(obj.BuildingItem, obj.Position);
        }
    }
}
