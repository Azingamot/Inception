using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TMP_Text saveName;

    private Image background;
    private int index = -1;
    private SavesUI savesUI;

    public void Initialize(int index, string name, SavesUI savesUI)
    {
        this.index = index;
        saveName.text = name;
        this.savesUI = savesUI;
        background = GetComponentInChildren<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        savesUI.OpenSave(index);
        savesUI.ChangeSelection(this);
    }

    public void Select()
    {
        background.color = Color.gray;
    }

    public void Deselect()
    {
        background.color = Color.white;
    }
}
