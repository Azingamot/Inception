using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapFrameSync : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private float timeScale = 1f;

    private float globalAnimTime;

    private readonly HashSet<Vector3Int> animatedPositions = new HashSet<Vector3Int>();

    private void Start()
    {
        InitializeAnimation();
    }

    private void InitializeAnimation()
    {
        TilemapSaveData tilemapSaveData = TilemapSaver.CreateSaveDataFromTilemap(tilemap);

        foreach (var tile in tilemapSaveData.tiles)
        {
            SetAnimatedTile(ReceiveTilePosition(tile), ResourcesHelper.FindTileBaseResource(tile.tileName));
        }
    }

    private Vector3Int ReceiveTilePosition(TileData tileData)
    {
        return new Vector3Int(tileData.x, tileData.y, tileData.z);
    }


    private void FixedUpdate()
    {
        globalAnimTime += Time.deltaTime * timeScale;

        foreach (var pos in animatedPositions)
            tilemap.SetAnimationTime(pos, globalAnimTime);
    }

    public void SetAnimatedTile(Vector3Int pos, TileBase tile)
    {
        tilemap.SetTile(pos, tile);
        animatedPositions.Add(pos);

        tilemap.SetAnimationTime(pos, globalAnimTime);
    }

    public void RemoveTile(Vector3Int pos)
    {
        tilemap.SetTile(pos, null);
        animatedPositions.Remove(pos);
    }
}
