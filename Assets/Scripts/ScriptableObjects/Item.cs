using UnityEngine;

public abstract class Item : ScriptableObject
{
    public int MaxStack;
    public Sprite ItemSprite;
    public UsableItem ItemUsage;
    public RuntimeAnimatorController Animator;
}
