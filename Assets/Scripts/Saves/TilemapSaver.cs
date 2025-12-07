using System.IO;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public static class TilemapSaver
{
    public static WorldSaveData SaveAllTilemaps(List<Tilemap> tilemaps)
    {
        var worldData = new WorldSaveData();

        foreach (var tilemap in tilemaps)
        {
            if (tilemap.gameObject.activeInHierarchy)
            {
                var tilemapData = CreateSaveDataFromTilemap(tilemap);
                worldData.tilemaps.Add(tilemapData);
            }
        }

        worldData.saveTime = DateTime.Now;
        return worldData;
    }

    public static TilemapSaveData CreateSaveDataFromTilemap(Tilemap tilemap)
    {
        var saveData = new TilemapSaveData()
        {
            name = tilemap.name,
            position = new float[] {
                tilemap.transform.position.x,
                tilemap.transform.position.y,
                tilemap.transform.position.z
            },
            tiles = new List<TileData>()
        };

        tilemap.CompressBounds();
        BoundsInt bounds = tilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            var tileData = CreateTileData(tilemap, pos);
            saveData.tiles.Add(tileData);
        }
        return saveData;
    }

    private static TileData CreateTileData(Tilemap tilemap, Vector3Int position)
    {
        TileBase tileBase = tilemap.GetTile(position);
        Color color = tilemap.GetColor(position);

        return new TileData
        {
            x = (short)position.x,
            y = (short)position.y,
            z = (short)position.z,
            tileName = tileBase.name,
        };
    }
}