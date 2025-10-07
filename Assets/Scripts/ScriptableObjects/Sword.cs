using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Scriptable Objects/Sword")]
public class Sword : DamageItem
{
    public float AttackSpeed = 1.0f;

    public Sword()
    {
        ItemUsage = new SwordUsage(this);
    }
}
