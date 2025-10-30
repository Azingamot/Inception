using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMaterialUI : MonoBehaviour
{
    [SerializeField] private Image materialImage;
    [SerializeField] private TMP_Text materialCount;
   
    public void Initialize(RecipeElement recipeElement)
    {
        materialImage.sprite = recipeElement.DropItem.ItemSprite;
        materialCount.text = recipeElement.Count.ToString();
    }
}
