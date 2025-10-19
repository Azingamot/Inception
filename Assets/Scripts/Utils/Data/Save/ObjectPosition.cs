using UnityEngine;

[System.Serializable]
public class ObjectPosition
{
    public Vector2 Position;
    public GameObject SavedObject;

    public ObjectPosition(Vector2 position, GameObject savedObject)
    {
        Position = position;
        SavedObject = savedObject;
    }
}
