using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


/// <summary>
/// UI элемент для объекта инвентаря
/// </summary>
public class InventoryItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("UI")]
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text countText;

    [SerializeField] private InputActionReference mousePos;
    [HideInInspector] public Transform ParentAfterDrag;
    [HideInInspector] public Item ItemInSlot;
    [HideInInspector] public int Count = 1;

    public void InitializeItem(Item newItem, int count = 1)
    {
        ItemInSlot = newItem;   
        image.sprite = ItemInSlot.ItemSprite;
        Count = count;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = Count != 1 ? Count.ToString() : "";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!UIControl.Instance.InventoryActive) return;
        image.raycastTarget = false;
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!UIControl.Instance.InventoryActive) return;
        transform.position = mousePos.action.ReadValue<Vector2>();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!UIControl.Instance.InventoryActive) return;
        image.raycastTarget = true;
        transform.SetParent(ParentAfterDrag);
        InventoryManager.instance.ChangeSelectedSlot(InventoryManager.instance.SelectedSlot);
    }
}
