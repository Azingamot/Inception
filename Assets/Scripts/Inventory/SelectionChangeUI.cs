using TMPro;
using UnityEngine;

public class SelectionChangeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameText;

    public void UpdateSelectionText(object context)
    {
        if (context != null && context is Item)
            itemNameText.text = ((Item)context).name;
        else
            itemNameText.text = string.Empty;
    }
}
