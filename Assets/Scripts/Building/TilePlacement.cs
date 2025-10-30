using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacement : MonoBehaviour
{
    [SerializeField] private Tilemap groundMap;
    [SerializeField] private Tilemap waterMap;
    [SerializeField] private Tilemap aboveMap;
    [SerializeField] private TileBase waterTile;
    [SerializeField] private TileBase waterAnim;
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

    public void PlaceGroundTile(TileBase tile, Vector3 position)
    {
        Vector3Int intPos = Vector3Int.FloorToInt(position);
        groundMap.SetTile(intPos, tile);
        waterMap.SetTile(intPos, null);

        AddAnimatedWater(intPos);
    }

    public void PlaceAbovegroundTile(TileBase tile, Vector3 position)
    {
        Vector3Int intPos = Vector3Int.FloorToInt(position);
        aboveMap.SetTile(intPos, tile);
    }

    public void RemoveTile(Vector3 position)
    {
        Vector3Int intPos = Vector3Int.FloorToInt(position);
        groundMap.SetTile(intPos, null);
        waterMap.SetTile(intPos, waterTile);
    }

    private void AddAnimatedWater(Vector3Int intPos)
    {
        Vector3Int below = new Vector3Int(intPos.x, intPos.y - 1);

        if (!CheckGround(below))
            waterMap.SetTile(below, waterAnim);
    }

    public bool CheckGround(Vector3 position)
    {
        return TileValidation.CheckTileOnPlace(groundMap, position);
    }
}
