using UnityEngine;
using System.Collections.Generic;

public class Node
{
    public Node CameFrom;
    public List<Node> Connections = new();

    public float GScore;
    public float HScore;

    public Vector3 Position;

    public float FScore => GScore + HScore;

    public Node(Vector3 position)
    {
        this.Position = position;
    }
}
