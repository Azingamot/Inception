using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacement : MonoBehaviour
{
    [SerializeField] private Tilemap groundMap;
    [SerializeField] private Tilemap waterMap;
    [SerializeField] private TileBase waterTile;
    public static TilePlacement instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    public void PlaceTile(TileBase tile, Vector3 position)
    {
        Vector3Int intPos = Vector3Int.FloorToInt(position);
        groundMap.SetTile(intPos, tile);
        waterMap.SetTile(intPos, null);
    }

    public void RemoveTile(Vector3 position)
    {
        Vector3Int intPos = Vector3Int.FloorToInt(position);
        groundMap.SetTile(intPos, null);
        waterMap.SetTile(intPos, waterTile);
    }

    public bool CheckGround(Vector3 position)
    {
        TileBase tile = groundMap.GetTile(Vector3Int.FloorToInt(position));
        return tile != null;
    }
}
