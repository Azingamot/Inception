using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilePlacement : MonoBehaviour
{
    [Header("Tile Maps")]
    [SerializeField] private Tilemap groundMap;
    [SerializeField] private Tilemap waterMap;
    [SerializeField] private Tilemap aboveMap;
    [SerializeField] private Tilemap objectMap;

    [Header("Tiles")]
    [SerializeField] private TileBase waterTile;
    [SerializeField] private TileBase waterAnim;

    [Header("Events")]
    [SerializeField] private UnityEvent tileUpdatedEvent;
    public static TilePlacement Instance;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public void PlaceGroundTile(TileBase tile, Vector3 position)
    {
        Vector3Int intPos = Vector3Int.FloorToInt(position);
        groundMap.SetTile(intPos, tile);
        waterMap.SetTile(intPos, null);

        AddAnimatedWater(intPos);
        tileUpdatedEvent.Invoke();
    }

    public void PlaceAbovegroundTile(TileBase tile, Vector3 position)
    {
        Vector3Int intPos = Vector3Int.FloorToInt(position);
        aboveMap.SetTile(intPos, tile);
    }

    public void PlaceObjectTile(TileBase tile, Vector3 position)
    {
        Vector3Int intPos = Vector3Int.FloorToInt(position);
        objectMap.SetTile(intPos, tile);
        tileUpdatedEvent.Invoke();
    }

    public void RemoveTile(Vector3 position)
    {
        Vector3Int intPos = Vector3Int.FloorToInt(position);
        RemoveAnimatedWater(intPos);
        groundMap.SetTile(intPos, null);
        objectMap.SetTile(intPos, null);
        aboveMap.SetTile(intPos, null);

        if (CheckGround(intPos + new Vector3Int(0, 1)))
            waterMap.SetTile(intPos, waterAnim);
        else
            waterMap.SetTile(intPos, waterTile);
        tileUpdatedEvent.Invoke();
    }

    private void RemoveAnimatedWater(Vector3Int intPos)
    {
        Vector3Int below = new Vector3Int(intPos.x, intPos.y - 1);

        if (!CheckGround(below))
            waterMap.SetTile(below, waterTile);
    }

    private void AddAnimatedWater(Vector3Int intPos)
    {
        Vector3Int below = new Vector3Int(intPos.x, intPos.y - 1);

        if (!CheckGround(below))
            waterMap.SetTile(below, waterAnim);
    }

    public bool CheckGround(Vector3 position)
    {
        return TileValidation.CheckTileOnPlace(groundMap, position) && !TileValidation.CheckTileOnPlace(objectMap, position);
    }

    public bool CheckWater(Vector3 position)
    {
        return !TileValidation.CheckTileOnPlace(groundMap, position);
    }

    public bool ValidateTile(Vector3 position)
    {
        return TileValidation.CanPlaceObject(groundMap, position, new Vector2Int(1, 1), ObjectsPositions.ReceivePositions()) && !TileValidation.CheckTileOnPlace(objectMap, position);
    }

    public Vector3 GetWaterTileCenter(Vector3 position)
    {
        return TileValidation.GetTileCenterOnPlace(waterMap, position).Value;
    }

    public Vector2 RandomTilePosition()
    {
        groundMap.CompressBounds();
        BoundsInt tilemapBounds = groundMap.cellBounds;
        List<Vector3> tilePoses = new();
        for (int x = tilemapBounds.xMin; x < tilemapBounds.xMax; x++)
        {
            for (int y = tilemapBounds.yMin; y < tilemapBounds.yMax; y++)
            {
                Vector3Int localPos = new Vector3Int(x, y, (int)groundMap.transform.position.z);
                Vector3 worldPos = groundMap.CellToWorld(localPos);
                if (groundMap.HasTile(localPos))
                {
                    tilePoses.Add(localPos);
                }
            }
        }
        return tilePoses[Random.Range(0, tilePoses.Count)];
    }
}
