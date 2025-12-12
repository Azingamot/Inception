using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructionEvent : MonoBehaviour
{
    [SerializeField] private int tilesCount = 10;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private LayerMask avoidMask;
    [SerializeField] private GameObject destructionParticles;
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
            Vector2 randomPos = TilePlacement.Instance.RandomTilePosition();

            while (!ValidateTileDestruction(randomPos))
            {
                randomPos = TilePlacement.Instance.RandomTilePosition();
                counter++;
                if (counter > 50)
                    yield break;
            }

            ObjectPoolController.SpawnObject(destructionParticles, (Vector2)TileValidation.GetTileCenterOnPlace(groundTilemap,randomPos), Quaternion.identity, ObjectPoolController.PoolType.ParticleSystem);

            yield return new WaitForSeconds(0.3f);

            TilePlacement.Instance.RemoveTile(randomPos);
            ObjectsPlacement.Instance.DestroyObject(randomPos);

            yield return new WaitForSeconds(0.3f);
        }
    }

    private bool ValidateTileDestruction(Vector2 pos)
    {
        return !Physics2D.OverlapCircle(pos, 1.5f, avoidMask) 
            && TileValidation.CanPlaceObject(groundTilemap, pos, new Vector2Int(1, 1), ObjectsPositions.ReceivePositions())
            && Vector2.Distance(pos, PlayerPosition.GetData()) < 30;
    }
}
