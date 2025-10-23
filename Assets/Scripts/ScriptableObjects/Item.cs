using System;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string UID;
    public int MaxStack;
    public Sprite ItemSprite;
    public UsableItem ItemUsage;
    public RuntimeAnimatorController Animator;

#if UNITY_EDITOR
    public void OnValidate()
    {
        if (string.IsNullOrEmpty(UID))
        {
            UID = Guid.NewGuid().ToString();
        }
    }
#endif

    public bool Compare(Item other)
    {
        return (other != null) && (this.UID == other.UID);
    }
}