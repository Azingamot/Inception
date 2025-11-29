using System.Linq;
using UnityEngine;

[RequireComponent(typeof(QuestSystem))]
public class QuestValidator : MonoBehaviour
{
    private QuestSystem questSystem;

	private void Start()
	{
		questSystem = GetComponent<QuestSystem>();
	}

    public bool Validate(EventData eventData)
    {
        if (eventData is QuestDialogueEventData questDialogueEventData)
            return Validate(questDialogueEventData.QuestData);
        return false;
    }

	public bool Validate(QuestData quest)
    {
        ActiveQuest activeQuest = CurrentQuests.Get().FirstOrDefault(u => u.QuestData.UID == quest.UID);
        if (activeQuest != null)
            return questSystem.QuestFulfill(activeQuest, ClockController.GetClockContext());
        return false;
    }
}
