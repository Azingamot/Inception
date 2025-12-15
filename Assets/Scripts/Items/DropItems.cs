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
                SpawnDrop(loot.DropItem, loot.DropCount);
        }
    }

    public void Drop(LootTable loot)
    {
        lootTable = loot;
        Drop();
    }

    public void Drop(EventData eventData)
    {
        if (eventData is QuestDialogueEventData questDialogue)
        {
            foreach (var dropItem in questDialogue.QuestData.Reward)
            {
                SpawnDropOnPosition(PlayerPosition.GetData() ,dropItem.Item, dropItem.Count);
            }
        }
    }

    public void SpawnDrop(Item item, int count = 1)
    {
       SpawnDropOnPosition(transform.position, item, count);
    }

    public void SpawnDrop(Vector2 pos, Item item, int count = 1)
    {
        SpawnDropOnPosition(pos, item, count);
    }

    public void SpawnDropOnPosition(Vector2 pos, Item item, int count)
    {
        CollectableItem itemToCollect = Instantiate<CollectableItem>(dropPrefab, pos, Quaternion.identity);
        itemToCollect.Initialize(item, count);
        itemToCollect.LaunchUp(launchPower);
    }

    public void DestroyItself(float time = 0)
    {
        Destroy(gameObject, time);
    }
}
