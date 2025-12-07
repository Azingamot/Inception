using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stats", menuName = "Enemies/Stats")]
public class EnemyStats : ScriptableObject
{
    public float MaxHealth = 50;
    public float BaseSpeed = 1.0f;
    public float BaseDamage = 5f;
    public float KnockbackValue = 1f;
}
