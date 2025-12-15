using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class AStarManager : MonoBehaviour
{
    [SerializeField] private Tilemap walkable;
    [SerializeField] private Tilemap unwalkable;
    [SerializeField] private LayerMask avoidMask;

    private UnityEvent onPathChanged = new();

    public static AStarManager Instance { get; private set; }

    private Vector3[] directionalVectors = new Vector3[4] {
        Vector2.up,
        Vector2.down,
        Vector2.right,
        Vector2.left};

    public List<Node> Nodes { get; set; } = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        GenerateNodeMap();
    }

    public void GenerateNodeMap()
    {
        Nodes.Clear();

        TilemapSaveData walkableData = TilemapSaver.CreateSaveDataFromTilemap(walkable);
        TilemapSaveData unwalkableData = TilemapSaver.CreateSaveDataFromTilemap(unwalkable);

        List<Vector3> walkableTiles = walkableData.tiles.Select(u => TileDataToVector(walkable, u)).ToList();
        List<Vector3> unwalkableTiles = unwalkableData.tiles.Select(u => TileDataToVector(unwalkable, u)).ToList();
        Node prevNode = null;

        foreach (Vector3 tile in walkableTiles)
        {
            if (IsWalkableTile(unwalkableTiles, tile))
                GenerateNode(ref prevNode, tile);
        }

        foreach (Node node in Nodes)
        {
            GenerateNodeConnections(Nodes, node);
        }

        onPathChanged.Invoke();
    }

    private Node GenerateNode(ref Node prev, Vector2 position)
    {
        Node newNode = new(position);
        newNode.CameFrom = prev;
        prev = newNode;
        Nodes.Add(newNode);
        return newNode;
    }

    private bool IsWalkableTile(List<Vector3> unwalkableTiles, Vector3 position)
    {
        return !unwalkableTiles.Any(u => u == position) && !Physics2D.OverlapCircle(position, 0.5f, avoidMask);
    }

    private void GenerateNodeConnections(List<Node> nodes, Node node)
    {
        foreach (Vector3 direction in directionalVectors)
        {
            Node connectedNode = nodes.FirstOrDefault(u => u.Position == node.Position + direction);

            if (connectedNode != null)
            {
                node.Connections.Add(connectedNode);
            }  
        }
    }

    private Vector3 TileDataToVector(Tilemap tilemap, TileData data)
    {
        return TileValidation.PointToCell(tilemap, new Vector3(data.x, data.y, data.z));
    }

    public List<Node> GeneratePath(Node start, Node end)
    {
        List<Node> openSet = new();
        HashSet<Node> closedSet = new();

        foreach (Node n in Nodes)
        {
            n.GScore = float.MaxValue;
            n.CameFrom = null;
        }

        start.GScore = 0;
        start.HScore = Vector2.Distance(start.Position, end.Position);
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.OrderBy(n => n.FScore).First();

            if (currentNode == end)
            {
                return ReconstructPath(end);
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            foreach (Node neighbour in currentNode.Connections)
            {
                if (closedSet.Contains(neighbour))
                    continue;

                float tentativeG = currentNode.GScore +
                                   Vector2.Distance(currentNode.Position, neighbour.Position);

                if (!openSet.Contains(neighbour))
                    openSet.Add(neighbour);
                else if (tentativeG >= neighbour.GScore)
                    continue;

                neighbour.CameFrom = currentNode;
                neighbour.GScore = tentativeG;
                neighbour.HScore = Vector2.Distance(neighbour.Position, end.Position);
            }
        }

        return new();
    }

    private List<Node> ReconstructPath(Node end)
    {
        List<Node> path = new();
        Node current = end;

        while (current != null)
        {
            path.Add(current);
            current = current.CameFrom;
        }

        path.Reverse();
        return path;
    }

    public Node FindNearestNode(Vector2 position)
    {
        Node foundNode = null;
        float minDistance = float.MaxValue;

        foreach (Node node in Nodes)
        {
            float currentDistance = Vector2.Distance(position, node.Position);
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                foundNode = node;
            }
        }

        return foundNode;
    }

    public Node FindFurthestNode(Vector2 position)
    {
        Node foundNode = null;
        float maxDistance = 0;

        foreach (Node node in Nodes)
        {
            float currentDistance = Vector2.Distance(position, node.Position);
            if (currentDistance > maxDistance)
            {
                maxDistance = currentDistance;
                foundNode = node;
            }
        }

        return foundNode;
    }

    public Node RandomNode(Vector2 start, float distance)
    {
        Node[] distantNodes = Nodes.Where(u => Vector2.Distance(u.Position, start) > distance).ToArray();
        if (distantNodes.Length > 0)
            return distantNodes[Random.Range(0, distantNodes.Length)];
        else
            return Nodes[Random.Range(0, Nodes.Count)];
    }

    private void OnDrawGizmos()
    {
        if (Nodes != null)
        {
            Gizmos.color = new Color(255,0,0,0.2f);
            foreach (Node node in Nodes)
            {
                Gizmos.DrawSphere(node.Position, 0.2F);
                foreach (Node neighbour in node.Connections)
                {
                    Gizmos.DrawLine(node.Position, neighbour.Position);
                }
            }
        }
    }

    public void AddPathChangedListener(UnityAction listener)
    {
        onPathChanged.AddListener(listener);
    }

    public void RemovePathChangedListener(UnityAction listener)
    {
        onPathChanged.RemoveListener(listener);
    }
}
