using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Scriptable Objects/Sword")]
public class Sword : Item
{
    public float Damage = 5;
    public float AttackSpeed = 1.0f;
    public AnimatorController Animator;
    public DamageType[] DamageTypes;

    public Sword()
    {
        ItemUsage = new SwordUsage(this);
    }
}
