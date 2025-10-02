using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BuildingItem", menuName = "Scriptable Objects/Building Item")]
public class BuildingItem : Item
{
    public TileBase TileToPlace;
    public GameObject ObjectToPlace;
}
