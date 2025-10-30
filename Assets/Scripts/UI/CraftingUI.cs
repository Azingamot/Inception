using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] private CraftingItemDescriptionUI itemDescription;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private CraftingItemUI itemUI;
    [SerializeField] private LayoutGroup itemsLayout;
    [SerializeField] private CraftItem craftItem;
   
    public void Open(List<CraftRecipe> recipes)
    {
        if (!craftUI.activeInHierarchy)
        {
            itemDescription.gameObject.SetActive(false);
            SetItems(recipes);
            craftUI.SetActive(true);
        }
    }
    
    public void Close()
    {
        if (craftUI.activeInHierarchy)
        {
            craftUI.SetActive(false);
        }
    }

    public void SetItems(List<CraftRecipe> recipes)
    {
        Clear();

        foreach(CraftRecipe recipe in recipes)
        {
            CraftingItemUI craftingItem = Instantiate<CraftingItemUI>(itemUI, itemsLayout.transform, false);
            craftingItem.Initialize(recipe, itemDescription);
        }
    }

    public void CreateButtonClick()
    {
        craftItem.CreateItem(itemDescription.SelectedRecipe);
    }

    private void Clear()
    {
        for (int i = 0; i < itemsLayout.transform.childCount; i++)
        {
            Destroy(itemsLayout.transform.GetChild(i).gameObject);
        }
    }
}
