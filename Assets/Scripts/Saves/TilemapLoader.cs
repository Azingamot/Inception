using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapLoader : MonoBehaviour
{
    [SerializeField] private bool clearBeforeLoad = true;

    public void LoadWorld(WorldSaveData saveData)
    {
        foreach (TilemapSaveData save in saveData.tilemaps)
        {
            LoadTilemapData(save);
        }
    }

    private void LoadTilemapData(TilemapSaveData saveData)
    {
        Tilemap tilemap = FindTilemap(saveData.name);

        if (clearBeforeLoad)
        {
            tilemap.ClearAllTiles();
        }

        if (saveData.position != null && saveData.position.Length >= 3)
        {
            tilemap.transform.position = new Vector3(
                saveData.position[0],
                saveData.position[1],
                saveData.position[2]
            );
        }

        LoadTilesToTilemap(tilemap, saveData.tiles);

        Debug.Log($"Tilemap {saveData.name} with {saveData.tiles.Count} tiles");
    }

    private Tilemap FindTilemap(string tilemapName)
    {
        Tilemap[] allTilemaps = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
        foreach (Tilemap tm in allTilemaps)
        {
            if (tm.name == tilemapName)
                return tm;
        }

        return null;
    }

    private void LoadTilesToTilemap(Tilemap tilemap, List<TileData> tiles)
    {
        foreach (TileData tileData in tiles)
        {
            LoadTileToPosition(tilemap, tileData);
        }

        tilemap.RefreshAllTiles();
    }

    private TileBase FindTileByName(string name)
    {
        return ResourcesHelper.FindTileBaseResource(name);
    }

    private void LoadTileToPosition(Tilemap tilemap, TileData tileData)
    {
        Vector3Int position = new Vector3Int(tileData.x, tileData.y, tileData.z);
        TileBase tile = FindTileByName(tileData.tileName);
        if (tile != null)
            tilemap.SetTile(position, tile);
    }
}
