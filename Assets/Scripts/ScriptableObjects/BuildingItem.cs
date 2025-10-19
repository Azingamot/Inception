using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BuildingItem", menuName = "Scriptable Objects/Building Item")]
public class BuildingItem : Item
{
    public GameObject ObjectToPlace;
    public Sprite PreviewSprite;
    public Vector2Int Size = Vector2Int.one;

    public BuildingItem()
    {
        ItemUsage = new BuildUsage(this);
    }
}
