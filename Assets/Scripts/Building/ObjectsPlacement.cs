using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectsPlacement : MonoBehaviour
{
    [SerializeField] private Tilemap groundMap, aboveMap;
    public static ObjectsPlacement Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public bool PlaceObject(BuildingItem item, Vector2 center)
    {
        if (CanPlaceObject(item,center))
        {
            Vector2 worldPosition = PlacementPosition(center);
            GameObject newObject = Instantiate(item.ObjectToPlace, worldPosition, Quaternion.identity);
            if (item is CropItem cropItem && newObject.TryGetComponent<Crop>(out Crop crop))
            {
                crop.Initialize(cropItem.CropData);
            }
            ObjectsPositions.AddObject(new ObjectPosition(worldPosition, newObject));
            return true;
        }
        return false;
    }

    public bool CanPlaceObject(BuildingItem item, Vector2 center)
    {
        if (item is CropItem)
            return EnsurePlacementAboveGround(item.Size, center);
        else 
            return EnsurePlacementOnGround(item.Size, center);

    }

    public bool EnsurePlacementOnGround(Vector2Int size, Vector2 center)
    {
        return TileValidation.CanPlaceObject(groundMap, center, size, ObjectsPositions.ReceivePositions());
    }

    public bool EnsurePlacementAboveGround(Vector2Int size, Vector2 center)
    {
        return TileValidation.CanPlaceObject(aboveMap, center, size, ObjectsPositions.ReceivePositions());
    }

    public Vector2 PlacementPosition(Vector3 position)
    {
        return TileValidation.PointToCell(groundMap, position);
    }
}
