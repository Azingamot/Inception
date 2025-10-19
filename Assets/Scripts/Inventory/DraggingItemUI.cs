using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggingItemUI : MonoBehaviour
{
    public static DraggingItemUI Instance { get; private set; }

    [SerializeField] private Image dragImage;
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool isDragging = false;
    private InventorySlotUI sourceSlot;

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
        rectTransform = dragImage.GetComponent<RectTransform>();
        canvasGroup = dragImage.GetComponent<CanvasGroup>();
        dragImage.enabled = false;
    }

    public void StartDrag(Sprite sprite, PointerEventData eventData, InventorySlotUI fromSlot = null)
    {
        dragImage.sprite = sprite;
        dragImage.enabled = true;
        canvasGroup.blocksRaycasts = false;
        isDragging = true;
        sourceSlot = fromSlot;

        UpdateDragPosition(eventData);
    }

    public void UpdateDragPosition(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out position
        );

        rectTransform.position = canvas.transform.TransformPoint(position);
    }

    public void EndDrag()
    {
        dragImage.enabled = false;
        canvasGroup.blocksRaycasts = true;
        isDragging = false;
        sourceSlot = null;
    }

    public bool IsDragging => isDragging;
    public InventorySlotUI SourceSlot => sourceSlot;
}
