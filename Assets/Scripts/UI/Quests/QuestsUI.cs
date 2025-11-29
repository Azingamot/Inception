using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestsUI : MonoBehaviour
{
    [SerializeField] private QuestItemUI questItem;
    [SerializeField] private LayoutGroup questsLayout;
    [SerializeField] private QuestDescriptionUI questDescription;
    [SerializeField] private List<Expression> expressions;
    private List<QuestItemUI> questItemUIs = new();

    public void LoadQuestItems()
    {
        Clear();
        foreach (ActiveQuest activeQuest in CurrentQuests.Get())
        {
            QuestItemUI questItemUI = Instantiate(questItem, questsLayout.transform, false);
            questItemUI.Initialize(activeQuest.QuestData, this, ReceiveExpression(activeQuest));
            questItemUIs.Add(questItemUI);
        }
    }

    private Expression ReceiveExpression(ActiveQuest quest)
    {
        return expressions.FirstOrDefault(u => u.QuestStatus == quest.QuestStatus);
    }

    public void Select(QuestItemUI questItemUI)
    {
        questItemUI.Select();
        foreach (QuestItemUI quest in questItemUIs)
        {
            if (quest != questItemUI)
                quest.Deselect();
        }
        questDescription.SetDescription(questItemUI.CurrentData);
    }

    private void Clear()
    {
        questItemUIs.Clear();
        for (int i = 0; i < questsLayout.transform.childCount; i++)
        {
            Destroy(questsLayout.transform.GetChild(i).gameObject);
        }
    }
}
