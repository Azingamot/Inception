using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IDragHandler, IDropHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Image selectionIndicator;
    [SerializeField] private InventorySlotData slotData;
    [SerializeField] private InventoryController controller;
    private int slotIndex;
	private ItemDescription itemDescription;

	public void Setup(InventorySlotData data, int index, InventoryController ctrl, ItemDescription itemDescription = null)
    {
        slotData = data;
        slotIndex = index;
        controller = ctrl;
		if (itemDescription != null) this.itemDescription = itemDescription;
		Refresh();
    }

    public void Refresh()
    {
        if (slotData?.ItemInSlot != null)
        {
            icon.sprite = slotData.ItemInSlot.ItemSprite;
            icon.enabled = true;
            countText.text = slotData.Count > 1 ? slotData.Count.ToString() : "";
        }
        else
        {
            icon.enabled = false;
            countText.text = "";
        }
    }

    public void SetSelected(bool isSelected)
    {
        selectionIndicator.enabled = isSelected;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotData?.ItemInSlot == null) return;

        DraggingItemUI.Instance.StartDrag(icon.sprite, eventData, this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        DraggingItemUI.Instance.UpdateDragPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DraggingItemUI.Instance.EndDrag();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!DraggingItemUI.Instance.IsDragging) return;

        var draggedFrom = DraggingItemUI.Instance.SourceSlot;

        if (draggedFrom != null && draggedFrom != this)
        {
            controller.SwapItems(draggedFrom.slotIndex, slotIndex);
            ShowDescription(eventData);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (slotIndex < controller.Data.HotbarSize)
        {
            controller.ChangeSelectedSlot(slotIndex);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowDescription(eventData);
    }

    private void ShowDescription(PointerEventData eventData)
    {
        eventData.position = transform.position;
        itemDescription.ShowDescription(eventData, slotData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemDescription.HideDescription();
    }
}
