using System.Collections.Generic;
using UnityEngine;

public static class SpawnedEnemies 
{
    public static List<EnemyPosition> EnemyPositions = new();
    
    public static void AddEnemy(EnemyData enemy, GameObject enemyInstance, Vector3 position)
    {
        ClearListFromEmpties();   
        EnemyPositions.Add(new EnemyPosition() { EnemyInstance = enemyInstance, Position = position, EnemyUID = enemy.UID });
    }

    private static void ClearListFromEmpties()
    {
        EnemyPositions.RemoveAll(x => x.EnemyInstance == null);
    }

    public static List<EnemyPosition> Get()
    {
        ClearListFromEmpties();
        EnemyPositions.ForEach(u => u.Position = u.EnemyInstance.transform.position);
        return EnemyPositions;
    }
}
