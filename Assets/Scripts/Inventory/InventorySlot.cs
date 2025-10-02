using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image outline;
    private Vector3 baseScale;

    private void Awake()
    {
        baseScale = transform.localScale;
        SelectionChange(false);
    }

    public void SelectionChange(bool selected)
    {
        outline.enabled = selected;
        ScaleChange(selected);
    }

    private void ScaleChange(bool selected)
    {
        Vector2 scale = selected ? baseScale * 1.2f : baseScale;
        transform.localScale = scale;
        outline.transform.localScale = scale;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
            item.ParentAfterDrag = transform;
        }
    }
}
