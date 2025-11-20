using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCController : MonoBehaviour
{
    [SerializeField] private Tilemap groundMap;
    [SerializeField] private float radius;
    [SerializeField] private GameObject NPCInstance;
    private Animator animator;

    private void Start()
    {
        animator = NPCInstance.GetComponent<Animator>();
    }

    public void Spawn()
    {
        NPCInstance.SetActive(true);

        SetPositionNearPlayer();

        animator.SetTrigger("Rise");
    }

    private void SetPositionNearPlayer()
    {
        Vector2 playerPos = PlayerPosition.GetPosition();

        NPCInstance.transform.position = FindValidPosition(playerPos, radius);
    }

    private Vector2 FindValidPosition(Vector2 initialPosition, float radius)
    {
        Vector2? currentPos;
        do
        {
            currentPos = TileValidation.GetTileCenterOnPlace(groundMap, initialPosition + (Random.insideUnitCircle * radius));
        }
        while (currentPos == null);
        return currentPos.Value;
    }

    private IEnumerator WaitForHide()
    {
        yield return new WaitForSeconds(2);
        Hide();
    }

    public void Vanish()
    {
        animator.SetTrigger("Hide");
        StartCoroutine(WaitForHide());
    }

    private void Hide()
    {
        NPCInstance.transform.position = new Vector2(1000, 1000);
        NPCInstance.SetActive(false);
    }
}
