using System;
using UnityEngine;

[System.Serializable]
public class ObjectPosition
{
    public Vector2 Position;
    [NonSerialized] public GameObject SavedObject;
    public ObjectReference ObjectReference;

    public ObjectPosition(Vector2 position, GameObject savedObject, ObjectReference objectReference)
    {
        Position = position;
        SavedObject = savedObject;
        ObjectReference = objectReference;
    }
}
