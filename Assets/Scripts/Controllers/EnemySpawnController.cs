using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private LayerMask cameraMask;
    [SerializeField] private float currentChanceAdded = 0;
    [SerializeField] private int interval = 0;

    private int triesCounter = 0;
    private bool enemySpawned = false;
    private int minChanceAdded = -10;
    private List<EnemySpawnCondition> spawnList = new();

    private void Start()
    {
        spawnList = Resources.LoadAll<EnemySpawnCondition>("Enemies/SpawnConditions").ToList();
    }

    public void SpawnEnemies(ClockContext context)
    {
        if (context.Minutes % interval != 0)
            return;

        enemySpawned = false;

        foreach (EnemySpawnCondition condition in spawnList)
        {
            TryToSpawnEnemy(context, condition);
        }

        CheckEnemySpawn();
    }

    public void OnTimeChanged(ClockContext context)
    {
        currentChanceAdded = minChanceAdded;
    }

    private void CheckEnemySpawn()
    {
        if (!enemySpawned)
            currentChanceAdded = Mathf.Min(currentChanceAdded + 0.1f, 80);
        else
            currentChanceAdded = minChanceAdded;
    }

    private Vector2? FindLocationToSpawn()
    {
        triesCounter = 0;
        Vector2 position = TilePlacement.Instance.RandomTilePosition();

        while (!ValidateSpawnPlace(position))
        {
            position = TilePlacement.Instance.RandomTilePosition();
            triesCounter++;
            if (triesCounter > 50)
                return null;
        }

        return position;
    }

    private void TryToSpawnEnemy(ClockContext context, EnemySpawnCondition condition)
    {
        if (ValidateSpawnTime(context, condition) && ValidateSpawnChance(condition))
        {
            SpawnEnemy(condition);
        }
    }

    private bool ValidateSpawnTime(ClockContext context, EnemySpawnCondition condition)
    {
        return context.DayTime == condition.DayTime && context.Days > condition.AfterDay;
    }

    private bool ValidateSpawnChance(EnemySpawnCondition condition)
    {
        float randomNumber = Random.Range(0, 100);
        return randomNumber <= (condition.ChanceToSpawn + currentChanceAdded);
    }

    private bool ValidateSpawnPlace(Vector2 position)
    {
        Collider2D cameraCollider = Physics2D.OverlapCircle(position, 2, cameraMask);
        if (cameraCollider == null && Vector2.Distance(position, PlayerPosition.GetData()) < 20) 
            return true;
        return false;
    }

    private void SpawnEnemy(EnemySpawnCondition condition)
    {
        Vector2? spawnPosition = FindLocationToSpawn();
        if (spawnPosition.HasValue)
        {
            Debug.Log($"Spawning enemy at {spawnPosition}...");
            GameObject enemyInstance = Instantiate(condition.Enemy.EnemyPrefab, spawnPosition.Value, Quaternion.identity);
            SpawnedEnemies.AddEnemy(condition.Enemy, enemyInstance, spawnPosition.Value);
            enemySpawned = true;
        }
    }
}
