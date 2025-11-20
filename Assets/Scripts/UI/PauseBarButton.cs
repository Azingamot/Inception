using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PauseBarButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private bool selectable = true;
    [SerializeField] private TMP_FontAsset outlinedTextAsset;
    [SerializeField] private UnityEvent onButtonClick;
    private bool isSelected = false;
    private TMP_FontAsset baseAsset;
    private PauseBarUI pauseBarUI;

    public void Initialize(PauseBarUI pauseBarUI)
    {
        baseAsset = buttonText.font;
        this.pauseBarUI = pauseBarUI;
        ChangeSelection(false);
    }

    public void ChangeSelection(bool selected)
    {
        isSelected = selected;
        if (selectable) buttonText.font = selected ? outlinedTextAsset : baseAsset;
        if (selected)
            pauseBarUI.DeselectButtons(this);
    }

	public void OnPointerDown(PointerEventData eventData)
	{
        ChangeSelection(true);
		buttonText.color = Color.lightGray;
		onButtonClick.Invoke();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
        buttonText.color = Color.lightGray;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
        if (!isSelected) buttonText.color = Color.white;
	}
}
