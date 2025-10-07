using UnityEditor.Animations;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public float MaxStack;
    public Sprite ItemSprite;
    public UsableItem ItemUsage;
    public AnimatorController Animator;
}
