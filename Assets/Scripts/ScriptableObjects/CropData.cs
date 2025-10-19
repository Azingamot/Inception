using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CropData", menuName = "Scriptable Objects/CropData")]
public class CropData : ScriptableObject
{
    public List<CropState> cropStates;
    public LootTable lootTable;
}
