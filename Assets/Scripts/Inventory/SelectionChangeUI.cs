using TMPro;
using UnityEngine;

public class SelectionChangeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameText;

    public void UpdateSelectionText(object context)
    {
        if (context != null && context is Item item)
        {
			itemNameText.text = item.Name;
            itemNameText.color = Rarities.ItemColor(item);
		}
        else
            itemNameText.text = string.Empty;
    }
}
