using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class ObjectsPlacement : MonoBehaviour
{
    [SerializeField] private Tilemap groundMap, aboveMap;
    [SerializeField] private UnityEvent objectPlacedEvent;
    public static ObjectsPlacement Instance;

    public void Initialize()
    {
        if (Instance == null)
            Instance = this;
    }

    public bool TryPlaceObject(BuildingItem item, Vector2 center)
    {
        if (CanPlaceObject(item,center))
        {
            Vector2 worldPosition = PlacementPosition(center);
            
            PlaceObject(item, worldPosition);

            return true;
        }
        return false;
    }

    public void PlaceObject(BuildingItem item, Vector2 worldPosition)
    {
        GameObject newObject = Instantiate(item.ObjectToPlace, worldPosition, Quaternion.identity);

        ObjectPosition objectPosition = new ObjectPosition(worldPosition, newObject, new ObjectReference(item.UID));

        if (item is CropItem cropItem && newObject.TryGetComponent<Crop>(out Crop crop))
            crop.Initialize(cropItem.CropData);

        ObjectsPositions.AddObject(objectPosition);
        objectPlacedEvent.Invoke();
    }

    public void DestroyObject(Vector2 worldPosition)
    {
        ObjectPosition objectPos = ObjectsPositions.Objects.FirstOrDefault(u => Vector2.Distance(u.Position, worldPosition) < 1);
        if (objectPos != null && objectPos.SavedObject != null)
        {
            Debug.Log("Destroying... " + objectPos.Position);
            Destroy(objectPos.SavedObject);
            ObjectsPositions.RemoveObject(objectPos);
        }
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
        return TileValidation.CanPlaceObject(groundMap, center, size, ObjectsPositions.ReceivePositions()) && !ObjectsPositions.ReceivePositions().Any(u => Vector2.Distance(u, center) < 0.5f);
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
