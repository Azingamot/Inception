using UnityEngine;

public class DropItems : MonoBehaviour
{
    [SerializeField] private LootTable lootTable;
    [SerializeField] private CollectableItem dropPrefab;
    [SerializeField] private float launchPower = 2.5f;

    public void Drop()
    {
        foreach (LootElement loot in lootTable)
        {
            int chance = Random.Range(0, 100);
            if (chance <= loot.DropChance)
            {
                Debug.Log($"Drop {loot.DropItem} Count {loot.DropCount}");
                SpawnDrop(loot.DropItem, loot.DropCount);
            }
        }
    }

    private void SpawnDrop(Item item, int count = 1)
    {
        CollectableItem itemToCollect = Instantiate<CollectableItem>(dropPrefab, transform.position, Quaternion.identity);
        itemToCollect.Initialize(item, count);
        itemToCollect.LaunchUp(launchPower);
    }

    public void DestroyItself()
    {
        Destroy(gameObject, 0);
    }
}
