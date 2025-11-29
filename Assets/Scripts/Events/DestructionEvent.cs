using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructionEvent : MonoBehaviour
{
    [SerializeField] private int tilesCount = 10;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Sprite destructionIndicator;
    private int counter = 0;

    public void DestroyTiles()
    {
        StartCoroutine(DestroyTilesCoroutine());
    }

    public IEnumerator DestroyTilesCoroutine()
    {
        for (int i = 0; i < tilesCount; i++)
        {
            counter = 0;
            Vector2 randomPos = RandomTilePosition();

            while (!ValidateTileDestruction(randomPos))
            {
                randomPos = RandomTilePosition();
                counter++;
                if (counter > 50)
                    yield break;
            }

            ShowBuildingPlacement.instance.ActivateHighlight(randomPos, Color.red, destructionIndicator);

            yield return new WaitForSeconds(0.1f);

            TilePlacement.instance.RemoveTile(randomPos);
            ShowBuildingPlacement.instance.DeactivateHighlight();

            yield return new WaitForSeconds(0.3f);
        }
    }

    private bool ValidateTileDestruction(Vector2 pos)
    {
        return Vector2.Distance(PlayerPosition.GetData(), pos) > 1 && TileValidation.CanPlaceObject(groundTilemap, pos, new Vector2Int(1, 1), ObjectsPositions.ReceivePositions());
    }

    private Vector2 RandomTilePosition()
    {
        var bounds = groundTilemap.cellBounds;
        var tiles = groundTilemap.GetTilesBlock(bounds);

        var filledCells = new System.Collections.Generic.List<Vector3Int>();

        int index = 0;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                if (tiles[index] != null)
                    filledCells.Add(new Vector3Int(x, y, 0));

                index++;
            }
        }

        if (filledCells.Count == 0)
            return Vector3.zero;

        Vector3Int randomCell = filledCells[Random.Range(0, filledCells.Count)];

        return groundTilemap.GetCellCenterWorld(randomCell);
    }
}
