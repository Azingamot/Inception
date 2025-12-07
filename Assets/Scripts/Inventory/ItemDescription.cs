using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject itemDescriptionObject;
    [SerializeField] private TMP_Text itemTitleText, itemDescriptionText;
    [SerializeField] private Sprite descriptionSmall, descriptionLarge;
    [SerializeField] private Image itemDescriptionBackground;
    [SerializeField] private Transform itemDescriptionTransform;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 basePosition;

	private void Awake()
	{
		rectTransform = itemDescriptionObject.GetComponent<RectTransform>();
        canvasGroup = itemDescriptionObject.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        basePosition = itemDescriptionTransform.localPosition;
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
        CheckTitleSize(itemTitleText.text);

        itemTitleText.color = Rarities.ItemColor(slotData.ItemInSlot);

        itemDescriptionText.text = slotData.ItemInSlot.FormatDescription();
        CheckDescriptionSize(itemDescriptionText.text);

        Vector2 position;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			canvas.transform as RectTransform,
			eventData.position,
			eventData.pressEventCamera,
			out position
		);

		rectTransform.position = canvas.transform.TransformPoint(position);
	}

    private void CheckTitleSize(string titleText)
    {
        if (titleText.Length > 25)
            itemTitleText.fontSize = 14;
        else
            itemTitleText.fontSize = 20;
    }

    private void CheckDescriptionSize(string descriptionText)
    {
        if (descriptionText.Length > 100)
            SetDescriptionSize(14, descriptionLarge, TextAlignmentOptions.TopLeft, basePosition);
        else if (descriptionText.Length <= 1)
            SetDescriptionSize(20, descriptionSmall, TextAlignmentOptions.CenterGeoAligned, new Vector2(-42, 59));
        else
            SetDescriptionSize(20, descriptionLarge, TextAlignmentOptions.TopLeft, basePosition);
    }

    private void SetDescriptionSize(int fontSize, Sprite sprite, TextAlignmentOptions alignment, Vector2 position)
    {
        itemDescriptionText.fontSize = fontSize;
        itemDescriptionBackground.sprite = sprite;
        itemTitleText.alignment = alignment;
        itemDescriptionTransform.localPosition = position;
    }

    public void HideDescription()
    {
        itemDescriptionObject.SetActive(false);
    }
}
