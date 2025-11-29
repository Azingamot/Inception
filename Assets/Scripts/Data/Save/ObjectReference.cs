using System;
using UnityEngine;

[System.Serializable]
public class ObjectReference
{
    [SerializeField] public string UID;
    public Type type;
    private BuildingItem item { get; set; }

    public GameObject ReceiveObjectToPlace()
    {
        if (item == null)
            item = ResourcesHelper.FindItemResource<BuildingItem>(UID);
        return item.ObjectToPlace;
    }

    public Item ReceiveItemData()
    {
        if (item == null)
            item = ResourcesHelper.FindItemResource<BuildingItem>(UID);
        return item;    
    }

    public ObjectReference(string UID)
    {
        this.UID = UID;
    }
}
