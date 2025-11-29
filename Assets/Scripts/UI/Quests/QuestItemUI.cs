using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestItemUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TMP_Text questTitle;
    [SerializeField] private Image faceImage, handImage;
    private QuestsUI questsUI;
    private Image background;
    public QuestData CurrentData { get; private set; }

    public void Initialize(QuestData quest, QuestsUI questsUI, Expression expression)
    {
        questTitle.text = quest.Title;
        this.questsUI = questsUI;
        background = GetComponentInChildren<Image>();
        CurrentData = quest;
        faceImage.sprite = expression.FaceSprite;
        handImage.sprite = expression.HandSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        questsUI.Select(this);
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
