using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Axe", menuName = "Scriptable Objects/Axe")]
public class Axe : DamageItem
{
    public float SwingSpeed = 1.5f;

    public Axe()
    {
        ItemUsage = new AxeUsage(this);
    }
}
