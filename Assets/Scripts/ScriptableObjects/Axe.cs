using UnityEngine;

[CreateAssetMenu(fileName = "Axe", menuName = "Scriptable Objects/Axe")]
public class Axe : DamageItem
{
    public Axe()
    {
        ItemUsage = new AxeUsage(this);
    }
}
