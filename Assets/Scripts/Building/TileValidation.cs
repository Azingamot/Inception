using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileValidation
{
    public static bool CheckTileOnPlace(Tilemap map, Vector2 position)
    {
        TileBase tile = map.GetTile(Vector3Int.FloorToInt(position));
        return tile != null;
    }

    public static Vector2? GetTileCenterOnPlace(Tilemap map, Vector2 position)
    {
        TileBase tile = map.GetTile(Vector3Int.FloorToInt(position));
        if (tile != null)
            return PointToCell(map, position);
        else
            return null;
    }

    public static bool CanPlaceObject(Tilemap tilemap, Vector3 worldPosition, Vector2Int objectSize, System.Collections.Generic.HashSet<Vector2> occupiedPositions = null)
    {
        Vector3Int minTile = tilemap.WorldToCell(worldPosition);

        minTile.x -= Mathf.FloorToInt(objectSize.x / 2f);
        minTile.y -= Mathf.FloorToInt(objectSize.y / 2f);

        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                Vector2Int tilePos = new Vector2Int(minTile.x + x, minTile.y + y);

                if (!CheckTileOnPlace(tilemap, tilePos))
                {
                    return false;
                }

                if (occupiedPositions != null && occupiedPositions.Contains(PointToCell(tilemap, (Vector2)tilePos)))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static Vector2 PointToCell(Tilemap map, Vector3 position)
    {
        Vector3Int tilePos = map.WorldToCell(position);
        Vector2 center = map.GetCellCenterWorld(tilePos);
        return center;
    }
}
