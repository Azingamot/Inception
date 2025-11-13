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
    [SerializeField] private DropItems dropItems;
    public bool IsOpened = false;

    private void Start()
    {
        itemDescription.gameObject.SetActive(false);
    }

    public void Open(List<CraftRecipe> recipes, bool firstOpen = false)
    {
        if (!InventoryUIControl.Instance.InventoryActive)
            return;

        SetItems(recipes);
        craftUI.SetActive(true);
        IsOpened = true;
    }
    
    public void Close()
    {
        craftUI.SetActive(false);
        IsOpened = false;
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
        if (!craftItem.CreateItem(itemDescription.SelectedRecipe, dropItems, PlayerPosition.GetData()))
        {
            TextNotificationUI.Instance.Notify("Not enough resources");
        }
    }

    private void Clear()
    {
        for (int i = 0; i < itemsLayout.transform.childCount; i++)
        {
            Destroy(itemsLayout.transform.GetChild(i).gameObject);
        }
    }
}
