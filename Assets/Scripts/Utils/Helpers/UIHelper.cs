using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public static class UIHelper
{
    public static bool IsPointerOverUI()
    {
        List<RaycastResult> raycastResults = new();

        if (EventSystem.current == null)
            return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = MousePosition.GetData()
        };

        EventSystem.current.RaycastAll(eventData, raycastResults);

        return raycastResults.Count > 0;
    }
}
