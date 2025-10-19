using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectsPlacement : MonoBehaviour
{
    [SerializeField] private Tilemap groundMap;
    public static ObjectsPlacement Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public bool PlaceObject(BuildingItem item, Vector2 center)
    {
        if (EnsurePlacement(item.Size, center))
        {
            Vector2 worldPosition = PlacementPosition(center);
            Instantiate(item.ObjectToPlace, worldPosition, Quaternion.identity);
            ObjectsPositions.AddObject(new ObjectPosition(worldPosition, item.ObjectToPlace));
            return true;
        }
        return false;
    }

    public bool EnsurePlacement(Vector2Int size, Vector2 center)
    {
        return TileValidation.CanPlaceObject(groundMap, center, size, ObjectsPositions.ReceivePositions());
    }

    public Vector2 PlacementPosition(Vector3 position)
    {
        return TileValidation.PointToCell(groundMap, position);
    }
}
