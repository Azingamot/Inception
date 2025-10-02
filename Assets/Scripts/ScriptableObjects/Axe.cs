using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Axe", menuName = "Scriptable Objects/Axe")]
public class Axe : Item
{
    public float Damage = 5;
    public float SwingSpeed = 1.5f;
    public DamageType[] DamageTypes;
    public AnimatorController Animator;

    public Axe()
    {
        ItemUsage = new AxeUsage(this);
    }
}
