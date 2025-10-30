using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CropItem", menuName = "Scriptable Objects/Crop Item")]
public class CropItem : BuildingItem
{
    public CropData CropData;

    public CropItem()
    {
        ItemUsage = new BuildUsage(this);
    }
}
