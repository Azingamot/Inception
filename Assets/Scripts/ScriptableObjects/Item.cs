using System;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string UID;

    public string Name => NamesHelper.ReceiveName(name);
    public string Description => NamesHelper.ReceiveName(name + "_description");

    public int MaxStack;
    public Sprite ItemSprite;
    public UsableItem ItemUsage;
    public RuntimeAnimatorController Animator;
    public Rarities.Rarity Rarity = Rarities.Rarity.Basic;
    public ClipType SoundType;

#if UNITY_EDITOR
    public void OnValidate()
    {
        if (string.IsNullOrEmpty(UID))
        {
            UID = Guid.NewGuid().ToString();
            EditorUtility.SetDirty(this);
        }
    }
#endif

    public bool Compare(Item other)
    {
        return (other != null) && (this.UID == other.UID);
    }

    public virtual string FormatDescription()
    {
        return "\n" + Description;
    }
}