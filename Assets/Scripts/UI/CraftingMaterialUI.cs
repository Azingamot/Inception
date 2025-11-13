using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMaterialUI : MonoBehaviour
{
    [SerializeField] private Image materialImage;
    [SerializeField] private TMP_Text materialCount;
   
    public void Initialize(RecipeElement recipeElement)
    {
        if (recipeElement == null)
            Debug.LogError("Инициализирован пустой объект");
        materialImage.sprite = recipeElement.Item.ItemSprite;
        materialCount.text = recipeElement.Count.ToString();
    }
}
