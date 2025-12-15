using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class FishingSystem : MonoBehaviour
{
    [SerializeField] private FishingCircleUI fishingCircleUI;
    [SerializeField] private LootTable commonLoot, rareLoot;
    [SerializeField] private DropItems dropItems;
    [SerializeField] private List<FishingLootRarity> rarities;

    private Vector2 currentFishingPosition;
    private bool isFishing = false;
    private float currentFishingPower = 5;
    private FishingLootRarity currentRarity;

    public static FishingSystem Instance { get ; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void StartFishing(Vector2 fishingPosition, UnityAction<bool> fishingFinished, float fishingPower = 5)
    {
        if (isFishing)
            return;

        fishingCircleUI.gameObject.SetActive(true);

        currentRarity = ChooseRarity();

        fishingCircleUI.SetRandomGreenZone(currentRarity.Size);
        fishingCircleUI.StartSpinning(currentRarity.Speed, CheckForFishing, fishingFinished);

        currentFishingPosition = fishingPosition;
        isFishing = true;
        currentFishingPower = fishingPower;
    }

    public void CheckForFishing(bool result)
    {
        if (result)
            DropRandomItem();
        isFishing = false;
        fishingCircleUI.gameObject.SetActive(false);
    }

    private void DropRandomItem()
    {
        LootTable fishingTable = currentRarity.LootTable;
        LootElement lootElement = ReceiveRandomItem(fishingTable);
        if (lootElement != null)
            dropItems.SpawnDropOnPosition(currentFishingPosition, lootElement.DropItem, lootElement.DropCount);
    }

    private FishingLootRarity ChooseRarity()
    {
        int randomNumber = Random.Range(0, 100);
        foreach (var item in rarities)
        {
            if (randomNumber > item.LowerThreshold && randomNumber <= item.UpperThreshold)
            {
                return item;
            }
        }
        return rarities[0];
    }

    private LootElement ReceiveRandomItem(LootTable fishingLoot)
    {
        float randomNumber = Mathf.Clamp(Random.Range(0, 100) / (currentFishingPower / 10), 0, 100);
        LootElement[] availableElements = fishingLoot.lootElements.Where(u => randomNumber <= u.DropChance).ToArray();
        if (availableElements.Length > 0)
            return availableElements[Random.Range(0, availableElements.Length)];    
        return null;
    }
}
