using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ObjectsPositions
{
    private static HashSet<ObjectPosition> _positions = new HashSet<ObjectPosition>();
    public static HashSet<ObjectPosition> Objects { 
        get 
        { 
            _positions.RemoveWhere(u => u.SavedObject == null);
            return _positions; 
        } 
        set { _positions = value; } 
    }

    public static void AddObject(ObjectPosition position)
    {
        _positions.Add(position);
    }

    public static void RemoveObject(ObjectPosition position)
    {
        _positions.Remove(position);
    }

    public static HashSet<Vector2> ReceivePositions()
    {
        return Objects.Select(u => u.Position).ToHashSet();
    }
}
