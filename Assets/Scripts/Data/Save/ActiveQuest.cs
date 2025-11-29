using UnityEngine;

[System.Serializable]
public class ActiveQuest
{
    public string UID;
    public QuestData QuestData => ResourcesHelper.FindQuestResource(UID);
    public ClockContext TimeStarted;
    public QuestStatuses QuestStatus;
}
