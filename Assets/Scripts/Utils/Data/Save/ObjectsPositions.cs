using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ObjectsPositions
{
    public static HashSet<ObjectPosition> Objects { get; private set; } = new();

    public static void AddObject(ObjectPosition position)
    {
        Objects.Add(position);
    }

    public static void RemoveObject(ObjectPosition position)
    {
        Objects.Remove(position);
    }

    public static HashSet<Vector2> ReceivePositions()
    {
        return Objects.Select(u => u.Position).ToHashSet();
    }
}
