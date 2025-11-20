using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class ResourcesHelper
{
    public static T FindItemResource<T>(string UID) where T : Item
    {
        T item = Resources.LoadAll<T>("Items").FirstOrDefault(u => u.UID == UID);
        return item;
    }

    public static TileBase FindTileBaseResource(string name)
    {
        TileBase tile = Resources.Load<TileBase>($"Tiles/{name}");
        return tile;
    }

    public static EventData FindEventResource(string UID)
    {
        EventData eventData = Resources.LoadAll<EventData>("Events").FirstOrDefault(u => u.UID == UID);
        return eventData;
    }
}
