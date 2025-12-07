using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Spawn Condition", menuName = "Enemies/Enemy Spawn Condition")]
public class EnemySpawnCondition : ScriptableObject
{
    public EnemyData Enemy;
    public float ChanceToSpawn = 10;
    public DayTime DayTime;
    public float AfterDay = 0;
}
