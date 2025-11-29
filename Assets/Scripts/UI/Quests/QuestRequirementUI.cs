using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequirementUI : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemText;
    
    public void Initialize(RecipeElement requirement)
    {
        itemImage.sprite = requirement.Item.ItemSprite;
        itemText.text = requirement.Item.Name + " x" + requirement.Count;
    }
}
