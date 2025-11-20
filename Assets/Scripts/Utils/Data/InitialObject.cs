using UnityEditor;
using UnityEngine;

[System.Serializable]
public class InitialObject
{
    public bool IsCrop => BuildingItem is CropItem;
    public Vector3 Position;
    public BuildingItem BuildingItem;
}