using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItemDescriptionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text itemName, itemDescription;
    [SerializeField] private LayoutGroup materialsGroup;
    [SerializeField] private CraftingMaterialUI materialUI;
    private List<RecipeElement> materials;
    [HideInInspector] public CraftRecipe SelectedRecipe;

    public void SetData(CraftRecipe recipe)
    {
        itemName.text = recipe.Result.Name;
        itemName.color = Rarities.ItemColor(recipe.Result);

        SetDescription(recipe.Result);

        materials = recipe.Elements;
        SelectedRecipe = recipe;
        InitializeMaterials(materials);
    }

    private void SetDescription(Item item)
    {
        itemDescription.text = string.IsNullOrEmpty(item.FormatDescription()) ? "No description": item.FormatDescription();
    }

    private void InitializeMaterials(List<RecipeElement> elements)
    {
        ClearMaterials();
        foreach (RecipeElement element in elements)
        {
            CraftingMaterialUI material = Instantiate<CraftingMaterialUI>(materialUI, materialsGroup.transform, false);
            material.Initialize(element);
        }
    }

    private void ClearMaterials()
    {
        for (int i = 0; i < materialsGroup.transform.childCount; i++)
        {
            Destroy(materialsGroup.transform.GetChild(i).gameObject);
        }
    }
}