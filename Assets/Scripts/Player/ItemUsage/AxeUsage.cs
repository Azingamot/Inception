using UnityEngine;

public class AxeUsage : WeaponUsage
{
    public AxeUsage(DamageItem item) : base(item) { }

    public override void Stop()
    {
        base.Stop();
        CheckIfAround.instance.Disable();
    }

    public override void Initialize(PlayerItemInHand playerItemInHand)
    {
        base.Initialize(playerItemInHand);
        CheckIfAround.instance.Enable(); 
    }
}
