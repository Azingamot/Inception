using UnityEngine;

public abstract class UsableItem
{
    protected Item item;
    public UsableItem(Item item)
    {
        this.item = item;
    }
    public abstract void Use();
    
    public virtual void InHandTick()
    {

    }
}
