using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDescription : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject itemDescriptionObject;
    [SerializeField] private TMP_Text itemTitleText, itemDescriptionText;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

	private void Awake()
	{
		rectTransform = itemDescriptionObject.GetComponent<RectTransform>();
        canvasGroup = itemDescriptionObject.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
	}

	public void ShowDescription(PointerEventData eventData, InventorySlotData slotData)
    {
        if (slotData.ItemInSlot == null)
        {
            HideDescription();
            return;
        }

        itemDescriptionObject.SetActive(true);
        itemTitleText.text = slotData.ItemInSlot.Name + " (" + slotData.Count + ")";
        itemTitleText.color = Rarities.ItemColor(slotData.ItemInSlot);
        itemDescriptionText.text = slotData.ItemInSlot.FormatDescription();

        Vector2 position;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			canvas.transform as RectTransform,
			eventData.position,
			eventData.pressEventCamera,
			out position
		);

		rectTransform.position = canvas.transform.TransformPoint(position);
	}

    public void HideDescription()
    {
        itemDescriptionObject.SetActive(false);
    }
}
