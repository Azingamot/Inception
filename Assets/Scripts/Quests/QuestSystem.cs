using UnityEngine;
using System.Collections.Generic;

public class QuestSystem : MonoBehaviour
{
    public void AddQuest(EventData data)
    {
        if (data is QuestDialogueEventData questEventData)
        {
            AddQuest(questEventData.QuestData, ClockController.GetClockContext());
        }
    }

    public void AddQuest(QuestData questData, ClockContext timeStarted)
    {
        ActiveQuest activeQuest = new ActiveQuest() { UID = questData.UID, TimeStarted = timeStarted};
        TextNotificationUI.Instance.Notify(questData.Title);
        CurrentQuests.AddData(activeQuest);
    }

    public bool QuestValidation(ActiveQuest quest, ClockContext context)
    {
        if (HourDifference(quest.TimeStarted, context) > quest.QuestData.HoursToFinish)
            quest.QuestStatus = QuestStatuses.Failed;
        return quest.QuestStatus == QuestStatuses.Failed;
    }

    public bool QuestFulfill(ActiveQuest quest, ClockContext context)
    {
        InventoryController inventory = InventoryController.Instance;
        foreach (RecipeElement element in quest.QuestData.Requirements)
        {
            InventorySlotData slotData = inventory.data.GetSlotWithItem(element.Item, element.Count);
            if (slotData == null)
            {
                TextNotificationUI.Instance.Notify(quest.QuestData.Title + " " + NamesHelper.ReceiveName("failed") + "!");
                quest.QuestStatus = QuestStatuses.Failed;
                return false;
            }
            else
                InventoryController.Instance.RemoveItem(slotData.ItemInSlot, element.Count);
        }
        TextNotificationUI.Instance.Notify(quest.QuestData.Title + " " + NamesHelper.ReceiveName("completed") + "!");
        quest.QuestStatus = QuestStatuses.Completed;
        return true;
    }

    private int HourDifference(ClockContext first, ClockContext second)
    {
        int firstHours = ContextToHours(first);
        int secondHours = ContextToHours(second);
        return secondHours - firstHours;
    }

    private int ContextToHours(ClockContext context)
    {
        return context.Hours + (context.Days * 24);
    }
}
