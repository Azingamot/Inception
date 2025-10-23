using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftRecipe", menuName = "Scriptable Objects/CraftRecipe")]
public class CraftRecipe : ScriptableObject
{
    public Item Result;
    public int Count;
    public List<RecipeElement> Elements;
}
