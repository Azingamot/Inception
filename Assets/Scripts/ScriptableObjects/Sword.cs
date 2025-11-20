using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Scriptable Objects/Sword")]
public class Sword : DamageItem
{
    public Sword()
    {
        ItemUsage = new SwordUsage(this);
    }
}
