using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingItemUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemText;
    [SerializeField] private CraftingMaterialUI materialUI;
    private CraftRecipe recipe;
    private CraftingItemDescriptionUI descriptionUI;

    public void Initialize(CraftRecipe craftRecipe, CraftingItemDescriptionUI descriptionUI)
    {
        itemImage.sprite = craftRecipe.Result.ItemSprite;
        itemText.text = craftRecipe.Result.name + " x" + craftRecipe.Count;
        this.descriptionUI = descriptionUI;
        this.recipe = craftRecipe;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!descriptionUI.gameObject.activeInHierarchy)
            descriptionUI.gameObject.SetActive(true);
        descriptionUI?.SetData(recipe);
    }
}
