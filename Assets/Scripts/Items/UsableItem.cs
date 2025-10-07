using UnityEngine;

public abstract class UsableItem
{
    protected Item item;
    protected PlayerItemInHand playerItemInHand;

    public UsableItem(Item item)
    {
        this.item = item;
    }

    public abstract void Use();

    public virtual void Initialize(PlayerItemInHand playerItemInHand)
    {
        this.playerItemInHand = playerItemInHand;
    }
    
    public virtual void InHandTick()
    {

    }
}
