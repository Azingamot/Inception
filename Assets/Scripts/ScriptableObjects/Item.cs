using System;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string UID;

    [TextArea(3,10)]
    public string Description;

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
            EditorUtility.SetDirty(this);
        }
    }
#endif

    public bool Compare(Item other)
    {
        return (other != null) && (this.UID == other.UID);
    }
}